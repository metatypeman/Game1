using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSoundModeling
{
    public class LogicalSoundBus
    {
        public void PublishSoundPackage(LogicalSoundPackage logicalSoundPackage)
        {
#if DEBUG
            LogInstance.Log($"logicalSoundPackage = {logicalSoundPackage}");
#endif

            throw new NotImplementedException();
        }
    }
}
