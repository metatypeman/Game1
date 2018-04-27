using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.TstSmartObj
{
    public abstract class AbstractLogicalObject
    {
        public object this[string propertyName]
        {
            get
            {
                return 12;
            }        
        }

        public object this[string propertyName, int a]
        {
            get
            {
                return 15;
            }
        }

        public LogicalPropertyInfo GetProperty(string propertyName)
        {
            return new LogicalPropertyInfo();
        }

        public static bool operator == (AbstractLogicalObject item1, AbstractLogicalObject item2)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("==");

            return true;
        }

        public static bool operator != (AbstractLogicalObject item1, AbstractLogicalObject item2)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("!=");

            return !(item1 == item2);
        }
    }
}
