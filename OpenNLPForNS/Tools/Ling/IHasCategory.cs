//OpenNLPForNS is based on AlexPoint/OpenNlp
//I just need OpenNLP for Net. Standard 1.6.

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Ling
{
    /// <summary>
    /// Something that implements the <code>HasCategory</code> interface knows about categories.
    /// 
    /// @author Christopher Manning
    /// 
    /// Code retrieved on the Stanford parser and ported to C# (see http://nlp.stanford.edu/software/lex-parser.shtml)
    /// </summary>
    public interface IHasCategory
    {
        /// <summary>
        /// Returns the category value of the label (or null if none).
        /// </summary>
        string Category();

        /// <summary>
        /// Sets the category value for the label (if one is stored).
        /// </summary>
        void SetCategory(string category);

    }
}
