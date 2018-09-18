using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MyCurveControlledBob
{
    public float HorizontalBobRange = 0.33f;
    public float VerticalBobRange = 0.33f;
    public AnimationCurve Bobcurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f),
                                                        new Keyframe(1f, 0f), new Keyframe(1.5f, -1f),
                                                        new Keyframe(2f, 0f)); // sin curve for head bob
    public float VerticaltoHorizontalRatio = 1f;

    private float mCyclePositionX;
    private float mCyclePositionY;
    private float mBobBaseInterval;
    private Vector3 mOriginalCameraPosition;
    private float mTime;

    public void Setup(Camera camera, float bobBaseInterval)
    {
        mBobBaseInterval = bobBaseInterval;
        mOriginalCameraPosition = camera.transform.localPosition;

        // get the length of the curve in time
        mTime = Bobcurve[Bobcurve.length - 1].time;
    }


    public Vector3 DoHeadBob(float speed)
    {
        float xPos = mOriginalCameraPosition.x + (Bobcurve.Evaluate(mCyclePositionX) * HorizontalBobRange);
        float yPos = mOriginalCameraPosition.y + (Bobcurve.Evaluate(mCyclePositionY) * VerticalBobRange);

        mCyclePositionX += (speed * Time.deltaTime) / mBobBaseInterval;
        mCyclePositionY += ((speed * Time.deltaTime) / mBobBaseInterval) * VerticaltoHorizontalRatio;

        if (mCyclePositionX > mTime)
        {
            mCyclePositionX = mCyclePositionX - mTime;
        }
        if (mCyclePositionY > mTime)
        {
            mCyclePositionY = mCyclePositionY - mTime;
        }

        return new Vector3(xPos, yPos, 0f);
    }
}

public class MyHeadBob : MonoBehaviour {
    public Camera Camera;
    public MyCurveControlledBob motionBob = new MyCurveControlledBob();
    public MyLerpControlledBob jumpAndLandingBob = new MyLerpControlledBob();
    public MyFirstPersonController rigidbodyFirstPersonController;
    public float StrideInterval;
    [Range(0f, 1f)] public float RunningStrideLengthen;

    private bool mPreviouslyGrounded;
    private Vector3 mOriginalCameraPosition;

    private void Start()
    {
        motionBob.Setup(Camera, StrideInterval);
        mOriginalCameraPosition = Camera.transform.localPosition;
    }

    private void Update()
    {
        Vector3 newCameraPosition;
        if (rigidbodyFirstPersonController.Velocity.magnitude > 0 && rigidbodyFirstPersonController.Grounded)
        {
            Camera.transform.localPosition = motionBob.DoHeadBob(rigidbodyFirstPersonController.Velocity.magnitude * (rigidbodyFirstPersonController.Running ? RunningStrideLengthen : 1f));
            newCameraPosition = Camera.transform.localPosition;
            newCameraPosition.y = Camera.transform.localPosition.y - jumpAndLandingBob.Offset();
        }
        else
        {
            newCameraPosition = Camera.transform.localPosition;
            newCameraPosition.y = mOriginalCameraPosition.y - jumpAndLandingBob.Offset();
        }
        Camera.transform.localPosition = newCameraPosition;

        if (!mPreviouslyGrounded && rigidbodyFirstPersonController.Grounded)
        {
            StartCoroutine(jumpAndLandingBob.DoBobCycle());
        }

        mPreviouslyGrounded = rigidbodyFirstPersonController.Grounded;
    }
}
