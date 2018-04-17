using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    public class TestedBootNPCProcess: TestedBaseNPCProcess
    {
        protected override void Awake()
        {
#if UNITY_EDITOR
            Debug.Log("TestedBootNPCProcess Awake");
#endif
        }

        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedBootNPCProcess Main");
#endif
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("key press")]
    public class TestedKeyListenerProcess : TestedBaseNPCProcess
    {
        private void Main(KeyCode key)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedKeyListenerProcess Main key = {key}");
#endif

            switch(key)
            {
                case KeyCode.F:
                    break;
                    
                case KeyCode.G:
                    break;
                    
                case KeyCode.K:
                    break;
                    
                case KeyCode.N:
                    break;
                    
                case KeyCode.H:
                    break;
                    
                case KeyCode.L:
                    break;
                    
                case KeyCode.I:
                    break;
                    
                case KeyCode.P:
                    break;
        
                case KeyCode.U:
                    var command = new NPCCommand();
                    command.Name = "go to enemy base";
                    Execute(command);
                    break;
                    
                case KeyCode.M:
                    break;
                    
                case KeyCode.B:
                    break;
                    
                case KeyCode.J:
                    break;
                    
                case KeyCode.Q:
                    break;
            }
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.NewStandaloneInstance)]
    [NPCProcessName("go to enemy base")]
    public class TestedGoToEnemyBaseProcess : TestedBaseNPCProcess
    {
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedGoToEnemyBaseProcess Main");
#endif

            var targetWayPoint = WaypointsBus.GetByTag("enemy military base");

            if (targetWayPoint != null)
            {
                var moveCommand = new HumanoidHStateCommand();
                moveCommand.State = HumanoidHState.Walk;
                moveCommand.TargetPosition = targetWayPoint.Position;

#if UNITY_EDITOR
                Debug.Log($"TestedGoToEnemyBaseProcess OnRun moveCommand = {moveCommand}");
#endif

                var childProcess = ExecuteBody(moveCommand);
                Wait(childProcess);
            }

            var l = new List<int>();
        }
    }
}
