using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IWayPoint
    {
        IList<IPlane> PlanesList { get; }
        Vector3 Position { get; }
        string Name { get; }
    }
}
