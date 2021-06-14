using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseWayPoint: LogicalGameObject, IWayPoint
    {
        public List<GameObject> PlanesList = new List<GameObject>();

        public float Radius = 3;

        private List<IPlane> mPlanesList = new List<IPlane>();

        public System.Numerics.Vector3 Position
        {
            get
            {
                return VectorsConvertor.UnityToNumeric(transform.position);
            }
        }

        private string mName;
        public string Name
        {
            get
            {
                return mName;
            }
        }

        IList<IPlane> IWayPoint.PlanesList
        {
            get
            {
                return mPlanesList;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(transform.position, transform.position + transform.up * 3);

            Gizmos.DrawLine(transform.position, transform.position + transform.forward * Radius);

            Gizmos.DrawRay(transform.position, GetFarPoint(45, 0, Radius));

            Gizmos.DrawRay(transform.position, GetFarPoint(90, 0, Radius));

            Gizmos.DrawRay(transform.position, GetFarPoint(135, 0, Radius));

            Gizmos.DrawRay(transform.position, GetFarPoint(180, 0, Radius));

            Gizmos.DrawRay(transform.position, GetFarPoint(225, 0, Radius));

            Gizmos.DrawRay(transform.position, GetFarPoint(270, 0, Radius));

            Gizmos.DrawRay(transform.position, GetFarPoint(315, 0, Radius));
        }

        private Vector3 GetFarPoint(float x, float y, float distance)
        {
            var dy = Mathf.Sin(y * Mathf.Deg2Rad);

            var dx = Mathf.Sin(x * Mathf.Deg2Rad);
            var dz = Mathf.Cos(x * Mathf.Deg2Rad);

            var localDirection = new Vector3(dx, dy, dz) * distance;
            return localDirection;
        }

        //protected override void OnStart()
        //{
        //    mName = name;

        //    //Debug.Log($"PlanesList.Count = {PlanesList.Count} name = {name}");
        //    foreach (var plane in PlanesList)
        //    {
        //        //Debug.Log($"plane.name = {plane.name}");

        //        mPlanesList.Add(plane.GetComponent<IPlane>());
        //    }

        //    var commonLevelHost = LevelCommonHostFactory.Get();

        //    commonLevelHost.HostNavigationRegistry.RegWayPoint(this);
        //}

        //protected override void OnInitFacts()
        //{
        //    base.OnInitFacts();

        //    //this["class"] = "waypoint";
        //    this["class"] = "place";
        //}
    }
}
