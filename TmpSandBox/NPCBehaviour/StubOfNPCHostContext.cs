using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
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
        public INPCProcess Send(INPCCommand command)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Beging Send command = {command}");

            var process = new NPCThingProcess();
            process.State = StateOfNPCProcess.Running;

            Task.Run(() => {
                try
                {
                    NLog.LogManager.GetCurrentClassLogger().Info($"Send Task.Run command = {command}");

                    process.State = StateOfNPCProcess.Running;

                    Thread.Sleep(1000);

                    process.State = StateOfNPCProcess.RanToCompletion;

                    NLog.LogManager.GetCurrentClassLogger().Info($"Send Task.Run command = {command}");
                }
                catch(Exception e)
                {
                    NLog.LogManager.GetCurrentClassLogger().Info($"Send Task.Run e = {e}");
                }
            });

            NLog.LogManager.GetCurrentClassLogger().Info($"End Send command = {command}");

            return process;
        }

        public object Get(string propertyName)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Get propertyName = {propertyName}");

            return "The Beatles!!!";
        }
    }

    public class StubOfNPCHostContext: INPCHostContext
    {
        public StubOfNPCHostContext(IEntityDictionary entityDictionary = null)
        {
            if(entityDictionary == null)
            {
                entityDictionary = new EntityDictionary();
            }

            mBodyHost = new StubOfNPCBodyHost();
            mRightHandHost = new StubOfNPCHandHost();
            mLeftHandHost = new StubOfNPCHandHost();
            LogicalIndexStorageImpl = new LogicalIndexStorage();
            mSelfLogicalObject = new PassiveLogicalObject(entityDictionary, LogicalIndexStorageImpl);
            LogicalIndexStorageImpl.RegisterObject(mSelfLogicalObject);
        }

        private StubOfNPCBodyHost mBodyHost;
        private StubOfNPCHandHost mRightHandHost;
        private StubOfNPCHandHost mLeftHandHost;
        public LogicalIndexStorage LogicalIndexStorageImpl { get; private set; }
        private PassiveLogicalObject mSelfLogicalObject;

        public INPCBodyHost BodyHost => mBodyHost;
        public INPCHandHost RightHandHost => mRightHandHost;
        public INPCHandHost LeftHandHost => mLeftHandHost;
        public ILogicalStorage HostLogicalStorage => LogicalIndexStorageImpl;
        public ulong SelfEntityId => mSelfLogicalObject.EntityId;
        public bool IsReady => mBodyHost.IsReady;
        public event Action OnReady;

        public IList<IHostVisionObject> VisibleObjects
        {
            get
            {
                NLog.LogManager.GetCurrentClassLogger().Info("VisibleObjects");

                var result = new List<IHostVisionObject>();

                var item = new HostVisionObject();
                item.EntityId = 2;

                result.Add(item);
                item.VisionItems = new List<IVisionItem>();

                return result;
            }
        }
    }
}
