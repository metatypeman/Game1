//using MyNPCLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.Scripts
//{
//    public class ProxyForStatesOfHumanoidBodyHost : IObjectToString, IStatesOfHumanoidBodyHost
//    {
//        public ProxyForStatesOfHumanoidBodyHost(IInternalBodyHumanoidHost internalBodyHumanoidHost)
//        {
//            mInternalBodyHumanoidHost = internalBodyHumanoidHost;
//        }

//        private IInternalBodyHumanoidHost mInternalBodyHumanoidHost;

//        public HumanoidHState HState
//        {
//            get
//            {
//                var state = mInternalBodyHumanoidHost.States;

//                if(state == null)
//                {
//                    return HumanoidHState.Stop;
//                }

//                return state.HState;
//            }
//        }

//        public Vector3? TargetPosition
//        {
//            get
//            {
//                var state = mInternalBodyHumanoidHost.States;

//                if (state == null)
//                {
//                    return null;
//                }

//                return VectorsConvertor.NullableUnityToNumeric(state.TargetPosition);
//            }
//        }

//        public HumanoidVState VState
//        {
//            get
//            {
//                var state = mInternalBodyHumanoidHost.States;

//                if (state == null)
//                {
//                    return HumanoidVState.Ground;
//                }

//                return state.VState;
//            }
//        }

//        public HumanoidHandsState HandsState
//        {
//            get
//            {
//                var state = mInternalBodyHumanoidHost.States;

//                if (state == null)
//                {
//                    return HumanoidHandsState.FreeHands;
//                }

//                return state.HandsState;
//            }
//        }

//        public HumanoidHandsActionState HandsActionState
//        {
//            get
//            {
//                var state = mInternalBodyHumanoidHost.States;

//                if (state == null)
//                {
//                    return HumanoidHandsActionState.Empty;
//                }

//                return state.HandsActionState;
//            }
//        }

//        public HumanoidHeadState HeadState
//        {
//            get
//            {
//                var state = mInternalBodyHumanoidHost.States;

//                if (state == null)
//                {
//                    return HumanoidHeadState.LookingForward;
//                }

//                return state.HeadState;
//            }
//        }

//        public Vector3? TargetHeadPosition
//        {
//            get
//            {
//                var state = mInternalBodyHumanoidHost.States;

//                if (state == null)
//                {
//                    return null;
//                }

//                return VectorsConvertor.NullableUnityToNumeric(state.TargetHeadPosition);
//            }
//        }

//        public KindOfHumanoidThingsCommand KindOfThingsCommand
//        {
//            get
//            {
//                var state = mInternalBodyHumanoidHost.States;

//                if (state == null)
//                {
//                    return KindOfHumanoidThingsCommand.Undefined;
//                }

//                return state.KindOfThingsCommand;
//            }
//        }

//        public ulong? EntityIdOfThing
//        {
//            get
//            {
//                var state = mInternalBodyHumanoidHost.States;

//                if (state == null)
//                {
//                    return 0ul;
//                }

//                return state.EntityIdOfThing;
//            }
//        }

//        public override string ToString()
//        {
//            return ToString(0u);
//        }

//        public string ToString(uint n)
//        {
//            return this.GetDefaultToStringInformation(n);
//        }

//        public string PropertiesToString(uint n)
//        {
//            var spaces = StringHelper.Spaces(n);
//            var sb = new StringBuilder();

//            sb.AppendLine($"{spaces}{nameof(HState)} = {HState}");

//            if (TargetPosition == null)
//            {
//                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
//            }
//            else
//            {
//                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");
//            }

//            sb.AppendLine($"{spaces}{nameof(VState)} = {VState}");
//            sb.AppendLine($"{spaces}{nameof(HandsState)} = {HandsState}");
//            sb.AppendLine($"{spaces}{nameof(HandsActionState)} = {HandsActionState}");
//            sb.AppendLine($"{spaces}{nameof(HeadState)} = {HeadState}");
//            if (TargetHeadPosition == null)
//            {
//                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = null");
//            }
//            else
//            {
//                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = {TargetHeadPosition}");
//            }

//            sb.AppendLine($"{spaces}{nameof(KindOfThingsCommand)} = {KindOfThingsCommand}");
//            sb.AppendLine($"{spaces}{nameof(EntityIdOfThing)} = {EntityIdOfThing}");

//            return sb.ToString();
//        }
//    }
//}
