using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class LogProxyForDebug : ILogProxy
    {
        public void Log(string message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }

        public void Error(string message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
        }
        public void Warning(string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }
    }
}
