using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public abstract class BaseATNNodeFactory: IATNNodeFactory
    {
        protected BaseATNNodeFactory(ATNExtendedToken extendedToken, GoalOfATNExtendToken goal)
        {
            ExtendedToken = extendedToken;
            Goal = goal;
        }

        public ATNExtendedToken ExtendedToken { get; private set; }
        public GoalOfATNExtendToken Goal { get; private set; }
        public abstract int? InternalState { get; }
        public abstract BaseATNParsingNode Create(ContextOfATNParsing context);
    }
}
