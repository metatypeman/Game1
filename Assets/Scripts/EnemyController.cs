using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

#region Common Types
public enum HumanoidHState
{
    Stop,
    Walk,
    Run,
    LookAt,
    AimAt,
    Rotate,
    Move
}

public enum HumanoidVState
{
    Ground,
    Jump,
    Crouch
}

public enum HumanoidHandsState
{
    FreeHands,
    HasRifle
}

public enum HumanoidHandsActionState
{
    Empty,
    StrongAim
}

public enum HumanoidHeadState
{
    LookingForward,
    LookAt,
    Rotate
}

public enum MoveHumanoidCommandKind
{
    HState,
    VState,
    HandsState,
    HandsActionState,
    HeadState
}

public enum HumanoidStateKind
{
    HState,
    TargetPosition,
    VState,
    HandsState,
    HandsActionState,
    HeadState,
    TargetHeadPosition
}

public interface IMoveHumanoidCommand: IObjectToString
{
    MoveHumanoidCommandKind Kind { get; }
}

public interface IMoveHumanoidCommandsPackage: IObjectToString
{
    List<IMoveHumanoidCommand> Commands { get; }
}

public interface IHumanoidHStateCommand: IMoveHumanoidCommand
{
    HumanoidHState State { get; }
    Vector3? TargetPosition { get; }
    float Speed { get; }
}

public interface IHumanoidVStateCommand : IMoveHumanoidCommand
{
    HumanoidVState State { get; }
}

public interface IHumanoidHandsStateCommand : IMoveHumanoidCommand
{
    HumanoidHandsState State { get; }
}

public interface IHumanoidHandsActionStateCommand : IMoveHumanoidCommand
{
    HumanoidHandsActionState State { get; }
}

public interface IHumanoidHeadStateCommand : IMoveHumanoidCommand
{
    HumanoidHeadState State { get; }
    Vector3? TargetPosition { get; }
    float Speed { get; }
}
//----
public class MoveHumanoidCommandsPackage: IMoveHumanoidCommandsPackage
{
    public List<IMoveHumanoidCommand> Commands { get; set; } = new List<IMoveHumanoidCommand>();

    public override string ToString()
    {
        return ToString(0);
    }

    public string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}Begin {nameof(MoveHumanoidCommandsPackage)}");
        sb.Append(PropertiesToSting(n));
        sb.AppendLine($"{spaces}End {nameof(MoveHumanoidCommandsPackage)}");
        return sb.ToString();
    }

    public string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var nextN = n + 4;
        var sb = new StringBuilder();
        if (Commands == null)
        {
            sb.AppendLine($"{spaces}{nameof(Commands)} == null");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(Commands)}.Count = {Commands.Count}");
            sb.AppendLine($"{spaces}Begin {nameof(Commands)}");
            foreach(var command in Commands)
            {
                sb.Append(command.ToString(nextN));
            }
            sb.AppendLine($"{spaces}End {nameof(Commands)}");
        }
        return sb.ToString();
    }
}

public abstract class MoveHumanoidCommand: IMoveHumanoidCommand
{
    public abstract MoveHumanoidCommandKind Kind { get; }
    public abstract string ToString(int n);
    public virtual string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
        return sb.ToString();
    }
}

public class HumanoidHStateCommand: MoveHumanoidCommand, IHumanoidHStateCommand
{
    public override MoveHumanoidCommandKind Kind => MoveHumanoidCommandKind.HState;
    public HumanoidHState State { get; set; }
    public Vector3? TargetPosition { get; set; }
    public float Speed { get; set; }

    public override string ToString()
    {
        return ToString(0);
    }

    public override string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}Begin {nameof(HumanoidHStateCommand)}");
        sb.Append(PropertiesToSting(n));
        sb.AppendLine($"{spaces}End {nameof(HumanoidHStateCommand)}");
        return sb.ToString();
    }

    public override string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(base.PropertiesToSting(n));
        sb.AppendLine($"{spaces}{nameof(State)} = {State}");

        if(TargetPosition.HasValue)
        {
            var targetPosition = TargetPosition.Value;
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
        }
        sb.AppendLine($"{spaces}{nameof(Speed)} = {Speed}");
        return sb.ToString();
    }
}

public class HumanoidVStateCommand: MoveHumanoidCommand, IHumanoidVStateCommand
{
    public override MoveHumanoidCommandKind Kind => MoveHumanoidCommandKind.VState;
    public HumanoidVState State { get; set; }

    public override string ToString()
    {
        return ToString(0);
    }

    public override string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}Begin {nameof(HumanoidVStateCommand)}");
        sb.Append(PropertiesToSting(n));
        sb.AppendLine($"{spaces}End {nameof(HumanoidVStateCommand)}");
        return sb.ToString();
    }

    public override string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(base.PropertiesToSting(n));
        sb.AppendLine($"{spaces}{nameof(State)} = {State}");
        return sb.ToString();
    }
}

public class HumanoidHandsStateCommand: MoveHumanoidCommand, IHumanoidHandsStateCommand
{
    public override MoveHumanoidCommandKind Kind => MoveHumanoidCommandKind.HandsState;
    public HumanoidHandsState State { get; set; }

    public override string ToString()
    {
        return ToString(0);
    }

    public override string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}Begin {nameof(HumanoidHandsStateCommand)}");
        sb.Append(PropertiesToSting(n));
        sb.AppendLine($"{spaces}End {nameof(HumanoidHandsStateCommand)}");
        return sb.ToString();
    }

    public override string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(base.PropertiesToSting(n));
        sb.AppendLine($"{spaces}{nameof(State)} = {State}");
        return sb.ToString();
    }
}

public class HumanoidHandsActionStateCommand: MoveHumanoidCommand, IHumanoidHandsActionStateCommand
{
    public override MoveHumanoidCommandKind Kind => MoveHumanoidCommandKind.HandsActionState;
    public HumanoidHandsActionState State { get; set; }

    public override string ToString()
    {
        return ToString(0);
    }

    public override string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}Begin {nameof(HumanoidHandsActionStateCommand)}");
        sb.Append(PropertiesToSting(n));
        sb.AppendLine($"{spaces}End {nameof(HumanoidHandsActionStateCommand)}");
        return sb.ToString();
    }

    public override string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(base.PropertiesToSting(n));
        sb.AppendLine($"{spaces}{nameof(State)} = {State}");
        return sb.ToString();
    }
}

public class HumanoidHeadStateCommand: MoveHumanoidCommand, IHumanoidHeadStateCommand
{
    public override MoveHumanoidCommandKind Kind => MoveHumanoidCommandKind.HeadState;
    public HumanoidHeadState State { get; set; }
    public Vector3? TargetPosition { get; set; }
    public float Speed { get; set; }
    
    public override string ToString()
    {
        return ToString(0);
    }

    public override string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}Begin {nameof(HumanoidHeadStateCommand)}");
        sb.Append(PropertiesToSting(n));
        sb.AppendLine($"{spaces}End {nameof(HumanoidHeadStateCommand)}");
        return sb.ToString();
    }

    public override string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(base.PropertiesToSting(n));
        sb.AppendLine($"{spaces}{nameof(State)} = {State}");

        if(TargetPosition.HasValue)
        {
            var targetPosition = TargetPosition.Value;
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
        }
        sb.AppendLine($"{spaces}{nameof(Speed)} = {Speed}");
        return sb.ToString();
    }   
}

//----
public delegate void HumanoidStatesChangedAction(List<HumanoidStateKind> changedStates);

public interface IMoveHumanoidController
{
    void ExecuteAsync(TargetStateOfHumanoidController targetState);
    StatesOfHumanoidController States { get; }
    event HumanoidStatesChangedAction OnHumanoidStatesChanged;
    void Die();
    void SetAimCorrector(IAimCorrector corrector);
}

public class TargetStateOfHumanoidController : IObjectToString
{
    public HumanoidHState? HState { get; set; }
    public Vector3? TargetPosition { get; set; }
    public HumanoidVState? VState { get; set; }
    public HumanoidHandsState? HandsState { get; set; }
    public HumanoidHandsActionState? HandsActionState { get; set; }
    public HumanoidHeadState? HeadState { get; set; }
    public Vector3? TargetHeadPosition { get; set; }

    public override string ToString()
    {
        return ToString(0);
    }

    public string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}Begin {nameof(TargetStateOfHumanoidController)}");
        sb.Append(PropertiesToSting(n));
        sb.AppendLine($"{spaces}End {nameof(TargetStateOfHumanoidController)}");
        return sb.ToString();
    }

    public string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        if (HState.HasValue)
        {
            var state = HState.Value;
            sb.AppendLine($"{spaces}{nameof(HState)} = {state}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(HState)} = null");
        }

        if (TargetPosition.HasValue)
        {
            var targetPosition = TargetPosition.Value;
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
        }

        if (VState.HasValue)
        {
            var state = VState.Value;
            sb.AppendLine($"{spaces}{nameof(VState)} = {state}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(VState)} = null");
        }

        if (HandsState.HasValue)
        {
            var state = HandsState.Value;
            sb.AppendLine($"{spaces}{nameof(HandsState)} = {state}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(HandsState)} = null");
        }

        if (HandsActionState.HasValue)
        {
            var state = HandsActionState.Value;
            sb.AppendLine($"{spaces}{nameof(HandsActionState)} = {state}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(HandsActionState)} = null");
        }
        
        if (HeadState.HasValue)
        {
            var state = HeadState.Value;
            sb.AppendLine($"{spaces}{nameof(HeadState)} = {state}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(HeadState)} = null");
        }
        
        if (TargetHeadPosition.HasValue)
        {
            var state = TargetHeadPosition.Value;
            sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = {state}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = null");
        }
        
        return sb.ToString();
    }
}

public class StatesOfHumanoidController : IObjectToString
{
    public HumanoidHState HState = HumanoidHState.Stop;
    public Vector3? TargetPosition;
    public HumanoidVState VState = HumanoidVState.Ground;
    public HumanoidHandsState HandsState = HumanoidHandsState.FreeHands;
    public HumanoidHandsActionState HandsActionState = HumanoidHandsActionState.Empty;
    public HumanoidHeadState HeadState = HumanoidHeadState.LookingForward;
    public Vector3? TargetHeadPosition;

    public StatesOfHumanoidController Clone()
    {
        var result = new StatesOfHumanoidController();
        result.HState = HState;
        result.TargetPosition = TargetPosition;
        result.VState = VState;
        result.HandsState = HandsState;
        result.HandsActionState = HandsActionState;
        result.HeadState = HeadState;
        result.TargetHeadPosition = TargetHeadPosition;
        return result;
    }

    public void Append(StatesOfHumanoidController source)
    {
        HState = source.HState;
        TargetPosition = source.TargetPosition;
        VState = source.VState;
        HandsState = source.HandsState;
        HandsActionState = source.HandsActionState;
        HeadState = source.HeadState;
        TargetHeadPosition = source.TargetHeadPosition;
    }

    public override string ToString()
    {
        return ToString(0);
    }

    public string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}Begin {nameof(StatesOfHumanoidController)}");
        sb.Append(PropertiesToSting(n));
        sb.AppendLine($"{spaces}End {nameof(StatesOfHumanoidController)}");
        return sb.ToString();
    }

    public string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();

        sb.AppendLine($"{spaces}{nameof(HState)} = {HState}");

        if (TargetPosition.HasValue)
        {
            var targetPosition = TargetPosition.Value;
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {targetPosition}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
        }

        sb.AppendLine($"{spaces}{nameof(VState)} = {VState}");
        sb.AppendLine($"{spaces}{nameof(HandsState)} = {HandsState}");
        sb.AppendLine($"{spaces}{nameof(HandsActionState)} = {HandsActionState}");
        sb.AppendLine($"{spaces}{nameof(HeadState)} = {HeadState}");
        if (TargetHeadPosition.HasValue)
        {
            var targetPosition = TargetHeadPosition.Value;
            sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = {targetPosition}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = null");
        }
        
        return sb.ToString();
    }
}

public class BehaviourFlagsOfHumanoidController : IObjectToString
{
    public bool HasRifle;
    public bool Walk;
    public bool IsAim;
    public bool IsDead;

    public void Append(BehaviourFlagsOfHumanoidController source)
    {
        HasRifle = source.HasRifle;
        Walk = source.Walk;
        IsAim = source.IsAim;
        IsDead = source.IsDead;
    }

    public override string ToString()
    {
        return ToString(0);
    }

    public string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}Begin {nameof(BehaviourFlagsOfHumanoidController)}");
        sb.Append(PropertiesToSting(n));
        sb.AppendLine($"{spaces}End {nameof(BehaviourFlagsOfHumanoidController)}");
        return sb.ToString();
    }

    public string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();

        sb.AppendLine($"{spaces}{nameof(HasRifle)} = {HasRifle}");
        sb.AppendLine($"{spaces}{nameof(Walk)} = {Walk}");
        sb.AppendLine($"{spaces}{nameof(IsAim)} = {IsAim}");
        sb.AppendLine($"{spaces}{nameof(IsDead)} = {IsDead}");

        return sb.ToString();
    }
}
#endregion

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

    // Use this for initialization
    void Start () {
        mStates = new StatesOfHumanoidController();
        mBehaviourFlags = new BehaviourFlagsOfHumanoidController();

        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();
        mNavMeshAgent = GetComponent<NavMeshAgent>();

        mRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        mCurrentHeadAngle = 0f;

        ApplyCurrentStates();

        StartCoroutine(Timer());
    }

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
    private Queue<TargetStateOfHumanoidController> mTargetStateQueue = new Queue<TargetStateOfHumanoidController>();

    public void ExecuteAsync(TargetStateOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        //Debug.Log($"EnemyController ExecuteAsync targetState = {targetState}");
#endif

        lock(mLockObj)
        {
            mTargetStateQueue.Enqueue(targetState);

#if UNITY_EDITOR
            //Debug.Log($"EnemyController ExecuteAsync mTargetStateQueue.Count = {mTargetStateQueue.Count}");
#endif
        }
    }

    private IEnumerator Timer()
    {
        lock(mLockObj)
        {
            if (mTargetStateQueue.Count > 0)
            {
                var targetState = mTargetStateQueue.Dequeue();
                Execute(targetState);
            }
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(Timer());
    }

    private void Execute(TargetStateOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        //Debug.Log($"EnemyController Execute targetState = {targetState}");
#endif

        var newState = CreateTargetState(mStates, targetState);

#if UNITY_EDITOR
        //Debug.Log($"EnemyController Execute newState = {newState}");
#endif

        ApplyTargetState(newState);
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
                break;

            case HumanoidHeadState.LookAt:
                mCurrentHeadPosition = mStates.TargetHeadPosition.Value;
                mUseIkAnimation = true;
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

                        mNeedHeadChanges = false;
                    }

                    var radAngle = mCurrentHeadAngle * Mathf.Deg2Rad;
                    var x = Mathf.Sin(radAngle);
                    var y = Mathf.Cos(radAngle);
                    var localDirection = new Vector3(x, 0f, y);
                    var globalDirection = transform.TransformDirection(localDirection);
                    var oldY = Head.position.y;
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
