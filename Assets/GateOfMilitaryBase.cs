using MyNPCLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOfMilitaryBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
        mClosedZPosition = transform.position.z;
        mOpenedZPosition = mClosedZPosition + 8;
    }

    private float mClosedZPosition;
    private float mOpenedZPosition;
    private bool mIsOpended;
    private bool mIsProcessing;

	// Update is called once per frame
	void Update () {
        if(mIsProcessing)
        {
            mIsProcessing = false;

            if(mIsOpended)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, mOpenedZPosition);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, mClosedZPosition);
            }
        }
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 30);
    }

    public void Open()
    {
        LogInstance.Log("Begin");

        mIsOpended = true;
        mIsProcessing = true;
    }

    public void Close()
    {
        LogInstance.Log("Begin");

        mIsOpended = false;
        mIsProcessing = true;

        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 30);
    }
}
