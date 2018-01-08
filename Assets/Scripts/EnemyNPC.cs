using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyNPC : MonoBehaviour {
    private EnemyController mEnemyController;

    // Use this for initialization
    void Start () {
        mEnemyController = GetComponent<EnemyController>();
    }
	
	// Update is called once per frame
	void Update () {
        Debug.LogWarning("EnemyController Update");
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        Debug.LogWarning("EnemyController FixedUpdate");

        //var crouch = Input.GetKey(KeyCode.C);

        var goAhead = Input.GetKey(KeyCode.W);

        if(goAhead)
        {
            mEnemyController.HState = EnemyHState.Walk;
        }
        else
        {
            mEnemyController.HState = EnemyHState.Stop;
        }
    }
}
