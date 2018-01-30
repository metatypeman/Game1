using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficBarrierRedInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var gameInfo = new MyGameObject();
        var tmpTransform = transform;
        gameInfo.InstanceID = tmpTransform.GetInstanceID();
        gameInfo.Name = tmpTransform.name;
        gameInfo.Tag = tmpTransform.tag;

        MyGameObjectsBus.RegisterObject(gameInfo);
    }
}
