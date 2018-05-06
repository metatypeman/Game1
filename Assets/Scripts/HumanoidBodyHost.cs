using Assets.Scripts;
using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class HumanoidBodyHost : MonoBehaviour, IInternalBodyHumanoidHost, IInternalHumanoid/*, IInternalLogicalObject*/
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

    private GameObjectsBus mGameObjectsBus;

    public void CallInMainUI(Action function)
    {
        var invocable = new InvocableInMainThreadObj(function, this);
        invocable.Run();
    }
    
    public TResult CallInMainUI<TResult>(Func<TResult> function)
    {
        var invocable = new InvocableInMainThreadObj<TResult>(function, this);
        return invocable.Run();
    }

    private object mTmpQueueLockObj = new object();
    private Queue<IInvocableInMainThreadObj> mTmpQueue = new Queue<IInvocableInMainThreadObj>();

    private void ProcessInvocable()
    {
        List<IInvocableInMainThreadObj> invocableList = null;

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

    public void SetInvocableObj(IInvocableInMainThreadObj invokableObj)
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
    private LogicalIndexStorage mLogicalObjectsBus;

    public ILogicalStorage HostLogicalStorage => mLogicalObjectsBus;

    private PassiveLogicalObject mSelfLogicalObject;

    public ulong SelfEntityId => mSelfEntityId;

    private ulong mSelfEntityId;

    private readonly object mIsReadyLockObj = new object();
    private bool mIsReady;

    public bool IsReady
    {
        get
        {
            lock(mIsReadyLockObj)
            {
                return mIsReady;
            }
        }
    }

    public event Action OnReady;

    // Use this for initialization
    void Start ()
    {
        var commonLevelHost = LevelCommonHostFactory.Get();
        mGameObjectsBus = commonLevelHost.GameObjectsBus;
        mLogicalObjectsBus = commonLevelHost.LogicalObjectsBus;

        mSelfLogicalObject = new PassiveLogicalObject(commonLevelHost.EntityDictionary, mLogicalObjectsBus);
        mLogicalObjectsBus.RegisterObject(mSelfLogicalObject.EntityId, mSelfLogicalObject);

        mSelfEntityId = mSelfLogicalObject.EntityId;

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

        lock(mIsReadyLockObj)
        {
            mIsReady = true;

            Task.Run(() => {
                OnReady?.Invoke();
            });
        }
#if DEBUG
        LogInstance.Log("End HumanoidBodyHost Start");
#endif
    }

    public IList<IHostVisionObject> VisibleObjects
    {
        get
        {
#if DEBUG
            LogInstance.Log("HumanoidBodyHost VisibleObjects Not Implemented Yet!!!!!!!");
#endif

            return new List<IHostVisionObject>();
        }
    }

    public GameObject RightHand { get; private set; }
    public GameObject RightHandWP { get; private set; }
    public GameObject LeftHand { get; private set; }
    public GameObject LeftHandWP { get; private set; }

    private InternalStatesOfHumanoidController mStates;

    public InternalStatesOfHumanoidController States => mStates;

    private BehaviourFlagsOfHumanoidController mBehaviourFlags;

    public event HumanoidStatesChangedAction OnHumanoidStatesChanged;

    private void EmitOnHumanoidStatesChanged(params HumanoidStateKind[] changedStates)
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
        var invocable = new InvocableInMainThreadObj(() => {
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

        if (newState.KindOfThingsCommand != KindOfHumanoidThingsCommand.Undefined && newState.InstanceOfThingId != 0)
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

        var gameObjectOfThing = mGameObjectsBus.GetObject(targetState.InstanceOfThingId);

        var thing = gameObjectOfThing.GetComponent<IHandThing>();

        switch (kindOfThingsCommand)
        {
            case KindOfHumanoidThingsCommand.Take:
                {
                    thing.SetToHandsOfHumanoid(this, mInternalHumanoidHostContext);
                    mStates.HandsState = HumanoidHandsState.HasRifle;
                    ApplyCurrentStates();
                    EmitOnHumanoidStatesChanged(HumanoidStateKind.ThingsCommand);
                }
                break;

            case KindOfHumanoidThingsCommand.PutToBagpack:
                {
                    thing.SetAsAloneAndHide();
                    mStates.HandsState = HumanoidHandsState.FreeHands;
                    mStates.HandsActionState = HumanoidHandsActionState.Empty;
                    ApplyCurrentStates();
                    EmitOnHumanoidStatesChanged(HumanoidStateKind.ThingsCommand);
                }
                break;

            case KindOfHumanoidThingsCommand.ThrowOutToSurface:
                {
                    thing.ThrowOutToSurface();
                    mStates.HandsState = HumanoidHandsState.FreeHands;
                    mStates.HandsActionState = HumanoidHandsActionState.Empty;
                    ApplyCurrentStates();
                    EmitOnHumanoidStatesChanged(HumanoidStateKind.ThingsCommand);
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
            case HumanoidHState.Stop:
                result.TargetPosition = null;
                break;

            case HumanoidHState.Walk:
            case HumanoidHState.Run:
            case HumanoidHState.LookAt:
            case HumanoidHState.Move:
            case HumanoidHState.Rotate:
                if (!result.TargetPosition.HasValue)
                {
                    result.HState = HumanoidHState.Stop;
                }
                break;

            case HumanoidHState.AimAt:
                if (result.HandsState != HumanoidHandsState.HasRifle || result.HandsActionState != HumanoidHandsActionState.StrongAim || !result.TargetPosition.HasValue)
                {
                    result.HState = HumanoidHState.Stop;
                }
                break;
        }

        switch (result.HandsState)
        {
            case HumanoidHandsState.FreeHands:
                result.HandsActionState = HumanoidHandsActionState.Empty;
                break;
        }

        return result;
    }

    private BehaviourFlagsOfHumanoidController CreateBehaviourFlags(InternalStatesOfHumanoidController sourceState)
    {
        var result = new BehaviourFlagsOfHumanoidController();

        switch (sourceState.HandsState)
        {
            case HumanoidHandsState.FreeHands:
                result.HasRifle = false;
                break;

            case HumanoidHandsState.HasRifle:
                result.HasRifle = true;
                break;
        }

        switch (sourceState.HandsActionState)
        {
            case HumanoidHandsActionState.Empty:
                result.IsAim = false;
                break;

            case HumanoidHandsActionState.StrongAim:
                result.IsAim = true;
                break;
        }

        switch (sourceState.HState)
        {
            case HumanoidHState.Stop:
            case HumanoidHState.AimAt:
            case HumanoidHState.LookAt:
            case HumanoidHState.Rotate:
                result.Walk = false;
                break;

            case HumanoidHState.Walk:
            case HumanoidHState.Move:
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
            case HumanoidHState.Walk:
            case HumanoidHState.Run:
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
            case HumanoidHState.Walk:
            case HumanoidHState.Run:
                {
                    if (mStates.TargetPosition.HasValue)
                    {
                        mNavMeshAgent.ResetPath();
                        mNavMeshAgent.SetDestination(mStates.TargetPosition.Value);
                    }
                }
                break;

            case HumanoidHState.Move:
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

            case HumanoidHState.LookAt:
                {
                    if (mStates.TargetPosition.HasValue)
                    {
                        mNavMeshAgent.isStopped = true;
                        var targetPosition = mStates.TargetPosition.Value;
                        transform.LookAt(targetPosition);
                    }
                }
                break;

            case HumanoidHState.AimAt:
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

            case HumanoidHState.Rotate:
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
            case HumanoidHeadState.LookingForward:
                mUseIkAnimation = false;
                mCurrentHeadAngle = 0f;
                EmitOnHumanoidStatesChanged(HumanoidStateKind.HeadState, HumanoidStateKind.TargetHeadPosition);
                break;

            case HumanoidHeadState.LookAt:
                mCurrentHeadPosition = mStates.TargetHeadPosition.Value;
                mUseIkAnimation = true;
                EmitOnHumanoidStatesChanged(HumanoidStateKind.HeadState, HumanoidStateKind.TargetHeadPosition);
                break;

            case HumanoidHeadState.Rotate:
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
        mStates.HState = HumanoidHState.Stop;
        mStates.TargetPosition = null;
        ApplyCurrentStates();
        EmitOnHumanoidStatesChanged(HumanoidStateKind.HState, HumanoidStateKind.TargetPosition);
    }

    private void ApplyAchiveDestinationOfHead()
    {
        if (mStates.TargetHeadPosition.HasValue)
        {
            EmitOnHumanoidStatesChanged(HumanoidStateKind.HeadState, HumanoidStateKind.TargetHeadPosition);
        }
        else
        {
            EmitOnHumanoidStatesChanged(HumanoidStateKind.HeadState);
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
            case HumanoidHState.Walk:
            case HumanoidHState.Run:
            case HumanoidHState.Move:
                {
                    var targetPosition = mStates.TargetPosition.Value;
                    var nextPosition = mNavMeshAgent.nextPosition;
                    if (targetPosition.x == nextPosition.x && targetPosition.z == nextPosition.z)
                    {
                        ApplyAchieveDestinationOfMoving();
                    }
                }
                break;

            case HumanoidHState.Rotate:
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
            case HumanoidHeadState.Rotate:
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
                        EmitOnHumanoidStatesChanged(HumanoidStateKind.HeadState, HumanoidStateKind.TargetHeadPosition);
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
            case HumanoidHeadState.LookingForward:
                break;

            case HumanoidHeadState.LookAt:
            case HumanoidHeadState.Rotate:
                mAnimator.SetLookAtWeight(1);
                mAnimator.SetLookAtPosition(mCurrentHeadPosition);
                Head.LookAt(mCurrentHeadPosition);
                break;
        }
    }
}
