using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IHostNavigationRegistry
    {
        void RegPlane(IPlane plane);
        IList<IPlane> GetPlanesByPoint(Vector3 position);
        void RegWayPoint(IWayPoint wayPoint);
        void RegLinkOfWayPoints(ILinkOfWayPoints linkOfWayPoints);
    }
}
