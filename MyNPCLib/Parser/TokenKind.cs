using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public enum TokenKind
    {
        Unknown,
        Word,
        Number,
        OpenRoundBracket,
        CloseRoundBracket,
        And,
        Or,
        Not,
        Assing
    }
}
