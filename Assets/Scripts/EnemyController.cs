using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IMoveHumanoidController, IHumanoid
{
    private Rigidbody mRigidbody;
    private Animator mAnimator;
    private NavMeshAgent mNavMeshAgent;

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

    private IAimCorrector mAimCorrector;

    public void SetAimCorrector(IAimCorrector corrector)
    {
        mAimCorrector = corrector;
    }

    public void TstTakeRifle(IHandThing thing)
    {
        thing.SetToHandsOfHumanoid(this);
        mStates.HandsState = HumanoidHandsState.HasRifle;
        ApplyCurrentStates();
    }

    // Use this for initialization
    void Start () {
        mStates = new StatesOfHumanoidController();
        mBehaviourFlags = new BehaviourFlagsOfHumanoidController();

        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();
        mNavMeshAgent = GetComponent<NavMeshAgent>();

        var rightHandLocator = GetComponentInChildren<RightHandLocator>();

        if(rightHandLocator != null)
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
            LeftHand  = leftHandLocator.gameObject;
        }
        
        var leftHandWPLocator = GetComponentInChildren<LeftHandWPLocator>();

        if (leftHandWPLocator != null)
        {
            LeftHandWP = leftHandWPLocator.gameObject;
        }
        
        mRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        mCurrentHeadAngle = 0f;

        ApplyCurrentStates();

        StartCoroutine(Timer());
    }

    public GameObject RightHand { get; private set; }
    public GameObject RightHandWP { get; private set; }
    public GameObject LeftHand { get; private set; }
    public GameObject LeftHandWP { get; private set; }

    private StatesOfHumanoidController mStates;

    public StatesOfHumanoidController States => mStates;

    private BehaviourFlagsOfHumanoidController mBehaviourFlags;

    public HumanoidHState HState => mStates.HState;
    public Vector3? TargetPosition => mStates.TargetPosition;
    public HumanoidVState VState => mStates.VState;
    public HumanoidHandsState HandsState => mStates.HandsState;
    public HumanoidHandsActionState HandsActionState => mStates.HandsActionState;
    public event HumanoidStatesChangedAction OnHumanoidStatesChanged;

    private void EmitOnHumanoidStatesChanged(params HumanoidStateKind[] changedStates)
    {
        Task.Run(()=> {
            OnHumanoidStatesChanged?.Invoke(changedStates.ToList());
        });
    }

    private void ApplyCurrentStates()
    {
        mBehaviourFlags.Append(CreateBehaviourFlags(mStates));

        ApplyInternalStates();
    }

    private object mLockObj = new object();
    private HumanoidTaskOfExecuting mTargetStateForExecuting;

    public HumanoidTaskOfExecuting ExecuteAsync(TargetStateOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        //Debug.Log($"EnemyController ExecuteAsync targetState = {targetState}");
#endif

        lock(mLockObj)
        {
            if(mTargetStateForExecuting != null)
            {
                mTargetStateForExecuting.State = StateOfHumanoidTaskOfExecuting.Canceled;
            }

            var targetStateForExecuting = new HumanoidTaskOfExecuting();
            targetStateForExecuting.ProcessedState = targetState;

            mTargetStateForExecuting = targetStateForExecuting;

#if UNITY_EDITOR
            //Debug.Log($"EnemyController ExecuteAsync mTargetStateQueue.Count = {mTargetStateQueue.Count}");
#endif

            return targetStateForExecuting;
        }
    }

    private IEnumerator Timer()
    {
        lock(mLockObj)
        {
            if(mTargetStateForExecuting != null)
            {
                var targetStateForExecuting = mTargetStateForExecuting;
                mTargetStateForExecuting = null;
                Execute(targetStateForExecuting);
            }
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(Timer());
    }

    private void Execute(HumanoidTaskOfExecuting targetStateForExecuting)
    {
        var targetState = targetStateForExecuting.ProcessedState;

#if UNITY_EDITOR
        Debug.Log($"EnemyController Execute targetState = {targetState}");
#endif

        var newState = CreateTargetState(mStates, targetState);

#if UNITY_EDITOR
        Debug.Log($"EnemyController Execute newState = {newState}");
#endif

        if(newState.KindOfThingsCommand != KindOfHumanoidThingsCommand.Undefined && newState.InstanceOfThingId != 0)
        {
            ExecuteThingsCommand(newState);
        }
        else
        {
            ApplyTargetState(newState);
        }

        targetStateForExecuting.State = StateOfHumanoidTaskOfExecuting.Executed;
    }

    private void ExecuteThingsCommand(StatesOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        Debug.Log($"EnemyController ExecuteThingsCommand targetState = {targetState}");
#endif

        var kindOfThingsCommand = targetState.KindOfThingsCommand;

        var myGameObjectOfThing = MyGameObjectsBus.GetObject(targetState.InstanceOfThingId);
        var gameObjectOfThing = myGameObjectOfThing.GameObject;

        var thing = gameObjectOfThing.GetComponent<IHandThing>();

        switch (kindOfThingsCommand)
        {
            case KindOfHumanoidThingsCommand.Take:
                {
                    thing.SetToHandsOfHumanoid(this);
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
#if UNITY_EDITOR
                    Debug.Log($"EnemyController ExecuteThingsCommand case KindOfHumanoidThingsCommand.ThrowOutToSurface Not implemented yet!!!!!!");
#endif

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

    private StatesOfHumanoidController CreateTargetState(StatesOfHumanoidController sourceState, TargetStateOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        //Debug.Log("EnemyController CreateTargetState sourceState = " + sourceState);
        //Debug.Log("EnemyController CreateTargetState targetState = " + targetState);
#endif

        var result = sourceState.Clone();

        if(targetState.HState.HasValue)
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

        if(targetState.HeadState.HasValue)
        {
            result.HeadState = targetState.HeadState.Value;
            if(targetState.TargetHeadPosition.HasValue)
            {
                result.TargetHeadPosition = targetState.TargetHeadPosition.Value;
            }       
        }

        if(targetState.KindOfThingsCommand.HasValue)
        {
            result.KindOfThingsCommand = targetState.KindOfThingsCommand.Value;
            if(targetState.InstanceOfThingId.HasValue)
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
                if(result.HandsState != HumanoidHandsState.HasRifle || result.HandsActionState != HumanoidHandsActionState.StrongAim || !result.TargetPosition.HasValue)
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

    private BehaviourFlagsOfHumanoidController CreateBehaviourFlags(StatesOfHumanoidController sourceState)
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

    public void Die()
    {
        mBehaviourFlags.IsDead = true;
        var hState = mStates.HState;
        switch(hState)
        {
            case HumanoidHState.Walk:
            case HumanoidHState.Run:
                mNavMeshAgent.ResetPath();
                break;
        }
        UpdateAnimator();
    }

    private void ApplyTargetState(StatesOfHumanoidController targetState)
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
        switch(hState)
        {
            case HumanoidHState.Walk:
            case HumanoidHState.Run:
                {
                    if(mStates.TargetPosition.HasValue)
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
                    if(mStates.TargetPosition.HasValue)
                    {
                        mNavMeshAgent.isStopped = true;
                        var targetPosition = mStates.TargetPosition.Value;
                        transform.LookAt(targetPosition);
                    }
                }              
                break;
                
            case HumanoidHState.AimAt:
                {
                    if(mStates.TargetPosition.HasValue)
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

        switch(headState)
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

                if(mTargetHeadAngle != mCurrentHeadAngle)
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
        if(mStates.TargetHeadPosition.HasValue)
        {
            EmitOnHumanoidStatesChanged(HumanoidStateKind.HeadState, HumanoidStateKind.TargetHeadPosition);
        }
        else
        {
            EmitOnHumanoidStatesChanged(HumanoidStateKind.HeadState);
        }  
    }

    // Update is called once per frame
    void Update () {
#if UNITY_EDITOR
        //Debug.Log("EnemyController Update mNavMeshAgent.pathStatus = " + mNavMeshAgent.pathStatus + " mNavMeshAgent.isOnOffMeshLink = " + mNavMeshAgent.isOnOffMeshLink + " mNavMeshAgent.isStopped = " + mNavMeshAgent.isStopped + " mNavMeshAgent.nextPosition = " + mNavMeshAgent.nextPosition);
#endif
        if(mBehaviourFlags.IsDead)
        {
            return;
        }

        var hState = mStates.HState;
        switch(hState)
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
                if(mNeedBodyChanges)
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

                    if(!mNeedBodyChanges)
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
                if(mNeedHeadChanges)
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
                    Debug.Log($"EnemyController Update oldY = {oldY}");
                    Debug.Log($"EnemyController Update Head.localPosition.y = {Head.localPosition.y}");
#endif

                    var newPosition = globalDirection + transform.position;
                    mCurrentHeadPosition = new Vector3(newPosition.x, oldY, newPosition.z);
                }
                break;
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if(!mUseIkAnimation)
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

/*
    class Program
    {
        static void Main(string[] args)
        {
            //var tmpVar = new TmpClass();
            //tmpVar.DynamicData.Color = "red";
            //tmpVar.DynamicData.State = "Filed";

            //NLog.LogManager.GetCurrentClassLogger().Info($"Main tmpVar.DynamicData.Color = {tmpVar.DynamicData.Color}");
            //NLog.LogManager.GetCurrentClassLogger().Info($"Main tmpVar.DynamicData.State = {tmpVar.DynamicData.State}");

            //var dynamicDict = tmpVar.DynamicData as IDictionary<string, object>;

            //foreach (var item in dynamicDict)
            //{
            //    NLog.LogManager.GetCurrentClassLogger().Info($"Main item.Value = {item.Value} item.Key = {item.Key}");
            //}

            //try
            //{
            //    Console.WriteLine("Input target PID:");
            //    var targetPIDStr = Console.ReadLine();

            //    NLog.LogManager.GetCurrentClassLogger().Info($"Main targetPIDStr = '{targetPIDStr}'");
            //    var targetPID = int.Parse(targetPIDStr);
            //    NLog.LogManager.GetCurrentClassLogger().Info($"Main targetPID = {targetPID}");
            //    var killedProcess = Process.GetProcessById(targetPID);
            //    killedProcess.Kill();
            //}
            //catch(Exception e)
            //{
            //    NLog.LogManager.GetCurrentClassLogger().Info($"Main e = {e}");
            //}

            TSTRotate(15f);
            NLog.LogManager.GetCurrentClassLogger().Info("Main-------------------------------");
            TSTRotate(-12f);
            NLog.LogManager.GetCurrentClassLogger().Info("Main-------------------------------");
            TSTRotate(6f);
        }

        private static float mCurrentAngle = 0f;
        private static float mAngleSpeed = 2f;
        private static float mTargetAngle;
        private static float mAngleDelta;
        private static float mAbsAngleDelta;

        private static bool mNeedStopChanges;

        private static void TSTRotate(float targetAngle)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin TSTRotate");

            if(targetAngle == mCurrentAngle)
            {
                return;
            }

            mTargetAngle = targetAngle;
            mNeedStopChanges = false;

            InitRotate();

            if(mNeedStopChanges)
            {
                return;
            }

            var n = 0;

            while(true)
            {
                Update();

                if(mNeedStopChanges)
                {
                    break;
                }

                n++;

                if(n == 100)
                {
                    break;
                }
            }

            NLog.LogManager.GetCurrentClassLogger().Info("End TSTRotate");
        }

        private static void InitRotate()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin InitRotate");
            NLog.LogManager.GetCurrentClassLogger().Info($"InitRotate mCurrentAngle = {mCurrentAngle}");
            NLog.LogManager.GetCurrentClassLogger().Info($"InitRotate mAngleSpeed = {mAngleSpeed}");

            if(mTargetAngle > mCurrentAngle)
            {
                mAngleDelta = mAngleSpeed;
            }
            else
            {
                mAngleDelta = -1 * mAngleSpeed;
            }

            NLog.LogManager.GetCurrentClassLogger().Info($"InitRotate mAngleDelta = {mAngleDelta}");

            mAbsAngleDelta = Math.Abs(mAngleDelta);

            NLog.LogManager.GetCurrentClassLogger().Info($"InitRotate mAbsAngleDelta = {mAbsAngleDelta}");

            NLog.LogManager.GetCurrentClassLogger().Info("End InitRotate");
        }

        private static void Update()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("Begin Update");
            NLog.LogManager.GetCurrentClassLogger().Info($"Update before mCurrentAngle = {mCurrentAngle}");

            var newAngle = mCurrentAngle + mAngleDelta;

            NLog.LogManager.GetCurrentClassLogger().Info($"Update newAngle = {newAngle}");

            var tmpAbsDelta = Math.Abs(mTargetAngle - newAngle);

            NLog.LogManager.GetCurrentClassLogger().Info($"Update tmpAbsDelta = {tmpAbsDelta}");

            if(tmpAbsDelta >= mAbsAngleDelta)
            {
                mCurrentAngle = newAngle;
            }
            else
            {
                NLog.LogManager.GetCurrentClassLogger().Info("Update tmpAbsDelta < mAbsAngleDelta");

                mCurrentAngle = mTargetAngle;

                mNeedStopChanges = true;
            }

            NLog.LogManager.GetCurrentClassLogger().Info($"Update after mCurrentAngle = {mCurrentAngle}");
            NLog.LogManager.GetCurrentClassLogger().Info("End Update");
        }
    }

 
*/
