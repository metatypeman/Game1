using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class SelfLogicalObject : BaseAbstractLogicalObject
    {
        public SelfLogicalObject()
        {

        }

        public override bool IsConcrete => true;
        public override IList<ulong> CurrentEntitiesIdList => throw new NotImplementedException();
        public override ulong CurrentEntityId => 0ul;//tmp
        public override object this[ulong propertyKey]
        {
            get
            {
#if DEBUG
                LogInstance.Log($"SelfLogicalObject this get propertyKey = {propertyKey}");
#endif

                throw new NotImplementedException();
            }

            set
            {
#if DEBUG
                LogInstance.Log($"SelfLogicalObject this set propertyKey = {propertyKey} value = {value}");
#endif

                throw new NotImplementedException();
            }
        }

        public override object this[string propertyName]
        {
            get
            {
#if DEBUG
                LogInstance.Log($"SelfLogicalObject this get propertyName = {propertyName}");
#endif

                throw new NotImplementedException();
            }

            set
            {
#if DEBUG
                LogInstance.Log($"SelfLogicalObject this set propertyName = {propertyName} value = {value}");
#endif

                throw new NotImplementedException();
            }
        }
    }
}
