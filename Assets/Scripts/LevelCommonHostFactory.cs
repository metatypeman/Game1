//namespace Assets.Scripts
//{
//    public static class LevelCommonHostFactory
//    {
//        private static object mLockObj = new object();
//        private static ILevelCommonHost mLevelCommonHost;

//        public static ILevelCommonHost Get()
//        {
//            lock(mLockObj)
//            {
//                if(mLevelCommonHost != null)
//                {
//                    return mLevelCommonHost;
//                }

//                var gameObjectOfCommonLevelHost = UnityEngine.Object.FindObjectOfType<LevelCommonHost>();

//                if (gameObjectOfCommonLevelHost == null)
//                {
//                    return null;
//                }

//                mLevelCommonHost = gameObjectOfCommonLevelHost.GetComponent<ILevelCommonHost>();
//                return mLevelCommonHost;
//            }
//        }
//    }
//}
