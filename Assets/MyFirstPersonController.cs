using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[Serializable]
public class MyMouseLook
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    
    private Quaternion mCharacterTargetRot;
    private Quaternion mCameraTargetRot;
    private IUserClientCommonHost mUserClientCommonHost;

    public void Init(Transform character, Transform camera, IUserClientCommonHost userClientCommonHost)
    {
        mCharacterTargetRot = character.localRotation;
        mCameraTargetRot = camera.localRotation;
        mUserClientCommonHost = userClientCommonHost;
        mUserClientCommonHost.SetCharacterMode();
    }

    public void LookRotation(Transform character, Transform camera)
    {
        var yRot = mUserClientCommonHost.GetAxis("Mouse X") * XSensitivity;
        var xRot = mUserClientCommonHost.GetAxis("Mouse Y") * YSensitivity;

        mCharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
        mCameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
        {
            mCameraTargetRot = ClampRotationAroundXAxis(mCameraTargetRot);
        }

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, mCharacterTargetRot, smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, mCameraTargetRot, smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = mCharacterTargetRot;
            camera.localRotation = mCameraTargetRot;
        }
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        var angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MyFirstPersonController : MonoBehaviour {
    [Serializable]
    public class MyMovementSettings
    {
        public float ForwardSpeed = 8.0f;   // Speed when walking forward
        public float BackwardSpeed = 4.0f;  // Speed when walking backwards
        public float StrafeSpeed = 4.0f;    // Speed when walking sideways
        public float RunMultiplier = 2.0f;   // Speed when sprinting
        public KeyCode RunKey = KeyCode.LeftShift;
        public float JumpForce = 30f;
        public AnimationCurve SlopeCurveModifier { get; set; } = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
        [HideInInspector] public float CurrentTargetSpeed = 8f;

#if !MOBILE_INPUT
        private bool m_Running;
#endif

        public void UpdateDesiredTargetSpeed(Vector2 input)
        {
            if (input == Vector2.zero) return;
            if (input.x > 0 || input.x < 0)
            {
                //strafe
                CurrentTargetSpeed = StrafeSpeed;
            }
            if (input.y < 0)
            {
                //backwards
                CurrentTargetSpeed = BackwardSpeed;
            }
            if (input.y > 0)
            {
                //forwards
                //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                CurrentTargetSpeed = ForwardSpeed;
            }
#if !MOBILE_INPUT
            if (Input.GetKey(RunKey))
            {
                CurrentTargetSpeed *= RunMultiplier;
                m_Running = true;
            }
            else
            {
                m_Running = false;
            }
#endif
        }

#if !MOBILE_INPUT
        public bool Running
        {
            get { return m_Running; }
        }
#endif
    }

    [Serializable]
    public class MyAdvancedSettings
    {
        public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
        public float stickToGroundHelperDistance = 0.5f; // stops the character
        public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
        public bool airControl; // can the user control the direction that is being moved in the air
        [Tooltip("set it to 0.1 or more if you get stuck in wall")]
        public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
    }

    public Camera cam;
    public MyMovementSettings movementSettings = new MyMovementSettings();
    public MyMouseLook mouseLook = new MyMouseLook();
    public MyAdvancedSettings advancedSettings = new MyAdvancedSettings();

    private Rigidbody mRigidBody;
    private CapsuleCollider mCapsule;
    private float mYRotation;
    private Vector3 mGroundContactNormal { get; set; }
    private bool mJump, mPreviouslyGrounded, mJumping, mIsGrounded;
    public Vector3 Velocity
    {
        get { return mRigidBody.velocity; }
    }

    public bool Grounded
    {
        get { return mIsGrounded; }
    }

    public bool Jumping
    {
        get { return mJumping; }
    }

    public bool Running
    {
        get
        {
#if !MOBILE_INPUT
            return movementSettings.Running;
#else
	            return false;
#endif
        }
    }

    private void Start()
    {
        mUserClientCommonHost = UserClientCommonHostFactory.Get();
        mRigidBody = GetComponent<Rigidbody>();
        mCapsule = GetComponent<CapsuleCollider>();
        mouseLook.Init(transform, cam.transform, mUserClientCommonHost);
    }

    private IUserClientCommonHost mUserClientCommonHost;

    private void Update()
    {
        RotateView();

        if (CrossPlatformInputManager.GetButtonDown("Jump") && !mJump)
        {
            mJump = true;
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        var input = GetInput();

        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || mIsGrounded))
        {
            // always move along the camera forward as it is the direction that it being aimed at
            var desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
            desiredMove = Vector3.ProjectOnPlane(desiredMove, mGroundContactNormal).normalized;

            desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
            desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
            desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;
            if (mRigidBody.velocity.sqrMagnitude < (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed))
            {
                mRigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
            }
        }

        if (mIsGrounded)
        {
            mRigidBody.drag = 5f;

            if (mJump)
            {
                mRigidBody.drag = 0f;
                mRigidBody.velocity = new Vector3(mRigidBody.velocity.x, 0f, mRigidBody.velocity.z);
                mRigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                mJumping = true;
            }

            if (!mJumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && mRigidBody.velocity.magnitude < 1f)
            {
                mRigidBody.Sleep();
            }
        }
        else
        {
            mRigidBody.drag = 0f;
            if (mPreviouslyGrounded && !mJumping)
            {
                StickToGroundHelper();
            }
        }
        mJump = false;
    }

    private float SlopeMultiplier()
    {
        float angle = Vector3.Angle(mGroundContactNormal, Vector3.up);
        return movementSettings.SlopeCurveModifier.Evaluate(angle);
    }

    private void StickToGroundHelper()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, mCapsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                               ((mCapsule.height / 2f) - mCapsule.radius) +
                               advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
            {
                mRigidBody.velocity = Vector3.ProjectOnPlane(mRigidBody.velocity, hitInfo.normal);
            }
        }
    }

    private Vector2 GetInput()
    {
        var input = new Vector2
        {
            x = mUserClientCommonHost.GetAxis("Horizontal"),
            y = mUserClientCommonHost.GetAxis("Vertical")
        };
        movementSettings.UpdateDesiredTargetSpeed(input);
        return input;
    }

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon)
        {
            return;
        }

        // get the rotation before it's changed
        var oldYRotation = transform.eulerAngles.y;

        mouseLook.LookRotation(transform, cam.transform);

        if (mIsGrounded || advancedSettings.airControl)
        {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            var velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            mRigidBody.velocity = velRotation * mRigidBody.velocity;
        }
    }

    private void GroundCheck()
    {
        mPreviouslyGrounded = mIsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, mCapsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                               ((mCapsule.height / 2f) - mCapsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            mIsGrounded = true;
            mGroundContactNormal = hitInfo.normal;
        }
        else
        {
            mIsGrounded = false;
            mGroundContactNormal = Vector3.up;
        }
        if (!mPreviouslyGrounded && mIsGrounded && mJumping)
        {
            mJumping = false;
        }
    }
}
