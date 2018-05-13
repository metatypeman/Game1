using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MyNPCLib.Logical;

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

        // Use this for initialization
        void Start()
        {
            //var gameInfo = MyGameObjectFactory.CreateByComponent(this);

#if UNITY_EDITOR
            //Debug.Log("BasePassiveLogicalGameObject Start");
#endif

            var commonLevelHost = LevelCommonHostFactory.Get();

            mPassiveLogicalObject = new PassiveLogicalObject(commonLevelHost.EntityDictionary, commonLevelHost.LogicalObjectsBus);

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
