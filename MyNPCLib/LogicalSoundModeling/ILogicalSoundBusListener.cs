using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSoundModeling
{
    public delegate void OnLogicalSoundAction(OutputLogicalSoundPackage logicalSoundPackage);

    public interface ILogicalSoundBusListener
    {
        void ReceiveLogicalSound(OutputLogicalSoundPackage logicalSoundPackage);
    }
}
