using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class NavigationRegistry: IHostNavigationRegistry
    {
        public NavigationRegistry(RTreeNode rTreeNode)
        {
            mRTreeNode = rTreeNode;

            IndexRTreeNodes();
        }

        private RTreeNode mRTreeNode;
        private List<IPlane> mPlanesList = new List<IPlane>();
        private RTreeNode[,] mRTreeNodesDict;

        private void IndexRTreeNodes()
        {
            var finalNodesList = mRTreeNode.GetFinalNodes();

#if DEBUG
            Debug.Log($"finalNodesList.Count = {finalNodesList.Count}");
#endif

            var groupedByZDict = finalNodesList.GroupBy(p => p.LeftBottomPoint.z).ToDictionary(p => p.Key, p => p.ToList());

#if DEBUG
            Debug.Log($"groupedByZDict.Count = {groupedByZDict.Count}");
#endif

            var maxZ = groupedByZDict.Count;
            var maxX = groupedByZDict.First().Value.Count;

#if DEBUG
            Debug.Log($"maxZ = {maxZ} maxX = {maxX}");
#endif

            mRTreeNodesDict = new RTreeNode[maxZ, maxX];

            var zN = 0;
            
            foreach (var groupedByZKVPItem in groupedByZDict)
            {
#if DEBUG
                Debug.Log($"groupedByZKVPItem.Key = {groupedByZKVPItem.Key}");
#endif

                var groupedByXDict = groupedByZKVPItem.Value.ToDictionary(p => p.LeftBottomPoint.x, p => p);

#if DEBUG
                Debug.Log($"groupedByXDict.Count = {groupedByXDict.Count}");
#endif
                var xN = 0;

                foreach (var groupedByXKVPItem in groupedByXDict)
                {
#if DEBUG
                    Debug.Log($"groupedByXKVPItem.Key = {groupedByXKVPItem.Key} zN = {zN} xN = {xN}");
#endif
                    var value = groupedByXKVPItem.Value;
                    value.Zn = zN;
                    value.Xn = xN;
                    mRTreeNodesDict[zN, xN] = value;

                    xN++;
                }

                zN++;
            }
        }

        public void RegPlane(IPlane plane)
        {
            mPlanesList.Add(plane);

#if DEBUG
            Debug.Log($"plane.FRPoint = {plane.FRPoint}");
            Debug.Log($"plane.BRPoint = {plane.BRPoint}");
            Debug.Log($"plane.FLPoint = {plane.FLPoint}");
            Debug.Log($"plane.BLPoint = {plane.BLPoint}");
#endif

            var frNode = mRTreeNode.GetFinalNodeByPoint(plane.FRPoint);

#if DEBUG
            Debug.Log($"frNode.Zn = {frNode.Zn} frNode.Xn = {frNode.Xn}");
#endif

            var brNode = mRTreeNode.GetFinalNodeByPoint(plane.BRPoint);

#if DEBUG
            Debug.Log($"brNode.Zn = {brNode.Zn} brNode.Xn = {brNode.Xn}");
#endif

            var flNode = mRTreeNode.GetFinalNodeByPoint(plane.FLPoint);

#if DEBUG
            Debug.Log($"flNode.Zn = {flNode.Zn} flNode.Xn = {flNode.Xn}");
#endif

            var blNode = mRTreeNode.GetFinalNodeByPoint(plane.BLPoint);

#if DEBUG
            Debug.Log($"blNode.Zn = {blNode.Zn} blNode.Xn = {blNode.Xn}");
#endif

            var zList = new List<int>() { frNode.Zn, brNode.Zn, flNode.Zn, blNode.Zn };
            zList = zList.Distinct().ToList();

            var xList = new List<int>() { frNode.Xn, brNode.Xn, flNode.Xn, blNode.Xn };
            xList = xList.Distinct().ToList();

            var maxZ = zList.Max();
            var minZ = zList.Min();

            var maxX = xList.Max();
            var minX = xList.Min();

#if DEBUG
            Debug.Log($"maxZ = {maxZ} minZ = {minZ} maxX = {maxX} minX = {minX}");
#endif

            for(var z = minZ; z <= maxZ; z++)
            {
                for(var x = minX; x <= maxX; x++)
                {
#if DEBUG
                    Debug.Log($"z = {z} x = {x}");
#endif

                    var plane = m
                }
            }
        }
    }
}
