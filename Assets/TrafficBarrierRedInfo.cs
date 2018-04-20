using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficBarrierRedInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var gameInfo = MyGameObjectFactory.CreateByComponent(this);

#if UNITY_EDITOR
        Debug.Log($"TrafficBarrierRedInfo Start gameInfo = {gameInfo}");
#endif

        MyGameObjectsBus.RegisterObject(gameInfo);
    }
}
