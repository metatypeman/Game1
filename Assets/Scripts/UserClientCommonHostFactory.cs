using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class UserClientCommonHostFactory
    {
        private static object mLockObj = new object();
        private static IUserClientCommonHost mUserClientCommonHost;

        public static IUserClientCommonHost Get()
        {
            lock (mLockObj)
            {
                if (mUserClientCommonHost != null)
                {
                    return mUserClientCommonHost;
                }

                var gameObjectOfCommonLevelHost = UnityEngine.Object.FindObjectOfType<UserClientCommonHost>();

                if (gameObjectOfCommonLevelHost == null)
                {
                    return null;
                }

                mUserClientCommonHost = gameObjectOfCommonLevelHost.GetComponent<IUserClientCommonHost>();
                return mUserClientCommonHost;
            }
        }
    }
}
