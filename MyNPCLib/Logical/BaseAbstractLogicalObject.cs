using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public abstract class BaseAbstractLogicalObject
    {
        public static bool operator == (BaseAbstractLogicalObject item1, BaseAbstractLogicalObject item2)
        {
#if DEBUG
            LogInstance.Log("==");
#endif
            return true;
        }

        public static bool operator !=(BaseAbstractLogicalObject item1, BaseAbstractLogicalObject item2)
        {
#if DEBUG
            LogInstance.Log("!=");
#endif
            return !(item1 == item2);
        }
    }
}
