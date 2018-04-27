using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.TstSmartObj
{
    public class LogicalContext: ILogicalContext
    {
        public AbstractLogicalObject Get(string query)
        {
#if DEBUG
            NLog.LogManager.GetCurrentClassLogger().Info($"Get query = '{query}'");
#endif

            var tmpLogicalObject = new LogicalObject();

            return tmpLogicalObject;
        }
    }
}
