using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IPlane
    {
        bool Contains(Vector3 position);
        Vector3 FRPoint { get; }
        Vector3 BRPoint { get; }
        Vector3 FLPoint { get; }
        Vector3 BLPoint { get; }
        string Name { get; }
        IList<IWayPoint> PointsList { get; }
    }
}
