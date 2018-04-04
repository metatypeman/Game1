using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNPCLib
{
    public class NPCBodyResourcesManager: INPCBodyResourcesManager
    {
        public NPCBodyResourcesManager(IIdFactory idFactory, IEntityDictionary entityDictionary, IHumanoidBodyController humanoidBodyController)
        {
            mIdFactory = idFactory;
            mEntityDictionary = entityDictionary;
            mHumanoidBodyController = humanoidBodyController;
        }

#region private members
        private IIdFactory mIdFactory;
        private IEntityDictionary mEntityDictionary;
        private IHumanoidBodyController mHumanoidBodyController;
        private object mStateLockObj = new object();
        private StateOfNPCContext mState = StateOfNPCContext.Created;
        #endregion

        public void Bootstrap()
        {
            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }

                if (mState == StateOfNPCContext.Working)
                {
                    return;
                }

                mState = StateOfNPCContext.Working;
            }
        }

        public INPCProcess Send(IHumanoidBodyCommand command)
        {
#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager Send command = {command}");
#endif

            lock (mStateLockObj)
            {
                if (mState != StateOfNPCContext.Working)
                {
                    return new NotValidResourceNPCProcess();
                }
            }

            var id = mIdFactory.GetNewId();

            var process = new ProxyForNPCResourceProcess(id);

            var task = new Task(() => {
                NExecute(command, process);
            });

            process.Task = task;

            task.Start();

            return process;
        }

        private void NExecute(IHumanoidBodyCommand command, ProxyForNPCResourceProcess process)
        {
#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager Begin NExecute command = {command}");
#endif
            var processId = command.InitiatingProcessId;

            var targetState = CreateTargetState(command);

#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager NExecute targetState = {targetState}");
#endif
            var resolution = CreateResolution(mHumanoidBodyController.States, targetState, processId);

#if DEBUG
            //LogInstance.Log($"NPCBodyResourcesManager NExecute resolution = {resolution}");
#endif

#if DEBUG
            LogInstance.Log($"NPCBodyResourcesManager End NExecute command = {command}");
#endif
        }

        private TargetStateOfHumanoidBody CreateTargetState(IHumanoidBodyCommand command)
        {
#if DEBUG
            LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState command = {command}");
#endif

            var result = new TargetStateOfHumanoidBody();

            var kind = command.Kind;

            switch (kind)
            {
                case HumanoidBodyCommandKind.HState:
                    {
                        var targetCommand = command as IHumanoidHStateCommand;

                        var targetHState = targetCommand.State;
                        var targetPosition = targetCommand.TargetPosition;

                        result.HState = targetCommand.State;
                        result.TargetPosition = targetCommand.TargetPosition;

                        switch (targetHState)
                        {
                            case HumanoidHState.Stop:
                                result.TargetPosition = null;
                                break;

                            case HumanoidHState.Walk:
                            case HumanoidHState.Run:
                                if (targetPosition == null)
                                {
                                    result.HState = HumanoidHState.Stop;
                                }
                                break;
                        }
                    }
                    break;

                case HumanoidBodyCommandKind.VState:
                    {
                        var targetCommand = command as IHumanoidVStateCommand;

                        result.VState = targetCommand.State;
                    }
                    break;

                case HumanoidBodyCommandKind.HandsState:
                    {
                        var targetCommand = command as IHumanoidHandsStateCommand;

                        result.HandsState = targetCommand.State;
                    }
                    break;

                case HumanoidBodyCommandKind.HandsActionState:
                    {
                        var targetCommand = command as IHumanoidHandsActionStateCommand;

                        result.HandsActionState = targetCommand.State;
                    }
                    break;

                case HumanoidBodyCommandKind.HeadState:
                    {
                        var targetCommand = command as IHumanoidHeadStateCommand;

                        result.HeadState = targetCommand.State;
                        result.TargetHeadPosition = targetCommand.TargetPosition;
                    };
                    break;

                case HumanoidBodyCommandKind.Things:
                    {
                        var targetCommand = command as IHumanoidThingsCommand;

                        result.KindOfThingsCommand = targetCommand.State;
                        result.InstanceOfThingId = targetCommand.InstanceId;
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }

            if (result.HandsState.HasValue)
            {
                var targeHandsState = result.HandsState.Value;

                switch (targeHandsState)
                {
                    case HumanoidHandsState.FreeHands:
                        result.HandsActionState = HumanoidHandsActionState.Empty;
                        break;
                }
            }

#if DEBUG
            LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState result = {result}");
#endif

            return result;
        }

        private NPCResourcesResulution CreateResolution(StatesOfHumanoidBodyController sourceState, TargetStateOfHumanoidBody targetState, int processId)
        {
#if DEBUG
            LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState sourceState = {sourceState}");
            LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState targetState = {targetState}");
            LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState processId = {processId}");
            //DumpProcesses();
#endif

            var result = new NPCResourcesResulution();
            result.TargetProcessId = processId;
            result.TargetState = targetState;

            var theSame = true;

            var currentStates = sourceState;

            if (targetState.HState.HasValue)
            {
                var targetHState = targetState.HState.Value;

                if (mHState.Count == 0)
                {
                    theSame = false;
                }
                else
                {
                    if (!mHState.Contains(processId))
                    {
                        theSame = false;
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

                        var disagreement = new DisagreementByHStateInfo();
                        result.DisagreementByHState = disagreement;
                        disagreement.CurrentProcessesId = mHState.ToList();
                        disagreement.CurrentValue = currentStates.HState;
                        disagreement.TargetProcessId = processId;
                        disagreement.TargetValue = targetHState;
                    }
                }
            }

            if (targetState.TargetPosition != null)
            {
                var targetPosition = targetState.TargetPosition;

                if (mTargetPosition.Count == 0)
                {
                    theSame = false;
                }
                else
                {
                    if (!mTargetPosition.Contains(processId))
                    {
                        theSame = false;
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

                        var disagreement = new DisagreementByTargetPositionInfo();
                        result.DisagreementByTargetPosition = disagreement;
                        disagreement.CurrentProcessesId = mTargetPosition.ToList();
                        disagreement.CurrentValue = currentStates.TargetPosition;
                        disagreement.TargetProcessId = processId;
                        disagreement.TargetValue = targetPosition;
                    }
                }
            }

            if (targetState.VState.HasValue)
            {
                var targetVState = targetState.VState.Value;

                if (mVState.Count == 0)
                {
                    theSame = false;
                }
                else
                {
                    if (!mVState.Contains(processId))
                    {
                        theSame = false;
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

                        var disagreement = new DisagreementByVStateInfo();
                        result.DisagreementByVState = disagreement;
                        disagreement.CurrentProcessesId = mVState.ToList();
                        disagreement.CurrentValue = currentStates.VState;
                        disagreement.TargetProcessId = processId;
                        disagreement.TargetValue = targetVState;
                    }
                }
            }

            if (targetState.HandsState.HasValue || targetState.KindOfThingsCommand.HasValue)
            {
                var targetHandsState = HumanoidHandsState.FreeHands;

                if (targetState.HandsState.HasValue)
                {
                    targetHandsState = targetState.HandsState.Value;
                }
                else
                {
                    var kindOfThingsCommand = targetState.KindOfThingsCommand.Value;

                    switch (kindOfThingsCommand)
                    {
                        case KindOfHumanoidThingsCommand.Take:
                            targetHandsState = HumanoidHandsState.HasRifle;
                            break;

                        case KindOfHumanoidThingsCommand.PutToBagpack:
                        case KindOfHumanoidThingsCommand.ThrowOutToSurface:
                            targetHandsState = HumanoidHandsState.FreeHands;
                            break;
                    }
                }

#if DEBUG
                LogInstance.Log($"NPCThreadSafeMeshController CreateTargetState targetHandsState = {targetHandsState}");
#endif

                if (mHandsState.Count == 0)
                {
                    theSame = false;
                }
                else
                {
                    if (!mHandsState.Contains(processId))
                    {
                        theSame = false;
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

                        var disagreement = new DisagreementByHandsStateInfo();
                        result.DisagreementByHandsState = disagreement;
                        disagreement.CurrentProcessesId = mHandsState.ToList();
                        disagreement.CurrentValue = currentStates.HandsState;
                        disagreement.TargetProcessId = processId;
                        disagreement.TargetValue = targetHandsState;
                    }
                }
            }

            if (targetState.HandsActionState.HasValue)
            {
                var targetHandsActionState = targetState.HandsActionState.Value;

                if (mHandsActionState.Count == 0)
                {
                    theSame = false;
                }
                else
                {
                    if (!mHandsActionState.Contains(processId))
                    {
                        theSame = false;
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

                        var disagreement = new DisagreementByHandsActionStateInfo();
                        result.DisagreementByHandsActionState = disagreement;
                        disagreement.CurrentProcessesId = mHandsActionState.ToList();
                        disagreement.CurrentValue = currentStates.HandsActionState;
                        disagreement.TargetProcessId = processId;
                        disagreement.TargetValue = targetHandsActionState;
                    }
                }
            }

            if (targetState.HeadState.HasValue)
            {
                var targetHeadState = targetState.HeadState.Value;

                if (mHeadState.Count == 0)
                {
                    theSame = false;
                }
                else
                {
                    if (!mHeadState.Contains(processId))
                    {
                        theSame = false;
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

                        var disagreement = new DisagreementByHeadStateInfo();
                        result.DisagreementByHeadState = disagreement;
                        disagreement.CurrentProcessesId = mHeadState.ToList();
                        disagreement.CurrentValue = currentStates.HeadState;
                        disagreement.TargetProcessId = processId;
                        disagreement.TargetValue = targetHeadState;
                    }
                }
            }

            if (targetState.TargetHeadPosition.HasValue)
            {
                var targetHeadPosition = targetState.TargetHeadPosition.Value;

                if (mTargetHeadPosition.Count == 0)
                {
                    theSame = false;
                }
                else
                {
                    if (!mTargetHeadPosition.Contains(processId))
                    {
                        theSame = false;
                        result.Kind = NPCMeshTaskResulutionKind.Forbiden;

                        var disagreement = new DisagreementByTargetHeadPositionInfo();
                        result.DisagreementByTargetHeadPosition = disagreement;
                        disagreement.CurrentProcessesId = mTargetHeadPosition.ToList();
                        disagreement.CurrentValue = currentStates.TargetHeadPosition;
                        disagreement.TargetProcessId = processId;
                        disagreement.TargetValue = targetHeadPosition;
                    }
                }
            }

            if (result.Kind == NPCMeshTaskResulutionKind.Unknow)
            {
                if (theSame)
                {
                    result.Kind = NPCMeshTaskResulutionKind.AllowAdd;
                }
                else
                {
                    result.Kind = NPCMeshTaskResulutionKind.Allow;
                }
            }

#if DEBUG
            LogInstance.Log("NPCThreadSafeMeshController CreateTargetState NEXT");
            LogInstance.Log("End NPCThreadSafeMeshController CreateTargetState");
#endif
            return result;
        }

        private List<int> mHState = new List<int>();
        private List<int> mTargetPosition = new List<int>();
        private List<int> mVState = new List<int>();
        private List<int> mHandsState { get; set; } = new List<int>();
        private List<int> mHandsActionState = new List<int>();
        private List<int> mHeadState = new List<int>();
        private List<int> mTargetHeadPosition = new List<int>();


        public void Dispose()
        {
#if DEBUG
            LogInstance.Log("NPCBodyResourcesManager Dispose");
#endif

            lock (mStateLockObj)
            {
                if (mState == StateOfNPCContext.Destroyed)
                {
                    return;
                }

                mState = StateOfNPCContext.Destroyed;
            }
        }
    }
}
