using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class MyGameObjectsBus
    {
        private static object mLockObj = new object();
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
            result.InstanceID = component.GetInstanceID();
            result.GameObject = component.gameObject;
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
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(MyGameObject)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(MyGameObject)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
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
