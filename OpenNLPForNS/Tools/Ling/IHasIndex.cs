//OpenNLPForNS is based on AlexPoint/OpenNlp
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
    public interface IHasIndex
    {
        string DocId();

        void SetDocId(string docId);

        int SentIndex();

        void SetSentIndex(int sentIndex);

        int Index();

        void SetIndex(int index);
    }
}
