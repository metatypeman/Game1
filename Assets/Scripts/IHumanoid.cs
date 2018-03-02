using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IHumanoid : IObject
    {
        GameObject RightHand { get; }
        GameObject RightHandWP { get; }
        GameObject LeftHand { get; }
        GameObject LeftHandWP { get; }
    }
}
