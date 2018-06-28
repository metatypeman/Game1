using MyNPCLib.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNLexer
    {
        private enum LexerState
        {
            Init,
            InWord
        }

        public ATNLexer(string text)
        {
            mItems = new Queue<char>(text.ToList());
        }

        private ATNLexer()
        {
        }

        private readonly object mLockObj = new object();
        private Queue<char> mItems;
        private Queue<ATNToken> mRecoveriesTokens = new Queue<ATNToken>();
        private LexerState mLexerState = LexerState.Init;
        private readonly CultureInfo mCultureInfo = new CultureInfo("en-GB");
        private int mCurrentPos;
        private int mCurrentLine = 1;

        public ATNLexer Fork()
        {
            lock (mLockObj)
            {
                var result = new ATNLexer();
                result.mItems = new Queue<char>(mItems.ToList());
                result.mRecoveriesTokens = new Queue<ATNToken>(mRecoveriesTokens.ToList());
                result.mLexerState = mLexerState;
                result.mCurrentPos = mCurrentPos;
                result.mCurrentLine = mCurrentLine;
                return result;
            }
        }

        public ATNToken GetToken()
        {
            lock (mLockObj)
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
                                    return CreateToken(KindOfATNToken.Word, tmpBuffer.ToString());
                                }
                                break;
                            }

                            switch (tmpChar)
                            {
                                case ' ':
                                    break;

                                case '(':
                                    return CreateToken(KindOfATNToken.OpenRoundBracket);

                                case ')':
                                    return CreateToken(KindOfATNToken.CloseRoundBracket);

                                case ',':
                                    return CreateToken(KindOfATNToken.Comma);

                                case ':':
                                    return CreateToken(KindOfATNToken.Colon);

                                case '.':
                                    return CreateToken(KindOfATNToken.Point);

                                case '-':
                                    return CreateToken(KindOfATNToken.Dash);

                                case ';':
                                    return CreateToken(KindOfATNToken.Semicolon);

                                case '!':
                                    return CreateToken(KindOfATNToken.ExclamationMark);

                                case '?':
                                    return CreateToken(KindOfATNToken.QuestionMark);

                                case '\'':
                                    return CreateToken(KindOfATNToken.SingleQuotationMark);

                                case '"':
                                    return CreateToken(KindOfATNToken.DoubleQuotationMark);

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
                                    return CreateToken(KindOfATNToken.Word, tmpBuffer.ToString());
                                }

                                var tmpNextChar = mItems.Peek();

                                if (!char.IsLetterOrDigit(tmpNextChar) && tmpNextChar != '_')
                                {
                                    mLexerState = LexerState.Init;
                                    return CreateToken(KindOfATNToken.Word, tmpBuffer.ToString());
                                }
                            }
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(mLexerState), mLexerState, null);
                    }
                }

                return null;
            }
        }

        private ATNToken CreateToken(KindOfATNToken kind, string content = null)
        {
            var contentLength = 0;

            switch(kind)
            {
                case KindOfATNToken.Word:
                    int r;
                    if(int.TryParse(content, out r))
                    {
                        kind = KindOfATNToken.Number;
                    }
                    break;
            }

            var result = new ATNToken();
            result.Kind = kind;
            result.Content = content;
            result.Pos = mCurrentPos - contentLength;
            result.Line = mCurrentLine;

            return result;
        }

        public void Recovery(ATNToken token)
        {
            lock (mLockObj)
            {
                mRecoveriesTokens.Enqueue(token);
            }           
        }

        /// <summary>
        /// Number of remaining characters.
        /// </summary>
        public int Count
        {
            get
            {
                lock (mLockObj)
                {
                    return mRecoveriesTokens.Count + mItems.Count;
                }        
            }
        }
    }
}
