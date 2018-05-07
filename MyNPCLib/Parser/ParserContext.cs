using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public class ParserContext
    {
        public ParserContext(string text)
        {
            mLexer = new Lexer(text);
        }

        private Lexer mLexer;
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
    }
}
