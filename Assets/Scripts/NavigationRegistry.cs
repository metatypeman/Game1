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

        public NavigationRegistry()
        {
        }

        public RTreeNode RTreeNode
        {
            set
            {
                if(value == mRTreeNode)
                {
                    return;
                }

                mRTreeNode = value;
                IndexRTreeNodes();
            }
        }

        private RTreeNode mRTreeNode;
        private List<IPlane> mPlanesList = new List<IPlane>();
        private RTreeNode[,] mRTreeNodesDict;

        private void IndexRTreeNodes()
        {
            var finalNodesList = mRTreeNode.GetFinalNodes();

#if DEBUG
            //Debug.Log($"finalNodesList.Count = {finalNodesList.Count}");
#endif

            var groupedByZDict = finalNodesList.GroupBy(p => p.LeftBottomPoint.z).ToDictionary(p => p.Key, p => p.ToList());

#if DEBUG
            //Debug.Log($"groupedByZDict.Count = {groupedByZDict.Count}");
#endif

            var maxZ = groupedByZDict.Count;
            var maxX = groupedByZDict.First().Value.Count;

#if DEBUG
            //Debug.Log($"maxZ = {maxZ} maxX = {maxX}");
#endif

            mRTreeNodesDict = new RTreeNode[maxZ, maxX];

            var zN = 0;
            
            foreach (var groupedByZKVPItem in groupedByZDict)
            {
#if DEBUG
                //Debug.Log($"groupedByZKVPItem.Key = {groupedByZKVPItem.Key}");
#endif

                var groupedByXDict = groupedByZKVPItem.Value.ToDictionary(p => p.LeftBottomPoint.x, p => p);

#if DEBUG
                //Debug.Log($"groupedByXDict.Count = {groupedByXDict.Count}");
#endif
                var xN = 0;

                foreach (var groupedByXKVPItem in groupedByXDict)
                {
#if DEBUG
                    //Debug.Log($"groupedByXKVPItem.Key = {groupedByXKVPItem.Key} zN = {zN} xN = {xN}");
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

            mIndexOfPlanesDict[plane] = mCurrIndex;
            mBackIndexOfPlanesDict[mCurrIndex] = plane;
            mCurrIndex++;
        }

        public IList<IPlane> GetPlanesByPoint(Vector3 position)
        {
#if DEBUG
            //Debug.Log($"position = {position}");
#endif

            var node = mRTreeNode.GetFinalNodeByPoint(position);

#if DEBUG
            //Debug.Log($"node.Zn = {node.Zn} node.Xn = {node.Xn}");
#endif

            var planesList = node.PlanesList;

#if DEBUG
            //Debug.Log($"planesList.Count = {planesList.Count}");
#endif

            if(planesList.Count == 0)
            {
                return planesList;
            }

            var result = new List<IPlane>();

            foreach(var plane in planesList)
            {
                if(plane.Contains(position))
                {
                    result.Add(plane);
                }
            }

            return result;
        }

        public void RegWayPoint(IWayPoint wayPoint)
        {
            mPointsList.Add(wayPoint);

            foreach (var plane in wayPoint.PlanesList)
            {
                if (plane.PointsList.Contains(wayPoint))
                {
                    continue;
                }

                plane.PointsList.Add(wayPoint);
            }
        }

        public void RegLinkOfWayPoints(ILinkOfWayPoints linkOfWayPoints)
        {
            mLinksOfPointsList.Add(linkOfWayPoints);

#if DEBUG
            Debug.Log($"(linkOfWayPoints.FirstPoint == null) = {linkOfWayPoints.FirstPoint == null}");
            Debug.Log($"(linkOfWayPoints.SecondPoint == null) = {linkOfWayPoints.SecondPoint == null}");
#endif

            mPointsOfLinksOfPointsList.Add(new KeyValuePair<IWayPoint, IWayPoint>(linkOfWayPoints.FirstPoint, linkOfWayPoints.SecondPoint));
            mPointsOfLinksOfPointsList.Add(new KeyValuePair<IWayPoint, IWayPoint>(linkOfWayPoints.SecondPoint, linkOfWayPoints.FirstPoint));
        }

        public void PrepareAllInfo()
        {
            PrepareAllPlanes();
            CalculatePaths();
        }

        private void PrepareAllPlanes()
        {
            if(mPlanesList.Count == 0)
            {
                return;
            }

            foreach(var plane in mPlanesList)
            {
#if DEBUG
                //Debug.Log($"plane.FRPoint = {plane.FRPoint}");
                //Debug.Log($"plane.BRPoint = {plane.BRPoint}");
                //Debug.Log($"plane.FLPoint = {plane.FLPoint}");
                //Debug.Log($"plane.BLPoint = {plane.BLPoint}");
#endif

                var frNode = mRTreeNode.GetFinalNodeByPoint(plane.FRPoint);

#if DEBUG
                //Debug.Log($"frNode.Zn = {frNode.Zn} frNode.Xn = {frNode.Xn}");
#endif

                var brNode = mRTreeNode.GetFinalNodeByPoint(plane.BRPoint);

#if DEBUG
                //Debug.Log($"brNode.Zn = {brNode.Zn} brNode.Xn = {brNode.Xn}");
#endif

                var flNode = mRTreeNode.GetFinalNodeByPoint(plane.FLPoint);

#if DEBUG
                //Debug.Log($"flNode.Zn = {flNode.Zn} flNode.Xn = {flNode.Xn}");
#endif

                var blNode = mRTreeNode.GetFinalNodeByPoint(plane.BLPoint);

#if DEBUG
                //Debug.Log($"blNode.Zn = {blNode.Zn} blNode.Xn = {blNode.Xn}");
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
                //Debug.Log($"maxZ = {maxZ} minZ = {minZ} maxX = {maxX} minX = {minX}");
#endif

                for (var z = minZ; z <= maxZ; z++)
                {
                    for (var x = minX; x <= maxX; x++)
                    {
#if DEBUG
                        //Debug.Log($"z = {z} x = {x}");
#endif

                        var node = mRTreeNodesDict[z, x];

                        node.AddPlane(plane);
                    }
                }
            }
        }

        private int mCurrIndex;
        private Dictionary<IPlane, int> mIndexOfPlanesDict = new Dictionary<IPlane, int>();
        private Dictionary<int, IPlane> mBackIndexOfPlanesDict = new Dictionary<int, IPlane>();
        private bool?[,] mPlanesDict;

        private List<IWayPoint> mPointsList = new List<IWayPoint>();

        private List<ILinkOfWayPoints> mLinksOfPointsList = new List<ILinkOfWayPoints>();
        private List<KeyValuePair<IWayPoint, IWayPoint>> mPointsOfLinksOfPointsList = new List<KeyValuePair<IWayPoint, IWayPoint>>();

        private Dictionary<IPlane, Dictionary<IPlane, IList<IPlane>>> mPathsDict = new Dictionary<IPlane, Dictionary<IPlane, IList<IPlane>>>();

        private void CalculatePaths()
        {
#if DEBUG
            Debug.Log($"mPlanesList.Count = {mPlanesList.Count}");
            Debug.Log($"mPointsList.Count = {mPointsList.Count}");
            Debug.Log($"mLinksOfPointsList.Count = {mLinksOfPointsList.Count}");
#endif

            mPlanesDict = new bool?[mPlanesList.Count, mPlanesList.Count];
            var pointsOfLinksOfPointsDict = mPointsOfLinksOfPointsList.GroupBy(p => p.Key).ToDictionary(p => p.Key, p => p.Select(x => x.Value).ToList());

            var listOfPaths = new List<List<IPlane>>();

            foreach (var outerPlane in mPlanesList)
            {
                foreach (var innerPlane in mPlanesList)
                {
                    var outerIndex = mIndexOfPlanesDict[outerPlane];
                    var innerIndex = mIndexOfPlanesDict[innerPlane];

#if DEBUG
                    Debug.Log($"outerIndex = {outerIndex} innerIndex = {innerIndex}");
#endif

                    if (outerIndex == innerIndex)
                    {
                        mPlanesDict[outerIndex, innerIndex] = null;
                        continue;
                    }

                    if (mPlanesDict[outerIndex, innerIndex] != null)
                    {
                        continue;
                    }

#if DEBUG
                    Debug.Log($"NEXT outerIndex = {outerIndex} innerIndex = {innerIndex}");
                    Debug.Log($"outerPlane = {outerPlane}");
                    Debug.Log($"innerPlane = {innerPlane}");
#endif

                    var outerPointsList = outerPlane.PointsList.ToList();

                    foreach (var outerPoint in outerPlane.PointsList)
                    {
#if DEBUG
                        Debug.Log($"outerPoint = {outerPoint}");
#endif

                        if (pointsOfLinksOfPointsDict.ContainsKey(outerPoint))
                        {
                            outerPointsList.AddRange(pointsOfLinksOfPointsDict[outerPoint]);
                        }
                    }

                    var innerPointsList = innerPlane.PointsList.ToList();

                    foreach (var innerPoint in innerPlane.PointsList)
                    {
#if DEBUG
                        Debug.Log($"innerPoint = {innerPoint}");
#endif

                        if (pointsOfLinksOfPointsDict.ContainsKey(innerPoint))
                        {
                            innerPointsList.AddRange(pointsOfLinksOfPointsDict[innerPoint]);
                        }
                    }

                    var intersectedPointsList = outerPointsList.Intersect(innerPointsList).ToList();

#if DEBUG
                    Debug.Log($"intersectedPointsList.Count = {intersectedPointsList.Count}");
                    foreach (var intersectedPoint in intersectedPointsList)
                    {
                        Debug.Log($"intersectedPoint = {intersectedPoint}");
                    }
#endif

                    if (intersectedPointsList.Count == 0)
                    {
                        mPlanesDict[outerIndex, innerIndex] = false;
                        mPlanesDict[innerIndex, outerIndex] = false;
                    }
                    else
                    {
                        mPlanesDict[outerIndex, innerIndex] = true;
                        mPlanesDict[innerIndex, outerIndex] = true;
                    }
                }
            }

#if DEBUG
            for (var outerIndex = 0; outerIndex < mPlanesList.Count; outerIndex++)
            {
                for (var innerIndex = 0; innerIndex < mPlanesList.Count; innerIndex++)
                {
                    Debug.Log($"mPlanesDict[{outerIndex}, {innerIndex}] = {mPlanesDict[outerIndex, innerIndex]}");
                }
            }
#endif

            for (var i = 0; i < mPlanesList.Count; i++)
            {
                var plane = mBackIndexOfPlanesDict[i];

#if DEBUG
                Debug.Log($"i = {i} plane = {plane}");
#endif

                FillPaths(plane, new List<IPlane>(), ref listOfPaths);
            }

#if DEBUG
            Debug.Log($"listOfPaths.Count = {listOfPaths.Count}");
#endif

            var pathListForGrouping = new List<PathGroupItem>();

            foreach (var path in listOfPaths)
            {
                var firstItem = path.First();
                var lastItem = path.Last();

                var itemForGrouping = new PathGroupItem();
                itemForGrouping.FirstItem = firstItem;
                itemForGrouping.LastItem = lastItem;
                itemForGrouping.StepItems = path;

                pathListForGrouping.Add(itemForGrouping);

#if DEBUG
                var sb = new StringBuilder();

                foreach (var item in path)
                {
                    sb.Append($"{item.Name},");
                }
                sb.Remove(sb.Length - 1, 1);

                var str = sb.ToString().Replace(",", " -> ");

                Debug.Log(str);
#endif
            }

            mPathsDict = pathListForGrouping.GroupBy(p => p.FirstItem).ToDictionary(p => p.Key, p => p.ToDictionary(x => x.LastItem, x => (IList<IPlane>)x.StepItems));

#if DEBUG
            Debug.Log("End");
#endif
        }

        private class PathGroupItem
        {
            public IPlane FirstItem { get; set; }
            public IPlane LastItem { get; set; }
            public List<IPlane> StepItems { get; set; }
        }

        private void FillPaths(IPlane plane, List<IPlane> localPath, ref List<List<IPlane>> result)
        {
#if DEBUG
            Debug.Log($"plane = {plane}");
#endif

            if (localPath.Contains(plane))
            {
                return;
            }

            localPath.Add(plane);

            if (localPath.Count > 1)
            {
                result.Add(localPath.ToList());
            }

            var index = mIndexOfPlanesDict[plane];

#if DEBUG
            Debug.Log($"index = {index}");
#endif

            for (var i = 0; i < mPlanesList.Count; i++)
            {
                var nextLink = mPlanesDict[index, i];

#if DEBUG
                Debug.Log($"nextLink = {nextLink}");
#endif

                if (nextLink == true)
                {
                    var nextPlane = mBackIndexOfPlanesDict[i];

#if DEBUG
                    Debug.Log($"nextPlane = {nextPlane}");
#endif

                    FillPaths(nextPlane, localPath, ref result);
                }
            }
        }
    }
}
