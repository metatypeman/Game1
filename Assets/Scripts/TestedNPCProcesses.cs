using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class TestedBootNPCProcess: TestedBaseNPCProcess
    {
        protected override void Awake()
        {
#if UNITY_EDITOR
            Debug.Log("TestedBootNPCProcess Awake");
#endif
        }

        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedBootNPCProcess Main");
#endif
        }
    }
}
