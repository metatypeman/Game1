﻿using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class SentenceNodeOfSemanticAnalyzer: BaseNodeOfSemanticAnalyzer
    {
        public SentenceNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, Sentence sentence)
            : base(context)
        {         
            mSentence = sentence;
        }
    
        private Sentence mSentence;
        private ConceptualGraph mConceptualGraph;

        public ResultOfNodeOfSemanticAnalyzer Run()
        {
#if DEBUG
            LogInstance.Log($"mSentence = {mSentence}");
#endif

            var result = new ResultOfNodeOfSemanticAnalyzer();
            var resultPrimaryRolesDict = result.PrimaryRolesDict;
            var resultSecondaryRolesDict = result.SecondaryRolesDict;
            mConceptualGraph = new ConceptualGraph();
            Context.ConceptualGraph = mConceptualGraph;
            mConceptualGraph.Name = NamesHelper.CreateEntityName();

            if(mSentence.NounPhrase != null)
            {
                var nounPhraseNode = new NounPhraseNodeOfSemanticAnalyzer(Context, mSentence.NounPhrase);
                var nounResult = nounPhraseNode.Run();

#if DEBUG
                LogInstance.Log($"nounResult = {nounResult}");
#endif

                PrimaryRolesDict.Assing(nounResult.PrimaryRolesDict);
            }

            if (mSentence.VerbPhrase != null)
            {
                var verbPhraseNode = new VerbPhraseNodeOfSemanticAnalyzer(Context, mSentence.VerbPhrase);
                var verbResult = verbPhraseNode.Run();

#if DEBUG
                LogInstance.Log($"verbResult = {verbResult}");
#endif

                PrimaryRolesDict.Assing(verbResult.PrimaryRolesDict);
            }

#if DEBUG
            LogInstance.Log($"PrimaryRolesDict = {PrimaryRolesDict}");
#endif

#if DEBUG
            LogInstance.Log("End");
#endif

            return result;
        }
    }
}
