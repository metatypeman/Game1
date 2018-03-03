using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IAimCorrector
    {
        float GetCorrectingAngle(Vector3 targetPos);
    }

    public interface ITargetOfShoot
    {
        void SetHit(RaycastHit shootHit, int damagePerShot);
    }

    public enum FireMode
    {
        Single,
        Multiple
    }

    public enum TurnState
    {
        On,
        Off
    }

    public enum InternalStateOfRapidFireGun
    {
        TurnedOf,
        TurnedOnShot,
        TurnedOnWasShot,
        BeforeOffIfSingle
    }

    public interface IRapidFireGun : IAimCorrector, IHandThing
    {
        bool UseDebugLine { get; set; }
        FireMode FireMode { get; set; }
        TurnState TurnState { get; set; }
        event Action OnFire;
    }
}
