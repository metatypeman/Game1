using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSoundModeling
{
    public class LogicalSoundBus
    {
        public void PublishSoundPackage(InputLogicalSoundPackage logicalSoundPackage)
        {
#if DEBUG
            LogInstance.Log($"logicalSoundPackage = {logicalSoundPackage}");
#endif

            throw new NotImplementedException();
        }

        public void AddListener(ILogicalSoundBusListener listener)
        {
#if DEBUG
            LogInstance.Log($"listener = {listener}");
#endif

            throw new NotImplementedException();
        }

        public void RemoveListener(ILogicalSoundBusListener listener)
        {
#if DEBUG
            LogInstance.Log($"listener = {listener}");
#endif

            throw new NotImplementedException();
        }
    }
}
