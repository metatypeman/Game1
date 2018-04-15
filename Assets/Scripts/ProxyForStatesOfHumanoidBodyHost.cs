using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class ProxyForStatesOfHumanoidBodyHost : IObjectToString, IStatesOfHumanoidBodyHost
    {
        public ProxyForStatesOfHumanoidBodyHost(InternalStatesOfHumanoidController internalState)
        {
            mInternalState = internalState;
        }

        private InternalStatesOfHumanoidController mInternalState;

        public HumanoidHState HState
        {
            get
            {
                return InternalStatesConverter.HumanoidHStateFromInternal(mInternalState.HState);
            }
        }

        public object TargetPosition
        {
            get
            {
                return mInternalState.TargetPosition;
            }
        }

        public HumanoidVState VState
        {
            get
            {
                return InternalStatesConverter.HumanoidVStateFromInternal(mInternalState.VState);
            }
        }

        public HumanoidHandsState HandsState
        {
            get
            {
                return InternalStatesConverter.HumanoidHandsStateFromInternal(mInternalState.HandsState);
            }
        }

        public HumanoidHandsActionState HandsActionState
        {
            get
            {
                return InternalStatesConverter.HumanoidHandsActionStateFromInternal(mInternalState.HandsActionState);
            }
        }

        public HumanoidHeadState HeadState
        {
            get
            {
                return InternalStatesConverter.HumanoidHeadStateFromInternal(mInternalState.HeadState);
            }
        }

        public object TargetHeadPosition
        {
            get
            {
                return mInternalState.TargetHeadPosition;
            }
        }

        public KindOfHumanoidThingsCommand KindOfThingsCommand
        {
            get
            {
                return InternalStatesConverter.KindOfHumanoidThingsCommandFromInternal(mInternalState.KindOfThingsCommand);
            }
        }

        public int InstanceOfThingId
        {
            get
            {
                return mInternalState.InstanceOfThingId;
            }
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(HState)} = {HState}");

            if (TargetPosition == null)
            {
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");
            }

            sb.AppendLine($"{spaces}{nameof(VState)} = {VState}");
            sb.AppendLine($"{spaces}{nameof(HandsState)} = {HandsState}");
            sb.AppendLine($"{spaces}{nameof(HandsActionState)} = {HandsActionState}");
            sb.AppendLine($"{spaces}{nameof(HeadState)} = {HeadState}");
            if (TargetHeadPosition == null)
            {
                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(TargetHeadPosition)} = {TargetHeadPosition}");
            }

            sb.AppendLine($"{spaces}{nameof(KindOfThingsCommand)} = {KindOfThingsCommand}");
            sb.AppendLine($"{spaces}{nameof(InstanceOfThingId)} = {InstanceOfThingId}");

            return sb.ToString();
        }
    }
}
