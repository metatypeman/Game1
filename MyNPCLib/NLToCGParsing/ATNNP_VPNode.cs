using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNNP_VPNodeFactory: BaseATNNodeFactory
    {
        public ATNNP_VPNodeFactory(ATNExtendedToken extendedToken, GoalOfATNExtendToken goal)
            : this(extendedToken, ATNNP_VPNode.State.Init, goal, CompositionCommand.Undefined)
        {
        }

        public ATNNP_VPNodeFactory(ATNExtendedToken extendedToken, ATNNP_VPNode.State internalState, GoalOfATNExtendToken goal, CompositionCommand compositionCommand)
            : base(extendedToken, goal, compositionCommand)
        {
            mInternalState = internalState;
        }

        private ATNNP_VPNode.State mInternalState = ATNNP_VPNode.State.Init;
      
        public override BaseATNParsingNode Create(ContextOfATNParsing context)
        {
            var result = new ATNNP_VPNode(ExtendedToken, mInternalState, Goal, CompositionCommand, context);
            return result;
        }
    }

    public class ATNNP_VPNode : BaseATNParsingNode
    {
        public enum State
        {
            Init
        }

        public ATNNP_VPNode(ATNExtendedToken extendedToken, State internalState, GoalOfATNExtendToken goal, CompositionCommand compositionCommand, ContextOfATNParsing context)
            : base(goal, compositionCommand, context)
        {
            mTargetExtendedToken = extendedToken;
            mInternalState = internalState;
        }

        private ATNExtendedToken mTargetExtendedToken;
        private State mInternalState = State.Init;

        protected override void NormalizeCompositionCommand()
        {
#if DEBUG
            LogInstance.Log($"CompositionCommand= {CompositionCommand}");
#endif



#if DEBUG
            LogInstance.Log("End");
#endif
        }

        protected override void ImplementInternalState()
        {
#if DEBUG
            LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken}");
            LogInstance.Log($"mInternalState= {mInternalState}");
            LogInstance.Log($"CompositionCommand= {CompositionCommand}");
#endif

            switch (mInternalState)
            {
                case State.Init:
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(mInternalState), mInternalState, null);
            }

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        protected override void BornNewNodes()
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif



#if DEBUG
            LogInstance.Log("End");
#endif
        }
    }
}
