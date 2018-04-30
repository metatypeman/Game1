namespace Assets.Scripts
{
    public static class LevelCommonHostFactory
    {
        public static ILevelCommonHost Get()
        {
            var gameObjectOfCommonLevelHost = UnityEngine.Object.FindObjectOfType<LevelCommonHost>();

            if(gameObjectOfCommonLevelHost == null)
            {
                return null;
            }

            return gameObjectOfCommonLevelHost.GetComponent<ILevelCommonHost>();
        }
    }
}
