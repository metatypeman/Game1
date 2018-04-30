using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class OldTrafficBarrierRedInfo : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            var gameInfo = MyGameObjectFactory.CreateByComponent(this);

#if UNITY_EDITOR
            Debug.Log($"OldTrafficBarrierRedInfo Start gameInfo = {gameInfo}");
#endif

            MyGameObjectsBus.RegisterObject(gameInfo);
        }
    }
}
