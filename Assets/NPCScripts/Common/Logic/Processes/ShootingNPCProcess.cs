//using Assets.Scripts;
//using MyNPCLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.NPCScripts.Common.Logic.Processes
//{
//    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
//    [NPCProcessName("shooting")]
//    public class ShootingNPCProcess: CommonBaseNPCProcess
//    {
//        public static NPCCommand CreateCommand()
//        {
//            var command = new NPCCommand();
//            command.Name = "shooting";
//            return command;
//        }

//        private void Main()
//        {
//#if UNITY_EDITOR
//            Log("Begin");
//#endif

//            Task.Run(() => {
//#if UNITY_EDITOR
//                Log("Reg");
//#endif

//                OnCanceledChanged += (INPCProcess sender) => {
//#if UNITY_EDITOR
//                    Log("OnCanceledChanged !!!!!");
//#endif

//                    var command = new NPCCommand();
//                    command.Name = "shoot off";

//                    ExecuteDefaultHand(command);

//#if UNITY_EDITOR
//                    Log("End OnCanceledChanged !!!!!");
//#endif
//                };
//            });

//            {
//                var command = new NPCCommand();
//                command.Name = "shoot on";

//                ExecuteDefaultHand(command);
//            }

//            Wait();

//#if UNITY_EDITOR
//            Log("End");
//#endif
//        }
//    }
//}
