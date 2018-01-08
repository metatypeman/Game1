using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] float m_GroundCheckDistance = 0.3f;

    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private CapsuleCollider m_Capsule;

    private float m_CapsuleHeight;
    private Vector3 m_CapsuleCenter;
    private float m_OrigGroundCheckDistance;

    // Use this for initialization
    void Start () {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
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

    public void Move()
    {

    }

    private void UpdateAnimator()
    {
        Debug.LogWarning("EnemyController UpdateAnimator");

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


    public void OnAnimatorMove()
    {

    }

    // Update is called once per frame
    void Update () {
        //mAnim.SetLookAtPosition(mTargetCube.transform.position);
        Debug.LogWarning("EnemyController Update");
    }
}
