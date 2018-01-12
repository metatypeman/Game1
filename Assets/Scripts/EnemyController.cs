using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    public MoveHumanoidCommandsPackage()
    {
        Commands = new List<IMoveHumanoidCommand>();
    }

    public List<IMoveHumanoidCommand> Commands { get; set; }

    public override string ToString()
    {
        return ToString(0);
    }

    public string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.Append(spaces);
        sb.AppendLine("Begin MoveHumanoidCommandsPackage");
        sb.Append(PropertiesToSting(n));
        sb.Append(spaces);
        sb.AppendLine("End MoveHumanoidCommandsPackage");
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
            sb.AppendLine("Commands == null");
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
    public override MoveHumanoidCommandKind Kind
    {
        get
        {
            return MoveHumanoidCommandKind.HState;
        }
    }

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

public class EnemyController : MonoBehaviour, IMoveHumanoidController
{
    private class NewState : IObjectToString
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
            sb.AppendLine("Begin HumanoidHandsActionStateCommand");
            sb.Append(PropertiesToSting(n));
            sb.Append(spaces);
            sb.AppendLine("End HumanoidHandsActionStateCommand");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.Append(spaces);
            sb.Append("State = ");
            sb.AppendLine(State.ToString());
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
            sb.Append("State = ");
            sb.AppendLine(State.ToString());

            sb.Append(spaces);
            sb.Append("State = ");
            sb.AppendLine(State.ToString());

            sb.Append(spaces);
            sb.Append("State = ");
            sb.AppendLine(State.ToString());
            return sb.ToString();
        }
    }

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

    NavMeshAgent _navMeshAgent;

    // Use this for initialization
    void Start () {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;

        UpdateAnimator();
    }

    private HumanoidHState mHState = HumanoidHState.Stop;

    public HumanoidHState HState
    {
        get
        {
            return mHState;
        }
    }

    private Vector3? mTargetPosition;

    public Vector3? TargetPosition
    {
        get
        {
            return mTargetPosition;
        }
    }

    private HumanoidVState mVState = HumanoidVState.Ground;

    public HumanoidVState VState
    {
        get
        {
            return mVState;
        }
    }


    private HumanoidHandsState mHandsState = HumanoidHandsState.HasRifle;

    public HumanoidHandsState HandsState
    {
        get
        {
            return mHandsState;
        }
    }

    private HumanoidHandsActionState mHandsActionState = HumanoidHandsActionState.Empty;

    public HumanoidHandsActionState HandsActionState
    {
        get
        {
            return mHandsActionState;
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

        var commandsList = package.Commands;

        var hStateCommandsList = new List<IHumanoidHStateCommand>();
        var vStateCommandsList = new List<IHumanoidVStateCommand>();
        var handsStateCommandsList = new List<IHumanoidHandsStateCommand>();
        var handsActionStateCommandsList = new List<IHumanoidHandsActionStateCommand>();

        foreach(var command in commandsList)
        {
            var kind = command.Kind;

            switch(kind)
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

        //throw new NotImplementedException();
    }

    /*
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
    */
    //    public void Move(Vector3 targetPosition)
    //    {
    //#if UNITY_EDITOR
    //        Debug.Log("EnemyController Move targetPosition = " + targetPosition);
    //#endif

    //        mHState = HumanoidHState.Walk;

    //        UpdateAnimator();

    //        _navMeshAgent.SetDestination(targetPosition);
    //    }

    //    public void Stop()
    //    {
    //        _navMeshAgent.ResetPath();
    //    }

    private void UpdateAnimator()
    {
#if UNITY_EDITOR
        //Debug.Log("EnemyController UpdateAnimator");
#endif
        var hasRifle = false;
        var walk = false;

        switch (mHandsState)
        {
            case HumanoidHandsState.FreeHands:
                hasRifle = false;
                break;

            case HumanoidHandsState.HasRifle:
                hasRifle = true;
                break;
        }

        switch(mHState)
        {
            case HumanoidHState.Stop:
                walk = false;
                break;

            case HumanoidHState.Walk:
                walk = true;
                break;
        }

        m_Animator.SetBool("hasRifle", hasRifle);
        m_Animator.SetBool("walk", walk);
    }

    // Update is called once per frame
    void Update () {
        //mAnim.SetLookAtPosition(mTargetCube.transform.position);
        //Debug.Log("EnemyController Update");
    }
}
