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

            var verbOfSentence = sentence.VerbPhrase;

            if(verbOfSentence != null)
            {
                var verbDtNode = new VerbDTNode();
                result.Verb = verbDtNode;
                FillRootVerbOfSentence(verbOfSentence, verbDtNode);
            }

            return result;
        }

        private static void FillRootVerbOfSentence(VerbPhrase verbPhrase, VerbDTNode dest)
        {
#if DEBUG
            LogInstance.Log($"verbPhrase = {verbPhrase}");
#endif

            dest.ExtendedToken = verbPhrase.Verb;

            var verbObject = verbPhrase.Object;

            if (verbObject != null)
            {
                if (verbObject.IsPrepositionalPhrase)
                {
                    var prepositionalObject = verbObject.AsPrepositionalPhrase;

#if DEBUG
                    LogInstance.Log($"prepositionalObject = {prepositionalObject}");
#endif
                    var prepositionalDTNode = new PrepositionalDTNode();
                    dest.AddPrepositionalObject(prepositionalDTNode);
                    FillPrepositional(prepositionalObject, prepositionalDTNode);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private static void FillPrepositional(PrepositionalPhrase prepositionalPhrase, PrepositionalDTNode dest)
        {
            dest.ExtendedToken = prepositionalPhrase.Preposition;

            var prepositionObject = prepositionalPhrase.Object;

            if (prepositionObject != null)
            {
#if DEBUG
                LogInstance.Log($"prepositionObject = {prepositionObject}");
#endif

                if (prepositionObject.IsAdjectivePhrase)
                {
                    var adjectiveDTNode = new AdjectiveDTNode();

#if DEBUG
                    LogInstance.Log($"adjectiveDTNode = {adjectiveDTNode}");
#endif

                    //dest.AdjectiveObject = adjectiveDTNode;
                    //FillAdjective(prepositionObject.AsAdjectivePhrase, adjectiveDTNode);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        //        private static void FillAdjective(AdjectivePhrase adjectivePhrase, AdjectiveDTNode dest)
        //        {
        //            dest.AdjectiveExtendedToken = adjectivePhrase.Adjective;

        //            var ajectiveObject = adjectivePhrase.Object;

        //            if(ajectiveObject != null)
        //            {
        //#if DEBUG
        //                LogInstance.Log($"ajectiveObject = {ajectiveObject}");
        //#endif

        //                if(ajectiveObject.IsNounPhrase)
        //                {

        //                    var nounDtNode = new NounDTNode();
        //                    var parentOfAjective = dest.Parent;
        //                    parentOfAjective.SetObject(nounDtNode);
        //                    nounDtNode.AddAjective(dest);

        //                    FillNoun(ajectiveObject.AsNounPhrase, nounDtNode);
        //                }
        //                else
        //                {
        //                    throw new NotImplementedException();
        //                }
        //            }
        //        }

        //        private static void FillNoun(NounPhrase nounPhrase, NounDTNode dest)
        //        {
        //            dest.NounExtendedToken = nounPhrase.Noun;
        //        }
    }
}
