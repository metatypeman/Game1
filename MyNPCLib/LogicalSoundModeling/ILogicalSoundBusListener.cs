using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSoundModeling
{
    public interface ILogicalSoundBusListener
    {
        void OnLogicalSound(OutputLogicalSoundPackage logicalSoundPackage);
    }
}
