using MyNPCLib;
using MyNPCLib.LogicalSoundModeling;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace TmpSandBox
{
    public class TSTFakeLogicalSoundBusListener: ILogicalSoundBusListener
    {
        public TSTFakeLogicalSoundBusListener(Vector3 position)
        {
            mPosition = position;

#if DEBUG
            LogInstance.Log($"mPosition = {mPosition}");
#endif
        }

        private Vector3 mPosition = Vector3.Zero;

        public void ReceiveLogicalSound(OutputLogicalSoundPackage logicalSoundPackage)
        {
#if DEBUG
            LogInstance.Log($"logicalSoundPackage = {logicalSoundPackage}");
#endif
        }
    }
}
