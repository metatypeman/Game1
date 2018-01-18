using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRayScaner : MonoBehaviour {
    public int rays = 6;
    public int distance = 15;
    public float angle = 20;
    public Vector3 Offset = new Vector3(0, 1.6f, 0);

    public int HRaysCount = 6;
    public int VRaysCount = 6;
    public int Distance = 20;
    public int Angle = 20;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RayToScan();
	}

    private void RayToScan()
    {
        Debug.Log("EnemyRayScaner RayToScan");

        var dz = 0f;

        for(var n = 0; n < rays; n++)
        {
            float j = 0;

            var z = Mathf.Sin(dz);

            dz += angle * Mathf.Deg2Rad / rays;

            for (int i = 0; i < rays; i++)
            {
                var x = Mathf.Sin(j);
                var y = Mathf.Cos(j);

                Debug.Log($"EnemyRayScaner RayToScan j = {j} x = {x} y = {y} z = {z}");

                j += angle * Mathf.Deg2Rad / rays;

                var dir = transform.TransformDirection(new Vector3(x, z, y));

                GetRaycast(dir);

                if(z != 0)
                {
                    dir = transform.TransformDirection(new Vector3(x, -z, y));
                    GetRaycast(dir);
                }

                if (x != 0)
                {
                    dir = transform.TransformDirection(new Vector3(-x, z, y));
                    GetRaycast(dir);

                    if(z != 0)
                    {
                        dir = transform.TransformDirection(new Vector3(-x, -z, y));
                        GetRaycast(dir);
                    }
                }
            }
        }
    }

    private void GetRaycast(Vector3 dir)
    {
        var hit = new RaycastHit();

        var pos = transform.position + Offset;

        if (Physics.Raycast(pos, dir, out hit, distance))
        {
            Debug.DrawLine(pos, hit.point, Color.blue);
        }
        else
        {
            Debug.DrawRay(pos, dir * distance, Color.red);
        }
    }
}
