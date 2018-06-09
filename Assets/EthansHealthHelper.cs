using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HumanoidBodyHost))]
public class EthansHealthHelper : MonoBehaviour, ITargetOfShoot
{
    private HumanoidBodyHost mEnemyController;
    private int Health = 20;
    
    // Use this for initialization
    void Start () {
        mEnemyController = GetComponent<HumanoidBodyHost>();
    }
	
	// Update is called once per frame
	//void Update () {
		
	//}

    public void SetHit(RaycastHit shootHit, int damagePerShot)
    {
#if UNITY_EDITOR
        //LogInstance.Log($"damagePerShot = {damagePerShot}");
#endif

        Health -= damagePerShot;

#if UNITY_EDITOR
        //LogInstance.Log($"Health = {Health}");
#endif

        if (Health <= 0)
        {
            mEnemyController.Die();
        }
    }
}
