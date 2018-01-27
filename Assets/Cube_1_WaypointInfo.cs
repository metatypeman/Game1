using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Cube_1_WaypointInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var waypointInfo = new WaypointInfo();
        waypointInfo.Position = transform.position;
        waypointInfo.Name = transform.name;
        waypointInfo.Tags.Add("our military base");

        WaypointsBus.RegisterWaypoint(waypointInfo);
    }
}
