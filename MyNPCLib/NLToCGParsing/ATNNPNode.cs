using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNNPNode : BaseATNParsingNode
    {
        public ATNNPNode(ATNExtendedToken extendedToken, ContextOfATNParsing context)
            : base(context)
        {
            mTargetExtendedToken = extendedToken;
        }

        private ATNExtendedToken mTargetExtendedToken;
        private bool mHasNoun;
        private NounPhrase mResult;

        public NounPhrase Run()
        {
#if DEBUG
            LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken}");
            var n = 0;
#endif

            mResult = new NounPhrase();
           
            while(true)
            {
                var resultOfIteration = RunItem();

                if(resultOfIteration == false)
                {
                    break;
                }

                var сlusterOfExtendedTokens = Context.GetСlusterOfExtendedTokens();
#if DEBUG
                LogInstance.Log($"сlusterOfExtendedTokens.Count = {сlusterOfExtendedTokens?.Count}");
#endif

                if(сlusterOfExtendedTokens.IsEmpty())
                {
                    return NTakeResult();
                }
#if DEBUG

                foreach (var extendedToken in сlusterOfExtendedTokens)
                {
                    LogInstance.Log($"extendToken = {extendedToken}");
                }
#endif

                if(сlusterOfExtendedTokens.Count != 1)
                {
                    throw new NotImplementedException();
                }

                mTargetExtendedToken = сlusterOfExtendedTokens.Single();

                var goalsList = GetGoals(mTargetExtendedToken);

#if DEBUG
                LogInstance.Log($"goalsList.Count = {goalsList.Count}");
                foreach (var goal in goalsList)
                {
                    LogInstance.Log($"goal = {goal}");
                }
#endif

                if(!goalsList.Contains( GoalOfATNExtendToken.NP))
                {
                    return NTakeResult();
                }

                n++;

                if (n > 10)
                {
                    break;
                }
            }
 
#if DEBUG
            LogInstance.Log("End");
#endif
            return NTakeResult();
        }

        private NounPhrase NTakeResult()
        {
            return mResult;
        }

        private bool RunItem()
        {
#if DEBUG
            LogInstance.Log($"mTargetExtendedToken = {mTargetExtendedToken}");
#endif

            if (mTargetExtendedToken.IsDeterminer)
            {
                throw new NotImplementedException();
            }
            else
            {
                var partOfSpeech = mTargetExtendedToken.PartOfSpeech;

                switch (partOfSpeech)
                {
                    case GrammaticalPartOfSpeech.Noun:
                        throw new NotImplementedException();

                    case GrammaticalPartOfSpeech.Pronoun:
                        {
                            var person = mTargetExtendedToken.Person;
                            switch(person)
                            {
                                case GrammaticalPerson.First:
                                    mResult.Noun = mTargetExtendedToken;
                                    mHasNoun = true;
                                    return true;
                            }
                            throw new NotImplementedException();
                        }
                        break;

                    case GrammaticalPartOfSpeech.Adjective:
                        throw new NotImplementedException();

                    case GrammaticalPartOfSpeech.Verb:
                        throw new NotImplementedException();

                    case GrammaticalPartOfSpeech.Adverb:
                        throw new NotImplementedException();

                    case GrammaticalPartOfSpeech.Preposition:
                        throw new NotImplementedException();

                    case GrammaticalPartOfSpeech.Conjunction:
                        throw new NotImplementedException();

                    case GrammaticalPartOfSpeech.Interjection:
                        throw new NotImplementedException();

                    case GrammaticalPartOfSpeech.Article:
                        throw new NotImplementedException();

                    case GrammaticalPartOfSpeech.Numeral:
                        throw new NotImplementedException();

                    default: throw new ArgumentOutOfRangeException(nameof(partOfSpeech), partOfSpeech, null);
                }
            }
        }
    }
}
