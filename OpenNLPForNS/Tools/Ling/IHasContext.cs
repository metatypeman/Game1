﻿//OpenNLPForNS is based on AlexPoint/OpenNlp
//I just need OpenNLP for Net. Standard 1.6.

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Ling
{
    /// <summary>
    /// @author grenager
    /// 
    /// Code retrieved on the Stanford parser and ported to C# (see http://nlp.stanford.edu/software/lex-parser.shtml)
    /// </summary>
    public interface IHasContext
    {
        /// <summary>
        /// Returns the string before the word
        /// </summary>
        string Before();

        /// <summary>
        /// Set the whitespace string before the word.
        /// </summary>
        void SetBefore(string before);

        /// <summary>
        /// Returns the string which is the original character sequence of the token.
        /// </summary>
        string OriginalText();

        /// <summary>
        /// Set the string which is the original character sequence of the token.
        /// </summary>
        void SetOriginalText(string originalText);

        /// <summary>
        /// Returns the whitespace string after the word.
        /// </summary>
        string After();

        /// <summary>
        /// Sets the whitespace string after the word.
        /// </summary>
        void SetAfter(string after);

    }
}
