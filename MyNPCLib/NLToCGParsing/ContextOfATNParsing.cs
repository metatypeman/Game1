using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ContextOfATNParsing
    {
        public ContextOfATNParsing(string text, IWordsDict wordsDict)
        {
            mATNExtendedLexer = new ATNExtendedLexer(text, wordsDict);
        }

        private ContextOfATNParsing()
        {
        }

        private ATNExtendedLexer mATNExtendedLexer;

        public ContextOfATNParsing Fork()
        {
            var result = new ContextOfATNParsing();
            result.mATNExtendedLexer = mATNExtendedLexer.Fork();
            return result;
        }

        public IList<ATNExtendedToken> GetСlusterOfExtendedTokens()
        {
            return mATNExtendedLexer.GetСlusterOfExtendedTokens();
        }
    }
}
