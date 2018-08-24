using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public class ContextOfCGStorage
    {
        public ContextOfCGStorage(IEntityDictionary entityDictionary)
        {
            mEntityDictionary = entityDictionary;
        }

        private IEntityDictionary mEntityDictionary;

        public IEntityDictionary EntityDictionary => mEntityDictionary;

        private GlobalCGStorage mGlobalCGStorage;

        public GlobalCGStorage GlobalCGStorage => mGlobalCGStorage;

        public ICGStorage MainCGStorage => mGlobalCGStorage;

        public void Init()
        {
            mGlobalCGStorage = new GlobalCGStorage(this);
        }
    }
}
