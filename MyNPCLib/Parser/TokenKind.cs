using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public enum TokenKind
    {
        Unknown,
        Word,

        /// <summary>
        /// Represents a number.
        /// </summary>
        Number,

        FuzzyLogicalValue,

        /// <summary>
        /// Represents a variable.
        /// </summary>
        Var,

        /// <summary>
        /// Represents a param with question, like '?x'.
        /// </summary>
        QuestionParam,

        /// <summary>
        /// Represents a value from program.
        /// </summary>
        BindedParam,

        /// <summary>
        /// Represents word which starts by #. Like '#some_entity'
        /// </summary>
        Entity,

        /// <summary>
        /// Represents symbol `{`.
        /// </summary>
        OpenFigureBracket,

        /// <summary>
        /// Represents symbol `}`.
        /// </summary>
        CloseFigureBracket,

        /// <summary>
        /// Represents symbol `(`.
        /// </summary>
        OpenRoundBracket,

        /// <summary>
        /// Represents symbol `)`.
        /// </summary>
        CloseRoundBracket,

        /// <summary>
        /// Represents symbol `[`.
        /// </summary>
        OpenSquareBracket,

        /// <summary>
        /// Represents symbol `]`.
        /// </summary>
        CloseSquareBracket,

        /// <summary>
        /// Represents symbol `,`.
        /// </summary>
        Comma,

        /// <summary>
        /// Represents symbol `.`.
        /// </summary>
        Point,

        /// <summary>
        /// Represents symbol `+`.
        /// </summary>
        Plus,

        /// <summary>
        /// Represents symbol `-`.
        /// </summary>
        Minus,

        /// <summary>
        /// Represents symbol `*`.
        /// </summary>
        Mul,

        /// <summary>
        /// Represents symbol `/`.
        /// </summary>
        Div,

        /// <summary>
        /// Represents symbol `:`.
        /// </summary>
        Colon,

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
        /// Represents symbol `&gt;`.
        /// </summary>
        More,

        /// <summary>
        /// Represents symbol `&lt;`.
        /// </summary>
        Less,

        /// <summary>
        /// Represents symbol `?`.
        /// </summary>
        QuestionMark,

        /// <summary>
        /// Represents symbol `=`.
        /// </summary>
        Assing,

        /// <summary>
        /// Represents symbol `@`.
        /// </summary>
        AtSign,

        /// <summary>
        /// Represents symbols `@@`.
        /// </summary>
        DoubleAtSign,

        /// <summary>
        /// Represents symbols `@$`.
        /// </summary>
        AtSignDollar,

        /// <summary>
        /// Represents symbol `#`.
        /// </summary>
        Sharp,

        /// <summary>
        /// Represents symbols `##`.
        /// </summary>
        DoubleSharp,

        /// <summary>
        /// Represents symbols `#@`.
        /// </summary>
        SharpAtSign,

        /// <summary>
        /// Represents symbols `{:`.
        /// </summary>
        BeginFact,

        /// <summary>
        /// Represents symbols `:}`.
        /// </summary>
        EndFact,

        /// <summary>
        /// Represents symbols `[:`.
        /// </summary>
        BeginAnnotaion,

        /// <summary>
        /// Represents symbols `:]`.
        /// </summary>
        EndAnnotation,

        /// <summary>
        /// Represents symbols `-&gt;`.
        /// </summary>
        RightwardArrow,

        /// <summary>
        /// Represents symbols `&lt;-`.
        /// </summary>
        LeftwardArrow,

        /// <summary>
        /// Represents symbols `&lt;-&gt;`.
        /// </summary>
        LeftRightArrow,

        /// <summary>
        /// Represents symbols `!:`.
        /// </summary>
        BeginAccessPolicy,

        Public,
        Private,
        Visible,
        True,
        False
    }
}
