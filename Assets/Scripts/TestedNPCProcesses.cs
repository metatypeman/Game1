using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            command.Priority = NPCProcessPriorities.Highest;
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

            var trigger = CreateTrigger(() => {
                var visibleObjects = BlackBoard.VisibleObjects;

#if UNITY_EDITOR
                Debug.Log($"TestedInspectingNPCProcess Trigger visibleObjects.Count = {visibleObjects.Count}");
                foreach(var tmpVisibleObject in visibleObjects)
                {
                    Debug.Log($"TestedInspectingNPCProcess Trigger tmpVisibleObject.GameObject != null = {tmpVisibleObject.GameObject != null} tmpVisibleObject.GameObject?.Name = {tmpVisibleObject.GameObject?.Name}");
                }
#endif

                if (visibleObjects.Any(p => p.GameObject != null && p.GameObject.Name == "TrafficBarrierRed"))
                {
                    return true;
                }

                return false;
            }, 1000);

            trigger.OnFire += Trigger_OnFire;
        }

        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "inspecting";  
            return command;
        }

        private void Trigger_OnFire()
        {
#if UNITY_EDITOR
            Debug.Log("TestedInspectingNPCProcess Trigger_OnFire");
#endif
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
                        var instanceIdOfRifle = BlackBoard.PossibleIdOfRifle;
                        
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
                        
                        BlackBoard.InstanceIdOfRifle = 0;
                        
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
                Debug.Log($"TestedGoToEnemyBaseNPCProcess Main moveCommand = {moveCommand}");
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
            Debug.Log("Begin TestedRunAwayNPCProcess Main");
#endif

            var targetName = "Cube_1";
            GoToTargetWayPoint(targetName);

#if UNITY_EDITOR
            Debug.Log("End TestedRunAwayNPCProcess Main");
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
            Debug.Log("Begin TestedRunAtOurBaseNPCProcess Main");
#endif

            for (var n = 1; n <= 3; n++)
            {
                var targetName = $"Cube_{n}";

#if UNITY_EDITOR
                //Debug.Log($"TestedRunAtOurBaseNPCProcess Main targetName = {targetName}");
#endif

                GoToTargetWayPoint(targetName);

#if UNITY_EDITOR
                //Debug.Log($"TestedRunAtOurBaseNPCProcess Main goal '{targetName}' had achieved!!!!");
#endif
            }

            var targetName1 = "Cube_1";
            var targetName2 = "Cube_2";

            GoToTargetWayPoint(targetName1, false);

            Thread.Sleep(1000);

            GoToTargetWayPoint(targetName2);
            
#if UNITY_EDITOR
            Debug.Log("End TestedRunAtOurBaseNPCProcess Main");
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
            Debug.Log("Begin TestedSimpleAimNPCProcess Main");
#endif

            var tmpCommand = new HumanoidHandsActionStateCommand();
            tmpCommand.State = HumanoidHandsActionState.StrongAim;

            var tmpTask = ExecuteBody(tmpCommand);

#if UNITY_EDITOR
            Debug.Log("End TestedSimpleAimNPCProcess Main");
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
            Debug.Log($"Begin TestedFireToEthanNPCProcess Main targetPosition = {targetPosition}");
#endif

            var tmpCommand = new HumanoidHStateCommand();
            tmpCommand.State = HumanoidHState.AimAt;
            tmpCommand.TargetPosition = targetPosition;
            
            var tmpTask = ExecuteBody(tmpCommand);

#if UNITY_EDITOR
            Debug.Log($"End TestedFireToEthanNPCProcess Main targetPosition = {targetPosition}");
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
            Debug.Log($"Begin TestedRotateNPCProcess Main angle = {angle}");
#endif

            var tmpCommand = new HumanoidHStateCommand();
            tmpCommand.State = HumanoidHState.Rotate;
            tmpCommand.TargetPosition = new Vector3(0, angle, 0);

            var tmpTask = ExecuteBody(tmpCommand);

#if UNITY_EDITOR
            Debug.Log($"End TestedRotateNPCProcess Main angle = {angle}");
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
            Debug.Log($"Begin TestedRotateHeadNPCProcess Main angle = {angle}");
#endif

            var tmpCommand = new HumanoidHeadStateCommand();
            tmpCommand.State = HumanoidHeadState.Rotate;
            tmpCommand.TargetPosition = new Vector3(0, angle, 0);

            var tmpTask = ExecuteBody(tmpCommand);

#if UNITY_EDITOR
            Debug.Log($"End TestedRotateHeadNPCProcess Main angle = {angle}");
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
            Debug.Log("Begin TestedHeadToForvardNPCProcess Main");
#endif

            var tmpCommand = new HumanoidHeadStateCommand();
            tmpCommand.State = HumanoidHeadState.LookingForward;
            var tmpTask = ExecuteBody(tmpCommand);

#if UNITY_EDITOR
            Debug.Log("End TestedHeadToForvardNPCProcess Main");
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
            Debug.Log("Begin TestedMoveNPCProcess Main");
#endif

            var tmpCommand = new HumanoidHStateCommand();
            tmpCommand.State = HumanoidHState.Move;
            tmpCommand.TargetPosition = new Vector3(0, 0, 1);
            var tmpTask = ExecuteBody(tmpCommand);

#if UNITY_EDITOR
            Debug.Log("End TestedMoveNPCProcess Main");
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
            Debug.Log($"Begin TestedTakeFromSurfaceNPCProcess Main instanceId = {instanceId}");
#endif

            var tmpCommand = new HumanoidThingsCommand();
            tmpCommand.State = KindOfHumanoidThingsCommand.Take;
            tmpCommand.InstanceId = instanceId;
            var tmpTask = ExecuteBody(tmpCommand);

            tmpTask.OnRanToCompletionChanged += () => {
#if UNITY_EDITOR
                //Debug.Log("TestedTakeFromSurfaceNPCProcess Main tmpTask.OnStateChangedToRanToCompletion");
#endif

                var targetGameObj = MyGameObjectsBus.GetObject(instanceId);
                
                var gun = targetGameObj.GetInstance<IRapidFireGun>();
                BlackBoard.RapidFireGunProxy.Instance = gun;
                BlackBoard.InstanceIdOfRifle = instanceId;
            };

#if UNITY_EDITOR
            Debug.Log($"End TestedTakeFromSurfaceNPCProcess Main instanceId = {instanceId}");
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
            Debug.Log($"Begin TestedHideRifleToBagPackNPCProcess Main instanceId = {instanceId}");
#endif

            BlackBoard.RapidFireGunProxy.Instance = null;
            var tmpCommand = new HumanoidThingsCommand();
            tmpCommand.State = KindOfHumanoidThingsCommand.PutToBagpack;
            tmpCommand.InstanceId = instanceId;
            var tmpTask = ExecuteBody(tmpCommand);

            tmpTask.OnRanToCompletionChanged += () => {
#if UNITY_EDITOR
                //Debug.Log("TestedHideRifleToBagPackNPCProcess Main tmpTask.OnStateChangedToRanToCompletion");
#endif

            };
            
#if UNITY_EDITOR
            Debug.Log($"End TestedHideRifleToBagPackNPCProcess Main instanceId = {instanceId}");
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
            Debug.Log($"Begin TestedThrowOutToSurfaceRifleToSurfaceNPCProcess Main instanceId = {instanceId}");
#endif

            BlackBoard.RapidFireGunProxy.Instance = null;
            var tmpCommand = new HumanoidThingsCommand();
            tmpCommand.State = KindOfHumanoidThingsCommand.ThrowOutToSurface;
            tmpCommand.InstanceId = instanceId;
            var tmpTask = ExecuteBody(tmpCommand);

            tmpTask.OnRanToCompletionChanged += () => {
#if UNITY_EDITOR
                //Debug.Log("TestedThrowOutToSurfaceRifleToSurfaceNPCProcess Main tmpTask.OnStateChangedToRanToCompletion");
#endif
            };

#if UNITY_EDITOR
            Debug.Log($"End TestedThrowOutToSurfaceRifleToSurfaceNPCProcess Main instanceId = {instanceId}");
#endif
        }   
    }
}
