﻿using Assets.NPCScripts.Common;
using Assets.Scripts;
using MyNPCLib;
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
        private HipsterNPCContext mNPCProcessesContext;

        private InputKeyHelper mInputKeyHelper;
        private IInternalBodyHumanoidHost mInternalBodyHumanoidHost;
        private IUserClientCommonHost mUserClientCommonHost;

        private InvokingInMainThreadHelper mInvokingInMainThreadHelper;

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

        void Start()
        {
            mInvokingInMainThreadHelper = new InvokingInMainThreadHelper();

            var internalBodyHost = GetComponent<IInternalBodyHumanoidHost>();

            mInternalBodyHumanoidHost = internalBodyHost;

            internalBodyHost.OnReady += InternalBodyHost_OnReady;

            mUserClientCommonHost = UserClientCommonHostFactory.Get();

            mInputKeyHelper = new InputKeyHelper(mUserClientCommonHost);
            mInputKeyHelper.AddPressListener(KeyCode.F, OnPressAction);
            mInputKeyHelper.AddPressListener(KeyCode.G, OnPressAction);
            mInputKeyHelper.AddPressListener(KeyCode.H, OnPressAction);
            mInputKeyHelper.AddPressListener(KeyCode.J, OnPressAction);
            mInputKeyHelper.AddPressListener(KeyCode.K, OnPressAction);
            mInputKeyHelper.AddPressListener(KeyCode.L, OnPressAction);
        }

        private void OnPressAction(KeyCode key)
        {
            var command = KeyToNPCCommandConverter.Convert(key);
            Log($"command = {command}");
            mNPCProcessesContext?.Send(command);
        }

        private void InternalBodyHost_OnReady()
        {
            CreateNPCHostContext();
        }

        private void CreateNPCHostContext()
        {
            lock (mEntityLoggerLockObj)
            {
                mEntityLogger = mInternalBodyHumanoidHost.EntityLogger;
            }

            mInvokingInMainThreadHelper.CallInMainUI(() => {
                var commonLevelHost = LevelCommonHostFactory.Get();
#if DEBUG
                Log($"(commonLevelHost == null) = {commonLevelHost == null}");
#endif
                var hostContext = new NPCHostContext(mEntityLogger, mInternalBodyHumanoidHost);
                mNPCProcessesContext = new HipsterNPCContext(mEntityLogger, commonLevelHost.EntityDictionary, commonLevelHost.NPCProcessInfoCache, hostContext);

                mNPCProcessesContext.Bootstrap();
            });
        }

        void Update()
        {
            //Log("Begin");
            mInputKeyHelper.Update();
            mInvokingInMainThreadHelper.Update();
        }
    }
}
