using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeZero1_WaypointInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var waypointInfo = new WaypointInfo();
        waypointInfo.Position = transform.position;
        waypointInfo.Name = transform.name;
        waypointInfo.Tags.Add("zero cubes");

        WaypointsBus.RegisterWaypoint(waypointInfo);
    }
}
