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
    Run
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
    Empty
}

public enum MoveHumanoidCommandKind
{
    HState,
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
        if(Commands == null)
        {
            sb.Append(spaces);
            sb.AppendLine($"Commands == null");
        }
        else
        {
            sb.Append(spaces);
            sb.Append("Commands.Count = ");
            sb.AppendLine(Commands.Count.ToString());
            sb.Append(spaces);
            sb.AppendLine("Begin Commands");
            foreach(var command in Commands)
            {
                sb.Append(command.ToString(nextN));
            }
            sb.Append(spaces);
            sb.AppendLine("End Commands");
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
        sb.Append(spaces);
        sb.Append("Kind = ");
        sb.AppendLine(Kind.ToString());

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
        sb.Append(spaces);
        sb.AppendLine("Begin HumanoidHStateCommand");
        sb.Append(PropertiesToSting(n));
        sb.Append(spaces);
        sb.AppendLine("End HumanoidHStateCommand");
        return sb.ToString();
    }

    public override string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(base.PropertiesToSting(n));
        sb.Append(spaces);
        sb.Append("State = ");
        sb.AppendLine(State.ToString());
        if(TargetPosition.HasValue)
        {
            var targetPosition = TargetPosition.Value;

            sb.Append(spaces);
            sb.Append("TargetPosition = ");
            sb.AppendLine(TargetPosition.ToString());
        }
        else
        {
            sb.Append(spaces);
            sb.AppendLine("TargetPosition = null");
        }
        return sb.ToString();
    }
}

public class HumanoidVStateCommand: MoveHumanoidCommand, IHumanoidVStateCommand
{
    public override MoveHumanoidCommandKind Kind
    {
        get
        {
            return MoveHumanoidCommandKind.VState;
        }
    }

    public HumanoidVState State { get; set; }

    public override string ToString()
    {
        return ToString(0);
    }

    public override string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(spaces);
        sb.AppendLine("Begin HumanoidVStateCommand");
        sb.Append(PropertiesToSting(n));
        sb.Append(spaces);
        sb.AppendLine("End HumanoidVStateCommand");
        return sb.ToString();
    }

    public override string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(base.PropertiesToSting(n));
        sb.Append(spaces);
        sb.Append("State = ");
        sb.AppendLine(State.ToString());
        return sb.ToString();
    }
}

public class HumanoidHandsStateCommand: MoveHumanoidCommand, IHumanoidHandsStateCommand
{
    public override MoveHumanoidCommandKind Kind
    {
        get
        {
            return MoveHumanoidCommandKind.HandsState;
        }
    }

    public HumanoidHandsState State { get; set; }

    public override string ToString()
    {
        return ToString(0);
    }

    public override string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(spaces);
        sb.AppendLine("Begin HumanoidHandsStateCommand");
        sb.Append(PropertiesToSting(n));
        sb.Append(spaces);
        sb.AppendLine("End HumanoidHandsStateCommand");
        return sb.ToString();
    }

    public override string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(base.PropertiesToSting(n));
        sb.Append(spaces);
        sb.Append("State = ");
        sb.AppendLine(State.ToString());
        return sb.ToString();
    }
}

public class HumanoidHandsActionStateCommand: MoveHumanoidCommand, IHumanoidHandsActionStateCommand
{
    public override MoveHumanoidCommandKind Kind
    {
        get
        {
            return MoveHumanoidCommandKind.HandsActionState;
        }
    }

    public HumanoidHandsActionState State { get; set; }

    public override string ToString()
    {
        return ToString(0);
    }

    public override string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(spaces);
        sb.AppendLine("Begin HumanoidHandsActionStateCommand");
        sb.Append(PropertiesToSting(n));
        sb.Append(spaces);
        sb.AppendLine("End HumanoidHandsActionStateCommand");
        return sb.ToString();
    }

    public override string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(base.PropertiesToSting(n));
        sb.Append(spaces);
        sb.Append("State = ");
        sb.AppendLine(State.ToString());
        return sb.ToString();
    }
}
//----

public interface IMoveHumanoidController
{
    void Execute(IMoveHumanoidCommand command);
    void Execute(IMoveHumanoidCommandsPackage package);
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
        sb.Append(spaces);
        sb.AppendLine("Begin TargetStateOfHumanoidController");
        sb.Append(PropertiesToSting(n));
        sb.Append(spaces);
        sb.AppendLine("End TargetStateOfHumanoidController");
        return sb.ToString();
    }

    public string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        if (HState.HasValue)
        {
            var state = HState.Value;

            sb.Append(spaces);
            sb.Append("HState = ");
            sb.AppendLine(state.ToString());
        }
        else
        {
            sb.Append(spaces);
            sb.AppendLine("HState = null");
        }

        if (TargetPosition.HasValue)
        {
            var targetPosition = TargetPosition.Value;

            sb.Append(spaces);
            sb.Append("TargetPosition = ");
            sb.AppendLine(TargetPosition.ToString());
        }
        else
        {
            sb.Append(spaces);
            sb.AppendLine("TargetPosition = null");
        }

        if (VState.HasValue)
        {
            var state = VState.Value;

            sb.Append(spaces);
            sb.Append("VState = ");
            sb.AppendLine(state.ToString());
        }
        else
        {
            sb.Append(spaces);
            sb.AppendLine("VState = null");
        }

        if (HandsState.HasValue)
        {
            var state = HandsState.Value;

            sb.Append(spaces);
            sb.Append("HandsState = ");
            sb.AppendLine(state.ToString());
        }
        else
        {
            sb.Append(spaces);
            sb.AppendLine("HandsState = null");
        }

        if (HandsActionState.HasValue)
        {
            var state = HandsActionState.Value;

            sb.Append(spaces);
            sb.Append("HandsActionState = ");
            sb.AppendLine(state.ToString());
        }
        else
        {
            sb.Append(spaces);
            sb.AppendLine("HandsActionState = null");
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
        sb.Append(spaces);
        sb.AppendLine("Begin StatesOfHumanoidController");
        sb.Append(PropertiesToSting(n));
        sb.Append(spaces);
        sb.AppendLine("End StatesOfHumanoidController");
        return sb.ToString();
    }

    public string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();

        sb.Append(spaces);
        sb.Append("HState = ");
        sb.AppendLine(HState.ToString());

        if (TargetPosition.HasValue)
        {
            var targetPosition = TargetPosition.Value;

            sb.Append(spaces);
            sb.Append("TargetPosition = ");
            sb.AppendLine(TargetPosition.ToString());
        }
        else
        {
            sb.Append(spaces);
            sb.AppendLine("TargetPosition = null");
        }

        sb.Append(spaces);
        sb.Append("VState = ");
        sb.AppendLine(VState.ToString());

        sb.Append(spaces);
        sb.Append("HandsState = ");
        sb.AppendLine(HandsState.ToString());

        sb.Append(spaces);
        sb.Append("HandsActionState = ");
        sb.AppendLine(HandsActionState.ToString());

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
        sb.Append(spaces);
        sb.AppendLine("Begin BehaviourFlagsOfHumanoidController");
        sb.Append(PropertiesToSting(n));
        sb.Append(spaces);
        sb.AppendLine("End BehaviourFlagsOfHumanoidController");
        return sb.ToString();
    }

    public string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();

        sb.Append(spaces);
        sb.Append("HasRifle = ");
        sb.AppendLine(HasRifle.ToString());

        sb.Append(spaces);
        sb.Append("Walk = ");
        sb.AppendLine(Walk.ToString());

        sb.Append(spaces);
        sb.Append("IsAim = ");
        sb.AppendLine(IsAim.ToString());

        sb.Append(spaces);
        sb.Append("IsDead = ");
        sb.AppendLine(IsDead.ToString());

        return sb.ToString();
    }
}

public class EnemyController : MonoBehaviour, IMoveHumanoidController
{
    [SerializeField] float m_MoveSpeedMultiplier = 1f;
    [SerializeField] float m_GroundCheckDistance = 0.3f;
    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 180;

    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private CapsuleCollider m_Capsule;

    private float m_CapsuleHeight;
    private Vector3 m_CapsuleCenter;
    private float m_OrigGroundCheckDistance;
    Vector3 m_GroundNormal;
    bool m_IsGrounded;
    float m_TurnAmount;
    float m_ForwardAmount;

    private NavMeshAgent mNavMeshAgent;

    // Use this for initialization
    void Start () {
        mStates = new StatesOfHumanoidController();
        mBehaviourFlags = new BehaviourFlagsOfHumanoidController();

        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        mNavMeshAgent = GetComponent<NavMeshAgent>();

        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;

        ApplyCurrentStates();
    }

    private StatesOfHumanoidController mStates;
    private BehaviourFlagsOfHumanoidController mBehaviourFlags;

    public HumanoidHState HState
    {
        get
        {
            return mStates.HState;
        }
    }

    public Vector3? TargetPosition
    {
        get
        {
            return mStates.TargetPosition;
        }
    }

    public HumanoidVState VState
    {
        get
        {
            return mStates.VState;
        }
    }

    public HumanoidHandsState HandsState
    {
        get
        {
            return mStates.HandsState;
        }
    }

    public HumanoidHandsActionState HandsActionState
    {
        get
        {
            return mStates.HandsActionState;
        }
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

    public void Execute(IMoveHumanoidCommand command)
    {
        var commandsList = new MoveHumanoidCommandsPackage();
        commandsList.Commands.Add(command);
        Execute(commandsList);
    }

    public void Execute(IMoveHumanoidCommandsPackage package)
    {
#if UNITY_EDITOR
        Debug.Log("EnemyController Execute package = " + package);
#endif

        var targetState = CreateTargetState(package);

#if UNITY_EDITOR
        Debug.Log("EnemyController Execute targetState = " + targetState);
#endif

        var newState = CreateTargetState(mStates, targetState);

#if UNITY_EDITOR
        Debug.Log("EnemyController Execute newState = " + newState);
#endif

        ApplyTargetState(newState);
    }

    private TargetStateOfHumanoidController CreateTargetState(IMoveHumanoidCommandsPackage package)
    {
#if UNITY_EDITOR
        Debug.Log("EnemyController CreateTargetState package = " + package);
#endif

        var result = new TargetStateOfHumanoidController();

        var commandsList = package.Commands;

        if(commandsList.Count == 0)
        {
            return result;
        }

        var hStateCommandsList = new List<IHumanoidHStateCommand>();
        var vStateCommandsList = new List<IHumanoidVStateCommand>();
        var handsStateCommandsList = new List<IHumanoidHandsStateCommand>();
        var handsActionStateCommandsList = new List<IHumanoidHandsActionStateCommand>();

        foreach (var command in commandsList)
        {
            var kind = command.Kind;

            switch (kind)
            {
                case MoveHumanoidCommandKind.HState:
                    hStateCommandsList.Add(command as IHumanoidHStateCommand);
                    break;

                case MoveHumanoidCommandKind.VState:
                    vStateCommandsList.Add(command as IHumanoidVStateCommand);
                    break;

                case MoveHumanoidCommandKind.HandsState:
                    handsStateCommandsList.Add(command as IHumanoidHandsStateCommand);
                    break;

                case MoveHumanoidCommandKind.HandsActionState:
                    handsActionStateCommandsList.Add(command as IHumanoidHandsActionStateCommand);
                    break;

                default: throw new ArgumentOutOfRangeException("kind", kind, null);
            }
        }

#if UNITY_EDITOR
        Debug.Log("EnemyController Execute hStateCommandsList.Count = " + hStateCommandsList.Count);
        Debug.Log("EnemyController Execute vStateCommandsList.Count = " + vStateCommandsList.Count);
        Debug.Log("EnemyController Execute handsStateCommandsList.Count = " + handsStateCommandsList.Count);
        Debug.Log("EnemyController Execute handsActionStateCommandsList.Count = " + handsActionStateCommandsList.Count);
#endif

        if(hStateCommandsList.Count > 0)
        {
            var targetCommand = hStateCommandsList.First();

#if UNITY_EDITOR
            Debug.Log("EnemyController CreateTargetState targetCommand = " + targetCommand);
#endif

            result.HState = targetCommand.State;
            result.TargetPosition = targetCommand.TargetPosition;
        }

        if (vStateCommandsList.Count > 0)
        {
            var targetCommand = vStateCommandsList.First();

#if UNITY_EDITOR
            Debug.Log("EnemyController CreateTargetState targetCommand = " + targetCommand);
#endif

            result.VState = targetCommand.State;
        }

        if (handsStateCommandsList.Count > 0)
        {
            var targetCommand = handsStateCommandsList.First();

#if UNITY_EDITOR
            Debug.Log("EnemyController CreateTargetState targetCommand = " + targetCommand);
#endif

            result.HandsState = targetCommand.State;
        }

        if (handsActionStateCommandsList.Count > 0)
        {
            var targetCommand = handsActionStateCommandsList.First();

#if UNITY_EDITOR
            Debug.Log("EnemyController CreateTargetState targetCommand = " + targetCommand);
#endif

            result.HandsActionState = targetCommand.State;
        }

        return result;
    }

    private StatesOfHumanoidController CreateTargetState(StatesOfHumanoidController sourceState, TargetStateOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        Debug.Log("EnemyController CreateTargetState sourceState = " + sourceState);
        Debug.Log("EnemyController CreateTargetState targetState = " + targetState);
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
                if (!result.TargetPosition.HasValue)
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

    private void ApplyTargetState(StatesOfHumanoidController targetState)
    {
#if UNITY_EDITOR
        Debug.Log("EnemyController ApplyTargetState targetState = " + targetState);
#endif

        var targetBehaviourFlags = CreateBehaviourFlags(targetState);

#if UNITY_EDITOR
        Debug.Log("EnemyController ApplyTargetState targetBehaviourFlags = " + targetBehaviourFlags);
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

    private void UpdateAnimator()
    {
#if UNITY_EDITOR
        //Debug.Log("EnemyController UpdateAnimator");
#endif
        m_Animator.SetBool("hasRifle", mBehaviourFlags.HasRifle);
        m_Animator.SetBool("walk", mBehaviourFlags.Walk);
        m_Animator.SetBool("isAim", mBehaviourFlags.IsAim);
        m_Animator.SetBool("isDead", mBehaviourFlags.IsDead);
    }

    public void ApplyAchieveDestinationOfMoving()
    {
        mStates.HState = HumanoidHState.Stop;
        mStates.TargetPosition = null;

        ApplyCurrentStates();

        Task.Run(() => {
            OnAchieveDestinationOfMoving?.Invoke();
        });
    }

    // Update is called once per frame
    void Update () {
#if UNITY_EDITOR
        //Debug.Log("EnemyController Update mNavMeshAgent.pathStatus = " + mNavMeshAgent.pathStatus + " mNavMeshAgent.isOnOffMeshLink = " + mNavMeshAgent.isOnOffMeshLink + " mNavMeshAgent.isStopped = " + mNavMeshAgent.isStopped + " mNavMeshAgent.nextPosition = " + mNavMeshAgent.nextPosition);
        //Debug.Log("EnemyController Update = " + mNavMeshAgent);
        //Debug.Log("EnemyController Update = " + mNavMeshAgent);
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

    public Action OnAchieveDestinationOfMoving;
}
