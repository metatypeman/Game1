//OpenNLPForNS is based on https://github.com/AlexPoint/OpenNlp
//I just need OpenNLP what is based on Net. Standard 1.6.

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Parser
{
    /// <summary>
    /// Interface that a pos-tagger used by the parser must implement.
    /// </summary>
    public interface IParserTagger : PosTagger.IPosTagger
    {
        /*Util.Sequence[] TopKSequences(ArrayList sentence);*/
        Util.Sequence[] TopKSequences(string[] sentence);
    }
}
