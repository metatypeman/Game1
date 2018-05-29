using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IEntityLogger
    {
        void Log(string message);
        void Error(string message);
        void Warning(string message);
    }
}
