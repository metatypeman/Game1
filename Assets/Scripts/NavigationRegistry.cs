using MyNPCLib.NavigationSupport;
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

            RegLinkOfPointsByPoint(linkOfWayPoints.FirstPoint, linkOfWayPoints);
            RegLinkOfPointsByPoint(linkOfWayPoints.SecondPoint, linkOfWayPoints);
        }

        private void RegLinkOfPointsByPoint(IWayPoint point, ILinkOfWayPoints linkOfPoints)
        {
#if DEBUG
            Debug.Log($"point = {point}");
            Debug.Log($"linkOfPoints = {linkOfPoints}");
#endif

            if (mLinksOfPointsByPointDict.ContainsKey(point))
            {
                var targetList = mLinksOfPointsByPointDict[point];
                targetList.Add(linkOfPoints);
                return;
            }

            mLinksOfPointsByPointDict[point] = new List<ILinkOfWayPoints>() { linkOfPoints };
        }

        private List<ILinkOfWayPoints> GetLinkOfPointsByPoint(IWayPoint point)
        {
            if (mLinksOfPointsByPointDict.ContainsKey(point))
            {
                return mLinksOfPointsByPointDict[point];
            }

            return new List<ILinkOfWayPoints>();
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
        private Dictionary<IWayPoint, List<ILinkOfWayPoints>> mLinksOfPointsByPointDict = new Dictionary<IWayPoint, List<ILinkOfWayPoints>>();

        private Dictionary<IPlane, Dictionary<IPlane, IList<IList<IPlane>>>> mPathsDict = new Dictionary<IPlane, Dictionary<IPlane, IList<IList<IPlane>>>>();

        private Dictionary<IPlane, Dictionary<IPlane, IList<PlanesConnectionInfo>>> mPlanesConnectionDict = new Dictionary<IPlane, Dictionary<IPlane, IList<PlanesConnectionInfo>>>();

        private void RegConnectionPoint(IPlane from, IPlane to, IList<PlanesConnectionInfo> connectionsInfoList)
        {
            Dictionary<IPlane, IList<PlanesConnectionInfo>> targetDict = null;

            if (mPlanesConnectionDict.ContainsKey(from))
            {
                targetDict = mPlanesConnectionDict[from];
            }
            else
            {
                targetDict = new Dictionary<IPlane, IList<PlanesConnectionInfo>>();
                mPlanesConnectionDict[from] = targetDict;
            }

            targetDict[to] = connectionsInfoList;
        }

        private IList<PlanesConnectionInfo> GetConnectionPoints(IPlane from, IPlane to)
        {
            if (mPlanesConnectionDict.ContainsKey(from))
            {
                var targetDict = mPlanesConnectionDict[from];

                if (targetDict.ContainsKey(to))
                {
                    return targetDict[to];
                }
            }

            return new List<PlanesConnectionInfo>();
        }

        private IList<IList<IPlane>> GetPathsListByPlanes(IPlane startPlane, IPlane targePlane)
        {
            if (mPathsDict.ContainsKey(startPlane))
            {
                var targetDict = mPathsDict[startPlane];

                if (targetDict.ContainsKey(targePlane))
                {
                    return targetDict[targePlane];
                }
            }

            return new List<IList<IPlane>>();
        }

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

                    var connectionsList1 = new List<PlanesConnectionInfo>();
                    var connectionsList2 = new List<PlanesConnectionInfo>();

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

                    var directCommonPointsList = outerPlane.PointsList.Intersect(innerPlane.PointsList).ToList();

#if DEBUG
                    Debug.Log($"directCommonPointsList.Count = {directCommonPointsList.Count}");
#endif

                    foreach (var directCommonPoint in directCommonPointsList)
                    {
                        var directPointConnectionItem = new PlanesConnectionInfo();
                        directPointConnectionItem.IsDirect = true;
                        directPointConnectionItem.WayPoint = directCommonPoint;

                        connectionsList1.Add(directPointConnectionItem);
                        connectionsList2.Add(directPointConnectionItem);
                    }

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

                    var indirectCommonPointsList = intersectedPointsList.Except(directCommonPointsList).ToList();

                    foreach (var indirectCommonPoint in indirectCommonPointsList)
                    {
                        var directPointConnectionItem = new PlanesConnectionInfo();
                        directPointConnectionItem.IsDirect = false;
                        directPointConnectionItem.WayPoint = indirectCommonPoint;

                        var linksList = mLinksOfPointsByPointDict[indirectCommonPoint];
                        directPointConnectionItem.LinksList = linksList;

                        if (outerPlane.PointsList.Contains(indirectCommonPoint))
                        {
                            connectionsList1.Add(directPointConnectionItem);
                        }

                        if (innerPlane.PointsList.Contains(indirectCommonPoint))
                        {
                            connectionsList2.Add(directPointConnectionItem);
                        }
                    }

                    RegConnectionPoint(outerPlane, innerPlane, connectionsList1);
                    RegConnectionPoint(innerPlane, outerPlane, connectionsList2);

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
                var str = PathsHelper.DisplayPath(path);
                Debug.Log(str);
#endif
            }

            mPathsDict = pathListForGrouping.GroupBy(p => p.FirstItem).ToDictionary(p => p.Key, p => p.GroupBy(x => x.LastItem).ToDictionary(x => x.Key, x => (IList<IList<IPlane>>)x));

#if DEBUG
            Debug.Log("End");
#endif
        }

        private class PathGroupItem
        {
            public IPlane FirstItem { get; set; }
            public IPlane LastItem { get; set; }
            public IList<IPlane> StepItems { get; set; }
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

                    FillPaths(nextPlane, localPath.ToList(), ref result);
                }
            }
        }

        public IRoute GetRouteForPosition(IPointInfo pointInfo)
        {
#if DEBUG
            Debug.Log($"pointInfo = {pointInfo}");
#endif

            if (pointInfo.IsFirstPartOfLink)
            {
                return NGetRouteForPositionOfFirstPartOfLink(pointInfo);
            }

            return GetRouteForPosition(VectorsConvertor.NumericToUnity(pointInfo.Position.Value), VectorsConvertor.NumericToUnity(pointInfo.Route.TargetPosition));
        }

        private IRoute NGetRouteForPositionOfFirstPartOfLink(IPointInfo pointInfo)
        {
#if DEBUG
            Debug.Log($"pointInfo = {pointInfo}");
#endif

            var tstPointInfo = (PointInfo)pointInfo;

            var initialRoute = pointInfo.Route;

#if DEBUG
            Debug.Log($"initialRoute = {initialRoute}");
#endif

            var stepOfRoute = (StepOfRoute)pointInfo.StepOfRoute;

#if DEBUG
            Debug.Log($"stepOfRoute = {stepOfRoute}");
#endif

            var currentPlane = stepOfRoute.CurrentPlane;

            var nextPathsList = new List<IList<IPlane>>();

            foreach (var path in stepOfRoute.PathsList)
            {
#if DEBUG
                Debug.Log(PathsHelper.DisplayPath(path));
#endif

                var firstItem = path.First();

#if DEBUG
                Debug.Log($"firstItem = {firstItem}");
#endif

                if (firstItem != currentPlane)
                {
                    continue;
                }

                path.Remove(firstItem);

#if DEBUG
                Debug.Log(PathsHelper.DisplayPath(path));
#endif

                nextPathsList.Add(path);
            }

            var nextPathsDict = nextPathsList.GroupBy(p => p.First()).ToDictionary(p => p.Key, p => p.ToList());

#if DEBUG
            Debug.Log($"nextPathsDict.Count = {nextPathsDict.Count}");
#endif

            var result = new Route();
            result.TargetPosition = initialRoute.TargetPosition;

            var linksList = GetLinkOfPointsByPoint(tstPointInfo.WayPoint);

            var nextPointsList = linksList.Where(p => p.FirstPoint == tstPointInfo.WayPoint).Select(p => p.SecondPoint).ToList();
            nextPointsList.AddRange(linksList.Where(p => p.SecondPoint == tstPointInfo.WayPoint).Select(p => p.FirstPoint));

#if DEBUG
            Debug.Log($"nextPointsList.Count = {nextPointsList.Count}");
            foreach (var link in nextPointsList)
            {
                Debug.Log($"link = {link}");
            }
#endif

            var stepOfRouteDicts = new Dictionary<IPlane, StepOfRoute>();

            foreach (var nextPathsKVPItem in nextPathsDict)
            {
                var firstItem = nextPathsKVPItem.Key;
#if DEBUG
                Debug.Log($"firstItem = {firstItem}");
#endif

                var targetPointsList = nextPointsList.Where(p => p.PlanesList.Contains(firstItem)).ToList();

#if DEBUG
                Debug.Log($"targetPointsList.Count = {targetPointsList.Count}");
#endif

                if (targetPointsList.Count == 0)
                {
                    continue;
                }

                var pathsList = nextPathsKVPItem.Value;

                foreach (var targetPoint in targetPointsList)
                {
#if DEBUG
                    Debug.Log($"targetPoint = {targetPoint}");
#endif
                    StepOfRoute targetStepOfRoute = null;

                    if (stepOfRouteDicts.ContainsKey(firstItem))
                    {
                        targetStepOfRoute = stepOfRouteDicts[firstItem];
                    }
                    else
                    {
                        targetStepOfRoute = new StepOfRoute();
                        stepOfRouteDicts[firstItem] = targetStepOfRoute;
                        targetStepOfRoute.CurrentPlane = firstItem;
                        result.NextSteps.Add(targetStepOfRoute);
                    }

                    foreach (var path in pathsList)
                    {
                        stepOfRoute.PathsList.Add(path);
                    }

                    var targetPointInfo = new PointInfo();
                    targetPointInfo.Route = result;
                    targetPointInfo.StepOfRoute = targetStepOfRoute;
                    targetPointInfo.Position = targetPoint.Position;
                    targetPointInfo.WayPoint = targetPoint;
                    targetPointInfo.Plane = firstItem;

                    result.NextPoints.Add(targetPointInfo);
                    targetStepOfRoute.TargetPoints.Add(targetPointInfo);
                }
            }

            if (result.Status == StatusOfRoute.Unknown)
            {
                if (result.NextPoints.Count == 0)
                {
                    result.Status = StatusOfRoute.Impossible;
                }
                else
                {
                    result.Status = StatusOfRoute.Processed;
                }
            }

            return result;
        }

        public IRoute GetRouteForPosition(Vector3 startPosition, Vector3 targetPosition)
        {
#if DEBUG
            Debug.Log($"startPosition = {startPosition}");
            Debug.Log($"targetPosition = {targetPosition}");
#endif

            var result = new Route();
            result.TargetPosition = VectorsConvertor.UnityToNumeric(targetPosition);

            if (startPosition == targetPosition)
            {
                result.Status = StatusOfRoute.Finished;
                return result;
            }

            var initialPathsList = GetPathsListForPosition(startPosition, targetPosition);

#if DEBUG
            Debug.Log($"initialPathsList.Count = {initialPathsList.Count}");
#endif

            if (initialPathsList.Count == 0)
            {
                result.Status = StatusOfRoute.Impossible;
                return result;
            }

            result.InitPathsList = initialPathsList.SelectMany(p => p.PathsList).ToList();

            var stepOfRouteDicts = new Dictionary<IPlane, StepOfRoute>();

            foreach (var initialPathsItem in initialPathsList)
            {
                var pathsList = initialPathsItem.PathsList;

#if DEBUG
                Debug.Log($"pathsList.Count = {pathsList.Count}");
#endif

                foreach (var path in pathsList)
                {
#if DEBUG
                    Debug.Log(PathsHelper.DisplayPath(path));
#endif

                    var firstItem = path.First();

#if DEBUG
                    Debug.Log($"firstItem.Name = {firstItem.Name}");
#endif

                    if (firstItem.Contains(targetPosition))
                    {
#if DEBUG
                        Debug.Log("firstItem.Contains(targetPosition)");
#endif
                        StepOfRoute stepOfRoute = null;

                        if (stepOfRouteDicts.ContainsKey(firstItem))
                        {
                            stepOfRoute = stepOfRouteDicts[firstItem];
                        }
                        else
                        {
                            stepOfRoute = new StepOfRoute();
                            stepOfRouteDicts[firstItem] = stepOfRoute;
                            stepOfRoute.CurrentPlane = firstItem;
                            result.NextSteps.Add(stepOfRoute);
                        }

                        stepOfRoute.PathsList.Add(path);

                        var pointInfo = new PointInfo();
                        pointInfo.IsFinal = true;
                        pointInfo.Route = result;
                        pointInfo.StepOfRoute = stepOfRoute;
                        pointInfo.Position = VectorsConvertor.UnityToNumeric(targetPosition);
                        pointInfo.Plane = firstItem;

                        result.NextPoints.Add(pointInfo);
                        stepOfRoute.TargetPoints.Add(pointInfo);

                        continue;
                    }

                    var originPath = path.ToList();

                    path.Remove(firstItem);

#if DEBUG
                    Debug.Log($"path.Count = {path.Count}");
#endif

                    var nextItem = path.First();

#if DEBUG
                    Debug.Log(PathsHelper.DisplayPath(originPath));
                    Debug.Log($"firstItem.Name = {firstItem.Name}");
                    Debug.Log($"nextItem.Name = {nextItem.Name}");
                    Debug.Log(PathsHelper.DisplayPath(path));
#endif

                    var connectionList = GetConnectionPoints(firstItem, nextItem);

                    if (connectionList.Count == 0)
                    {
                        throw new NotSupportedException();
                    }

#if DEBUG
                    Debug.Log($"connectionList.Count = {connectionList.Count}");
#endif

                    foreach (var connection in connectionList)
                    {
#if DEBUG
                        Debug.Log($"connection = {connection}");
#endif

                        StepOfRoute stepOfRoute = null;

                        if (connection.IsDirect)
                        {
                            if (stepOfRouteDicts.ContainsKey(nextItem))
                            {
                                stepOfRoute = stepOfRouteDicts[nextItem];
                            }
                            else
                            {
                                stepOfRoute = new StepOfRoute();
                                stepOfRouteDicts[nextItem] = stepOfRoute;
                                stepOfRoute.CurrentPlane = nextItem;
                                result.NextSteps.Add(stepOfRoute);
                            }

                            stepOfRoute.PathsList.Add(path);

                            var pointInfo = new PointInfo();
                            pointInfo.Route = result;
                            pointInfo.StepOfRoute = stepOfRoute;
                            pointInfo.Position = connection.WayPoint.Position;
                            pointInfo.WayPoint = connection.WayPoint;
                            pointInfo.Plane = nextItem;

                            result.NextPoints.Add(pointInfo);
                            stepOfRoute.TargetPoints.Add(pointInfo);
                        }
                        else
                        {
                            if (stepOfRouteDicts.ContainsKey(firstItem))
                            {
                                stepOfRoute = stepOfRouteDicts[firstItem];
                            }
                            else
                            {
                                stepOfRoute = new StepOfRoute();
                                stepOfRouteDicts[firstItem] = stepOfRoute;
                                stepOfRoute.CurrentPlane = firstItem;
                                result.NextSteps.Add(stepOfRoute);
                            }

                            stepOfRoute.PathsList.Add(originPath);

                            var pointInfo = new PointInfo();
                            pointInfo.Route = result;
                            pointInfo.StepOfRoute = stepOfRoute;
                            pointInfo.Position = connection.WayPoint.Position;
                            pointInfo.WayPoint = connection.WayPoint;
                            pointInfo.Plane = firstItem;
                            pointInfo.IsFirstPartOfLink = true;

                            result.NextPoints.Add(pointInfo);
                            stepOfRoute.TargetPoints.Add(pointInfo);
                        }

#if DEBUG
                        Debug.Log($"stepOfRoute = {stepOfRoute}");
#endif
                    }
                }
            }

            if (result.Status == StatusOfRoute.Unknown)
            {
                if (result.NextPoints.Count == 0)
                {
                    result.Status = StatusOfRoute.Impossible;
                }
                else
                {
                    result.Status = StatusOfRoute.Processed;
                }
            }

            return result;
        }

        private class PathInfo
        {
            public IPlane FirstItem { get; set; }
            public IPlane LastItem { get; set; }
            public IList<IList<IPlane>> PathsList { get; set; }
        }

        private List<PathInfo> GetPathsListForPosition(Vector3 startPosition, Vector3 targetPosition)
        {
#if DEBUG
            Debug.Log($"startPosition = {startPosition}");
            Debug.Log($"targetPosition = {targetPosition}");
#endif

            var planesListForStartPosition = GetPlanesByPoint(startPosition);

#if DEBUG
            Debug.Log($"planesListForStartPosition.Count = {planesListForStartPosition.Count}");
            foreach (var item in planesListForStartPosition)
            {
                Debug.Log($"item.Name = {item.Name}");
            }
#endif

            if (planesListForStartPosition.Count == 0)
            {
                return new List<PathInfo>();
            }

            var planesListForTargetPosition = GetPlanesByPoint(targetPosition);

#if DEBUG
            Debug.Log($"planesListForTargetPosition.Count = {planesListForTargetPosition.Count}");
            foreach (var item in planesListForTargetPosition)
            {
                Debug.Log($"item.Name = {item.Name}");
            }
#endif

            if (planesListForTargetPosition.Count == 0)
            {
                return new List<PathInfo>();
            }

            var result = new List<PathInfo>();

            foreach (var startPlane in planesListForStartPosition)
            {
                foreach (var targetPlane in planesListForTargetPosition)
                {
                    var pathsList = GetPathsListByPlanes(startPlane, targetPlane);

                    if (pathsList.Count == 0)
                    {
                        continue;
                    }

#if DEBUG
                    Debug.Log($"pathsList.Count = {pathsList.Count}");
                    foreach (var path in pathsList)
                    {
                        Debug.Log(PathsHelper.DisplayPath(path));
                    }
#endif

                    var item = new PathInfo();
                    item.FirstItem = startPlane;
                    item.LastItem = targetPlane;
                    item.PathsList = pathsList;
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
