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

            var command = TestedInspectingNPCProcess.CreateCommand();
            Execute(command);
        }

        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedBootNPCProcess Main");
#endif
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("inspecting")]
    public class TestedInspectingNPCProcess : TestedBaseNPCProcess
    //OldTstInspectingProcess
    {
        protected override void Awake()
        {
#if UNITY_EDITOR
            Debug.Log("TestedInspectingNPCProcess Awake");
#endif
        }

        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "inspecting";  
            return command;
        }
        
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedInspectingNPCProcess Main");
#endif
        }    
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.Singleton)]
    [NPCProcessName("key press")]
    public class TestedKeyListenerNPCProcess : TestedBaseNPCProcess
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
            Debug.Log($"TestedKeyListenerNPCProcess Main key = {key}");
#endif

            switch(key)
            {
                case KeyCode.F:
                    {
                        var command = TestedHeadToForvardNPCProcess.CreateCommand();
                        Execute(command);
                    }
                    break;
                    
                case KeyCode.G:
                    {
                        var command = TestedRotateHeadNPCProcess.CreateCommand(12f);
                        Execute(command);
                    }
                    break;
                    
                case KeyCode.K:
                    {
                        var command = TestedRotateNPCProcess.CreateCommand(30f);
                        Execute(command);
                    }
                    break;
                    
                case KeyCode.N:
                    {
                        //var command = 
                    }
                    break;
                    
                case KeyCode.H:
                    {
                        //var command = 
                    }
                    break;
                    
                case KeyCode.L:
                    {
                        var command = TestedSimpleAimNPCProcess.CreateCommand();
                        Execute(command);
                    }
                    break;
                    
                case KeyCode.I:
                    {
                        var ethanPos = BlackBoard.EthanPosition;
                        
#if UNITY_EDITOR
            Debug.Log($"TestedKeyListenerNPCProcess Main ethanPos = {ethanPos}");
#endif
                        
                        if(!ethanPos.HasValue)
                        {
                            break;
                        }
                        
                        var command = TestedFireToEthanNPCProcess.CreateCommand(ethanPos.Value);
                        Execute(command);
                    }
                    break;
                    
                case KeyCode.P:
                    {
                        var command = TestedRunAtOurBaseNPCProcess.CreateCommand();
                        Execute(command);
                    }
                    break;
        
                case KeyCode.U:
                    {
                        var command = TestedGoToEnemyBaseNPCProcess.CreateCommand();
                        Execute(command);
                    }
                    break;
                    
                case KeyCode.M:
                    {
                        var command = TestedMoveNPCProcess.CreateCommand();
                        Execute(command);
                    }

                    break;
                    
                case KeyCode.B:
                    {
                        var instanceIdOfRifle = BlackBoard.InstanceIdOfRifle;
                        
#if UNITY_EDITOR
            Debug.Log($"TestedKeyListenerNPCProcess Main instanceIdOfRifle = {instanceIdOfRifle}");
#endif
                        if(instanceIdOfRifle == 0)
                        {
                            break;
                        }
                        
                        var command = TestedTakeFromSurfaceNPCProcess.CreateCommand(instanceIdOfRifle);
                        Execute(command);
                    }
                    break;
                    
                case KeyCode.J:
                    {
                        var instanceIdOfRifle = BlackBoard.InstanceIdOfRifle;
                        
#if UNITY_EDITOR
            Debug.Log($"TestedKeyListenerNPCProcess Main instanceIdOfRifle = {instanceIdOfRifle}");
#endif

                        if(instanceIdOfRifle == 0)
                        {
                            break;
                        }
                        
                        var command = TestedHideRifleToBagPackNPCProcess.CreateCommand(instanceIdOfRifle);
                        Execute(command);
                    }
                    break;
                    
                case KeyCode.Q:
                    {
                        var instanceIdOfRifle = BlackBoard.InstanceIdOfRifle;
                        
#if UNITY_EDITOR
            Debug.Log($"TestedKeyListenerNPCProcess Main instanceIdOfRifle = {instanceIdOfRifle}");
#endif                        
                        if(instanceIdOfRifle == 0)
                        {
                            break;
                        }
                        
                        var command = TestedThrowOutToSurfaceRifleToSurfaceNPCProcess.CreateCommand(instanceIdOfRifle);
                        Execute(command);
                    }
                    break;
            }
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("go to enemy base")]
    public class TestedGoToEnemyBaseNPCProcess : TestedBaseNPCProcess
    //OldTstGoToEnemyBaseProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "go to enemy base";
            return command;
        }
        
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
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("run away")]
    public class TestedRunAwayNPCProcess : TestedBaseNPCProcess
    //OldTstRunAwayProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "run away";
            return command;
        }
        
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedRunAwayNPCProcess Main");
#endif
        }
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("run at our base")]
    public class TestedRunAtOurBaseNPCProcess : TestedBaseNPCProcess
    //OldTstRunAtOurBaseProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "run at our base";
            return command;
        }
        
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedRunAtOurBaseNPCProcess Main");
#endif
        }
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("simple aim")]    
    public class TestedSimpleAimNPCProcess : TestedBaseNPCProcess
    //OldTstSimpleAimProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "simple aim";
            return command;
        }
        
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedSimpleAimNPCProcess Main");
#endif
        }    
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("fire to ethan")]    
    public class TestedFireToEthanNPCProcess : TestedBaseNPCProcess
    //OldTSTFireToEthanProcess
    {
        public static NPCCommand CreateCommand(Vector3 targetPosition)
        {
            var command = new NPCCommand();
            command.Name = "fire to ethan";
            command.AddParam(nameof(targetPosition), targetPosition);
            return command;
        }
        
        private void Main(Vector3 targetPosition)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedFireToEthanNPCProcess Main targetPosition = {targetPosition}");
#endif
        }    
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("rotate")]    
    public class TestedRotateNPCProcess : TestedBaseNPCProcess
    //OldTSTRotateProcess
    {
        public static NPCCommand CreateCommand(float angle)
        {
            var command = new NPCCommand();
            command.Name = "rotate";
            command.AddParam(nameof(angle), angle);
            return command;
        }
        
        private void Main(float angle)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedRotateNPCProcess Main angle = {angle}");
#endif
        }    
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("rotate head")]    
    public class TestedRotateHeadNPCProcess : TestedBaseNPCProcess
    //OldTSTRotateHeadProcess 
    {
        public static NPCCommand CreateCommand(float angle)
        {
            var command = new NPCCommand();
            command.Name = "rotate head";
            command.AddParam(nameof(angle), angle);
            return command;
        }
        
        private void Main(float angle)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedRotateHeadNPCProcess Main angle = {angle}");
#endif
        }    
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("head to forvard")]    
    public class TestedHeadToForvardNPCProcess : TestedBaseNPCProcess
    //OldTSTHeadToForvardProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "head to forvard";    
            return command;
        }
        
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedHeadToForvardNPCProcess Main");
#endif
        }    
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("move")]    
    public class TestedMoveNPCProcess : TestedBaseNPCProcess
    //OldTSTMoveProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "move"; 
            return command;
        }
        
        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("TestedMoveNPCProcess Main");
#endif
        }    
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("take from surface")]    
    public class TestedTakeFromSurfaceNPCProcess : TestedBaseNPCProcess
    //OldTSTTakeFromSurfaceProcess
    {
        public static NPCCommand CreateCommand(int instanceId)
        {
            var command = new NPCCommand();
            command.Name = "take from surface";
            command.AddParam(nameof(instanceId), instanceId);
            return command;
        }
        
        private void Main(int instanceId)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedTakeFromSurfaceNPCProcess Main instanceId = {instanceId}");
#endif
        }
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("hide rifle to bagpack")]    
    public class TestedHideRifleToBagPackNPCProcess : TestedBaseNPCProcess
    //OldTstHideRifleToBagPackProcess
    {
        public static NPCCommand CreateCommand(int instanceId)
        {
            var command = new NPCCommand();
            command.Name = "hide rifle to bagpack";
            command.AddParam(nameof(instanceId), instanceId);
            return command;
        }
        
        private void Main(int instanceId)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedHideRifleToBagPackNPCProcess Main instanceId = {instanceId}");
#endif
        }   
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("throw out to surface rifle to surface")]    
    public class TestedThrowOutToSurfaceRifleToSurfaceNPCProcess : TestedBaseNPCProcess
    //OldTstThrowOutToSurfaceRifleToSurfaceProcess
    {
        public static NPCCommand CreateCommand(int instanceId)
        {
            var command = new NPCCommand();
            command.Name = "throw out to surface rifle to surface";
            command.AddParam(nameof(instanceId), instanceId);
            return command;
        }
        
        private void Main(int instanceId)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedThrowOutToSurfaceRifleToSurfaceNPCProcess Main instanceId = {instanceId}");
#endif
        }   
    }
}
