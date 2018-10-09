using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTests.Helpers
{
    public class EmptyEntityLogger: IEntityLogger
    {
        public void Log(string message) { }
        public void Error(string message) { }
        public void Warning(string message) { }
    }
}
