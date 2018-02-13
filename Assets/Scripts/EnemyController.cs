using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public static class StringHelper
{
    public static string Spaces(int n)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < n; i++)
        {
            sb.Append(" ");
        }

        return sb.ToString();
    }
}

public enum HumanoidHState
{
    Stop,
    Walk,
    Run,
    LookAt,
    AimAt
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

public enum MoveHumanoidCommandKind
{
    HState,
    VState,
    HandsState,
    HandsActionState
}

public enum HumanoidStateKind
{
    HState,
    TargetPosition,
    VState,
    HandsState,
    HandsActionState
}

public interface IObjectToString
{
    string ToString(int n);
    string PropertiesToSting(int n);
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
//----
public delegate void HumanoidStatesChangedAction(List<HumanoidStateKind> changedStates);

public interface IMoveHumanoidController
{
    void ExecuteAsync(TargetStateOfHumanoidController targetState);
    StatesOfHumanoidController States { get; }
    event HumanoidStatesChangedAction OnHumanoidStatesChanged;
}

public class TargetStateOfHumanoidController : IObjectToString
{
    public HumanoidHState? HState { get; set; }
    public Vector3? TargetPosition { get; set; }
    public HumanoidVState? VState { get; set; }
    public HumanoidHandsState? HandsState { get; set; }
    public HumanoidHandsActionState? HandsActionState { get; set; }

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
        return sb.ToString();
    }
}

public class StatesOfHumanoidController : IObjectToString
{
    public HumanoidHState HState = HumanoidHState.Stop;
    public Vector3? TargetPosition;
    public HumanoidVState VState = HumanoidVState.Ground;
    public HumanoidHandsState HandsState = HumanoidHandsState.HasRifle;
    public HumanoidHandsActionState HandsActionState = HumanoidHandsActionState.Empty;

    public StatesOfHumanoidController Clone()
    {
        var result = new StatesOfHumanoidController();
        result.HState = HState;
        result.TargetPosition = TargetPosition;
        result.VState = VState;
        result.HandsState = HandsState;
        result.HandsActionState = HandsActionState;
        return result;
    }

    public void Append(StatesOfHumanoidController source)
    {
        HState = source.HState;
        TargetPosition = source.TargetPosition;
        VState = source.VState;
        HandsState = source.HandsState;
        HandsActionState = source.HandsActionState;
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
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");
        }
        else
        {
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
        }

        sb.AppendLine($"{spaces}{nameof(VState)} = {VState}");
        sb.AppendLine($"{spaces}{nameof(HandsState)} = {HandsState}");
        sb.AppendLine($"{spaces}{nameof(HandsActionState)} = {HandsActionState}");

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

public enum DeviceKind
{
    Head,
    LeftHand,
    RightHand,
    LeftLeg,
    RightLeg
}

public class EnemyController : MonoBehaviour, IMoveHumanoidController
{
    private Rigidbody mRigidbody;
    private Animator mAnimator;

    private NavMeshAgent mNavMeshAgent;

    // Use this for initialization
    void Start () {
        mStates = new StatesOfHumanoidController();
        mBehaviourFlags = new BehaviourFlagsOfHumanoidController();

        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();
        mNavMeshAgent = GetComponent<NavMeshAgent>();

        mRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

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

        UpdateAnimator();

        if(mStates.TargetPosition.HasValue)
        {
            mNavMeshAgent.SetDestination(mStates.TargetPosition.Value);
        }
    }

    private object mLockObj = new object();
    private Queue<TargetStateOfHumanoidController> mTargetStateQueue = new Queue<TargetStateOfHumanoidController>();

    public void ExecuteAsync(TargetStateOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        Debug.Log($"EnemyController ExecuteAsync targetState = {targetState}");
#endif

        lock(mLockObj)
        {
            mTargetStateQueue.Enqueue(targetState);

#if UNITY_EDITOR
            Debug.Log($"EnemyController ExecuteAsync mTargetStateQueue.Count = {mTargetStateQueue.Count}");
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
        Debug.Log($"EnemyController Execute targetState = {targetState}");
#endif

        var newState = CreateTargetState(mStates, targetState);

#if UNITY_EDITOR
        Debug.Log($"EnemyController Execute newState = {newState}");
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

        switch (result.HState)
        {
            case HumanoidHState.Stop:
                result.TargetPosition = null;
                break;

            case HumanoidHState.Walk:
            case HumanoidHState.Run:
                LookAt,
    
                if (!result.TargetPosition.HasValue)
                {
                    result.HState = HumanoidHState.Stop;
                }
                break;
                
                AimAt
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
                result.Walk = false;
                break;

            case HumanoidHState.Walk:
                result.Walk = true;
                break;
        }

        return result;
    }

    public void Die()
    {
        mBehaviourFlags.IsDead = true;
        UpdateAnimator();
    }

    public void tmpLookAt(Vector3 targetPosition)
    {
        //mNavMeshAgent.Lookat
    }

    private void ApplyTargetState(StatesOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        //Debug.Log("EnemyController ApplyTargetState targetState = " + targetState);
#endif

        var targetBehaviourFlags = CreateBehaviourFlags(targetState);

#if UNITY_EDITOR
        //Debug.Log("EnemyController ApplyTargetState targetBehaviourFlags = " + targetBehaviourFlags);
#endif

        mStates.Append(targetState);
        mBehaviourFlags.Append(targetBehaviourFlags);

        UpdateAnimator();

        if(targetState.TargetPosition.HasValue)
        {
            mNavMeshAgent.ResetPath();
            mNavMeshAgent.SetDestination(targetState.TargetPosition.Value);
        }
    }

    public void TmpAim()
    {
        if (mBehaviourFlags.HasRifle)
        {
            mBehaviourFlags.IsAim = true;
        }

        UpdateAnimator();
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

    // Update is called once per frame
    void Update () {
#if UNITY_EDITOR
        //Debug.Log("EnemyController Update mNavMeshAgent.pathStatus = " + mNavMeshAgent.pathStatus + " mNavMeshAgent.isOnOffMeshLink = " + mNavMeshAgent.isOnOffMeshLink + " mNavMeshAgent.isStopped = " + mNavMeshAgent.isStopped + " mNavMeshAgent.nextPosition = " + mNavMeshAgent.nextPosition);
#endif
        
        if(mStates.HState != HumanoidHState.Stop)
        {
            var targetPosition = mStates.TargetPosition.Value;
            var nextPosition = mNavMeshAgent.nextPosition;
            if (targetPosition.x == nextPosition.x && targetPosition.z == nextPosition.z)
            {
                ApplyAchieveDestinationOfMoving();
            }
        }
    }
}
