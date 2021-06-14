//using Assets.NPCScripts.Common;
using Assets.Scripts;
//using MyNPCLib;
//using MyNPCLib.Logical;
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
        //private AntagonistNPCContext mNPCProcessesContext;

        //private InputKeyHelper mInputKeyHelper;
        //private IInternalBodyHumanoidHost mInternalBodyHumanoidHost;
        //private IUserClientCommonHost mUserClientCommonHost;

        //private InvokingInMainThreadHelper mInvokingInMainThreadHelper;

        //private readonly object mEntityLoggerLockObj = new object();
        //private IEntityLogger mEntityLogger;

        //[MethodForLoggingSupport]
        //protected void Log(string message)
        //{
        //    lock (mEntityLoggerLockObj)
        //    {
        //        mEntityLogger?.Log(message);
        //    }
        //}

        //[MethodForLoggingSupport]
        //protected void Error(string message)
        //{
        //    lock (mEntityLoggerLockObj)
        //    {
        //        mEntityLogger?.Error(message);
        //    }
        //}

        //[MethodForLoggingSupport]
        //protected void Warning(string message)
        //{
        //    lock (mEntityLoggerLockObj)
        //    {
        //        mEntityLogger?.Warning(message);
        //    }
        //}

        public GameObject Gun;

//        void Start()
//        {
//            mInvokingInMainThreadHelper = new InvokingInMainThreadHelper();

//            var internalBodyHost = GetComponent<IInternalBodyHumanoidHost>();

//            mInternalBodyHumanoidHost = internalBodyHost;

//            internalBodyHost.OnReady += InternalBodyHost_OnReady;

//            mUserClientCommonHost = UserClientCommonHostFactory.Get();

//            mInputKeyHelper = new InputKeyHelper(mUserClientCommonHost);
//            mInputKeyHelper.AddPressListener(KeyCode.B, OnPressAction);
//            mInputKeyHelper.AddPressListener(KeyCode.L, OnPressAction);
//            mInputKeyHelper.AddPressListener(KeyCode.N, OnPressAction);
//            mInputKeyHelper.AddPressListener(KeyCode.H, OnPressAction);

//#if DEBUG
//            //var localPath = UnityEngine.Windows.Directory.localFolder;

//            //Debug.Log($"localPath = {localPath}");

//            //var roamingFolder = UnityEngine.Windows.Directory.roamingFolder;

//            //Debug.Log($"roamingFolder = {roamingFolder}");

//            //var temporaryFolder = UnityEngine.Windows.Directory.temporaryFolder;

//            //Debug.Log($"temporaryFolder = {temporaryFolder}");
//#endif
//        }

        //private void InternalBodyHost_OnReady()
        //{
        //    CreateNPCHostContext();
        //}

//        private void CreateNPCHostContext()
//        {
//            lock (mEntityLoggerLockObj)
//            {
//                mEntityLogger = mInternalBodyHumanoidHost.EntityLogger;
//            }

//            mInvokingInMainThreadHelper.CallInMainUI(() => {
//                var commonLevelHost = LevelCommonHostFactory.Get();
//#if DEBUG
//                Log($"(commonLevelHost == null) = {commonLevelHost == null}");
//#endif
//                var hostContext = new NPCHostContext(mEntityLogger, mInternalBodyHumanoidHost);
//                mNPCProcessesContext = new AntagonistNPCContext(mEntityLogger, commonLevelHost.EntityDictionary, commonLevelHost.NPCProcessInfoCache, hostContext);

//                if (Gun != null)
//                {
//                    var gunLogicalObject = Gun.GetComponent<RapidFireGun>();

//                    var entityId = gunLogicalObject.EntityId;

//#if DEBUG
//                    Log($"entityId = {entityId}");
//#endif

//                    mNPCProcessesContext.BlackBoard.EntityIdOfInitRifle = entityId;
//                    //mNPCProcessesContext.BlackBoard.Team = "Red";
//                }

//                mNPCProcessesContext.Bootstrap();
//            });
//        }

        //void Update()
        //{
        //    //Log("Begin");
        //    mInputKeyHelper.Update();
        //    mInvokingInMainThreadHelper.Update();
        //}

        //private void OnPressAction(KeyCode key)
        //{
        //    var command = KeyToNPCCommandConverter.Convert(key);
        //    Log($"command = {command}");
        //    mNPCProcessesContext?.Send(command);
        //}

        //void Stop()
        //{
        //    mNPCProcessesContext?.Dispose();
        //}
    }
}
