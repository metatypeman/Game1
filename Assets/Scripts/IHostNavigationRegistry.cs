using MyNPCLib;
using MyNPCLib.NavigationSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IHostNavigationRegistry: INavigationRegistry
    {
        void RegPlane(IPlane plane);
        IList<IPlane> GetPlanesByPoint(Vector3 position);
        void RegWayPoint(IWayPoint wayPoint);
        void RegLinkOfWayPoints(ILinkOfWayPoints linkOfWayPoints);
        //IRoute GetRouteForPosition(IPointInfo pointInfo);
        //IRoute GetRouteForPosition(Vector3 startPosition, Vector3 targetPosition);
    }
}
