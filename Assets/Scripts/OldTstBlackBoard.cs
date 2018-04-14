using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class OldTstBlackBoard
    {
        public RapidFireGunProxy RapidFireGunProxy { get; set; } = new RapidFireGunProxy();
        public event Action OnGunHasTaken;

        public void Tst()
        {
#if UNITY_EDITOR
            Debug.Log("Begin OldTstBlackBoard Tst");
#endif
        }
    }
}
