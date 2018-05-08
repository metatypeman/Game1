using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public class ParserContext
    {
        public ParserContext(string text, IEntityDictionary entityDictionary)
        {
            mLexer = new Lexer(text);
            mEntityDictionary = entityDictionary;
        }

        private Lexer mLexer;
        private IEntityDictionary mEntityDictionary;
        private Queue<Token> mRecoveriesTokens = new Queue<Token>();

        public Token GetToken()
        {
            if (mRecoveriesTokens.Count == 0)
            {
                return mLexer.GetToken();
            }

            return mRecoveriesTokens.Dequeue();
        }

        public void Recovery(Token token)
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
