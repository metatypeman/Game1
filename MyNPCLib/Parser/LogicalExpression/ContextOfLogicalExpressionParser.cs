using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public class ContextOfLogicalExpressionParser: IParserContext
    {
        public ContextOfLogicalExpressionParser(string text, IEntityDictionary entityDictionary)
        {
            mLexer = new Lexer(text);
            mEntityDictionary = entityDictionary;
        }

        private Lexer mLexer;
        private IEntityDictionary mEntityDictionary;
        private Queue<Token> mRecoveriesTokens = new Queue<Token>();

        public Token GetToken()
        {
            var result = NGetToken();

            if(result == null)
            {
                return null;
            }

            var tokenKind = result.TokenKind;

            switch(tokenKind)
            {
                case TokenKind.AtSign:
                    {
                        var nextToken = NGetToken();
                        var nextTokenKind = nextToken.TokenKind;
                        switch(nextTokenKind)
                        {
                            case TokenKind.Word:
                                nextToken.Content = $"@{nextToken.Content}";
                                nextToken.TokenKind = TokenKind.Var;
                                return nextToken;

                            default:
                                Recovery(nextToken);
                                break;
                        }
                    }
                    break;
            }

            return result;
        }

        private Token NGetToken()
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
