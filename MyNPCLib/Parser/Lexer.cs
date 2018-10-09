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
        private bool mIsDigitOnly = false;
        private Queue<Token> mRecoveriesTokens = new Queue<Token>();
        private static CultureInfo mCultureInfo = new CultureInfo("en-GB");

        private int mCurrentPos;
        private int mCurrentLine = 1;
        private char mEndStringChar { get; set; }

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
                //LogInstance.Log($"tmpChar = {tmpChar} (int)tmpChar = {(int)tmpChar} mLexerState = {mLexerState}");
#endif

                switch (mLexerState)
                {
                    case LexerState.Init:
                        if (char.IsLetterOrDigit(tmpChar))
                        {
                            tmpBuffer = new StringBuilder();
                            tmpBuffer.Append(tmpChar);

                            if(char.IsDigit(tmpChar))
                            {
                                mIsDigitOnly = true;
                            }

                            if (char.IsLetterOrDigit(mItems.Peek()))
                            {
                                mLexerState = LexerState.InWord;
                            }
                            else
                            {
                                return CreateToken(TokenKind.Word, tmpBuffer.ToString());
                            }
                            break;
                        }

                        switch (tmpChar)
                        {
                            case ' ':
                                break;

                            case '(':
                                return CreateToken(TokenKind.OpenRoundBracket);

                            case ')':
                                return CreateToken(TokenKind.CloseRoundBracket);

                            case '{':
                                return CreateToken(TokenKind.OpenFigureBracket);

                            case '}':
                                return CreateToken(TokenKind.CloseFigureBracket);

                            case '[':
                                return CreateToken(TokenKind.OpenSquareBracket);

                            case ']':
                                return CreateToken(TokenKind.CloseSquareBracket);

                            case ',':
                                return CreateToken(TokenKind.Comma);

                            case '+':
                                return CreateToken(TokenKind.Plus);

                            case '-':
                                return CreateToken(TokenKind.Minus);

                            case '*':
                                return CreateToken(TokenKind.Mul);

                            case '/':
                                return CreateToken(TokenKind.Div);

                            case ':':
                                return CreateToken(TokenKind.Colon);

                            case '&':
                                return CreateToken(TokenKind.And);

                            case '|':
                                return CreateToken(TokenKind.Or);

                            case '!':
                                return CreateToken(TokenKind.Not);

                            case '>':
                                return CreateToken(TokenKind.More);

                            case '<':
                                return CreateToken(TokenKind.Less);

                            case '?':
                                return CreateToken(TokenKind.QuestionMark);

                            case '=':
                                return CreateToken(TokenKind.Assing);

                            case '@':
                                return CreateToken(TokenKind.AtSign);

                            case '#':
                                return CreateToken(TokenKind.Sharp);

                            case '.':
                                return CreateToken(TokenKind.Point);

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

                            if(mIsDigitOnly)
                            {
                                if(!char.IsDigit(tmpChar))
                                {
                                    mIsDigitOnly = false;
                                }
                            }

                            mLexerState = LexerState.InWord;

                            if (mItems.Count == 0)
                            {
                                mLexerState = LexerState.Init;
                                return CreateToken(TokenKind.Word, tmpBuffer.ToString());
                            }

                            var tmpNextChar = mItems.Peek();

                            if (!char.IsLetterOrDigit(tmpNextChar) && tmpNextChar != '_')
                            {
                                mLexerState = LexerState.Init;
                                return CreateToken(TokenKind.Word, tmpBuffer.ToString());
                            }
                        }
                        break;

                    case LexerState.InRichWord:
                        switch (tmpChar)
                        {
                            case '`':
                            case '\'':
                                if (mEndStringChar == tmpChar)
                                {
                                    mLexerState = LexerState.Init;
                                    return CreateToken(TokenKind.Word, tmpBuffer.ToString());
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

        private Token CreateToken(TokenKind kind, string content = null)
        {
            var kindOfKeyWord = TokenKind.Unknown;
            var contentLength = 0;

            switch (kind)
            {
                case TokenKind.Word:
                    {
                        contentLength = content.Length - 1;

                        if(mIsDigitOnly)
                        {
                            mIsDigitOnly = false;
                            kind = TokenKind.Number;
                            break;
                        }

                        if(content.EndsWith("l") || content.EndsWith("L"))
                        {
#if DEBUG
                            //LogInstance.Log($"content = {content}");
#endif

                            if(content.Count(p => char.IsDigit(p)) == content.Length - 1)
                            {
                                content = content.Substring(0, content.Length - 1);

#if DEBUG
                                //LogInstance.Log($"content (2) = {content}");
#endif
                                contentLength = content.Length - 1;
                                kind = TokenKind.FuzzyLogicalValue;
                                break;
                            }             
                        }

                        if (string.Compare(content, "public", true) == 0)
                        {
                            kindOfKeyWord = TokenKind.Public;
                            break;
                        }
                        if (string.Compare(content, "private", true) == 0)
                        {
                            kindOfKeyWord = TokenKind.Private;
                            break;
                        }
                        if (string.Compare(content, "visible", true) == 0)
                        {
                            kindOfKeyWord = TokenKind.Visible;
                            break;
                        }

                        if(string.Compare(content, "true", true) == 0)
                        {
                            kindOfKeyWord = TokenKind.True;
                            break;
                        }

                        if (string.Compare(content, "false", true) == 0)
                        {
                            kindOfKeyWord = TokenKind.False;
                            break;
                        }
                    }
                    break;

                case TokenKind.OpenFigureBracket:
                    {
                        var tmpNextChar = mItems.Peek();

                        switch (tmpNextChar)
                        {
                            case ':':
                                kind = TokenKind.BeginFact;
                                mItems.Dequeue();
                                break;
                        }
                    }
                    break;

                case TokenKind.OpenSquareBracket:
                    {
                        var tmpNextChar = mItems.Peek();

                        switch (tmpNextChar)
                        {
                            case ':':
                                kind = TokenKind.BeginAnnotaion;
                                mItems.Dequeue();
                                break;
                        }
                    }
                    break;

                case TokenKind.Colon:
                    {
                        var tmpNextChar = mItems.Peek();
                        switch (tmpNextChar)
                        {
                            case '}':
                                kind = TokenKind.EndFact;
                                mItems.Dequeue();
                                break;

                            case ']':
                                kind = TokenKind.EndAnnotation;
                                mItems.Dequeue();
                                break;
                        }
                    }
                    break;

                case TokenKind.Minus:
                    {
                        var tmpNextChar = mItems.Peek();
                        switch (tmpNextChar)
                        {
                            case '>':
                                kind = TokenKind.RightwardArrow;
                                mItems.Dequeue();
                                break;
                        }
                    }
                    break;

                case TokenKind.Less:
                    {
                        var tmpNextChar = mItems.Peek();
                        switch (tmpNextChar)
                        {
                            case '-':
                                kind = TokenKind.LeftwardArrow;
                                mItems.Dequeue();
                                tmpNextChar = mItems.Peek();
                                switch (tmpNextChar)
                                {
                                    case '>':
                                        kind = TokenKind.LeftRightArrow;
                                        mItems.Dequeue();
                                        break;
                                }
                                break;
                        }
                    }
                    break;

                case TokenKind.AtSign:
                    {
                        var tmpNextChar = mItems.Peek();

#if DEBUG
                        //LogInstance.Log($"tmpNextChar = {tmpNextChar}");
#endif

                        switch (tmpNextChar)
                        {
                            case '@':
                                kind = TokenKind.DoubleAtSign;
                                mItems.Dequeue();
                                break;

                            case '$':
                                kind = TokenKind.AtSignDollar;
                                mItems.Dequeue();
                                break;
                        }
                    }
                    break;

                case TokenKind.Sharp:
                    {
                        var tmpNextChar = mItems.Peek();

                        switch (tmpNextChar)
                        {
                            case '#':
                                kind = TokenKind.DoubleSharp;
                                mItems.Dequeue();
                                break;

                            case '@':
                                kind = TokenKind.SharpAtSign;
                                mItems.Dequeue();
                                break;
                        }
                    }
                    break;

                case TokenKind.Not:
                    {
                        var tmpNextChar = mItems.Peek();
                        switch(tmpNextChar)
                        {
                            case ':':
                                kind = TokenKind.BeginAccessPolicy;
                                mItems.Dequeue();
                                break;
                        }
                    }
                    break;
            }

            var result = new Token();
            result.TokenKind = kind;
            result.KeyWordTokenKind = kindOfKeyWord;
            result.Content = content;
            result.Pos = mCurrentPos - contentLength;
            result.Line = mCurrentLine;

            return result;
        }

        public void Recovery(Token token)
        {
            mRecoveriesTokens.Enqueue(token);
        }

        public int Count
        {
            get
            {
                return mRecoveriesTokens.Count + mItems.Count;
            }
        }

        public string TailOfString
        {
            get
            {
                var listOfChars = mItems.ToList();

                var sb = new StringBuilder();

                foreach (var ch in listOfChars)
                {
                    sb.Append(ch);
                }
                return sb.ToString();
            }
        }
    }
}
