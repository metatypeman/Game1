using MyNPCLib.CG;
using MyNPCLib.CommonServiceGrammaticalElements;
using MyNPCLib.InternalCG;
using MyNPCLib.NLToCGParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.ConvertingCGToInternal
{
    public static class ConvertorCGToInternal
    {
        public static InternalConceptualGraph Convert(ConceptualGraph source)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            while(IsWrapperGraph(source))
            {
                source = (ConceptualGraph)source.Children.SingleOrDefault(p => p.Kind == KindOfCGNode.Graph);
            }

#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            var context = new ContextOfConvertingCGToInternal();
            return ConvertConceptualGraph(source, context);
        }

        private static bool IsWrapperGraph(ConceptualGraph source)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            var countOfConceptualGraphs = source.Children.Count(p => p.Kind == KindOfCGNode.Graph);

#if DEBUG
            LogInstance.Log($"countOfConceptualGraphs = {countOfConceptualGraphs}");
#endif

            var kindOfRelationsList = source.Children.Where(p => p.Kind == KindOfCGNode.Relation).Select(p => GrammaticalElementsHeper.GetKindOfGrammaticalRelationFromName(p.Name));

            var isGrammaticalRelationsOnly = !kindOfRelationsList.Any(p => p == KindOfGrammaticalRelation.Undefined);

#if DEBUG
            LogInstance.Log($"isGrammaticalRelationsOnly = {isGrammaticalRelationsOnly}");
#endif

            if(countOfConceptualGraphs == 1 && isGrammaticalRelationsOnly)
            {
                return true;
            }

            return false;
        }

        private static InternalConceptualGraph ConvertConceptualGraph(ConceptualGraph source, ContextOfConvertingCGToInternal context)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            if (context.ConceptualGraphsDict.ContainsKey(source))
            {
                return context.ConceptualGraphsDict[source];
            }

            var result = new InternalConceptualGraph();

            context.ConceptualGraphsDict[source] = result;

            return result;
        }
    }
}
