using Assets.Scripts;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Common
{
    public class NPCHandHost: INPCHandHost
    {
        public NPCHandHost(IEntityLogger entityLogger, IInternalHumanoidHostContext intenalHostContext)
        {
            mEntityLogger = entityLogger;
            mInternalHumanoidHostContext = intenalHostContext;
        }

        private IEntityLogger mEntityLogger;

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            mEntityLogger?.Log(message);
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            mEntityLogger?.Error(message);
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            mEntityLogger?.Warning(message);
        }

        private IInternalHumanoidHostContext mInternalHumanoidHostContext;

        public INPCProcess Send(INPCCommand command)
        {
            //Log($"Begin command = {command}");

            if (mInternalHumanoidHostContext.RightHandThing != null)
            {
                return mInternalHumanoidHostContext.RightHandThing.Send(command);
            }

            var process = new NPCThingProcess(mEntityLogger);
            process.State = StateOfNPCProcess.Faulted;
            return process;
        }

        public object Get(string propertyName)
        {
#if DEBUG
            //Log($"propertyName = {propertyName}");
#endif
            if (mInternalHumanoidHostContext.RightHandThing != null)
            {
                return mInternalHumanoidHostContext.RightHandThing.Get(propertyName);
            }

            return null;
        }
    }
}
