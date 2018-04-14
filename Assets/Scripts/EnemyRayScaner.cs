using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyNPCLib;

public class VisionItem : IObjectToString
{
    public Vector3 LocalDirection { get; set; }
    public Vector3 Point { get; set; }
    public float Distance { get; set; }
    public int InstanceID { get; set; }

    public override string ToString()
    {
        return ToString(0u);
    }

    public string ToString(uint n)
    {
        return this.GetDefaultToStringInformation(n);
    }

    public string PropertiesToSting(uint n)
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

public class VisionObject : IObjectToString
{
    public int InstanceID { get; set; }
    public MyGameObject GameObject { get; set; }
    public List<VisionItem> VisionItems { get; set; }

    public override string ToString()
    {
        return ToString(0u);
    }

    public string ToString(uint n)
    {
        return this.GetDefaultToStringInformation(n);
    }

    public string PropertiesToSting(uint n)
    {
        var spaces = StringHelper.Spaces(n);
        var nextN = n + 4;
        var sb = new StringBuilder();
        sb.AppendLine($"{spaces}{nameof(InstanceID)} = {InstanceID}");
        if(GameObject == null)
        {
            sb.AppendLine($"{spaces}{nameof(GameObject)} = null");
        }
        else
        {
            sb.Append($"{spaces}{nameof(GameObject)} = {GameObject.ToString(nextN)}");
        }

        if(VisionItems == null)
        {
            sb.AppendLine($"{spaces}{nameof(VisionItems)} = null");
        }
        else
        {
            sb.AppendLine($"{spaces}Begin {nameof(VisionItems)}");
            foreach(var visionItem in VisionItems)
            {
                sb.Append(visionItem.ToString(nextN));
            }
            sb.AppendLine($"{spaces}End {nameof(VisionItems)}");
        }
        return sb.ToString();
    }
}

public interface INPCRayScaner
{
    List<VisionObject> VisibleObjects { get; }
}

public class EnemyRayScaner : MonoBehaviour, INPCRayScaner
{
    public int distance = 15;
    public Vector3 Offset = new Vector3(0, 1.6f, 0);

    public int HRaysCount = 6;
    public int VRaysCount = 6;
    public int Distance = 20;
    public int Angle = 20;

    public Transform Head;

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
        if(Head == null)
        {
            RayToScan(transform);
        }
        else
        {
            RayToScan(Head);
        }   
	}

    private void RayToScan(Transform targetTransform)
    {
        var tmpVisibleItems = new List<VisionItem>();

        foreach(var localDirection in mRayDirectionsList)
        {
            GetRaycast(targetTransform, localDirection, tmpVisibleItems);
        }

        var newVisibleObjects = new List<VisionObject>();

        if(tmpVisibleItems.Count > 0)
        {
            var tmpGroupedVisibleItems = tmpVisibleItems.GroupBy(p => p.InstanceID).ToDictionary(p => p.Key, p => p.ToList());

            foreach(var tmpGroupedVisibleKVPItems in tmpGroupedVisibleItems)
            {
                var item = new VisionObject();
                var instanceID = tmpGroupedVisibleKVPItems.Key;
                item.InstanceID = instanceID;
                item.GameObject = MyGameObjectsBus.GetObject(instanceID);
                item.VisionItems = tmpGroupedVisibleKVPItems.Value;
                newVisibleObjects.Add(item);
            }
        }

        lock (mVisibleItemsLockObj)
        {
            mVisibleObjects = newVisibleObjects;
        }
    }

    private void GetRaycast(Transform targetTransform, Vector3 localDirection, List<VisionItem> visibleItems)
    {
        var globalDirection = targetTransform.TransformDirection(localDirection);

        var hit = new RaycastHit();

        var pos = transform.position + Offset;

        if (Physics.Raycast(pos, globalDirection, out hit, distance))
        {
#if UNITY_EDITOR
            //Debug.DrawLine(pos, hit.point, Color.blue);
#endif
            var visibleItem = new VisionItem();
            visibleItem.LocalDirection = localDirection;
            visibleItem.Point = hit.point;
            visibleItem.Distance = hit.distance;
            visibleItem.InstanceID = hit.transform.GetInstanceID();
            visibleItems.Add(visibleItem);
        }
#if UNITY_EDITOR
        else
        {
            //Debug.DrawRay(pos, globalDirection * distance, Color.red);
        }
#endif
    }

    private List<VisionObject> mVisibleObjects = new List<VisionObject>();
    private object mVisibleItemsLockObj = new object();

    public List<VisionObject> VisibleObjects
    {
        get
        {
            lock (mVisibleItemsLockObj)
            {
                return mVisibleObjects.ToList();
            }
        }
    }
}
