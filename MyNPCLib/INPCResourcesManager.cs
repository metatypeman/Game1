using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCResourcesManager
    {
        INPCProcess Send(INPCCommand command);
    }
}
