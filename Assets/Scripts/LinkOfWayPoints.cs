using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class LinkOfWayPoints : MonoBehaviour
    {
        public GameObject FirstPoint;
        public GameObject SecondPoint;

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
    }
}
