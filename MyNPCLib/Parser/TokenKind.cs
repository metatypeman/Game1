using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    /// <summary>
    /// It represents an instance of Token.
    /// </summary>
    public enum TokenKind
    {
        /// <summary>
        /// Default value. Represents nothing.
        /// </summary>
        Unknown,

        /// <summary>
        /// Represents symbol `(`.
        /// </summary>
        OpenRoundBracket,

        /// <summary>
        /// Represents symbol `)`.
        /// </summary>
        CloseRoundBracket,

        /// <summary>
        /// Represents symbol `!`.
        /// </summary>
        Not
    }
}
