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

                case InternalHumanoidHState.Walk:
                    return HumanoidHState.;
                        
                case InternalHumanoidHState.Run:
                    return HumanoidHState.;
                    
                case InternalHumanoidHState.LookAt:
                    return HumanoidHState.;
                    
                case InternalHumanoidHState.AimAt:
                    return HumanoidHState.;
                    
                case InternalHumanoidHState.Rotate:
                    return HumanoidHState.;
                    
                case InternalHumanoidHState.Move:
                    return HumanoidHState.;
            }
        }

        public static InternalHumanoidVState HumanoidVStateToInternal(HumanoidVState state)
        {
            switch (state)
            {
                case HumanoidVState.Ground:
                    return InternalHumanoidVState.;
                    
                case HumanoidVState.Jump:
                    return InternalHumanoidVState.;
                    
                case HumanoidVState.Crouch:
                    return InternalHumanoidVState.;
            }
        }

        public static HumanoidVState HumanoidVStateFromInternal(InternalHumanoidVState state)
        {
            switch (state)
            {
                case InternalHumanoidVState.Ground:
                    return .;
                    
                case InternalHumanoidVState.Jump:
                case InternalHumanoidVState.Crouch:
            }
        }

        public static InternalHumanoidHandsState HumanoidHandsStateToInternal(HumanoidHandsState state)
        {
            switch (state)
            {
                case HumanoidHandsState.FreeHands:
                    return .;
                    
                case HumanoidHandsState.HasRifle:
            }
        }

        public static HumanoidHandsState HumanoidHandsStateFromInternal(InternalHumanoidHandsState state)
        {
            switch (state)
            {
                case InternalHumanoidHandsState.FreeHands:
                    return .;
                    
                case InternalHumanoidHandsState.HasRifle:
            }
        }

        public static InternalHumanoidHandsActionState HumanoidHandsActionStateToInternal(HumanoidHandsActionState state)
        {
            switch (state)
            {
                case HumanoidHandsActionState.Empty:
                    return .;
                    
                case HumanoidHandsActionState.StrongAim:
            }
        }

        public static HumanoidHandsActionState HumanoidHandsActionStateFromInternal(InternalHumanoidHandsActionState state)
        {
            switch (state)
            {
                case InternalHumanoidHandsActionState.Empty:
                    return .;
                    
                case InternalHumanoidHandsActionState.StrongAim:
            }
        }

        public static InternalHumanoidHeadState HumanoidHeadStateToInternal(HumanoidHeadState state)
        {
            switch (state)
            {
                case HumanoidHeadState.LookingForward:
                    return .;
                    
                case HumanoidHeadState.LookAt:
                case HumanoidHeadState.Rotate:
            }
        }

        public static HumanoidHeadState HumanoidHeadStateFromInternal(InternalHumanoidHeadState state)
        {
            switch (state)
            {
                case InternalHumanoidHeadState.LookingForward:
                    return .;
                    
                case InternalHumanoidHeadState.LookAt:
                case InternalHumanoidHeadState.Rotate:
            }
        }

        public static InternalKindOfHumanoidThingsCommand KindOfHumanoidThingsCommandToInternal(KindOfHumanoidThingsCommand state)
        {
            switch (state)
            {
                case KindOfHumanoidThingsCommand.Undefined:
                    return .;
                    
                case KindOfHumanoidThingsCommand.Take:
                case KindOfHumanoidThingsCommand.PutToBagpack:
                case KindOfHumanoidThingsCommand.ThrowOutToSurface:
            }
        }

        public static KindOfHumanoidThingsCommand KindOfHumanoidThingsCommandFromInternal(InternalKindOfHumanoidThingsCommand state)
        {
            switch (state)
            {
                case InternalKindOfHumanoidThingsCommand.Undefined:
                    return .;
                    
                case InternalKindOfHumanoidThingsCommand.Take:
                case InternalKindOfHumanoidThingsCommand.PutToBagpack:
                case InternalKindOfHumanoidThingsCommand.ThrowOutToSurface:
            }
        }
    }
}
