using Assets.NPCScripts.Common.Logic.Processes;
using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.NPCScripts.Antagonist.Processes
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("key press")]
    public class AntagonistKeyListenerNPCProcess : CommonBaseNPCProcess
    {
        public static NPCCommand CreateCommand(KeyCode key)
        {
            var command = new NPCCommand();
            command.Name = "key press";
            command.AddParam(nameof(key), key);
            return command;
        }

        private void Main(KeyCode key)
        {
#if UNITY_EDITOR
            Log($"key = {key}");
#endif

            switch (key)
            {
                case KeyCode.B:
                    {
                        var rifle = Context.GetLogicalObject("{: name='M4A1 Sopmod' :}");

#if UNITY_EDITOR
                        Log($"rifle !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! = {rifle}");
#endif

                        //var tmpB = Context.GetLogicalObject("{: name='TrafficBarrierHazards (1)' :}");

                        //var tmpP = tmpB.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
                        //Log($"tmpP = {tmpP}");
#endif

                        if (rifle == null)
                        {
                            break;
                        }

                        var command = TakeFromSurfaceNPCProcess.CreateCommand(rifle);
                        Execute(command);
                    }
                    break;

                case KeyCode.L:
                    {
                        var command = SimpleAimNPCProcess.CreateCommand();
                        Execute(command);
                    }
                    break;

                case KeyCode.N:
                    {
                        var command = StartShootingNPCProcess.CreateCommand();
                        Execute(command);
                    }
                    break;

                case KeyCode.H:
                    {
                        var command = StopShootingNPCProcess.CreateCommand();
                        Execute(command);
                    }
                    break;
            }
        }
    }
}
