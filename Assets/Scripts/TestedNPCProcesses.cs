using MyNPCLib;
using MyNPCLib.Logical;
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
    {
        protected override void Awake()
        {
#if UNITY_EDITOR
            Debug.Log("TestedInspectingNPCProcess Awake");
#endif

            //var targetObject = GetLogicalObject("name='TrafficBarrierRed'");
            var targetObject = Context.GetLogicalObject("name='Ethan'");

#if UNITY_EDITOR
            Debug.Log("TestedInspectingNPCProcess Awake NEXT");
#endif

            var trigger = CreateTrigger(() => {
#if UNITY_EDITOR
                //var visibleObjects = BlackBoard.VisibleObjects;
                //Debug.Log($"TestedInspectingNPCProcess Trigger visibleObjects.Count = {visibleObjects.Count}");
                //foreach (var tmpVisibleObject in visibleObjects)
                //{
                //    Debug.Log($"TestedInspectingNPCProcess Trigger tmpVisibleObject = {tmpVisibleObject}");
                //}
#endif

                if (BlackBoard.VisibleObjects.Any(p => p == targetObject))
                {
                    return true;
                }

#if UNITY_EDITOR

                //Debug.Log($"TestedInspectingNPCProcess Trigger visibleObjects.Count = {visibleObjects.Count}");
                //foreach(var tmpVisibleObject in visibleObjects)
                //{
                //    Debug.Log($"TestedInspectingNPCProcess Trigger tmpVisibleObject.InstanceID = {tmpVisibleObject.InstanceID} tmpVisibleObject.GameObject != null = {tmpVisibleObject.GameObject != null} tmpVisibleObject.GameObject?.Name = {tmpVisibleObject.GameObject?.Name}");
                //}
#endif

                /*if (visibleObjects.Any(p => p.GameObject != null && p.GameObject.name == "TrafficBarrierRed"))
                {//A name of GameObject can to be taken not in additional threads. Only in main thread.
                //I have to repair this code and make it more convenient. 
                    return true;
                }*/

                return false;
            }, 100);

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
            Debug.LogError("TestedInspectingNPCProcess Trigger_OnFire");
            var targetObject = Context.GetLogicalObject("name='Ethan'");
            var targetPosition = targetObject.GetValue<System.Numerics.Vector3?>("global position");
            Debug.LogError($"TestedInspectingNPCProcess Trigger_OnFire targetPosition = {targetPosition}");
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
                        var command = TestedStartShootingNPCProcess.CreateCommand();
                        Execute(command);
                    }
                    break;
                    
                case KeyCode.H:
                    {
                        var command = TestedStopShootingNPCProcess.CreateCommand();
                        Execute(command);
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
                        var ethan = Context.GetLogicalObject("name='Ethan'");
                        
#if UNITY_EDITOR
                        Debug.Log($"TestedKeyListenerNPCProcess Main ethan = {ethan}");
#endif
                       
                        var command = TestedFireToEthanNPCProcess.CreateCommand(ethan);
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
                        var rifle = Context.GetLogicalObject("name='M4A1 Sopmod'");
                        
#if UNITY_EDITOR
                        Debug.Log($"TestedKeyListenerNPCProcess Main rifle = {rifle}");
#endif

                        var tmpB = Context.GetLogicalObject("name='TrafficBarrierHazards (1)'");

                        //var tmpP = tmpB.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
                        //Debug.Log($"TestedKeyListenerNPCProcess Main tmpP = {tmpP}");
#endif

                        if (rifle == null)
                        {
                            break;
                        }
                                               
                        var command = TestedTakeFromSurfaceNPCProcess.CreateCommand(rifle);
                        Execute(command);
                    }
                    break;
                    
                case KeyCode.J:
                    {
                        var instanceIdOfRifle = BlackBoard.EntityOfRifle;
                        
#if UNITY_EDITOR
            Debug.Log($"TestedKeyListenerNPCProcess Main instanceIdOfRifle = {instanceIdOfRifle}");
#endif

                        if(instanceIdOfRifle == null)
                        {
                            break;
                        }
                        
                        var command = TestedHideRifleToBagPackNPCProcess.CreateCommand(instanceIdOfRifle);
                        Execute(command);
                    }
                    break;
                    
                case KeyCode.Q:
                    {
                        var instanceIdOfRifle = BlackBoard.EntityOfRifle;
                        
#if UNITY_EDITOR
            Debug.Log($"TestedKeyListenerNPCProcess Main instanceIdOfRifle = {instanceIdOfRifle}");
#endif                        
                        if(instanceIdOfRifle == null)
                        {
                            break;
                        }
                        
                        BlackBoard.EntityOfRifle = null;
                        
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
            var targetWayPoint = Context.GetLogicalObject($"name='FarWaypoint'&class='waypoint'");
            //var targetWayPoint = WaypointsBus.GetByTag("enemy military base");

            if(targetWayPoint == null)
            {
                return;
            }

            var moveCommand = new HumanoidHStateCommand();
            moveCommand.State = HumanoidHState.Walk;
            moveCommand.TargetPosition = targetWayPoint.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
            Debug.Log($"TestedGoToEnemyBaseNPCProcess Main moveCommand = {moveCommand}");
#endif

            var childProcess = ExecuteBody(moveCommand);
            Wait(childProcess);
        }
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("run away")]
    public class TestedRunAwayNPCProcess : TestedBaseNPCProcess
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
    {
        public static NPCCommand CreateCommand(BaseAbstractLogicalObject enemy)
        {
            var command = new NPCCommand();
            command.Name = "fire to ethan";
            command.AddParam(nameof(enemy), enemy);
            return command;
        }
        
        private void Main(BaseAbstractLogicalObject enemy)
        {
#if UNITY_EDITOR
            Debug.Log($"Begin TestedFireToEthanNPCProcess Main enemy = {enemy}");
#endif

            var searchNearCommand = TestedSearchNearNPCProcess.CreateCommand(enemy, 100);

            var tmpSearchNearProcess = ExecuteAsChild(searchNearCommand);
            tmpSearchNearProcess.OnStateChanged += TmpTask_OnStateChanged;

            Wait(tmpSearchNearProcess);

            var targetPosition = enemy.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
            Debug.Log($"Begin TestedFireToEthanNPCProcess Main enemy = {enemy}");
            Debug.LogError($"TestedFireToEthanNPCProcess Main targetPosition = {targetPosition}");
#endif

            if (targetPosition == null)
            {
                return;
            }

#if UNITY_EDITOR
            var isAlive = enemy.GetValue<bool?>("alive");
            Debug.Log($"TestedFireToEthanNPCProcess Main isAlive = {isAlive}");

            var isDied = enemy.GetValue<bool?>("died");
            Debug.Log($"TestedFireToEthanNPCProcess Main isDied = {isDied}");
#endif

            var tmpCommand = new HumanoidHStateCommand();
            tmpCommand.State = HumanoidHState.AimAt;
            tmpCommand.TargetPosition = targetPosition;

            var tmpTask = ExecuteBody(tmpCommand);

            Wait(tmpTask);

            var command = TestedStartShootingNPCProcess.CreateCommand();
            ExecuteAsChild(command);

            Wait(5000);

            command = TestedStopShootingNPCProcess.CreateCommand();
            ExecuteAsChild(command);

#if UNITY_EDITOR
            Debug.Log($"End TestedFireToEthanNPCProcess Main targetPosition = {targetPosition}");
#endif
        }

        private void TmpTask_OnStateChanged(INPCProcess sender, StateOfNPCProcess state)
        {
            //Debug.Log($"TestedSearchNearNPCProcess TmpTask_OnStateChanged sender.Id = {sender.Id} state = {state}");
            Debug.LogWarning($"TestedFireToEthanNPCProcess TmpTask_OnStateChanged sender.Id = {sender.Id} state = {state}");
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("search near")]
    public class TestedSearchNearNPCProcess : TestedBaseNPCProcess
    {
        public static NPCCommand CreateCommand(BaseAbstractLogicalObject entity)
        {
            var command = new NPCCommand();
            command.Name = "search near";
            command.AddParam(nameof(entity), entity);
            return command;
        }

        public static NPCCommand CreateCommand(BaseAbstractLogicalObject entity, float angle)
        {
            var command = new NPCCommand();
            command.Name = "search near";
            command.AddParam(nameof(angle), angle);
            command.AddParam(nameof(entity), entity);
            return command;
        }

        private static float DEFAULT_DELTA = 5f;
        private static float DEFAULT_ANGLE = 90f;

        private void Main(BaseAbstractLogicalObject entity)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedSearchNearNPCProcess Main entity = {entity}");
            Debug.Log($"TestedSearchNearNPCProcess Main Id = {Id}");
#endif

            Main(entity, DEFAULT_ANGLE, DEFAULT_ANGLE, DEFAULT_DELTA);

#if UNITY_EDITOR
            Debug.Log("End TestedSearchNearNPCProcess Main");
#endif
        }

        private void Main(BaseAbstractLogicalObject entity, float angle)
        {
#if UNITY_EDITOR
            Debug.Log($"Begin TestedSearchNearNPCProcess Main entity = {angle}");
            Debug.Log($"TestedSearchNearNPCProcess Main enemy = {entity}");
            Debug.Log($"TestedSearchNearNPCProcess Main Id = {Id}");
            Debug.Log($"TestedSearchNearNPCProcess Begin Rotate to Angle angle = {angle} Id = {Id}");
#endif

            Main(entity, angle, angle, DEFAULT_DELTA);

#if UNITY_EDITOR
            Debug.Log("End TestedSearchNearNPCProcess Main");
#endif
        }

        private void Main(BaseAbstractLogicalObject entity, float leftAngle, float rightAngle)
        {
#if UNITY_EDITOR        
            Debug.Log($"TestedSearchNearNPCProcess Main entity = {entity}");
            Debug.Log($"Begin TestedSearchNearNPCProcess Main leftAngle = {leftAngle}");
            Debug.Log($"Begin TestedSearchNearNPCProcess Main rightAngle = {rightAngle}");
            Debug.Log($"TestedSearchNearNPCProcess Main Id = {Id}");
#endif

            Main(entity, leftAngle, rightAngle, DEFAULT_DELTA);

#if UNITY_EDITOR
            Debug.Log("End TestedSearchNearNPCProcess Main");
#endif
        }

        private void Main(BaseAbstractLogicalObject entity, float leftAngle, float rightAngle, float delta)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedSearchNearNPCProcess Main entity = {entity}");
            Debug.Log($"Begin TestedSearchNearNPCProcess Main leftAngle = {leftAngle}");
            Debug.Log($"Begin TestedSearchNearNPCProcess Main rightAngle = {rightAngle}");
            Debug.Log($"Begin TestedSearchNearNPCProcess Main delta = {delta}");
            Debug.Log($"TestedSearchNearNPCProcess Main Id = {Id}");
#endif

            var targetPosition = entity.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
            Debug.Log($"TestedSearchNearNPCProcess Main targetPosition = {targetPosition}");
#endif

            if (targetPosition.HasValue)
            {
                return;
            }

            leftAngle = Math.Abs(leftAngle);
            rightAngle = Math.Abs(rightAngle);
            delta = Math.Abs(delta);

            if (delta == 0f)
            {
                return;
            }

#if UNITY_EDITOR
            Debug.Log("TestedSearchNearNPCProcess Main NEXT");
#endif

            if (leftAngle > 0f)
            {
#if UNITY_EDITOR
                Debug.Log($"TestedSearchNearNPCProcess Main Begin left while");
#endif

                if(RotateTo(entity, leftAngle, delta))
                {
                    return;
                }

#if UNITY_EDITOR
                Debug.Log($"TestedSearchNearNPCProcess Main End left while");
#endif

            }

            if (rightAngle > 0f)
            {
#if UNITY_EDITOR
                Debug.Log($"TestedSearchNearNPCProcess Main Begin right while");
#endif
                if (leftAngle > 0f)
                {
                    if (RotateTo(entity, -leftAngle, delta))
                    {
                        return;
                    }
                }


                if (RotateTo(entity, -rightAngle, delta))
                {
                    return;
                }

#if UNITY_EDITOR
                Debug.Log($"TestedSearchNearNPCProcess Main End left while");
#endif
            }

#if UNITY_EDITOR
            Debug.Log("End TestedSearchNearNPCProcess Main");
#endif
        }

        private bool RotateTo(BaseAbstractLogicalObject entity, float angle, float delta)
        {
#if UNITY_EDITOR
            Debug.Log($"TestedSearchNearNPCProcess RotateTo entity = {entity}");
            Debug.Log($"Begin TestedSearchNearNPCProcess RotateTo angle = {angle}");
            Debug.Log($"Begin TestedSearchNearNPCProcess RotateTo delta = {delta}");
            Debug.Log($"TestedSearchNearNPCProcess RotateTo Id = {Id}");
#endif

            var anglesList = ListHelper.GetRange(0, angle, delta);

#if UNITY_EDITOR
            Debug.Log($"TestedSearchNearNPCProcess RotateTowhile anglesList.Count = {anglesList.Count}");
#endif

            var cmdDelta = delta;

            if (angle < 0f)
            {
                cmdDelta = -1f * cmdDelta;
            }

#if UNITY_EDITOR
            Debug.Log($"TestedSearchNearNPCProcess RotateTowhile cmdDelta = {cmdDelta}");
#endif

            var targetPosition = entity.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
            Debug.Log($"TestedSearchNearNPCProcess Main targetPosition = {targetPosition}");
#endif

            if (targetPosition.HasValue)
            {
                return true;
            }

            foreach (var currentAngle in anglesList)
            {
#if UNITY_EDITOR
                Debug.Log($"TestedSearchNearNPCProcess RotateTowhile currentAngle = {currentAngle}");
#endif

                var tmpCommand = new HumanoidHStateCommand();
                tmpCommand.State = HumanoidHState.Rotate;
                tmpCommand.TargetPosition = new System.Numerics.Vector3(0, cmdDelta, 0);

                var tmpTask = ExecuteBody(tmpCommand);
                tmpTask.OnStateChanged += TmpTask_OnStateChanged;
                Wait(tmpTask);

                var targetPosition_1 = entity.GetValue<System.Numerics.Vector3?>("global position");

#if UNITY_EDITOR
                Debug.Log($"TestedSearchNearNPCProcess Main targetPosition_1 = {targetPosition_1}");
#endif

                if(targetPosition_1.HasValue)
                {
                    return true;
                }
            }

#if UNITY_EDITOR
            Debug.Log("End TestedSearchNearNPCProcess RotateTo");
#endif

            return false;
        }

        private void TmpTask_OnStateChanged(INPCProcess sender, StateOfNPCProcess state)
        {
            //Debug.Log($"TestedSearchNearNPCProcess TmpTask_OnStateChanged sender.Id = {sender.Id} state = {state}");
            Debug.LogWarning($"TestedSearchNearNPCProcess TmpTask_OnStateChanged sender.Id = {sender.Id} state = {state}");
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("rotate")]    
    public class TestedRotateNPCProcess : TestedBaseNPCProcess
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
            tmpCommand.TargetPosition = new System.Numerics.Vector3(0, angle, 0);

            var tmpTask = ExecuteBody(tmpCommand);

#if UNITY_EDITOR
            Debug.Log($"End TestedRotateNPCProcess Main angle = {angle}");
#endif
        }    
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("rotate head")]    
    public class TestedRotateHeadNPCProcess : TestedBaseNPCProcess 
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
            tmpCommand.TargetPosition = new System.Numerics.Vector3(0, angle, 0);

            var tmpTask = ExecuteBody(tmpCommand);

#if UNITY_EDITOR
            Debug.Log($"End TestedRotateHeadNPCProcess Main angle = {angle}");
#endif
        }    
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("head to forvard")]    
    public class TestedHeadToForvardNPCProcess : TestedBaseNPCProcess
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
            tmpCommand.TargetPosition = new System.Numerics.Vector3(0, 0, 1);
            var tmpTask = ExecuteBody(tmpCommand);

#if UNITY_EDITOR
            Debug.Log("End TestedMoveNPCProcess Main");
#endif
        }    
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("take from surface")]    
    public class TestedTakeFromSurfaceNPCProcess : TestedBaseNPCProcess
    {
        public static NPCCommand CreateCommand(BaseAbstractLogicalObject thing)
        {
            var command = new NPCCommand();
            command.Name = "take from surface";
            command.AddParam(nameof(thing), thing);
            return command;
        }
        
        private void Main(BaseAbstractLogicalObject thing)
        {
#if UNITY_EDITOR
            Debug.Log($"Begin TestedTakeFromSurfaceNPCProcess Main thing = {thing}");
#endif

            var tmpCommand = new HumanoidThingsCommand();
            tmpCommand.State = KindOfHumanoidThingsCommand.Take;
            tmpCommand.Thing = thing;
            var tmpTask = ExecuteBody(tmpCommand);

            tmpTask.OnRanToCompletionChanged += (INPCProcess sender) => {
#if UNITY_EDITOR
                //Debug.Log("TestedTakeFromSurfaceNPCProcess Main tmpTask.OnStateChangedToRanToCompletion");
#endif

                BlackBoard.EntityOfRifle = thing;
            };

#if UNITY_EDITOR
            Debug.Log($"End TestedTakeFromSurfaceNPCProcess Main thing = {thing}");
#endif
        }
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("hide rifle to bagpack")]    
    public class TestedHideRifleToBagPackNPCProcess : TestedBaseNPCProcess
    {
        public static NPCCommand CreateCommand(BaseAbstractLogicalObject thing)
        {
            var command = new NPCCommand();
            command.Name = "hide rifle to bagpack";
            command.AddParam(nameof(thing), thing);
            return command;
        }
        
        private void Main(BaseAbstractLogicalObject thing)
        {
#if UNITY_EDITOR
            Debug.Log($"Begin TestedHideRifleToBagPackNPCProcess Main thing = {thing}");
#endif

            var tmpCommand = new HumanoidThingsCommand();
            tmpCommand.State = KindOfHumanoidThingsCommand.PutToBagpack;
            tmpCommand.Thing = thing;
            var tmpTask = ExecuteBody(tmpCommand);

            tmpTask.OnRanToCompletionChanged += (INPCProcess sender) => {
#if UNITY_EDITOR
                //Debug.Log("TestedHideRifleToBagPackNPCProcess Main tmpTask.OnStateChangedToRanToCompletion");
#endif

            };
            
#if UNITY_EDITOR
            Debug.Log($"End TestedHideRifleToBagPackNPCProcess Main thing = {thing}");
#endif
        }   
    }
    
    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("throw out to surface rifle to surface")]    
    public class TestedThrowOutToSurfaceRifleToSurfaceNPCProcess : TestedBaseNPCProcess
    {
        public static NPCCommand CreateCommand(BaseAbstractLogicalObject thing)
        {
            var command = new NPCCommand();
            command.Name = "throw out to surface rifle to surface";
            command.AddParam(nameof(thing), thing);
            return command;
        }
        
        private void Main(BaseAbstractLogicalObject thing)
        {
#if UNITY_EDITOR
            Debug.Log($"Begin TestedThrowOutToSurfaceRifleToSurfaceNPCProcess Main thing = {thing}");
#endif

            var tmpCommand = new HumanoidThingsCommand();
            tmpCommand.State = KindOfHumanoidThingsCommand.ThrowOutToSurface;
            tmpCommand.Thing = thing;
            var tmpTask = ExecuteBody(tmpCommand);

            tmpTask.OnRanToCompletionChanged += (INPCProcess sender) => {
#if UNITY_EDITOR
                //Debug.Log("TestedThrowOutToSurfaceRifleToSurfaceNPCProcess Main tmpTask.OnStateChangedToRanToCompletion");
#endif
            };

#if UNITY_EDITOR
            Debug.Log($"End TestedThrowOutToSurfaceRifleToSurfaceNPCProcess Main thing = {thing}");
#endif
        }   
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("start shooting")]
    public class TestedStartShootingNPCProcess : TestedBaseNPCProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "start shooting";
            return command;
        }

        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("Begin TestedStartShootingNPCProcess Main");
#endif

            var fireMode = GetDefaultHandProperty<FireMode?>("FireMode");

#if UNITY_EDITOR
            Debug.Log($"TestedStartShootingNPCProcess Main fireMode = {fireMode}");
#endif

            if(fireMode.HasValue)
            {
                var fireModeValue = fireMode.Value;

                switch(fireModeValue)
                {
                    case FireMode.Single:
                        SingleShoting();
                        break;

                    case FireMode.Multiple:
                        MultipleShooting();
                        break;
                }
            }

#if UNITY_EDITOR
            Debug.Log("End TestedStartShootingNPCProcess Main");
#endif
        }

        private void MultipleShooting()
        {
#if UNITY_EDITOR
            Debug.Log("Begin TestedStartShootingNPCProcess ");
#endif

            var command = new NPCCommand();
            command.Name = "shoot on";

            ExecuteDefaultHand(command);

#if UNITY_EDITOR
            Debug.Log("End TestedStartShootingNPCProcess ");
#endif
        }

        private void SingleShoting()
        {
#if UNITY_EDITOR
            Debug.Log("Begin TestedStartShootingNPCProcess ");
#endif



#if UNITY_EDITOR
            Debug.Log("End TestedStartShootingNPCProcess ");
#endif
        }
    }

    [NPCProcessStartupMode(NPCProcessStartupMode.NewInstance)]
    [NPCProcessName("stop shooting")]
    public class TestedStopShootingNPCProcess : TestedBaseNPCProcess
    {
        public static NPCCommand CreateCommand()
        {
            var command = new NPCCommand();
            command.Name = "stop shooting";
            return command;
        }

        private void Main()
        {
#if UNITY_EDITOR
            Debug.Log("Begin TestedStopShootingNPCProcess Main");
#endif

            var command = new NPCCommand();
            command.Name = "shoot off";

            ExecuteDefaultHand(command);

#if UNITY_EDITOR
            Debug.Log("End TestedStopShootingNPCProcess Main");
#endif
        }
    }
}
