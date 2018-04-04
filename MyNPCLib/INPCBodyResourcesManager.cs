using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCBodyResourcesManager : IDisposable
    {
        INPCProcess Send(IHumanoidBodyCommand command);
        void Bootstrap();
    }
}
