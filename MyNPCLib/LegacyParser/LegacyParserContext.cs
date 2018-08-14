using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LegacyParser
{
    public class LegacyParserContext
    {
        public LegacyParserContext(string text, IEntityDictionary entityDictionary)
        {
            mLexer = new LegacyLexer(text);
            mEntityDictionary = entityDictionary;
        }

        private LegacyLexer mLexer;
        private IEntityDictionary mEntityDictionary;
        private Queue<LegacyToken> mRecoveriesTokens = new Queue<LegacyToken>();

        public LegacyToken GetToken()
        {
            if (mRecoveriesTokens.Count == 0)
            {
                return mLexer.GetToken();
            }

            return mRecoveriesTokens.Dequeue();
        }

        public void Recovery(LegacyToken token)
        {
            mRecoveriesTokens.Enqueue(token);
        }

        public bool IsEmpty()
        {
            var tmpToken = GetToken();

            if (tmpToken == null)
            {
                return true;
            }

            Recovery(tmpToken);

            return false;
        }

        /// <summary>
        /// Number of remaining characters.
        /// </summary>
        public int Count
        {
            get
            {
                return mRecoveriesTokens.Count + mLexer.Count;
            }
        }

        public ulong GetKey(string name)
        {
            return mEntityDictionary.GetKey(name);
        }
    }
}
