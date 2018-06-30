using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public interface IATNNodeFactory
    {
        ATNExtendedToken ExtendedToken { get; }
        GoalOfATNExtendToken Goal { get; }
        CompositionCommand CompositionCommand { get; }
        BaseATNParsingNode Create(ContextOfATNParsing context);
    }
}
