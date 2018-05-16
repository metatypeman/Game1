﻿//OpenNLPForNS is based on AlexPoint/OpenNlp
//I just need OpenNLP for Net. Standard 1.6.
using System.Collections.Generic;
using System.IO;

namespace OpenNLP.Tools.Util.ObjectBank
{
    /// <summary>
    /// An IteratorFromReaderFactory is used to convert a java.io.Reader
    /// into an Iterator over the Objects of type T represented by the text
    /// in the java.io.Reader.
    /// 
    /// @author Jenny Finkel
    /// 
    /// Code retrieved on the Stanford parser and ported to C# (see http://nlp.stanford.edu/software/lex-parser.shtml)
    /// </summary>
    public interface IIteratorFromReaderFactory<T>
    {

        /// <summary>
        /// Return an iterator over the contents read from r.
        /// </summary>
        /// <param name="r">Where to read objects from</param>
        /// <returns>An Iterator over the objects</returns>
        IEnumerator<T> GetIterator(TextReader r);
    }
}
