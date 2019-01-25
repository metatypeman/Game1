using Assets.Scripts;
using MyNPCLib.Logical;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRayScaner : MonoBehaviour, INPCRayScaner
{
    //public int distance = 15;
    public Vector3 Offset = new Vector3(0, 1.6f, 0);

    public int HRaysCount = 6;
    public int VRaysCount = 6;
    public int Distance = 20;
    public int Angle = 20;

    public Transform Head;

    private List<Vector3> mRayDirectionsList = new List<Vector3>();
    private OldLogicalObjectsBus mOldLogicalObjectsBus;

    // Use this for initialization
    void Start () {
        var commonLevelHost = LevelCommonHostFactory.Get();
        mOldLogicalObjectsBus = commonLevelHost.OldLogicalObjectsBus;

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
        lock(mIsDiedLockObj)
        {
            if(mIsDied)
            {
                return;
            }
        }

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
        var tmpVisibleItems = new List<InternalVisionItem>();

        foreach(var localDirection in mRayDirectionsList)
        {
            GetRaycast(targetTransform, localDirection, tmpVisibleItems);
        }

        var newVisibleObjects = new List<IHostVisionObject>();

        if(tmpVisibleItems.Count > 0)
        {
            var tmpGroupedVisibleItems = tmpVisibleItems.GroupBy(p => p.InstanceID).ToDictionary(p => p.Key, p => p.ToList());
            var instancesIdList = tmpGroupedVisibleItems.Keys.ToList();

            var entitiesIdDict = mOldLogicalObjectsBus.GetEntitiesIdListByInstancesIdList(instancesIdList);

            foreach (var tmpGroupedVisibleKVPItems in tmpGroupedVisibleItems)
            {
                var item = new HostVisionObject();
                var instanceID = tmpGroupedVisibleKVPItems.Key;
 
                if(!entitiesIdDict.ContainsKey(instanceID))
                {
                    continue;
                }

                item.EntityId = entitiesIdDict[instanceID];

                var visibleItemsList = new List<IVisionItem>();

                var sourceVisibleItemsList = tmpGroupedVisibleKVPItems.Value;

                foreach(var sourceVisibleItem in sourceVisibleItemsList)
                {
                    var visibleItem = new VisionItem();
                    visibleItem.LocalDirection = VectorsConvertor.UnityToNumeric(sourceVisibleItem.LocalDirection);
                    visibleItem.Point = VectorsConvertor.UnityToNumeric(sourceVisibleItem.Point);
                    visibleItem.Distance = sourceVisibleItem.Distance;
                    visibleItemsList.Add(visibleItem);
                }

                item.VisionItems = visibleItemsList;
                newVisibleObjects.Add(item);
            }
        }

        lock (mVisibleItemsLockObj)
        {
            mVisibleObjects = newVisibleObjects;
        }
    }

    private void GetRaycast(Transform targetTransform, Vector3 localDirection, List<InternalVisionItem> visibleItems)
    {
        var globalDirection = targetTransform.TransformDirection(localDirection);

        var hit = new RaycastHit();

        var pos = transform.position + Offset;

        if (Physics.Raycast(pos, globalDirection, out hit, Distance))
        {
#if UNITY_EDITOR
            Debug.DrawLine(pos, hit.point, Color.blue);
#endif
            var visibleItem = new InternalVisionItem();
            visibleItem.LocalDirection = localDirection;
            visibleItem.Point = hit.point;
            visibleItem.Distance = hit.distance;
            visibleItem.InstanceID = hit.transform.gameObject.GetInstanceID();
            visibleItems.Add(visibleItem);
        }
#if UNITY_EDITOR
        else
        {
            Debug.DrawRay(pos, globalDirection * Distance, Color.red);
        }
#endif
    }

    private List<IHostVisionObject> mVisibleObjects = new List<IHostVisionObject>();
    private readonly object mVisibleItemsLockObj = new object();

    public IList<IHostVisionObject> VisibleObjects
    {
        get
        {
            lock (mVisibleItemsLockObj)
            {
                return mVisibleObjects.ToList();
            }
        }
    }

    private bool mIsDied;
    private readonly object mIsDiedLockObj = new object();

    public void Die()
    {
        lock(mIsDiedLockObj)
        {
            if(mIsDied)
            {
                return;
            }

            mIsDied = true;
        }
    }
}
