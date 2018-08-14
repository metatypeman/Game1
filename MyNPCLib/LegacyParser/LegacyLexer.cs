using MyNPCLib.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MyNPCLib.LegacyParser
{
    public class LegacyLexer
    {
        private enum LexerState
        {
            Init,
            InWord,
            InRichWord
        }

        public LegacyLexer(string text)
        {
            mItems = new Queue<char>(text.ToList());
        }

        private Queue<char> mItems;
        private LexerState mLexerState = LexerState.Init;
        private Queue<LegacyToken> mRecoveriesTokens = new Queue<LegacyToken>();
        private CultureInfo mCultureInfo = new CultureInfo("en-GB");

        private int mCurrentPos;
        private int mCurrentLine = 1;
        private char mEndStringChar { get; set; }
        public LegacyToken GetToken()
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
                //LogInstance.Log($"tmpChar = {tmpChar} (int)tmpChar = {(int)tmpChar} mLexerState = {mLexerState}");
#endif

                switch (mLexerState)
                {
                    case LexerState.Init:
                        if (char.IsLetterOrDigit(tmpChar))
                        {
                            tmpBuffer = new StringBuilder();
                            tmpBuffer.Append(tmpChar);

                            if (char.IsLetterOrDigit(mItems.Peek()))
                            {
                                mLexerState = LexerState.InWord;
                            }
                            else
                            {
                                return CreateToken(LegacyTokenKind.Word, tmpBuffer.ToString());
                            }
                            break;
                        }

                        switch (tmpChar)
                        {
                            case ' ':
                                break;

                            case '(':
                                return CreateToken(LegacyTokenKind.OpenRoundBracket);

                            case ')':
                                return CreateToken(LegacyTokenKind.CloseRoundBracket);

                            case '&':
                                return CreateToken(LegacyTokenKind.And);

                            case '|':
                                return CreateToken(LegacyTokenKind.Or);

                            case '!':
                                return CreateToken(LegacyTokenKind.Not);

                            case '=':
                                return CreateToken(LegacyTokenKind.Assing);

                            case '`':
                                mEndStringChar = tmpChar;
                                tmpBuffer = new StringBuilder();
                                mLexerState = LexerState.InRichWord;
                                break;

                            case '\'':
                                mEndStringChar = tmpChar;
                                tmpBuffer = new StringBuilder();
                                mLexerState = LexerState.InRichWord;
                                break;

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

                    case LexerState.InWord:
                        {
                            tmpBuffer.Append(tmpChar);
                            mLexerState = LexerState.InWord;

                            if (mItems.Count == 0)
                            {
                                mLexerState = LexerState.Init;
                                return CreateToken(LegacyTokenKind.Word, tmpBuffer.ToString());
                            }

                            var tmpNextChar = mItems.Peek();

                            if (!char.IsLetterOrDigit(tmpNextChar) && tmpNextChar != '_')
                            {
                                mLexerState = LexerState.Init;
                                return CreateToken(LegacyTokenKind.Word, tmpBuffer.ToString());
                            }
                        }
                        break;

                    case LexerState.InRichWord:
                        switch (tmpChar)
                        {
                            case '`':
                            case '\'':
                                if(mEndStringChar == tmpChar)
                                {
                                    mLexerState = LexerState.Init;
                                    return CreateToken(LegacyTokenKind.Word, tmpBuffer.ToString());
                                }
                                tmpBuffer.Append(tmpChar);
                                break;

                            default:
                                tmpBuffer.Append(tmpChar);
                                break;
                        }
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(mLexerState), mLexerState, null);
                }
            }

            return null;
        }

        private LegacyToken CreateToken(LegacyTokenKind kind, string content = null)
        {
            //char tmpNextChar;
            //int id;
            var kindOfKeyWord = LegacyTokenKind.Unknown;
            var contentLength = 0;

            var result = new LegacyToken();
            result.TokenKind = kind;
            result.KeyWordTokenKind = kindOfKeyWord;
            result.Content = content;
            result.Pos = mCurrentPos - contentLength;
            result.Line = mCurrentLine;

            return result;
        }

        public void Recovery(LegacyToken token)
        {
            mRecoveriesTokens.Enqueue(token);
        }

        /// <summary>
        /// Number of remaining characters.
        /// </summary>
        public int Count
        {
            get
            {
                return mRecoveriesTokens.Count + mItems.Count;
            }
        }
    }
}
