using MyNPCLib;
using MyNPCLib.CGStorage;
using MyNPCLib.Logical;
using MyNPCLib.LogicalHostEnvironment;
using MyNPCLib.LogicalSoundModeling;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TmpSandBox.NPCBehaviour
{
    public class StubOfNPCBodyHost: INPCBodyHost
    {
        private StatesOfHumanoidBodyController mStates = new StatesOfHumanoidBodyController();

        public IStatesOfHumanoidBodyHost States
        {
            get
            {
                return mStates;
            }
        }

        public event HumanoidStatesChangedAction OnHumanoidStatesChanged;

        public event Action OnDie;

        //private object mLockObj { get; set; } = new object();
        //private HumanoidTaskOfExecuting mTargetStateForExecuting { get; set; }

        public void Execute(TargetStateOfHumanoidBody targetState)
        {
#if DEBUG
            //NLog.LogManager.GetCurrentClassLogger().Info($"ExecuteAsync targetState = {targetState}");
#endif

            Thread.Sleep(1000);
        }

        public void CallInMainUI(Action function)
        {
            function();
        }

        public TResult CallInMainUI<TResult>(Func<TResult> function)
        {
            return function();
        }

        public bool IsReady => true;
        public event Action OnReady;
    }

    public class StubOfNPCHandHost: INPCHandHost
    {
        public StubOfNPCHandHost(IEntityLogger entityLogger)
        {
            mEntityLogger = entityLogger;
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

        public INPCProcess Send(INPCCommand command)
        {
            Log($"Beging command = {command}");

            var process = new NPCThingProcess(mEntityLogger);
            process.State = StateOfNPCProcess.Running;

            Task.Run(() => {
                try
                {
                    Log($"Task.Run command = {command}");

                    process.State = StateOfNPCProcess.Running;

                    Thread.Sleep(1000);

                    process.State = StateOfNPCProcess.RanToCompletion;

                    Log($"Task.Run command = {command}");
                }
                catch(Exception e)
                {
                    Error($"Task.Run e = {e}");
                }
            });

            Log($"End command = {command}");

            return process;
        }

        public object Get(string propertyName)
        {
            Log($"propertyName = {propertyName}");

            return "The Beatles!!!";
        }
    }

    public class StubOfNPCHostContext: INPCHostContext
    {
        public StubOfNPCHostContext(IEntityLogger entityLogger, IEntityDictionary entityDictionary = null)
        {
            mEntityLogger = entityLogger;

            if (entityDictionary == null)
            {
                entityDictionary = new EntityDictionary();
            }

            mBodyHost = new StubOfNPCBodyHost();
            mRightHandHost = new StubOfNPCHandHost(entityLogger);
            mLeftHandHost = new StubOfNPCHandHost(entityLogger);
            
            mHostLogicalObjectStorage = new HostLogicalObjectStorage(entityDictionary);
            mBusOfCGStorages = new BusOfCGStorages(entityDictionary);
            mBusOfCGStorages.AddStorage(mHostLogicalObjectStorage);
        }

        private IEntityLogger mEntityLogger;
        private StubOfNPCBodyHost mBodyHost;
        private StubOfNPCHandHost mRightHandHost;
        private StubOfNPCHandHost mLeftHandHost;
        
        public INPCBodyHost BodyHost => mBodyHost;
        public INPCHandHost RightHandHost => mRightHandHost;
        public INPCHandHost LeftHandHost => mLeftHandHost;
        
        private HostLogicalObjectStorage mHostLogicalObjectStorage;
        public ICGStorage SelfHostStorage => mHostLogicalObjectStorage.GeneralHost;

        private BusOfCGStorages mBusOfCGStorages;
        public IBusOfCGStorages BusOfCGStorages => mBusOfCGStorages;

        public ulong SelfEntityId => mHostLogicalObjectStorage.EntityId;
        public bool IsReady => mBodyHost.IsReady;
        public event Action OnReady;

        public event OnLogicalSoundAction OnLogicalSound;

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

        public IList<IHostVisionObject> VisibleObjects
        {
            get
            {
                Log("Begin");

                var result = new List<IHostVisionObject>();

                var item = new HostVisionObject();
                item.EntityId = 2;

                result.Add(item);
                item.VisionItems = new List<IVisionItem>();

                var visionItem = new VisionItem();
                visionItem.Point = new Vector3(1, 1, 1);
                visionItem.Distance = 12f;
                visionItem.LocalDirection = new Vector3(1, 1, 1);

                item.VisionItems.Add(visionItem);

                visionItem = new VisionItem();
                visionItem.Point = new Vector3(2, 2, 2);
                visionItem.Distance = 12f;
                visionItem.LocalDirection = new Vector3(2, 2, 2);

                item.VisionItems.Add(visionItem);

                return result;
            }
        }

        public Vector3 GlobalPosition
        {
            get
            {
                return new Vector3(10, 11, 12);
            }
        }
    }
}
