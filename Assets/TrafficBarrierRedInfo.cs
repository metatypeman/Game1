using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficBarrierRedInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var gameInfo = MyGameObjectFactory.CreateByComponent(this);
        MyGameObjectsBus.RegisterObject(gameInfo);
    }
}
