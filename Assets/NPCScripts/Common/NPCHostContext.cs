using Assets.Scripts;
using MyNPCLib;
using MyNPCLib.CGStorage;
using MyNPCLib.Logical;
using MyNPCLib.LogicalHostEnvironment;
using MyNPCLib.LogicalSoundModeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Common
{
    public class NPCHostContext: INPCHostContext
    {
        public NPCHostContext(IEntityLogger entityLogger, IInternalBodyHumanoidHost internalBodyHumanoidHost)
        {
            mEntityLogger = entityLogger;

            mInternalBodyHumanoidHost = internalBodyHumanoidHost;
            mInternalHumanoidHostContext = new InternalHumanoidHostContext();
            mInternalBodyHumanoidHost.OnLogicalSound += MInternalBodyHumanoidHost_OnLogicalSound;

            mBodyHost = new NPCBodyHost(entityLogger, mInternalHumanoidHostContext, internalBodyHumanoidHost);
            mBodyHost.OnReady += MBodyHost_OnReady;

            mRightHandHost = new NPCHandHost(entityLogger, mInternalHumanoidHostContext);
            mLeftHandHost = new NPCHandHost(entityLogger, mInternalHumanoidHostContext);
        }

        private IEntityLogger mEntityLogger;

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            mEntityLogger?.Log(message);
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            mEntityLogger?.Error(message);
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            mEntityLogger?.Warning(message);
        }

        private void MBodyHost_OnReady()
        {
#if DEBUG
            //Log("Begin");
#endif

            Task.Run(() => {
                try
                {
                    mOnReady?.Invoke();
                }
                catch (Exception e)
                {
#if DEBUG
                    Error($"e = {e}");
#endif
                }
            });
        }

        private IInternalBodyHumanoidHost mInternalBodyHumanoidHost;
        private InternalHumanoidHostContext mInternalHumanoidHostContext;
        private NPCBodyHost mBodyHost;
        private NPCHandHost mRightHandHost;
        private NPCHandHost mLeftHandHost;

        public INPCBodyHost BodyHost => mBodyHost;
        public INPCHandHost RightHandHost => mRightHandHost;
        public INPCHandHost LeftHandHost => mLeftHandHost;
        public ICGStorage SelfHostStorage => mInternalBodyHumanoidHost.SelfHostStorage;
        public IBusOfCGStorages BusOfCGStorages => mInternalBodyHumanoidHost.BusOfCGStorages;
        public ulong SelfEntityId => mInternalBodyHumanoidHost.SelfEntityId;
        public bool IsReady => mBodyHost.IsReady;
        private event Action mOnReady;
        public event Action OnReady
        {
            add
            {
                mOnReady += value;
                if (mBodyHost.IsReady)
                {
                    Task.Run(() => {
                        try
                        {
#if DEBUG
                            //Log($"value == null = ({value == null}) value = {value}");
                            if (value == null)
                            {
                                throw new ArgumentNullException(nameof(value));
                            }
#endif
                            value();
                        }
                        catch (Exception e)
                        {
#if DEBUG
                            Error($"e = {e}");
#endif
                        }
                    });
                }
            }

            remove
            {
                mOnReady -= value;
            }
        }

        public IList<IHostVisionObject> VisibleObjects => mInternalBodyHumanoidHost.VisibleObjects;
        public Vector3 GlobalPosition => mInternalBodyHumanoidHost.GlobalPosition;

        private void MInternalBodyHumanoidHost_OnLogicalSound(OutputLogicalSoundPackage logicalSoundPackage)
        {
#if DEBUG
            //Log($"logicalSoundPackage = {logicalSoundPackage}");
#endif

            Task.Run(() => {
                try
                {
                    OnLogicalSound?.Invoke(logicalSoundPackage);
                }
                catch (Exception e)
                {
#if DEBUG
                    Error($"e = {e}");
#endif
                }
            });
        }

        public event OnLogicalSoundAction OnLogicalSound;
    }
}
