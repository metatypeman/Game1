using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyHState
{
    Stop,
    Walk,
    Run
}

public enum EnemyVState
{
    Ground,
    Jump,
    Crouch
}

public enum EnemyHandsState
{
    FreeHands,
    HasRifle
}

public class EnemyController : MonoBehaviour {
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

    private EnemyHState mHState = EnemyHState.Stop;

    public EnemyHState HState
    {
        get
        {
            return mHState;
        }

        set
        {
            if(mHState == value)
            {
                return;
            }

            mHState = value;

            UpdateAnimator();
        }
    }

    private EnemyVState mVState = EnemyVState.Ground;

    public EnemyVState VState
    {
        get
        {
            return mVState;
        }

        set
        {
            if(mVState == value)
            {
                return;
            }

            mVState = value;

            UpdateAnimator();
        }
    }


    private EnemyHandsState mHandsState = EnemyHandsState.HasRifle;

    public EnemyHandsState HandsState
    {
        get
        {
            return mHandsState;
        }

        set
        {
            if(mHandsState == value)
            {
                return;
            }

            mHandsState = value;

            UpdateAnimator();
        }
    }

    public void Move(Vector3 targetPosition)
    {
#if UNITY_EDITOR
        Debug.Log("EnemyController Move targetPosition = " + targetPosition);
#endif
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        //        if (targetPosition.magnitude > 1f)
        //        {
        //            targetPosition.Normalize();
        //        }

        //#if UNITY_EDITOR
        //        Debug.Log("EnemyController Move NEXT targetPosition = " + targetPosition);
        //#endif

        //        targetPosition = transform.InverseTransformDirection(targetPosition);

        //#if UNITY_EDITOR
        //        Debug.Log("EnemyController Move NEXT (1) targetPosition = " + targetPosition);
        //#endif

        //        CheckGroundStatus();
        //        targetPosition = Vector3.ProjectOnPlane(targetPosition, m_GroundNormal);

        //#if UNITY_EDITOR
        //        Debug.Log("EnemyController Move NEXT (2) targetPosition = " + targetPosition);
        //#endif

        //        m_TurnAmount = Mathf.Atan2(targetPosition.x, targetPosition.z);
        //        m_ForwardAmount = targetPosition.z;

        //ApplyExtraTurnRotation();

        mHState = EnemyHState.Walk;

        UpdateAnimator();

        //_navMeshAgent.Move(targetPosition);
        _navMeshAgent.SetDestination(targetPosition);
    }

    public void Stop()
    {
        _navMeshAgent.ResetPath();
    }

    private void UpdateAnimator()
    {
#if UNITY_EDITOR
        //Debug.Log("EnemyController UpdateAnimator");
#endif
        var hasRifle = false;
        var walk = false;

        switch (mHandsState)
        {
            case EnemyHandsState.FreeHands:
                hasRifle = false;
                break;

            case EnemyHandsState.HasRifle:
                hasRifle = true;
                break;
        }

        switch(mHState)
        {
            case EnemyHState.Stop:
                walk = false;
                break;

            case EnemyHState.Walk:
                walk = true;
                break;
        }

        m_Animator.SetBool("hasRifle", hasRifle);
        m_Animator.SetBool("walk", walk);
    }

    //void ApplyExtraTurnRotation()
    //{
    //    // help the character turn faster (this is in addition to root rotation in the animation)
    //    var turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
    //    transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    //}

//    void CheckGroundStatus()
//    {
//        RaycastHit hitInfo;
//#if UNITY_EDITOR
//        // helper to visualise the ground check ray in the scene view
//        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
//#endif
//        // 0.1f is a small offset to start the ray from inside the character
//        // it is also good to note that the transform position in the sample assets is at the base of the character
//        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
//        {
//            m_GroundNormal = hitInfo.normal;
//            m_IsGrounded = true;
//            m_Animator.applyRootMotion = true;
//        }
//        else
//        {
//            m_IsGrounded = false;
//            m_GroundNormal = Vector3.up;
//            m_Animator.applyRootMotion = false;
//        }
//    }

    //public void OnAnimatorMove()
    //{
    //    // we implement this function to override the default root motion.
    //    // this allows us to modify the positional speed before it's applied.
    //    if (m_IsGrounded && Time.deltaTime > 0)
    //    {
    //        var v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

    //        // we preserve the existing y part of the current velocity.
    //        v.y = m_Rigidbody.velocity.y;
    //        m_Rigidbody.velocity = v;
    //    }
    //}

    // Update is called once per frame
    void Update () {
        //mAnim.SetLookAtPosition(mTargetCube.transform.position);
        //Debug.Log("EnemyController Update");
    }
}
