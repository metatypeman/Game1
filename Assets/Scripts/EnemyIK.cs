using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIK : MonoBehaviour
{
    public Transform Head;
    public Transform Left;
    public Transform Right;

    public bool isIkActive;

    Animator mAnim;
    
    // Use this for initialization
    void Start () {
        mAnim = GetComponent<Animator>();
        //transform.LookAt(mTargetCube.transform);
        //transform.
    }
	
	// Update is called once per frame
	void Update () {
        //LogInstance.Log("Begin");
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!mAnim)
            return;

        if (isIkActive)
        {
            if (!Head)
                return;

            //mAnim.SetLookAtWeight(1);
            //mAnim.SetLookAtPosition(Target.position);
            //Head.LookAt(Target);

            if (!Left)
                return;
            mAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            mAnim.SetIKPosition(AvatarIKGoal.RightHand, Right.position);

            mAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.3f);
            mAnim.SetIKPosition(AvatarIKGoal.LeftHand, Left.position);
        }

        //if (isGroup)
        //{
        //    Left.SetParent(Head);
        //    Right.SetParent(Head);
        //}
    }
}
