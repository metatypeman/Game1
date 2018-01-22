using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class WaypointGroupInfo : IObjectToString
    {
        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(WaypointGroupInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(WaypointGroupInfo)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            return sb.ToString();
        }
    }

    public class WaypointInfo : IObjectToString
    {
        public Vector3 Position { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(WaypointInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(WaypointInfo)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            return sb.ToString();
        }
    }

    public static class WaypointsBus
    {
        private static object mLockObj = new object();

        private static List<WaypointInfo> mWaypointsList = new List<WaypointInfo>();
        private static Dictionary<string, List<WaypointInfo>> mTagsDict = new Dictionary<string, List<WaypointInfo>>();

        public static void RegisterWaypoint(WaypointInfo waypoint)
        {
            lock(mLockObj)
            {
                if(!mWaypointsList.Contains(waypoint))
                {
                    mWaypointsList.Add(waypoint);
                }

                var tagsList = waypoint.Tags;

                if (tagsList.Count > 0)
                {
                    tagsList = tagsList.Distinct().ToList();

                    foreach(var tag in tagsList)
                    {
                        if(string.IsNullOrWhiteSpace(tag))
                        {
                            continue;
                        }

                        List<WaypointInfo> tmpInfoList = null;

                        if(mTagsDict.ContainsKey(tag))
                        {
                            tmpInfoList = mTagsDict[tag];
                        }
                        else
                        {
                            tmpInfoList = new List<WaypointInfo>();
                            mTagsDict[tag] = tmpInfoList;
                        }

                        if(!tmpInfoList.Contains(waypoint))
                        {
                            tmpInfoList.Add(waypoint);
                        }
                    }
                }
            }
        }

        public static WaypointInfo GetByTag(string tag)
        {
            if(string.IsNullOrWhiteSpace(tag))
            {
                return null;
            }

            lock (mLockObj)
            {
                if(mTagsDict.ContainsKey(tag))
                {
                    return mTagsDict[tag].FirstOrDefault();
                }

                return null;
            }
        }
    }
}
