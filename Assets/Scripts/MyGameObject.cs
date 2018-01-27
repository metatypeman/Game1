using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class MyGameObject : IObjectToString
    {
        public int InstanceID { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }

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
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(InstanceID)} = {InstanceID}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Tag)} = {Tag}");
            return sb.ToString();
        }
    }
}
