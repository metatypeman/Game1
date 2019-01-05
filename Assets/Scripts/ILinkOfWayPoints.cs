using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface ILinkOfWayPoints
    {
        IWayPoint FirstPoint { get; }
        IWayPoint SecondPoint { get; }
    }
}
