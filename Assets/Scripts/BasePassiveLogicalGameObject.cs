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
    public class BasePassiveLogicalGameObject: MonoBehaviour, IReadOnlyLogicalObject
    {
        protected BasePassiveLogicalGameObject(PassiveLogicalGameObjectOptions options = null)
        {
            if(options == null)
            {
                mOptions = new PassiveLogicalGameObjectOptions();
            }
            else
            {
                mOptions = options;
            }
        }

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

        private EntityLogger mEntityLogger = new EntityLogger();
        private PassiveLogicalGameObjectOptions mOptions;

        private HostLogicalObjectStorage mHostLogicalObjectStorage;
        private PassiveLogicalObject mPassiveLogicalObject;
        public ulong EntityId => mHostLogicalObjectStorage.EntityId;
        public object this[ulong propertyKey]
        {
            get
            {
                return mHostLogicalObjectStorage[propertyKey];
                //return mPassiveLogicalObject[propertyKey];
            }

            protected set
            {
                mHostLogicalObjectStorage[propertyKey] = value;
                //mPassiveLogicalObject[propertyKey] = value;
            }
        }

        public AccessPolicyToFact GetAccessPolicyToFact(ulong propertyKey)
        {
            return mPassiveLogicalObject.GetAccessPolicyToFact(propertyKey);
        }

        // Use this for initialization
        void Start()
        {
            mEntityLogger.Enabled = EnableLogging;
            mEntityLogger.Marker = Marker;

#if UNITY_EDITOR
            //Log("Begin");
#endif

            var commonLevelHost = LevelCommonHostFactory.Get();

            mHostLogicalObjectStorage = new HostLogicalObjectStorage(commonLevelHost.EntityDictionary);
            commonLevelHost.BusOfCGStorages.AddStorage(mHostLogicalObjectStorage);

            mPassiveLogicalObject = new PassiveLogicalObject(mEntityLogger, commonLevelHost.EntityDictionary, commonLevelHost.OldLogicalObjectsBus);

            var tmpGameObject = gameObject;
            var instanceId = tmpGameObject.GetInstanceID();

            mHostLogicalObjectStorage["name"] = tmpGameObject.name;

            mPassiveLogicalObject["name"] = tmpGameObject.name;

            if(mOptions.ShowGlobalPosition)
            {
                var position = VectorsConvertor.UnityToNumeric(tmpGameObject.transform.position);
                mHostLogicalObjectStorage["global position"] = position;
                mPassiveLogicalObject["global position"] = position;
            }

            OnInitFacts();

            commonLevelHost.OldLogicalObjectsBus.RegisterObject(instanceId, this);
        }

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
                //return mPassiveLogicalObject[propertyName];
            }

            set
            {
                mHostLogicalObjectStorage[propertyName] = value;
                //mPassiveLogicalObject[propertyName] = value;
            }
        }

        protected virtual void OnInitFacts()
        {
        }
    }
}
