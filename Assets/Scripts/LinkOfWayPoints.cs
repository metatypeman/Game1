using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class LinkOfWayPoints : MonoBehaviour, ILinkOfWayPoints
    {
        public GameObject FirstPoint;
        public GameObject SecondPoint;

        private IWayPoint mFirstPoint;
        private IWayPoint mSecondPoint;

        IWayPoint ILinkOfWayPoints.FirstPoint
        {
            get
            {
#if DEBUG
                //Debug.Log($"FirstPoint name = {name}");
#endif
                if(mFirstPoint == null)
                {
                    mFirstPoint = FirstPoint.GetComponent<IWayPoint>();
                }

                return mFirstPoint;
            }
        }

        IWayPoint ILinkOfWayPoints.SecondPoint
        {
            get
            {
#if DEBUG
                //Debug.Log($"SecondPoint name = {name}");
#endif
                if(mSecondPoint == null)
                {
                    mSecondPoint = SecondPoint.GetComponent<IWayPoint>();
                }

                return mSecondPoint;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            if(FirstPoint != null && SecondPoint != null)
            {
                var firstPoint = FirstPoint.transform.position + transform.up * 2;
                var secondPoint = SecondPoint.transform.position + transform.up * 2;

                Gizmos.DrawLine(firstPoint, secondPoint);
            }
        }

        void Awake()
        {
            var commonLevelHost = LevelCommonHostFactory.Get();
            commonLevelHost.HostNavigationRegistry.RegLinkOfWayPoints(this);

#if DEBUG
            //Debug.Log($"name = {name}");
            //Debug.Log($"FirstPoint?.name = {FirstPoint?.name}");
            //Debug.Log($"SecondPoint?.name = {SecondPoint?.name}");
#endif
#if DEBUG
            //Debug.Log($"(mFirstPoint == null) = {mFirstPoint == null}");
            //Debug.Log($"(mSecondPoint == null) = {mSecondPoint == null}");
#endif
        }
    }
}
