﻿using Assets.NPCScripts.Common;
using Assets.Scripts;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.NPCScripts.PixKeeper
{
    [RequireComponent(typeof(HumanoidBodyHost))]
    [RequireComponent(typeof(EnemyRayScaner))]
    public class PixKeeperNPC: MonoBehaviour
    {
        private PixKeeperNPCContext mNPCProcessesContext;

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

        public GameObject Gun;

        void Start()
        {
            mInvokingInMainThreadHelper = new InvokingInMainThreadHelper();

            var internalBodyHost = GetComponent<IInternalBodyHumanoidHost>();

            mInternalBodyHumanoidHost = internalBodyHost;

            internalBodyHost.OnReady += InternalBodyHost_OnReady;

            mUserClientCommonHost = UserClientCommonHostFactory.Get();

            mInputKeyHelper = new InputKeyHelper(mUserClientCommonHost);

            mInputKeyHelper.AddPressListener(KeyCode.J, OnPressAction);
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
                mNPCProcessesContext = new PixKeeperNPCContext(mEntityLogger, commonLevelHost.EntityDictionary, commonLevelHost.NPCProcessInfoCache, hostContext);

                if (Gun != null)
                {
                    var gunLogicalObject = Gun.GetComponent<RapidFireGun>();

                    var entityId = gunLogicalObject.EntityId;

#if DEBUG
                    Log($"entityId = {entityId}");
#endif

                    mNPCProcessesContext.BlackBoard.EntityIdOfInitRifle = entityId;
                    //mNPCProcessesContext.BlackBoard.Team = "Red";
                }

                //mNPCProcessesContext.BlackBoard.Team = "West";

                mNPCProcessesContext.Bootstrap();
            });
        }

        void Update()
        {
            //Log("Begin");
            mInputKeyHelper.Update();
            mInvokingInMainThreadHelper.Update();

#if DEBUG
            //var commonLevelHost = LevelCommonHostFactory.Get();

            //var planesList = commonLevelHost.HostNavigationRegistry.GetPlanesByPoint(transform.position);

            //Log($"planesList.Count = {planesList.Count}");
            //foreach(var plane in planesList)
            //{
            //    Log($"plane.Name = {plane.Name}");
            //}
#endif
        }

        private void OnPressAction(KeyCode key)
        {
            var command = KeyToNPCCommandConverter.Convert(key);
            Log($"command = {command}");
            mNPCProcessesContext?.Send(command);
        }

        void Stop()
        {
            mNPCProcessesContext?.Dispose();
        }
    }
}
