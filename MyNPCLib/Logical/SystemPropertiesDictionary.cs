using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class SystemPropertiesDictionary
    {
        public SystemPropertiesDictionary(IEntityDictionary entityDictionary)
        {
            mEntityDictionary = entityDictionary;

            Init();
        }

        private readonly IEntityDictionary mEntityDictionary;
        private readonly Dictionary<ulong, KindOfSystemProperties> mPropertiesMap = new Dictionary<ulong, KindOfSystemProperties>();

        private void Init()
        {
            var globalPositionName = "global position";

            mPropertiesMap[mEntityDictionary.GetKey(globalPositionName)] = KindOfSystemProperties.GlobalPosition;
        }

        public KindOfSystemProperties GetKindOfSystemProperty(ulong propertyKey)
        {
#if DEBUG
            //LogInstance.Log($"propertyKey = {propertyKey}");
#endif

            if(mPropertiesMap.ContainsKey(propertyKey))
            {
                return mPropertiesMap[propertyKey];
            }

            return KindOfSystemProperties.Undefined;
        }
    }
}
