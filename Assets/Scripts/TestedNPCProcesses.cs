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

    TestedInspectingNPCProcess 
    //OldTstInspectingProcess
    {
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("key press")]
    public class TestedKeyListenerNPCProcess : TestedBaseNPCProcess
    {
        private void Main(KeyCode key)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedKeyListenerNPCProcess Main key = {key}");
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
    public class TestedGoToEnemyBaseNPCProcess : TestedBaseNPCProcess
    //OldTstGoToEnemyBaseProcess
    {
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedGoToEnemyBaseNPCProcess Main");
#endif

            var targetWayPoint = WaypointsBus.GetByTag("enemy military base");

            if (targetWayPoint != null)
            {
                var moveCommand = new HumanoidHStateCommand();
                moveCommand.State = HumanoidHState.Walk;
                moveCommand.TargetPosition = targetWayPoint.Position;

#if UNITY_EDITOR
                Debug.Log($"TestedGoToEnemyBaseNPCProcess OnRun moveCommand = {moveCommand}");
#endif

                var childProcess = ExecuteBody(moveCommand);
                Wait(childProcess);
            }

            var l = new List<int>();
        }
    }
    
    public class TestedRunAwayNPCProcess : TestedBaseNPCProcess
    //OldTstRunAwayProcess
    {
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedRunAwayNPCProcess Main");
#endif
        }
    }
    
    public class TestedRunAtOurBaseNPCProcess : TestedBaseNPCProcess
    //OldTstRunAtOurBaseProcess
    {
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedRunAtOurBaseNPCProcess Main");
#endif
        }
    }
    
    public class TestedSimpleAimNPCProcess : TestedBaseNPCProcess
    //OldTstSimpleAimProcess
    {
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedSimpleAimNPCProcess Main");
#endif
        }    
    }
    
    public class TestedFireToEthanNPCProcess : TestedBaseNPCProcess
    //OldTSTFireToEthanProcess
    {
        private void Main(Vector3 targetPosition)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedFireToEthanNPCProcess Main targetPosition = {targetPosition}");
#endif
        }    
    }
    
    public class TestedRotateNPCProcess : TestedBaseNPCProcess
    //OldTSTRotateProcess
    {
        private void Main(float angle)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedRotateNPCProcess Main angle = {angle}");
#endif
        }    
    }
    
    public class TestedRotateHeadNPCProcess : TestedBaseNPCProcess
    //OldTSTRotateHeadProcess 
    {
        private void Main(float angle)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedRotateHeadNPCProcess Main angle = {angle}");
#endif
        }    
    }
    
    public class TestedHeadToForvardNPCProcess : TestedBaseNPCProcess
    //OldTSTHeadToForvardProcess
    {
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedHeadToForvardNPCProcess Main");
#endif
        }    
    }
    
    public class TestedMoveNPCProcess : TestedBaseNPCProcess
    //OldTSTMoveProcess
    {
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedMoveNPCProcess Main");
#endif
        }    
    }
    
    public class TestedTakeFromSurfaceNPCProcess : TestedBaseNPCProcess
    //OldTSTTakeFromSurfaceProcess
    {
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedTakeFromSurfaceNPCProcess Main");
#endif
        }
    }
    
    public class TestedHideRifleToBagPackNPCProcess : TestedBaseNPCProcess
    //OldTstHideRifleToBagPackProcess
    {
        private void Main(int instanceId)
        {
#if UNITY_EDITOR
            Debug.Log("TestedHideRifleToBagPackNPCProcess Main");
#endif
        }    
    }
    
    public class TestedThrowOutToSurfaceRifleToSurfaceNPCProcess : TestedBaseNPCProcess
    //OldTstThrowOutToSurfaceRifleToSurfaceProcess
    {
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedThrowOutToSurfaceRifleToSurfaceNPCProcess Main");
#endif
        }    
    }
}
