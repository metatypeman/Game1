using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public abstract class BaseATNNodeFactory: IATNNodeFactory
    {
        protected BaseATNNodeFactory(ATNExtendedToken extendedToken, GoalOfATNExtendToken goal, CompositionCommand compositionCommand)
        {
            ExtendedToken = extendedToken;
            Goal = goal;
            CompositionCommand = compositionCommand;
        }

        public ATNExtendedToken ExtendedToken { get; private set; }
        public GoalOfATNExtendToken Goal { get; private set; }
        public CompositionCommand CompositionCommand { get; private set; }
        public abstract BaseATNParsingNode Create(ContextOfATNParsing context);
    }
}
