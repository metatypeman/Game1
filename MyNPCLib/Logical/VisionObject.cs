using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class VisionObject: BaseAbstractLogicalObject
    {
        public override bool IsConcrete => true;
        public override IList<ulong> CurrentEntitiesIdList => throw new NotImplementedException();
        public override ulong CurrentEntityId => 0ul;//tmp
        public override object this[ulong propertyKey]
        {
            get
            {
#if DEBUG
                LogInstance.Log($"VisionObject this get propertyKey = {propertyKey}");
#endif

                throw new NotImplementedException();
            }

            set
            {
#if DEBUG
                LogInstance.Log($"VisionObject this set propertyKey = {propertyKey} value = {value}");
#endif

                throw new NotImplementedException();
            }
        }

        public override object this[string propertyName]
        {
            get
            {
#if DEBUG
                LogInstance.Log($"VisionObject this get propertyName = {propertyName}");
#endif

                throw new NotImplementedException();
            }

            set
            {
#if DEBUG
                LogInstance.Log($"VisionObject this set propertyName = {propertyName} value = {value}");
#endif

                throw new NotImplementedException();
            }
        }
    }
}
