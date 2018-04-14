using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MyNPCLib;

namespace Assets.Scripts
{
    public class WaypointGroupInfo : IObjectToString
    {
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
            //var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            return sb.ToString();
        }
    }

    public class WaypointInfo : IObjectToString
    {
        public Vector3 Position { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

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
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Position)} = {Position}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            if (Tags == null)
            {
                sb.AppendLine($"{spaces}{nameof(Tags)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Tags)}");
                foreach(var tag in Tags)
                {
                    sb.AppendLine($"{nextSpaces}{nameof(tag)} = {tag}");
                }
                sb.AppendLine($"{spaces}End {nameof(Tags)}");          
            }
            
            return sb.ToString();
        }
    }

    public static class WaypointsBus
    {
        private static object mLockObj = new object();

        private static List<WaypointInfo> mWaypointsList = new List<WaypointInfo>();
        private static Dictionary<string, List<WaypointInfo>> mTagsDict = new Dictionary<string, List<WaypointInfo>>();
        private static Dictionary<string, WaypointInfo> mNamesDict = new Dictionary<string, WaypointInfo>();

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

                var name = waypoint.Name;

                if(!string.IsNullOrWhiteSpace(name))
                {
                    mNamesDict[name] = waypoint;
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

        public static WaypointInfo GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            lock (mLockObj)
            {
                if (mNamesDict.ContainsKey(name))
                {
                    return mNamesDict[name];
                }

                return null;
            }
        }
    }
}
