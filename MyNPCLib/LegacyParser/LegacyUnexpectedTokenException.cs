using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LegacyParser
{
    public class LegacyUnexpectedTokenException : Exception
    {
        public LegacyUnexpectedTokenException(LegacyToken token)
            : base($"Unexpected token {token.ToDebugString()}")
        {
        }
    }
}
