using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MyNPCLib.Parser
{
    public class Lexer
    {
        private enum LexerState
        {
            Init,
            InWord,
            InRichWord
        }

        public Lexer(string text)
        {
            mItems = new Queue<char>(text.ToList());
        }

        private Queue<char> mItems;
        private LexerState mLexerState = LexerState.Init;
        private Queue<Token> mRecoveriesTokens = new Queue<Token>();
        private CultureInfo mCultureInfo = new CultureInfo("en-GB");

        private int mCurrentPos;
        private int mCurrentLine = 1;

        public Token GetToken()
        {
            if (mRecoveriesTokens.Count > 0)
            {
                return mRecoveriesTokens.Dequeue();
            }

            StringBuilder tmpBuffer = null;

            while (mItems.Count > 0)
            {
                var tmpChar = mItems.Dequeue();

                mCurrentPos++;

#if DEBUG
                LogInstance.Log($"Lexer this GetToken tmpChar = {tmpChar} (int)tmpChar = {(int)tmpChar} mLexerState = {mLexerState}");
#endif

                switch (mLexerState)
                {
                    case LexerState.Init:
                        switch (tmpChar)
                        {
                            case '!':
                                return CreateToken(TokenKind.Not);

                            default:
                                {
                                    var intCharCode = (int)tmpChar;

                                    if (intCharCode == 13)
                                    {
                                        break;
                                    }

                                    if (intCharCode == 10)
                                    {
                                        mCurrentPos = 0;
                                        mCurrentLine++;
                                        break;
                                    }

                                    throw new UnexpectedSymbolException(tmpChar);
                                }
                        }
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(mLexerState), mLexerState, null);
                }
            }

            return null;
        }

        private Token CreateToken(TokenKind kind, string content = null)
        {
            //char tmpNextChar;
            //int id;
            var kindOfKeyWord = TokenKind.Unknown;
            var contentLength = 0;

            var result = new Token();
            result.TokenKind = kind;
            result.KeyWordTokenKind = kindOfKeyWord;
            result.Content = content;
            result.Pos = mCurrentPos - contentLength;
            result.Line = mCurrentLine;

            return result;
        }
    }
}
