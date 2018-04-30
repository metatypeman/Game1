using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MyNPCLib;

namespace Assets.Scripts
{
    public static class MyGameObjectsBus
    {
        private static readonly object mLockObj = new object();
        private static Dictionary<int, MyGameObject> mObjectsDict = new Dictionary<int, MyGameObject>();

        public static void RegisterObject(MyGameObject value)
        {
            lock(mLockObj)
            {
                mObjectsDict[value.InstanceID] = value;
            }
        }

        public static MyGameObject GetObject(int instanceId)
        {
            lock (mLockObj)
            {
                if(mObjectsDict.ContainsKey(instanceId))
                {
                    return mObjectsDict[instanceId];
                }

                return null;
            }
        }
    }

    public static class MyGameObjectFactory
    {
        public static MyGameObject CreateByComponent(Component component, params Type[] targetComponents)
        {
#if UNITY_EDITOR
            //Debug.Log($"MyGameObjectFactory CreateByComponent targetComponents.Length = {targetComponents.Length}");
#endif

            var result = new MyGameObject();
            var tmpTransform = component.transform;
            var gameObject = component.gameObject;
            result.InstanceID = gameObject.GetInstanceID();
            result.GameObject = gameObject;
            result.Name = tmpTransform.name;
            result.Tag = tmpTransform.tag;
            foreach(var targetComponentType in targetComponents)
            {
                var targetComponent = component.GetComponent(targetComponentType);

                if(targetComponent == null)
                {
                    targetComponent = component.GetComponentInChildren(targetComponentType);
                }

                if(targetComponent == null)
                {
                    continue;
                }

#if UNITY_EDITOR
                //Debug.Log($"MyGameObjectFactory CreateByComponent (targetComponent == null) = {targetComponent == null}");
                //Debug.Log($"MyGameObjectFactory CreateByComponent targetComponentType.FullName = {targetComponentType.FullName}");
#endif

                result.RegisterInstance(targetComponent, targetComponentType);
            }
            return result;
        }
    }

    public class MyGameObject : IObjectToString
    {
        public int InstanceID { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public dynamic DynamicData { get; set; } = new ExpandoObject();
        public GameObject GameObject { get; set; }
        private NPCSimpleDI mSimpleDI = new NPCSimpleDI();
        public void RegisterInstance<T>(object instance) where T : class
        {
            RegisterInstance(instance, typeof(T));
        }

        public void RegisterInstance(object instance, params Type[] types)
        {
            mSimpleDI.RegisterInstance(instance, types);
        }

        public void RemoveInstance<T>() where T : class
        {
            mSimpleDI.RemoveInstance<T>();
        }

        public T GetInstance<T>() where T : class
        {
            return mSimpleDI.GetInstance<T>();
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(InstanceID)} = {InstanceID}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Tag)} = {Tag}");
            if(DynamicData == null)
            {
                sb.AppendLine($"{spaces}{nameof(DynamicData)} = null");
            }else{
                var dynamicDict = DynamicData as IDictionary<string, object>;
                sb.AppendLine($"{spaces}Begin {nameof(DynamicData)}");
                foreach(var dynamicKVPItem in dynamicDict)
                {
                    sb.AppendLine($"{nextSpaces}DinamicPropName = {dynamicKVPItem.Key}; DinamicPropValue = {dynamicKVPItem.Value}");
                }
                sb.AppendLine($"{spaces}End {nameof(DynamicData)}");
            }
            return sb.ToString();
        }
    }
}
