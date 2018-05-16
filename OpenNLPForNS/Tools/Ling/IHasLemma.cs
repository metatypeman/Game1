//OpenNLPForNS is based on AlexPoint/OpenNlp
//I just need OpenNLP for Net. Standard 1.6.

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Ling
{
    /// <summary>
    /// Something that implements the <code>HasLemma</code> interface knows about lemmas.
    /// 
    /// @author John Bauer
    /// 
    /// Code retrieved on the Stanford parser and ported to C# (see http://nlp.stanford.edu/software/lex-parser.shtml)
    /// </summary>
    public interface IHasLemma
    {
        /// <summary>
        /// Return the lemma value of the label (or null if none).
        /// </summary>
        string Lemma();

        /// <summary>
        /// Set the lemma value for the label (if one is stored).
        /// </summary>
        void SetLemma(string lemma);

    }
}
