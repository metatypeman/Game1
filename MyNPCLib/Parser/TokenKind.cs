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
        /// Represents some word. Next may transforms to Var, SELECT, INSERT.
        /// </summary>
        Word,

        /// <summary>
        /// Represents some number.
        /// </summary>
        Number,

        /// <summary>
        /// Represents symbol `(`.
        /// </summary>
        OpenRoundBracket,

        /// <summary>
        /// Represents symbol `)`.
        /// </summary>
        CloseRoundBracket,

        /// <summary>
        /// Represents symbol `&amp;`.
        /// </summary>
        And,

        /// <summary>
        /// Represents symbol `|`.
        /// </summary>
        Or,

        /// <summary>
        /// Represents symbol `!`.
        /// </summary>
        Not,

        /// <summary>
        /// Represents symbol `=`.
        /// </summary>
        Assing
    }
}
