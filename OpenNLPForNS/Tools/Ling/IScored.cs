﻿//OpenNLPForNS is based on AlexPoint/OpenNlp
//I just need OpenNLP what is based on Net. Standard 1.6. 

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Ling
{
    /// <summary>
    /// Scored: This is a simple interface that says that an object can answer
    /// requests for the score, or goodness of the object.
    /// 
    /// JavaNLP includes companion classes {@link ScoredObject} which is a simple
    /// composite of another object and a score, and {@link ScoredComparator}
    /// which compares Scored objects.
    /// 
    /// @author Dan Klein
    /// 
    /// Code retrieved on the Stanford parser and ported to C# (see http://nlp.stanford.edu/software/lex-parser.shtml)
    /// </summary>
    public interface IScored
    {
        /// <summary>
        /// Returns the score of this thing.
        /// </summary>
        double Score();
    }
}
