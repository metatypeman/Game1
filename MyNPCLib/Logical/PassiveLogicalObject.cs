using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class PassiveLogicalObject
    {
        public PassiveLogicalObject(IEntityDictionary entityDictionary)
        {
            mEntityDictionary = entityDictionary;

            var name = Guid.NewGuid().ToString("D");
            var entityId = mEntityDictionary.GetKey(name);
            mLogicalFrame = new LogicalFrame(entityId);
        }

        private IEntityDictionary mEntityDictionary;
        private LogicalFrame mLogicalFrame;

        public ulong EntityId => mLogicalFrame.EntityId;
        public object this[ulong propertyKey]
        {
            get
            {
                return mLogicalFrame[propertyKey];
            }

            set
            {
                mLogicalFrame[propertyKey] = value;
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
                mLogicalFrame[propertyKey] = value;
            }
        }
    }
}
