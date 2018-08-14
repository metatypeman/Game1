using System;
using System.Collections.Generic;
using System.Linq;
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
        private Stack<Token> mRecoveriesTokens = new Stack<Token>();

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

            return mRecoveriesTokens.Pop();
        }

        public void Recovery(Token token)
        {
            mRecoveriesTokens.Push(token);

#if DEBUG
            //var tmpTokensList = mRecoveriesTokens.ToList();
            //LogInstance.Log($"tmpTokensList.Count = {tmpTokensList.Count}");
            //foreach(var tmpToken in tmpTokensList)
            //{
            //    LogInstance.Log($"tmpToken = {tmpToken}");
            //}
#endif
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
