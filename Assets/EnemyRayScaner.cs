using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRayScaner : MonoBehaviour {
    public int distance = 15;
    public Vector3 Offset = new Vector3(0, 1.6f, 0);

    public int HRaysCount = 6;
    public int VRaysCount = 6;
    public int Distance = 20;
    public int Angle = 20;

    private List<Vector3> mRayDirectionsList = new List<Vector3>();

	// Use this for initialization
	void Start () {
        var dz = 0f;

        for (var n = 0; n < VRaysCount; n++)
        {
            var j = 0f;

            var z = Mathf.Sin(dz);

            dz += Angle * Mathf.Deg2Rad / VRaysCount;

            for (var i = 0; i < HRaysCount; i++)
            {
                var x = Mathf.Sin(j);
                var y = Mathf.Cos(j);

                j += Angle * Mathf.Deg2Rad / HRaysCount;

                var targetDirection = new Vector3(x, z, y);
                mRayDirectionsList.Add(targetDirection);

                if (z != 0)
                {
                    targetDirection = new Vector3(x, -z, y);
                    mRayDirectionsList.Add(targetDirection);
                }

                if (x != 0)
                {
                    targetDirection = new Vector3(-x, z, y);
                    mRayDirectionsList.Add(targetDirection);

                    if (z != 0)
                    {
                        targetDirection = new Vector3(-x, -z, y);
                        mRayDirectionsList.Add(targetDirection);
                    }
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        RayToScan();
	}

    private void RayToScan()
    {
        foreach(var vector in mRayDirectionsList)
        {
            var dir = transform.TransformDirection(vector);
            GetRaycast(dir);
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
