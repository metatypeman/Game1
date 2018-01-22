using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarWaypoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var waypointInfo = new WaypointInfo();
        waypointInfo.Position = transform.position;
        waypointInfo.Tags.Add("enemy military base");

        WaypointsBus.RegisterWaypoint(waypointInfo);
    }
}
