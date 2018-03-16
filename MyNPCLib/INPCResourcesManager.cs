using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCResourcesManager : IDisposable
    {
        INPCProcess Send(INPCCommand command);
    }
}
