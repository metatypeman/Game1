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
    GameObject mTargetCube;

    // Use this for initialization
    void Start () {
        mAnim = GetComponent<Animator>();
        mTargetCube = GameObject.Find("Cube_1");
        //transform.LookAt(mTargetCube.transform);
        //transform.
    }
	
	// Update is called once per frame
	void Update () {
        Debug.LogWarning("Update Update");
        
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (isIkActive)
        {
            Debug.LogWarning("EnemyIK OnAnimatorIK isIkActive");
        }        
    }
}
