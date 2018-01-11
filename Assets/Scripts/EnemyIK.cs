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
        //Debug.Log("Update Update");
        
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (isIkActive)
        {
            //Debug.Log("EnemyIK OnAnimatorIK isIkActive");
        }        
    }
}
