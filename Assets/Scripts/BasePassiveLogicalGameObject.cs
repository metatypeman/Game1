using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MyNPCLib.Logical;
using MyNPCLib;

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
        public string Marker = $"#{Guid.NewGuid().ToString("D")}";

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
        private PassiveLogicalObject mPassiveLogicalObject;
        public ulong EntityId => mPassiveLogicalObject.EntityId;
        public object this[ulong propertyKey]
        {
            get
            {
                return mPassiveLogicalObject[propertyKey];
            }

            protected set
            {
                mPassiveLogicalObject[propertyKey] = value;
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

            mPassiveLogicalObject = new PassiveLogicalObject(mEntityLogger, commonLevelHost.EntityDictionary, commonLevelHost.LogicalObjectsBus);

            var tmpGameObject = gameObject;
            var instanceId = tmpGameObject.GetInstanceID();

            mPassiveLogicalObject["name"] = tmpGameObject.name;

            if(mOptions.ShowGlobalPosition)
            {
                mPassiveLogicalObject["global position"] = VectorsConvertor.UnityToNumeric(tmpGameObject.transform.position);
            }

            OnInitFacts();

            commonLevelHost.LogicalObjectsBus.RegisterObject(instanceId, this);
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
                return mPassiveLogicalObject[propertyName];
            }

            set
            {
                mPassiveLogicalObject[propertyName] = value;
            }
        }

        protected virtual void OnInitFacts()
        {
        }
    }
}
