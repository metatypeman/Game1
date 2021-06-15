//using Assets.NPCScripts.Common;
using Assets.Scripts;
//using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.NPCScripts.Hipster
{
    [RequireComponent(typeof(HumanoidBodyHost))]
    [RequireComponent(typeof(EnemyRayScaner))]
    public class HipsterNPC: MonoBehaviour
    {
        private InputKeyHelper mInputKeyHelper;
        
        private IUserClientCommonHost mUserClientCommonHost;

        private InvokingInMainThreadHelper mInvokingInMainThreadHelper;
    }
}
