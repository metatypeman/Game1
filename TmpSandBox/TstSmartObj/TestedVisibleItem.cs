using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.TstSmartObj
{
    public class TestedVisibleItem : AbstractLogicalObject
    {
        /*public static bool operator == (TestedVisibleItem item1, ILogicalObject item2)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("==");

            return true;
        }

        public static bool operator != (TestedVisibleItem item1, ILogicalObject item2)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("!=");

            return !(item1 == item2);
        }*/
    }

    public abstract class A
    {
        public static bool operator == (A item1, A item2)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("==");

            return true;
        }

        public static bool operator != (A item1, A item2)
        {
            NLog.LogManager.GetCurrentClassLogger().Info("!=");

            return !(item1 == item2);
        }
    }

    public class B: A
    {
    }

    public class C: A
    {
    }
}
