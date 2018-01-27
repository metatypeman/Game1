using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class VisionItem : IObjectToString
{
    public Vector3 LocalDirection { get; set; }
    public Vector3 Point { get; set; }
    public float Distance { get; set; }
    public int InstanceID { get; set; }

    public override string ToString()
    {
        return ToString(0);
    }

    public string ToString(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}Begin {nameof(VisionItem)}");
        sb.Append(PropertiesToSting(n));
        sb.AppendLine($"{spaces}End {nameof(VisionItem)}");
        return sb.ToString();
    }

    public string PropertiesToSting(int n)
    {
        var spaces = StringHelper.Spaces(n);
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}{nameof(LocalDirection)} = {LocalDirection}");
        sb.AppendLine($"{spaces}{nameof(Point)} = {Point}");
        sb.AppendLine($"{spaces}{nameof(Distance)} = {Distance}");
        sb.AppendLine($"{spaces}{nameof(InstanceID)} = {InstanceID}");
        return sb.ToString();
    }
}

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
        mNewVisileItems = new List<VisionItem>();

        foreach(var localDirection in mRayDirectionsList)
        {
            GetRaycast(localDirection);
        }

        lock (mVisibleItemsLockObj)
        {
            mVisibleItems = mNewVisileItems;
        }
    }

    private void GetRaycast(Vector3 localDirection)
    {
        var globalDirection = transform.TransformDirection(localDirection);

        var hit = new RaycastHit();

        var pos = transform.position + Offset;

        if (Physics.Raycast(pos, globalDirection, out hit, distance))
        {
#if UNITY_EDITOR
            Debug.DrawLine(pos, hit.point, Color.blue);
#endif
            var visibleItem = new VisionItem();
            visibleItem.LocalDirection = localDirection;
            visibleItem.Point = hit.point;
            visibleItem.Distance = hit.distance;
            visibleItem.InstanceID = hit.transform.GetInstanceID();
            mNewVisileItems.Add(visibleItem);
        }
#if UNITY_EDITOR
        else
        {
            Debug.DrawRay(pos, globalDirection * distance, Color.red);
        }
#endif
    }

    private List<VisionItem> mVisibleItems = new List<VisionItem>();
    private List<VisionItem> mNewVisileItems;
    private object mVisibleItemsLockObj = new object();

    public List<VisionItem> VisibleItems
    {
        get
        {
            lock(mVisibleItemsLockObj)
            {
                return mVisibleItems.ToList();
            }
        }
    }
}
