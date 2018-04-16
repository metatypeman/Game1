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
        public static InternalHumanoidStateKind HumanoidStateKindToInternal(HumanoidStateKind state)
        {
            switch (state)
            {
                case HumanoidStateKind.HState:
                    return InternalHumanoidStateKind.HState;

                case HumanoidStateKind.TargetPosition:
                    return InternalHumanoidStateKind.TargetPosition;

                case HumanoidStateKind.VState:
                    return InternalHumanoidStateKind.VState;

                case HumanoidStateKind.HandsState:
                    return InternalHumanoidStateKind.HandsState;

                case HumanoidStateKind.HandsActionState:
                    return InternalHumanoidStateKind.HandsActionState;

                case HumanoidStateKind.HeadState:
                    return InternalHumanoidStateKind.HeadState;

                case HumanoidStateKind.TargetHeadPosition:
                    return InternalHumanoidStateKind.TargetHeadPosition;

                case HumanoidStateKind.ThingsCommand:
                    return InternalHumanoidStateKind.ThingsCommand;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static HumanoidStateKind HumanoidStateKindFromInternal(InternalHumanoidStateKind state)
        {
            switch (state)
            {
                case InternalHumanoidStateKind.HState:
                    return HumanoidStateKind.HState;

                case InternalHumanoidStateKind.TargetPosition:
                    return HumanoidStateKind.TargetPosition;

                case InternalHumanoidStateKind.VState:
                    return HumanoidStateKind.VState;

                case InternalHumanoidStateKind.HandsState:
                    return HumanoidStateKind.HandsState;

                case InternalHumanoidStateKind.HandsActionState:
                    return HumanoidStateKind.HandsActionState;

                case InternalHumanoidStateKind.HeadState:
                    return HumanoidStateKind.HeadState;

                case InternalHumanoidStateKind.TargetHeadPosition:
                    return HumanoidStateKind.TargetHeadPosition;

                case InternalHumanoidStateKind.ThingsCommand:
                    return HumanoidStateKind.ThingsCommand;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

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

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static HumanoidHState HumanoidHStateFromInternal(InternalHumanoidHState state)
        {
            switch (state)
            {
                case InternalHumanoidHState.Stop:
                    return HumanoidHState.Stop;

                case InternalHumanoidHState.Walk:
                    return HumanoidHState.Walk;
                        
                case InternalHumanoidHState.Run:
                    return HumanoidHState.Run;
                    
                case InternalHumanoidHState.LookAt:
                    return HumanoidHState.LookAt;
                    
                case InternalHumanoidHState.AimAt:
                    return HumanoidHState.AimAt;
                    
                case InternalHumanoidHState.Rotate:
                    return HumanoidHState.Rotate;
                    
                case InternalHumanoidHState.Move:
                    return HumanoidHState.Move;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static InternalHumanoidVState HumanoidVStateToInternal(HumanoidVState state)
        {
            switch (state)
            {
                case HumanoidVState.Ground:
                    return InternalHumanoidVState.Ground;
                    
                case HumanoidVState.Jump:
                    return InternalHumanoidVState.Jump;
                    
                case HumanoidVState.Crouch:
                    return InternalHumanoidVState.Crouch;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static HumanoidVState HumanoidVStateFromInternal(InternalHumanoidVState state)
        {
            switch (state)
            {
                case InternalHumanoidVState.Ground:
                    return HumanoidVState.Ground;
                    
                case InternalHumanoidVState.Jump:
                    return HumanoidVState.Jump;
                    
                case InternalHumanoidVState.Crouch:
                    return HumanoidVState.Crouch;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static InternalHumanoidHandsState HumanoidHandsStateToInternal(HumanoidHandsState state)
        {
            switch (state)
            {
                case HumanoidHandsState.FreeHands:
                    return InternalHumanoidHandsState.FreeHands;
                    
                case HumanoidHandsState.HasRifle:
                    return InternalHumanoidHandsState.HasRifle;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static HumanoidHandsState HumanoidHandsStateFromInternal(InternalHumanoidHandsState state)
        {
            switch (state)
            {
                case InternalHumanoidHandsState.FreeHands:
                    return HumanoidHandsState.FreeHands;
                    
                case InternalHumanoidHandsState.HasRifle:
                    return HumanoidHandsState.HasRifle;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static InternalHumanoidHandsActionState HumanoidHandsActionStateToInternal(HumanoidHandsActionState state)
        {
            switch (state)
            {
                case HumanoidHandsActionState.Empty:
                    return InternalHumanoidHandsActionState.Empty;
                    
                case HumanoidHandsActionState.StrongAim:
                    return InternalHumanoidHandsActionState.StrongAim;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static HumanoidHandsActionState HumanoidHandsActionStateFromInternal(InternalHumanoidHandsActionState state)
        {
            switch (state)
            {
                case InternalHumanoidHandsActionState.Empty:
                    return HumanoidHandsActionState.Empty;
                    
                case InternalHumanoidHandsActionState.StrongAim:
                    return HumanoidHandsActionState.StrongAim;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static InternalHumanoidHeadState HumanoidHeadStateToInternal(HumanoidHeadState state)
        {
            switch (state)
            {
                case HumanoidHeadState.LookingForward:
                    return InternalHumanoidHeadState.LookingForward;
                    
                case HumanoidHeadState.LookAt:
                    return InternalHumanoidHeadState.LookAt;
                    
                case HumanoidHeadState.Rotate:
                    return InternalHumanoidHeadState.Rotate;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static HumanoidHeadState HumanoidHeadStateFromInternal(InternalHumanoidHeadState state)
        {
            switch (state)
            {
                case InternalHumanoidHeadState.LookingForward:
                    return HumanoidHeadState.LookingForward;
                    
                case InternalHumanoidHeadState.LookAt:
                    return HumanoidHeadState.LookAt;
                    
                case InternalHumanoidHeadState.Rotate:
                    return HumanoidHeadState.Rotate;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static InternalKindOfHumanoidThingsCommand KindOfHumanoidThingsCommandToInternal(KindOfHumanoidThingsCommand state)
        {
            switch (state)
            {
                case KindOfHumanoidThingsCommand.Undefined:
                    return InternalKindOfHumanoidThingsCommand.Undefined;
                    
                case KindOfHumanoidThingsCommand.Take:
                    return InternalKindOfHumanoidThingsCommand.Take;
                    
                case KindOfHumanoidThingsCommand.PutToBagpack:
                    return InternalKindOfHumanoidThingsCommand.PutToBagpack;
                    
                case KindOfHumanoidThingsCommand.ThrowOutToSurface:
                    return InternalKindOfHumanoidThingsCommand.ThrowOutToSurface;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        public static KindOfHumanoidThingsCommand KindOfHumanoidThingsCommandFromInternal(InternalKindOfHumanoidThingsCommand state)
        {
            switch (state)
            {
                case InternalKindOfHumanoidThingsCommand.Undefined:
                    return KindOfHumanoidThingsCommand.Undefined;
                    
                case InternalKindOfHumanoidThingsCommand.Take:
                    return KindOfHumanoidThingsCommand.Take;
                    
                case InternalKindOfHumanoidThingsCommand.PutToBagpack:
                    return KindOfHumanoidThingsCommand.PutToBagpack;
                    
                case InternalKindOfHumanoidThingsCommand.ThrowOutToSurface:
                    return KindOfHumanoidThingsCommand.ThrowOutToSurface;
            }

            throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
