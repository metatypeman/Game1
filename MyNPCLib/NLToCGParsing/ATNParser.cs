using OpenNLP.Tools.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNParser
    {
        public Sentence Run(Parse node)
        {
#if DEBUG
            LogInstance.Log($"node = {OpenNLPParseNodeHelper.ToString(node)}");
#endif

            //throw new NotImplementedException();
            var sentence = new Sentence();
            return sentence;
        }
    }
}
