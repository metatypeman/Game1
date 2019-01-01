using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseWayPoint: BasePassiveLogicalGameObject, IWayPoint
    {
        protected BaseWayPoint()
            : base (new PassiveLogicalGameObjectOptions() {
                ShowGlobalPosition = true
            })
        {
        }

        public List<GameObject> PlanesList = new List<GameObject>();

        public float Radius = 3;

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

        protected override void OnStart()
        {
            Debug.Log($"PlanesList.Count = {PlanesList.Count}");
            foreach (var plane in PlanesList)
            {
                Debug.Log($"plane.name = {plane.name}");
            }
        }

        protected override void OnInitFacts()
        {
            base.OnInitFacts();

            //this["class"] = "waypoint";
            this["class"] = "place";
        }
    }
}
