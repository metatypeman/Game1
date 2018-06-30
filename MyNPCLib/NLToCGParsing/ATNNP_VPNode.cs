using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNNP_VPNodeFactory: BaseATNNodeFactory
    {
        public ATNNP_VPNodeFactory(ATNExtendedToken extendedToken, GoalOfATNExtendToken goal)
            : this(extendedToken, ATNNP_VPNode.State.Init, goal)
        {
        }

        public ATNNP_VPNodeFactory(ATNExtendedToken extendedToken, ATNNP_VPNode.State internalState, GoalOfATNExtendToken goal)
            : base(extendedToken, goal)
        {
            mInternalState = internalState;
        }

        private ATNNP_VPNode.State mInternalState = ATNNP_VPNode.State.Init;
        public override int? InternalState => (int)mInternalState;

        public override BaseATNParsingNode Create(ContextOfATNParsing context)
        {
            var result = new ATNNP_VPNode(ExtendedToken, mInternalState, context);
            return result;
        }
    }

    public class ATNNP_VPNode : BaseATNParsingNode
    {
        public enum State
        {
            Init
        }

        public ATNNP_VPNode(ATNExtendedToken extendedToken, State internalState, ContextOfATNParsing context)
            : base(context)
        {
            mTargetExtendedToken = extendedToken;
            mInternalState = internalState;
        }

        private ATNExtendedToken mTargetExtendedToken;
        private State mInternalState = State.Init;

        protected override void NRun()
        {
#if DEBUG
            LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken} mInternalState = {mInternalState}");
#endif

            ImplementInternalState();
            BornNewNodes();

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        private void ImplementInternalState()
        {
#if DEBUG
            LogInstance.Log("Begin");
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

        private void BornNewNodes()
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
