using MyNPCLib.NLToCGParsing.DependencyTree;
using MyNPCLib.NLToCGParsing.PhraseTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public static class DTNodeConverter
    {
        public static SentenceDTNode Convert(Sentence sentence)
        {
#if DEBUG
            LogInstance.Log($"sentence = {sentence}");
#endif

            var result = new SentenceDTNode();
            result.Aspect = sentence.Aspect;
            result.Tense = sentence.Tense;
            result.Voice = sentence.Voice;
            result.Mood = sentence.Mood;
            result.Modal = sentence.Modal;
            result.Verb = CovertRootVerbOfSentence(sentence);

            return result;
        }

        private static VerbDTNode CovertRootVerbOfSentence(Sentence sentence)
        {
            var verbOfSentence = sentence.VerbPhrase;

            if(verbOfSentence == null)
            {
                return null;
            }

#if DEBUG
            LogInstance.Log($"verbOfSentence = {verbOfSentence}");
#endif

            var result = new VerbDTNode();
            result.VerbExtendedToken = verbOfSentence.Verb;

            var verbObject = verbOfSentence.Object;

            if (verbObject != null)
            {
                if(verbObject.IsPrepositionalPhrase)
                {
                    var prepositionalObject = verbObject.AsPrepositionalPhrase;

#if DEBUG
                    LogInstance.Log($"prepositionalObject = {prepositionalObject}");
#endif

                    var prepositionalDTNode = ConvertPrepositional(prepositionalObject);
                    result.PrepositionalObjectsList.Add(prepositionalDTNode);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return result;
        }

        private static PrepositionalDTNode ConvertPrepositional(PrepositionalPhrase prepositionalPhrase)
        {
            var result = new PrepositionalDTNode();
            result.PrepositionalExtendedToken = prepositionalPhrase.Preposition;

            var prepositionObject = prepositionalPhrase.Object;

            if(prepositionObject != null)
            {
#if DEBUG
                LogInstance.Log($"prepositionObject = {prepositionObject}");
#endif

                if(prepositionObject.IsAdjectivePhrase)
                {
                    var adjectiveDTNode = ConvertAdjective(prepositionObject.AsAdjectivePhrase);

#if DEBUG
                    LogInstance.Log($"adjectiveDTNode = {adjectiveDTNode}");
#endif

                    //result.
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return result;
        }

        private static AdjectiveDTNode ConvertAdjective(AdjectivePhrase adjectivePhrase)
        {
            var result = new AdjectiveDTNode();

            return result;
        }
    }
}
