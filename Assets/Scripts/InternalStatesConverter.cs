using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class InternalStatesConverter
    {
        public static InternalHumanoidHState HumanoidHStateToInternal(HumanoidHState state)
        {
            switch(state)
            {
                case HumanoidHState.Stop:
                    return InternalHumanoidHState.Stop;

                case HumanoidHState.Walk:
                    return InternalHumanoidHState.Walk;

                case HumanoidHState.Run:
                    return InternalHumanoidHState.Run;

                case HumanoidHState.LookAt:
                    return InternalHumanoidHState.LookAt;

                case HumanoidHState.AimAt:
                    return InternalHumanoidHState.AimAt;

                case HumanoidHState.Rotate:
                    return InternalHumanoidHState.Rotate;

                case HumanoidHState.Move:
                    return InternalHumanoidHState.Move;
            }

            return InternalHumanoidHState.Stop;
        }

        public static HumanoidHState HumanoidHStateFromInternal(InternalHumanoidHState state)
        {
            switch (state)
            {
                case InternalHumanoidHState.Stop:
                    return HumanoidHState.Stop;

        case InternalHumanoidHState.Walk,
        case InternalHumanoidHState.Run,
        case InternalHumanoidHState.LookAt,
        case InternalHumanoidHState.AimAt,
        case InternalHumanoidHState.Rotate,
        case InternalHumanoidHState.Move
            }
        }

        public static InternalHumanoidVState HumanoidVStateToInternal(HumanoidVState state)
        {
            switch (state)
            {
                case HumanoidVState.Ground
        case HumanoidVState.Jump
        case HumanoidVState.Crouch
            }
        }

        public static HumanoidVState HumanoidVStateFromInternal(InternalHumanoidVState state)
        {
            switch (state)
            {
                case Ground
        Jump
        Crouch
            }
        }

        public static InternalHumanoidHandsState HumanoidHandsStateToInternal(HumanoidHandsState state)
        {
            switch (state)
            {
                case FreeHands
        HasRifle
            }
        }

        public static HumanoidHandsState HumanoidHandsStateFromInternal(InternalHumanoidHandsState state)
        {
            switch (state)
            {
                case FreeHands
        HasRifle
            }
        }

        public static InternalHumanoidHandsActionState HumanoidHandsActionStateToInternal(HumanoidHandsActionState state)
        {
            switch (state)
            {
                case Empty
        StrongAim
            }
        }

        public static HumanoidHandsActionState HumanoidHandsActionStateFromInternal(InternalHumanoidHandsActionState state)
        {
            switch (state)
            {
                case Empty
        StrongAim
            }
        }

        public static InternalHumanoidHeadState HumanoidHeadStateToInternal(HumanoidHeadState state)
        {
            switch (state)
            {
                case LookingForward
        LookAt
        Rotate
            }
        }

        public static HumanoidHeadState HumanoidHeadStateFromInternal(InternalHumanoidHeadState state)
        {
            switch (state)
            {
                case LookingForward
        LookAt
        Rotate
            }
        }

        public static InternalKindOfHumanoidThingsCommand KindOfHumanoidThingsCommandToInternal(KindOfHumanoidThingsCommand state)
        {
            switch (state)
            {
                case Undefined
        Take
        PutToBagpack
        ThrowOutToSurface
            }
        }

        public static KindOfHumanoidThingsCommand KindOfHumanoidThingsCommandFromInternal(InternalKindOfHumanoidThingsCommand state)
        {
            switch (state)
            {
                case Undefined
        Take
        PutToBagpack
        ThrowOutToSurface
            }
        }
    }
}
