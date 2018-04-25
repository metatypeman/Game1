using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class HumanoidBodyHost : MonoBehaviour, IInternalBodyHumanoidHost, IInternalHumanoid
{
    private Rigidbody mRigidbody;
    private Animator mAnimator;
    private NavMeshAgent mNavMeshAgent;

    [SerializeField]
    public Transform Head;

    #region body rotation
    public float DefaultBodyAngleSpeed = 0.5f;
    private float mCurrentBodyAngle;
    private float mBodyAngleSpeed;
    private float mTargetBodyAngle;
    private float mBodyAngleDelta;
    private float mAbsBodyAngleDelta;

    private bool mNeedBodyChanges;
    #endregion

    #region head rotation
    public float DefaultHeadAngleSpeed = 0.5f;
    private float mCurrentHeadAngle;
    private float mHeadAngleSpeed;
    private float mTargetHeadAngle;
    private float mHeadAngleDelta;
    private float mAbsHeadAngleDelta;
    private Vector3 mCurrentHeadPosition;

    private bool mNeedHeadChanges;
    #endregion

    private bool mUseIkAnimation;

    public void CallInMainUI(Action function)
    {
        var invocable = new InvocableObj(function, this);
        invocable.Run();
    }
    
    public TResult CallInMainUI<TResult>(Func<TResult> function)
    {
        var invocable = new InvocableObj(function, this);
        return invocable.Run();
    }

    private object mTmpQueueLockObj = new object();
    private Queue<IInvocableObj> mTmpQueue = new Queue<IInvocableObj>();

    private void ProcessInvocable()
    {
        List<IInvocableObj> invocableList = null;

        lock (mTmpQueueLockObj)
        {
            if (mTmpQueue.Count > 0)
            {
                invocableList = mTmpQueue.ToList();
                mTmpQueue.Clear();
            }
        }

        if (invocableList == null)
        {
            return;
        }

        foreach (var invocable in invocableList)
        {
            invocable.Invoke();
        }
    }

    public void SetInvocableObj(IInvocableObj invokableObj)
    {
        lock (mTmpQueueLockObj)
        {
            mTmpQueue.Enqueue(invokableObj);
        }
    }

    private IAimCorrector mAimCorrector;

    public void SetAimCorrector(IAimCorrector corrector)
    {
        mAimCorrector = corrector;
    }

    public void SetInternalHumanoidHostContext(IInternalHumanoidHostContext intenalHostContext)
    {
        mInternalHumanoidHostContext = intenalHostContext;
    }

    private IInternalHumanoidHostContext mInternalHumanoidHostContext;

    // Use this for initialization
    void Start ()
    {
        mStates = new InternalStatesOfHumanoidController();
        mBehaviourFlags = new BehaviourFlagsOfHumanoidController();

        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();
        mNavMeshAgent = GetComponent<NavMeshAgent>();

        var rightHandLocator = GetComponentInChildren<RightHandLocator>();

        if (rightHandLocator != null)
        {
            RightHand = rightHandLocator.gameObject;
        }

        var rightHandWPLocator = GetComponentInChildren<RightHandWPLocator>();

        if (rightHandWPLocator != null)
        {
            RightHandWP = rightHandWPLocator.gameObject;
        }

        var leftHandLocator = GetComponentInChildren<LeftHandLocator>();

        if (leftHandLocator != null)
        {
            LeftHand = leftHandLocator.gameObject;
        }

        var leftHandWPLocator = GetComponentInChildren<LeftHandWPLocator>();

        if (leftHandWPLocator != null)
        {
            LeftHandWP = leftHandWPLocator.gameObject;
        }

        mRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        mCurrentHeadAngle = 0f;

        ApplyCurrentStates();

        //StartCoroutine(Timer());
    }

    public GameObject RightHand { get; private set; }
    public GameObject RightHandWP { get; private set; }
    public GameObject LeftHand { get; private set; }
    public GameObject LeftHandWP { get; private set; }

    private InternalStatesOfHumanoidController mStates;

    public InternalStatesOfHumanoidController States => mStates;

    private BehaviourFlagsOfHumanoidController mBehaviourFlags;

    public event InternalHumanoidStatesChangedAction OnHumanoidStatesChanged;

    private void EmitOnHumanoidStatesChanged(params InternalHumanoidStateKind[] changedStates)
    {
        Task.Run(() => {
            OnHumanoidStatesChanged?.Invoke(changedStates.ToList());
        });
    }

    private void ApplyCurrentStates()
    {
        mBehaviourFlags.Append(CreateBehaviourFlags(mStates));

        ApplyInternalStates();
    }

    public void Execute(InternalTargetStateOfHumanoidController targetState)
    {
        var invocable = new InvocableObj(() => {
            NExecute(targetState);
        }, this);

        invocable.Run();
    }

    private void NExecute(InternalTargetStateOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        Debug.Log($"EnemyController NExecute targetState = {targetState}");
#endif

        var newState = CreateTargetState(mStates, targetState);

#if UNITY_EDITOR
        Debug.Log($"EnemyController NExecute newState = {newState}");
#endif

        if (newState.KindOfThingsCommand != InternalKindOfHumanoidThingsCommand.Undefined && newState.InstanceOfThingId != 0)
        {
            ExecuteThingsCommand(newState);
        }
        else
        {
            ApplyTargetState(newState);
        }
    }

    private void ExecuteThingsCommand(InternalStatesOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        //Debug.Log($"EnemyController ExecuteThingsCommand targetState = {targetState}");
#endif

        var kindOfThingsCommand = targetState.KindOfThingsCommand;

        var myGameObjectOfThing = MyGameObjectsBus.GetObject(targetState.InstanceOfThingId);
        var gameObjectOfThing = myGameObjectOfThing.GameObject;

        var thing = gameObjectOfThing.GetComponent<IHandThing>();

        switch (kindOfThingsCommand)
        {
            case InternalKindOfHumanoidThingsCommand.Take:
                {
                    thing.SetToHandsOfHumanoid(this, mInternalHumanoidHostContext);
                    mStates.HandsState = InternalHumanoidHandsState.HasRifle;
                    ApplyCurrentStates();
                    EmitOnHumanoidStatesChanged(InternalHumanoidStateKind.ThingsCommand);
                }
                break;

            case InternalKindOfHumanoidThingsCommand.PutToBagpack:
                {
                    thing.SetAsAloneAndHide();
                    mStates.HandsState = InternalHumanoidHandsState.FreeHands;
                    mStates.HandsActionState = InternalHumanoidHandsActionState.Empty;
                    ApplyCurrentStates();
                    EmitOnHumanoidStatesChanged(InternalHumanoidStateKind.ThingsCommand);
                }
                break;

            case InternalKindOfHumanoidThingsCommand.ThrowOutToSurface:
                {
                    thing.ThrowOutToSurface();
                    mStates.HandsState = InternalHumanoidHandsState.FreeHands;
                    mStates.HandsActionState = InternalHumanoidHandsActionState.Empty;
                    ApplyCurrentStates();
                    EmitOnHumanoidStatesChanged(InternalHumanoidStateKind.ThingsCommand);
                }
                break;

            default: throw new ArgumentOutOfRangeException(nameof(kindOfThingsCommand), kindOfThingsCommand, null);
        }
    }

    private InternalStatesOfHumanoidController CreateTargetState(InternalStatesOfHumanoidController sourceState, InternalTargetStateOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        //Debug.Log("EnemyController CreateTargetState sourceState = " + sourceState);
        //Debug.Log("EnemyController CreateTargetState targetState = " + targetState);
#endif

        var result = sourceState.Clone();

        if (targetState.HState.HasValue)
        {
            result.HState = targetState.HState.Value;
            result.TargetPosition = targetState.TargetPosition.Value;
        }

        if (targetState.VState.HasValue)
        {
            result.VState = targetState.VState.Value;
        }

        if (targetState.HandsState.HasValue)
        {
            result.HandsState = targetState.HandsState.Value;
        }

        if (targetState.HandsActionState.HasValue)
        {
            result.HandsActionState = targetState.HandsActionState.Value;
        }

        if (targetState.HeadState.HasValue)
        {
            result.HeadState = targetState.HeadState.Value;
            if (targetState.TargetHeadPosition.HasValue)
            {
                result.TargetHeadPosition = targetState.TargetHeadPosition.Value;
            }
        }

        if (targetState.KindOfThingsCommand.HasValue)
        {
            result.KindOfThingsCommand = targetState.KindOfThingsCommand.Value;
            if (targetState.InstanceOfThingId.HasValue)
            {
                result.InstanceOfThingId = targetState.InstanceOfThingId.Value;
            }
        }

        switch (result.HState)
        {
            case InternalHumanoidHState.Stop:
                result.TargetPosition = null;
                break;

            case InternalHumanoidHState.Walk:
            case InternalHumanoidHState.Run:
            case InternalHumanoidHState.LookAt:
            case InternalHumanoidHState.Move:
            case InternalHumanoidHState.Rotate:
                if (!result.TargetPosition.HasValue)
                {
                    result.HState = InternalHumanoidHState.Stop;
                }
                break;

            case InternalHumanoidHState.AimAt:
                if (result.HandsState != InternalHumanoidHandsState.HasRifle || result.HandsActionState != InternalHumanoidHandsActionState.StrongAim || !result.TargetPosition.HasValue)
                {
                    result.HState = InternalHumanoidHState.Stop;
                }
                break;
        }

        switch (result.HandsState)
        {
            case InternalHumanoidHandsState.FreeHands:
                result.HandsActionState = InternalHumanoidHandsActionState.Empty;
                break;
        }

        return result;
    }

    private BehaviourFlagsOfHumanoidController CreateBehaviourFlags(InternalStatesOfHumanoidController sourceState)
    {
        var result = new BehaviourFlagsOfHumanoidController();

        switch (sourceState.HandsState)
        {
            case InternalHumanoidHandsState.FreeHands:
                result.HasRifle = false;
                break;

            case InternalHumanoidHandsState.HasRifle:
                result.HasRifle = true;
                break;
        }

        switch (sourceState.HandsActionState)
        {
            case InternalHumanoidHandsActionState.Empty:
                result.IsAim = false;
                break;

            case InternalHumanoidHandsActionState.StrongAim:
                result.IsAim = true;
                break;
        }

        switch (sourceState.HState)
        {
            case InternalHumanoidHState.Stop:
            case InternalHumanoidHState.AimAt:
            case InternalHumanoidHState.LookAt:
            case InternalHumanoidHState.Rotate:
                result.Walk = false;
                break;

            case InternalHumanoidHState.Walk:
            case InternalHumanoidHState.Move:
                result.Walk = true;
                break;
        }

        return result;
    }

    public event Action OnDie;

    public void Die()
    {
        mBehaviourFlags.IsDead = true;
        var hState = mStates.HState;
        switch (hState)
        {
            case InternalHumanoidHState.Walk:
            case InternalHumanoidHState.Run:
                mNavMeshAgent.ResetPath();
                break;
        }
        Task.Run(() => {
            OnDie?.Invoke();
        });

        UpdateAnimator();
    }

    private void ApplyTargetState(InternalStatesOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        //Debug.Log($"EnemyController ApplyTargetState targetState = {targetState}");
#endif

        var targetBehaviourFlags = CreateBehaviourFlags(targetState);

#if UNITY_EDITOR
        //Debug.Log($"EnemyController ApplyTargetState targetBehaviourFlags = {targetBehaviourFlags}");
#endif

        mStates.Append(targetState);
        mBehaviourFlags.Append(targetBehaviourFlags);

        ApplyInternalStates();
    }

    private void ApplyInternalStates()
    {
        UpdateAnimator();

#if UNITY_EDITOR
        //Debug.Log($"EnemyController ApplyInternalStates mStates = {mStates}");
#endif

        var hState = mStates.HState;
        switch (hState)
        {
            case InternalHumanoidHState.Walk:
            case InternalHumanoidHState.Run:
                {
                    if (mStates.TargetPosition.HasValue)
                    {
                        mNavMeshAgent.ResetPath();
                        mNavMeshAgent.SetDestination(mStates.TargetPosition.Value);
                    }
                }
                break;

            case InternalHumanoidHState.Move:
                {
                    if (mStates.TargetPosition.HasValue)
                    {
                        mNavMeshAgent.ResetPath();

#if DEBUG
                        //Debug.Log($"EnemyController ApplyInternalStates mStates.TargetPosition = {mStates.TargetPosition}");
#endif
                        var direction = transform.TransformDirection(mStates.TargetPosition.Value);
                        mStates.TargetPosition = transform.position + direction;

#if DEBUG
                        //Debug.Log($"EnemyController ApplyInternalStates transform.position = {transform.position}");
                        //Debug.Log($"EnemyController ApplyInternalStates after mStates.TargetPosition = {mStates.TargetPosition}");
#endif

                        mNavMeshAgent.SetDestination(mStates.TargetPosition.Value);
                    }
                }
                break;

            case InternalHumanoidHState.LookAt:
                {
                    if (mStates.TargetPosition.HasValue)
                    {
                        mNavMeshAgent.isStopped = true;
                        var targetPosition = mStates.TargetPosition.Value;
                        transform.LookAt(targetPosition);
                    }
                }
                break;

            case InternalHumanoidHState.AimAt:
                {
                    if (mStates.TargetPosition.HasValue)
                    {
                        mNavMeshAgent.isStopped = true;
                        var targetPositionValue = mStates.TargetPosition.Value;
                        var targetPos = new Vector3(targetPositionValue.x, 0, targetPositionValue.z);
                        transform.LookAt(targetPos);

                        if (mAimCorrector != null)
                        {
                            StartCoroutine(CorrectAim(targetPos));
                        }
                    }
                }
                break;

            case InternalHumanoidHState.Rotate:
                if (mStates.TargetPosition.HasValue)
                {
                    mNavMeshAgent.isStopped = true;

                    mCurrentBodyAngle = 0f;
                    mTargetBodyAngle = mStates.TargetPosition.Value.y;

                    mNeedBodyChanges = true;
                    mBodyAngleSpeed = DefaultBodyAngleSpeed;
                    if (mTargetBodyAngle > mCurrentBodyAngle)
                    {
                        mBodyAngleDelta = mBodyAngleSpeed;
                    }
                    else
                    {
                        mBodyAngleDelta = -1 * mBodyAngleSpeed;
                    }
                    mAbsBodyAngleDelta = Math.Abs(mBodyAngleDelta);

#if UNITY_EDITOR
                    //Debug.Log("EnemyController ApplyInternalStates case HumanoidHState.Rotate");
#endif
                }
                break;
        }

        var headState = mStates.HeadState;

        switch (headState)
        {
            case InternalHumanoidHeadState.LookingForward:
                mUseIkAnimation = false;
                mCurrentHeadAngle = 0f;
                EmitOnHumanoidStatesChanged(InternalHumanoidStateKind.HeadState, InternalHumanoidStateKind.TargetHeadPosition);
                break;

            case InternalHumanoidHeadState.LookAt:
                mCurrentHeadPosition = mStates.TargetHeadPosition.Value;
                mUseIkAnimation = true;
                EmitOnHumanoidStatesChanged(InternalHumanoidStateKind.HeadState, InternalHumanoidStateKind.TargetHeadPosition);
                break;

            case InternalHumanoidHeadState.Rotate:
                mUseIkAnimation = true;

                mTargetHeadAngle = mStates.TargetHeadPosition.Value.y;

                if (mTargetHeadAngle != mCurrentHeadAngle)
                {
                    mNeedHeadChanges = true;
                    mHeadAngleSpeed = DefaultHeadAngleSpeed;
                    if (mTargetHeadAngle > mCurrentHeadAngle)
                    {
                        mHeadAngleDelta = mHeadAngleSpeed;
                    }
                    else
                    {
                        mHeadAngleDelta = -1 * mHeadAngleSpeed;
                    }
                    mAbsHeadAngleDelta = Math.Abs(mHeadAngleDelta);
                }
                break;
        }
    }

    private IEnumerator CorrectAim(Vector3 targetPos)
    {
        yield return new WaitForSeconds(0.1f);

        if (mAimCorrector != null)
        {
            var correctingAngle = mAimCorrector.GetCorrectingAngle(targetPos);

#if UNITY_EDITOR
            //Debug.Log($"EnemyController ApplyInternalStates correctingAngle = {correctingAngle}");
#endif

            if (Mathf.Abs(correctingAngle) > 8)
            {
                transform.rotation = Quaternion.Euler(0, -1 * correctingAngle * 0.8f, 0) * transform.rotation;
            }

            ApplyAchieveDestinationOfMoving();
        }
    }

    private void UpdateAnimator()
    {
#if UNITY_EDITOR
        //Debug.Log("EnemyController UpdateAnimator");
#endif
        mAnimator.SetBool("hasRifle", mBehaviourFlags.HasRifle);
        mAnimator.SetBool("walk", mBehaviourFlags.Walk);
        mAnimator.SetBool("isAim", mBehaviourFlags.IsAim);
        mAnimator.SetBool("isDead", mBehaviourFlags.IsDead);
    }

    public void ApplyAchieveDestinationOfMoving()
    {
        mStates.HState = InternalHumanoidHState.Stop;
        mStates.TargetPosition = null;
        ApplyCurrentStates();
        EmitOnHumanoidStatesChanged(InternalHumanoidStateKind.HState, InternalHumanoidStateKind.TargetPosition);
    }

    private void ApplyAchiveDestinationOfHead()
    {
        if (mStates.TargetHeadPosition.HasValue)
        {
            EmitOnHumanoidStatesChanged(InternalHumanoidStateKind.HeadState, InternalHumanoidStateKind.TargetHeadPosition);
        }
        else
        {
            EmitOnHumanoidStatesChanged(InternalHumanoidStateKind.HeadState);
        }
    }

    // Update is called once per frame
    void Update ()
    {
#if UNITY_EDITOR
        //Debug.Log("EnemyController Update mNavMeshAgent.pathStatus = " + mNavMeshAgent.pathStatus + " mNavMeshAgent.isOnOffMeshLink = " + mNavMeshAgent.isOnOffMeshLink + " mNavMeshAgent.isStopped = " + mNavMeshAgent.isStopped + " mNavMeshAgent.nextPosition = " + mNavMeshAgent.nextPosition);
#endif
        if (mBehaviourFlags.IsDead)
        {
            return;
        }

        ProcessInvocable();

        var hState = mStates.HState;
        switch (hState)
        {
            case InternalHumanoidHState.Walk:
            case InternalHumanoidHState.Run:
            case InternalHumanoidHState.Move:
                {
                    var targetPosition = mStates.TargetPosition.Value;
                    var nextPosition = mNavMeshAgent.nextPosition;
                    if (targetPosition.x == nextPosition.x && targetPosition.z == nextPosition.z)
                    {
                        ApplyAchieveDestinationOfMoving();
                    }
                }
                break;

            case InternalHumanoidHState.Rotate:
                if (mNeedBodyChanges)
                {
                    var newAngle = mCurrentBodyAngle + mBodyAngleDelta;

#if UNITY_EDITOR
                    //Debug.Log($"EnemyController Update newAngle = {newAngle}");
#endif
                    var tmpDelta = mTargetBodyAngle - newAngle;

#if UNITY_EDITOR
                    //Debug.Log($"EnemyController Update mBodyAngleDelta = {mBodyAngleDelta}");
                    //Debug.Log($"EnemyController Update tmpDelta = {tmpDelta}");
#endif

                    var tmpAbsDelta = Math.Abs(tmpDelta);

#if UNITY_EDITOR
                    //Debug.Log($"EnemyController Update tmpAbsDelta = {tmpAbsDelta}");
#endif

                    if (tmpAbsDelta >= mAbsBodyAngleDelta)
                    {
#if UNITY_EDITOR
                        //Debug.Log("EnemyController Update tmpAbsDelta >= mAbsBodyAngleDelta");
#endif
                        transform.rotation = Quaternion.Euler(0, mBodyAngleDelta, 0) * transform.rotation;
                        mCurrentBodyAngle = newAngle;
                    }
                    else
                    {
#if UNITY_EDITOR
                        //Debug.Log("EnemyController Update tmpAbsDelta < mAbsBodyAngleDelta");
#endif
                        transform.rotation = Quaternion.Euler(0, tmpDelta, 0) * transform.rotation;
                        mCurrentBodyAngle = mTargetBodyAngle;

                        mNeedBodyChanges = false;
                    }

#if UNITY_EDITOR
                    //Debug.Log($"EnemyController Update mCurrentBodyAngle = {mCurrentBodyAngle}");
                    //Debug.Log($"EnemyController Update mNeedBodyChanges = {mNeedBodyChanges}");
#endif

                    if (!mNeedBodyChanges)
                    {
                        ApplyAchieveDestinationOfMoving();
                    }
                }
                break;
        }

        var headState = mStates.HeadState;

        switch (headState)
        {
            case InternalHumanoidHeadState.Rotate:
                if (mNeedHeadChanges)
                {
                    var newAngle = mCurrentHeadAngle + mHeadAngleDelta;
                    var tmpAbsDelta = Math.Abs(mTargetHeadAngle - newAngle);
                    if (tmpAbsDelta >= mAbsHeadAngleDelta)
                    {
                        mCurrentHeadAngle = newAngle;
                    }
                    else
                    {
                        mCurrentHeadAngle = mTargetHeadAngle;
                        EmitOnHumanoidStatesChanged(InternalHumanoidStateKind.HeadState, InternalHumanoidStateKind.TargetHeadPosition);
                        mNeedHeadChanges = false;
                    }

                    var radAngle = mCurrentHeadAngle * Mathf.Deg2Rad;
                    var x = Mathf.Sin(radAngle);
                    var y = Mathf.Cos(radAngle);
                    var localDirection = new Vector3(x, 0f, y);
                    var globalDirection = transform.TransformDirection(localDirection);
                    var oldY = Head.position.y;

#if UNITY_EDITOR
                    //Debug.Log($"EnemyController Update oldY = {oldY}");
                    //Debug.Log($"EnemyController Update Head.localPosition.y = {Head.localPosition.y}");
#endif

                    var newPosition = globalDirection + transform.position;
                    mCurrentHeadPosition = new Vector3(newPosition.x, oldY, newPosition.z);
                }
                break;
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!mUseIkAnimation)
        {
            return;
        }

        var headState = mStates.HeadState;

        switch (headState)
        {
            case InternalHumanoidHeadState.LookingForward:
                break;

            case InternalHumanoidHeadState.LookAt:
            case InternalHumanoidHeadState.Rotate:
                mAnimator.SetLookAtWeight(1);
                mAnimator.SetLookAtPosition(mCurrentHeadPosition);
                Head.LookAt(mCurrentHeadPosition);
                break;
        }
    }
}
