using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MyNPCLib.Logical;
using MyNPCLib;
using MyNPCLib.LogicalHostEnvironment;

namespace Assets.Scripts
{
    public class LogicalGameObject: MonoBehaviour, IReadOnlyLogicalObject
    {
        public bool EnableLogging = false;
        public string Marker = NamesHelper.CreateEntityName();

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

        public bool ShowGlobalPosition;

        private EntityLogger mEntityLogger = new EntityLogger();

        private HostLogicalObjectStorage mHostLogicalObjectStorage;
        public HostLogicalObjectStorage HostLogicalObjectStorage => mHostLogicalObjectStorage;

        public ulong EntityId => mHostLogicalObjectStorage.EntityId;
        public object this[ulong propertyKey]
        {
            get
            {
                return mHostLogicalObjectStorage[propertyKey];
            }

            protected set
            {
                mHostLogicalObjectStorage[propertyKey] = value;
            }
        }

        public List<FactValueItem> FactValueItemsList = new List<FactValueItem>();
        public List<string> FactQueriesList = new List<string>();

        void Awake()
        {
            mEntityLogger.Enabled = EnableLogging;
            mEntityLogger.Marker = Marker;

            var commonLevelHost = LevelCommonHostFactory.Get();
            LevelCommonHost = commonLevelHost;

            mHostLogicalObjectStorage = new HostLogicalObjectStorage(commonLevelHost.EntityDictionary);
            commonLevelHost.BusOfCGStorages.AddStorage(mHostLogicalObjectStorage);

            InitFacts();

            OnStart();
            OnInitFacts();

            commonLevelHost.OldLogicalObjectsBus.RegisterObject(gameObject.GetInstanceID(), mHostLogicalObjectStorage.EntityId);
        }

        // Use this for initialization
        void Start()
        {

        }

        private void InitFacts()
        {
#if UNITY_EDITOR
            Log($"FactValueItemsList.Count = {FactValueItemsList.Count}");
#endif

            var nameIsDefined = false;

            foreach(var item in FactValueItemsList)
            {
#if UNITY_EDITOR
                Log($"item = {item}");
#endif

                if(item.PropertyName.ToLower() == "name")
                {
                    nameIsDefined = true;
                }

                if(item.GameObjectValue == null)
                {
                    if(string.IsNullOrWhiteSpace(item.StringValue))
                    {
                        continue;
                    }
                    mHostLogicalObjectStorage[item.PropertyName] = item.StringValue;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            if (!nameIsDefined)
            {
                mHostLogicalObjectStorage["name"] = gameObject.name;
            }
            
            if (ShowGlobalPosition)
            {
                var position = VectorsConvertor.UnityToNumeric(gameObject.transform.position);
                mHostLogicalObjectStorage["global position"] = position;
            }
        }

        protected ILevelCommonHost LevelCommonHost { get; private set; }

        // Update is called once per frame
        void Update()
        {
            mEntityLogger.Enabled = EnableLogging;
            mEntityLogger.Marker = Marker;
        }

        protected object this[string propertyName]
        {
            get
            {
                return mHostLogicalObjectStorage[propertyName];
            }

            set
            {
                mHostLogicalObjectStorage[propertyName] = value;
            }
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnInitFacts()
        {
        }
    }
}
