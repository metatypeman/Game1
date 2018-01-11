using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyNPC : MonoBehaviour {
    private EnemyController mEnemyController;
    GameObject mTargetCube;
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;

    // Use this for initialization
    void Start () {
        mEnemyController = GetComponent<EnemyController>();
        mTargetCube = GameObject.Find("Cube_1");
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("EnemyController Update");
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        Debug.Log("EnemyController FixedUpdate");

        //var crouch = Input.GetKey(KeyCode.C);

        var goAhead = Input.GetKey(KeyCode.W);

        if(goAhead)
        {
            mEnemyController.HState = EnemyHState.Walk;
        }
        else
        {
            var goToTarget = Input.GetKey(KeyCode.P);

            if(goToTarget)
            {
                mEnemyController.HState = EnemyHState.Walk;
                mEnemyController.Move(mTargetCube.transform.position);
            }
            else
            {
                mEnemyController.HState = EnemyHState.Stop;
            }
        }   
    }
}
