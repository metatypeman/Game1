using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException(Token token)
            : base($"Unexpected token {token.ToDebugString()}")
        {
        }
    }
}
