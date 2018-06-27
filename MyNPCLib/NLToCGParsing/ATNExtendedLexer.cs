using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
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

        private ATNLexer mLexer;
        private IWordsDict mWordsDict;

        public IList<ATNExtendToken> GetСlusterOfExtendTokens()
        {
            var token = mLexer.GetToken();

#if DEBUG
            LogInstance.Log($"token = {token}");
#endif

            if(token == null)
            {
                return null;
            }

            var result = new List<ATNExtendToken>();

            var tokenKind = token.Kind;

            if (tokenKind != KindOfATNToken.Word)
            {
                if(tokenKind == KindOfATNToken.SingleQuotationMark)
                {
                    var nextToken = mLexer.GetToken();
#if DEBUG
                    LogInstance.Log($"nextToken = {nextToken}");
#endif
                }

                result.Add(CreateExtendToken(token));
                return result;
            }

            var wordFrame = mWordsDict.GetWordFrame(token.Content);

#if DEBUG
            LogInstance.Log($"wordFrame = {wordFrame}");
#endif

            if(wordFrame == null || wordFrame.GrammaticalWordFrames.IsEmpty())
            {
                result.Add(CreateExtendToken(token));
                return result;
            }

            foreach(var grammaticalWordFrame in wordFrame.GrammaticalWordFrames)
            {
                var extendsToken = CreateExtendToken(token, grammaticalWordFrame);
                result.Add(extendsToken);
            }

            return result;
        }

        private IList<ATNExtendToken> ProcessWordToken(ATNToken token)
        {

        }

        private ATNExtendToken CreateExtendToken(ATNToken sourceToken)
        {
            var result = new ATNExtendToken();
            result.Kind = sourceToken.Kind;
            result.Content = sourceToken.Content;
            result.Pos = sourceToken.Pos;
            result.Line = sourceToken.Line;
            return result;
        }

        private ATNExtendToken CreateExtendToken(ATNToken sourceToken, BaseGrammaticalWordFrame grammaticalWordFrame)
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

        private ATNExtendToken CreateNounExtendToken(ATNToken sourceToken, NounGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            result.PartOfSpeech = grammaticalWordFrame.PartOfSpeech;
            result.IsName = grammaticalWordFrame.IsName;
            result.IsShortForm = grammaticalWordFrame.IsShortForm;
            result.Gender = grammaticalWordFrame.Gender;
            result.Number = grammaticalWordFrame.Number;
            result.IsCountable = grammaticalWordFrame.IsCountable;
            result.IsGerund = grammaticalWordFrame.IsGerund;
            result.IsPossessive = grammaticalWordFrame.IsPossessive;
            return result;
        }

        private ATNExtendToken CreatePronounExtendToken(ATNToken sourceToken, PronounGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            result.PartOfSpeech = grammaticalWordFrame.PartOfSpeech;
            result.Gender = grammaticalWordFrame.Gender;
            result.Number = grammaticalWordFrame.Number;
            result.Person = grammaticalWordFrame.Person;
            result.TypeOfPronoun = grammaticalWordFrame.TypeOfPronoun;
            result.CaseOfPersonalPronoun = grammaticalWordFrame.Case;
            result.IsQuestionWord = grammaticalWordFrame.IsQuestionWord;
            return result;
        }

        private ATNExtendToken CreateAdjectiveExtendToken(ATNToken sourceToken, AdjectiveGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            result.PartOfSpeech = grammaticalWordFrame.PartOfSpeech;
            result.Comparison = grammaticalWordFrame.Comparison;
            result.IsDeterminer = grammaticalWordFrame.IsDeterminer;
            return result;
        }

        private ATNExtendToken CreateVerbExtendToken(ATNToken sourceToken, VerbGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            result.PartOfSpeech = grammaticalWordFrame.PartOfSpeech;
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

        private ATNExtendToken CreateAdverbExtendToken(ATNToken sourceToken, AdverbGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            result.PartOfSpeech = grammaticalWordFrame.PartOfSpeech;
            result.Comparison = grammaticalWordFrame.Comparison;
            result.IsQuestionWord = grammaticalWordFrame.IsQuestionWord;
            result.IsDeterminer = grammaticalWordFrame.IsDeterminer;
            return result;
        }

        private ATNExtendToken CreatePrepositionExtendToken(ATNToken sourceToken, PrepositionGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            result.PartOfSpeech = grammaticalWordFrame.PartOfSpeech;
            return result;
        }

        private ATNExtendToken CreateConjunctionExtendToken(ATNToken sourceToken, ConjunctionGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            result.PartOfSpeech = grammaticalWordFrame.PartOfSpeech;
            return result;
        }

        private ATNExtendToken CreateInterjectionExtendToken(ATNToken sourceToken, InterjectionGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            result.PartOfSpeech = grammaticalWordFrame.PartOfSpeech;
            return result;
        }

        private ATNExtendToken CreateArticleExtendToken(ATNToken sourceToken, ArticleGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            result.PartOfSpeech = grammaticalWordFrame.PartOfSpeech;
            result.Number = grammaticalWordFrame.Number;
            result.IsDeterminer = grammaticalWordFrame.IsDeterminer;
            return result;
        }

        private ATNExtendToken CreateNumeralExtendToken(ATNToken sourceToken, NumeralGrammaticalWordFrame grammaticalWordFrame)
        {
            var result = CreateExtendToken(sourceToken);
            result.PartOfSpeech = grammaticalWordFrame.PartOfSpeech;
            result.NumeralType = grammaticalWordFrame.NumeralType;
            return result;
        }
    }
}
