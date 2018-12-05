using Assets.Scripts;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.NPCScripts.Antagonist
{
    [RequireComponent(typeof(HumanoidBodyHost))]
    [RequireComponent(typeof(EnemyRayScaner))]
    public class AntagonistNPC: MonoBehaviour
    {
        private AntagonistNPCContext mNPCProcessesContext;

        private InputKeyHelper mInputKeyHelper;
        private IInternalBodyHumanoidHost mInternalBodyHumanoidHost;
        private IUserClientCommonHost mUserClientCommonHost;

        private readonly object mEntityLoggerLockObj = new object();
        private IEntityLogger mEntityLogger;

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            lock (mEntityLoggerLockObj)
            {
                mEntityLogger?.Log(message);
            }
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            lock (mEntityLoggerLockObj)
            {
                mEntityLogger?.Error(message);
            }
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            lock (mEntityLoggerLockObj)
            {
                mEntityLogger?.Warning(message);
            }
        }


    }
}
