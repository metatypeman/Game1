using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib.LogicalSoundModeling
{
    public class LogicalSoundBus
    {
        public void PushSoundPackage(InputLogicalSoundPackage logicalSoundPackage)
        {
#if DEBUG
            //LogInstance.Log($"logicalSoundPackage = {logicalSoundPackage}");
#endif

            List<ILogicalSoundBusListener> listenersList = null;

            lock (mListenersListLockObj)
            {
                listenersList = mListenersList.ToList();
            }

#if DEBUG
            //LogInstance.Log($"listenersList.Count = {listenersList.Count}");
#endif

            foreach(var listener in listenersList)
            {
                Task.Run(() => {

                    var outputLogicalSoundPackage = new OutputLogicalSoundPackage(logicalSoundPackage.Position, logicalSoundPackage.Power, logicalSoundPackage.LogicalClases, logicalSoundPackage.SoundFactsDataSource);

#if DEBUG
                    //LogInstance.Log($"outputLogicalSoundPackage = {outputLogicalSoundPackage}");
#endif

                    listener.ReceiveLogicalSound(outputLogicalSoundPackage);
                });
            }      
        }

        private readonly object mListenersListLockObj = new object();
        private List<ILogicalSoundBusListener> mListenersList = new List<ILogicalSoundBusListener>();

        public void AddListener(ILogicalSoundBusListener listener)
        {
#if DEBUG
            //LogInstance.Log($"listener = {listener}");
#endif
            if(listener == null)
            {
                return;
            }

            lock(mListenersListLockObj)
            {
                if(mListenersList.Contains(listener))
                {
                    return;
                }

                mListenersList.Add(listener);
            }
        }

        public void RemoveListener(ILogicalSoundBusListener listener)
        {
#if DEBUG
            //LogInstance.Log($"listener = {listener}");
#endif
            if (listener == null)
            {
                return;
            }

            lock (mListenersListLockObj)
            {
                if (mListenersList.Contains(listener))
                {
                    mListenersList.Remove(listener);
                }
            }
        }
    }
}
