using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public interface IATNNodeFactory
    {
        BaseATNParsingNode Create(ATNExtendedToken extendedToken, GoalOfATNExtendToken goal, ContextOfATNParsing context);
    }
}
