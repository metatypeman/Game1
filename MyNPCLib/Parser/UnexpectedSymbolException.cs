using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public class UnexpectedSymbolException : Exception
    {
        public UnexpectedSymbolException(char symbol)
            : base($"Unexpected symbol `{symbol}`")
        {
        }
    }
}
