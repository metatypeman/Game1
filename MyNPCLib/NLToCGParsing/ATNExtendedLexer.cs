using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNExtendedLexer
    {
        public ATNExtendedLexer(string text, IWordsDict wordsDict)
        {
            mLexer = new ATNLexer(text);
            mWordsDict = wordsDict;
        }

        private ATNExtendedLexer()
        {
        }

        private readonly object mLockObj = new object();
        private ATNLexer mLexer;
        private IWordsDict mWordsDict;
        private Queue<IList<ATNExtendedToken>> mRecoveriesTokens = new Queue<IList<ATNExtendedToken>>();

        public ATNExtendedLexer Fork()
        {
            lock(mLockObj)
            {
                var result = new ATNExtendedLexer();
                result.mWordsDict = mWordsDict;
                result.mLexer = mLexer.Fork();
                result.mRecoveriesTokens = new Queue<IList<ATNExtendedToken>>(mRecoveriesTokens.ToList());
                return result;
            }
        }

        public IList<ATNExtendedToken> GetСlusterOfExtendedTokens()
        {
            lock (mLockObj)
            {
                if (mRecoveriesTokens.Count > 0)
                {
                    return mRecoveriesTokens.Dequeue();
                }

                var token = mLexer.GetToken();

#if DEBUG
                //LogInstance.Log($"token = {token}");
#endif

                if (token == null)
                {
                    return null;
                }

                var result = new List<ATNExtendedToken>();

                var tokenKind = token.Kind;

                if (tokenKind != KindOfATNToken.Word)
                {
                    if (tokenKind == KindOfATNToken.SingleQuotationMark)
                    {
                        var nextToken = mLexer.GetToken();
#if DEBUG
                        //LogInstance.Log($"nextToken = {nextToken}");
#endif
                        var nextTokenKind = nextToken.Kind;

                        if (nextTokenKind == KindOfATNToken.Word)
                        {
                            if (nextToken.Content == "ll")
                            {
                                token.Content = "will";
                                token.Kind = KindOfATNToken.Word;
                                return ProcessWordToken(token);
                            }
                            mLexer.Recovery(nextToken);
                        }
                        else
                        {
                            mLexer.Recovery(nextToken);
                        }
                    }

                    result.Add(CreateExtendToken(token));
                    return result;
                }

                return ProcessWordToken(token);
            }
        }

        private IList<ATNExtendedToken> ProcessWordToken(ATNToken token)
        {
            var result = new List<ATNExtendedToken>();

            var tokenContent = token.Content;

#if DEBUG
            //LogInstance.Log($"tokenContent = {tokenContent}");
#endif

            if (!mWordsDict.IsName(tokenContent))
            {
                tokenContent = tokenContent.ToLower();
            }

            var wordFrame = mWordsDict.GetWordFrame(tokenContent);

#if DEBUG
            //LogInstance.Log($"wordFrame = {wordFrame}");
#endif

            if (wordFrame == null || wordFrame.GrammaticalWordFrames.IsEmpty())
            {
                result.Add(CreateExtendToken(token));
                return result;
            }

            foreach (var grammaticalWordFrame in wordFrame.GrammaticalWordFrames)
            {
                var extendsToken = CreateExtendToken(token, grammaticalWordFrame);
                result.Add(extendsToken);
            }

            return result;
        }

        private ATNExtendedToken CreateExtendToken(ATNToken sourceToken)
        {
            var result = new ATNExtendedToken();
            result.Kind = sourceToken.Kind;
            result.Content = sourceToken.Content;
            result.Pos = sourceToken.Pos;
            result.Line = sourceToken.Line;
            return result;
        }

        private ATNExtendedToken CreateExtendToken(ATNToken sourceToken, BaseGrammaticalWordFrame grammaticalWordFrame)
        {
            var partOfSpeech = grammaticalWordFrame.PartOfSpeech;

            switch(partOfSpeech)
            {
                case GrammaticalPartOfSpeech.Noun:
                    return CreateNounExtendToken(sourceToken, grammaticalWordFrame.AsNoun);

                case GrammaticalPartOfSpeech.Pronoun:
                    return CreatePronounExtendToken(sourceToken, grammaticalWordFrame.AsPronoun);

                case GrammaticalPartOfSpeech.Adjective:
                    return CreateAdjectiveExtendToken(sourceToken, grammaticalWordFrame.AsAdjective);

                case GrammaticalPartOfSpeech.Verb:
                    return CreateVerbExtendToken(sourceToken, grammaticalWordFrame.AsVerb);

                case GrammaticalPartOfSpeech.Adverb:
                    return CreateAdverbExtendToken(sourceToken, grammaticalWordFrame.AsAdverb);

                case GrammaticalPartOfSpeech.Preposition:
                    return CreatePrepositionExtendToken(sourceToken, grammaticalWordFrame.AsPreposition);

                case GrammaticalPartOfSpeech.Conjunction:
                    return CreateConjunctionExtendToken(sourceToken, grammaticalWordFrame.AsConjunction);

                case GrammaticalPartOfSpeech.Interjection:
                    return CreateInterjectionExtendToken(sourceToken, grammaticalWordFrame.AsInterjection);

                case GrammaticalPartOfSpeech.Article:
                    return CreateArticleExtendToken(sourceToken, grammaticalWordFrame.AsArticle);

                case GrammaticalPartOfSpeech.Numeral:
                    return CreateNumeralExtendToken(sourceToken, grammaticalWordFrame.AsNumeral);

                default:
                    return null;
            }
        }

        private ATNExtendedToken CreateNounExtendToken(ATNToken sourceToken, NounGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            FillBaseGrammaticalWordFrame(grammaticalWordFrame, result);
            result.IsName = grammaticalWordFrame.IsName;
            result.IsShortForm = grammaticalWordFrame.IsShortForm;
            result.Gender = grammaticalWordFrame.Gender;
            result.Number = grammaticalWordFrame.Number;
            result.IsCountable = grammaticalWordFrame.IsCountable;
            result.IsGerund = grammaticalWordFrame.IsGerund;
            result.IsPossessive = grammaticalWordFrame.IsPossessive;
            return result;
        }

        private ATNExtendedToken CreatePronounExtendToken(ATNToken sourceToken, PronounGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            FillBaseGrammaticalWordFrame(grammaticalWordFrame, result);
            result.Gender = grammaticalWordFrame.Gender;
            result.Number = grammaticalWordFrame.Number;
            result.Person = grammaticalWordFrame.Person;
            result.TypeOfPronoun = grammaticalWordFrame.TypeOfPronoun;
            result.CaseOfPersonalPronoun = grammaticalWordFrame.Case;
            result.IsQuestionWord = grammaticalWordFrame.IsQuestionWord;
            return result;
        }

        private ATNExtendedToken CreateAdjectiveExtendToken(ATNToken sourceToken, AdjectiveGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            FillBaseGrammaticalWordFrame(grammaticalWordFrame, result);
            result.Comparison = grammaticalWordFrame.Comparison;
            result.IsDeterminer = grammaticalWordFrame.IsDeterminer;
            return result;
        }

        private ATNExtendedToken CreateVerbExtendToken(ATNToken sourceToken, VerbGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            FillBaseGrammaticalWordFrame(grammaticalWordFrame, result);
            result.VerbType = grammaticalWordFrame.VerbType;
            result.Number = grammaticalWordFrame.Number;
            result.Person = grammaticalWordFrame.Person;
            result.Tense = grammaticalWordFrame.Tense;
            result.IsModal = grammaticalWordFrame.IsModal;
            result.IsFormOfToBe = grammaticalWordFrame.IsFormOfToBe;
            result.IsFormOfToHave = grammaticalWordFrame.IsFormOfToHave;
            result.IsFormOfToDo = grammaticalWordFrame.IsFormOfToDo;
            return result;
        }

        private ATNExtendedToken CreateAdverbExtendToken(ATNToken sourceToken, AdverbGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            FillBaseGrammaticalWordFrame(grammaticalWordFrame, result);
            result.Comparison = grammaticalWordFrame.Comparison;
            result.IsQuestionWord = grammaticalWordFrame.IsQuestionWord;
            result.IsDeterminer = grammaticalWordFrame.IsDeterminer;
            return result;
        }

        private ATNExtendedToken CreatePrepositionExtendToken(ATNToken sourceToken, PrepositionGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            FillBaseGrammaticalWordFrame(grammaticalWordFrame, result);
            return result;
        }

        private ATNExtendedToken CreateConjunctionExtendToken(ATNToken sourceToken, ConjunctionGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            FillBaseGrammaticalWordFrame(grammaticalWordFrame, result);
            return result;
        }

        private ATNExtendedToken CreateInterjectionExtendToken(ATNToken sourceToken, InterjectionGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            FillBaseGrammaticalWordFrame(grammaticalWordFrame, result);
            return result;
        }

        private ATNExtendedToken CreateArticleExtendToken(ATNToken sourceToken, ArticleGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            FillBaseGrammaticalWordFrame(grammaticalWordFrame, result);
            result.Number = grammaticalWordFrame.Number;
            result.IsDeterminer = grammaticalWordFrame.IsDeterminer;
            return result;
        }

        private ATNExtendedToken CreateNumeralExtendToken(ATNToken sourceToken, NumeralGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            FillBaseGrammaticalWordFrame(grammaticalWordFrame, result);
            result.NumeralType = grammaticalWordFrame.NumeralType;
            return result;
        }

        private void FillBaseGrammaticalWordFrame(BaseGrammaticalWordFrame source, ATNExtendedToken dest)
        {
            dest.PartOfSpeech = source.PartOfSpeech;
            dest.LogicalMeaning = source.LogicalMeaning;
            dest.FullLogicalMeaning = source.FullLogicalMeaning;
            dest.RootWord = source.RootWord;
        }

        public void Recovery(IList<ATNExtendedToken> tokensList)
        {
            lock (mLockObj)
            {
                mRecoveriesTokens.Enqueue(tokensList);
            }
        }
    }
}
