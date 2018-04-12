using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCHandHost
    {
        INPCProcess Send(INPCCommand command);
        object Get(string propertyName);
    }
}
