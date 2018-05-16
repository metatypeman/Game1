//OpenNLPForNS is based on AlexPoint/OpenNlp
//I just need OpenNLP for Net. Standard 1.6.

using System;
using System.Collections.Generic;
using System.Text;
using OpenNLP.Tools.Ling;

namespace OpenNLP.Tools.Trees
{
    /// <summary>
    /// A <code>ConstituentFactory</code> is a factory for creating objects
    /// of class <code>Constituent</code>, or some descendent class.
    /// 
    /// @author Christopher Manning
    /// 
    /// Code retrieved on the Stanford parser and ported to C# (see http://nlp.stanford.edu/software/lex-parser.shtml)
    /// </summary>
    public interface IConstituentFactory
    {
        /// <summary>
        /// Build a constituent with this start and end.
        /// </summary>
        Constituent NewConstituent(int start, int end);

        /// <summary>
        /// Build a constituent with this start and end.
        /// </summary>
        Constituent NewConstituent(int start, int end, ILabel label, double score);
    }
}
