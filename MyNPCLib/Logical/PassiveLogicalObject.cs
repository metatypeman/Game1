using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class PassiveLogicalObject: IReadOnlyLogicalObject
    {
        public PassiveLogicalObject(IEntityDictionary entityDictionary, ILogicalStorage logicalIndexingBus)
        {
            mEntityDictionary = entityDictionary;
            mLogicalIndexingBus = logicalIndexingBus;

            var name = Guid.NewGuid().ToString("D");
            mEntityId = mEntityDictionary.GetKey(name);
            mLogicalFrame = new LogicalFrame(mEntityId);
        }

        private IEntityDictionary mEntityDictionary;
        private ILogicalStorage mLogicalIndexingBus;
        private LogicalFrame mLogicalFrame;
        private ulong mEntityId;

        public ulong EntityId => mEntityId;
        public object this[ulong propertyKey]
        {
            get
            {
                return mLogicalFrame[propertyKey];
            }

            set
            {
                SetProperty(propertyKey, value);
            }
        }

        public object this[string propertyName]
        {
            get
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);
                return mLogicalFrame[propertyKey];
            }

            set
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);
                SetProperty(propertyKey, value);
            }
        }

        private void SetProperty(ulong propertyKey, object value)
        {
            mLogicalFrame[propertyKey] = value;
            mLogicalIndexingBus.PutPropertyValue(mEntityId, propertyKey, value);
        }
    }
}
