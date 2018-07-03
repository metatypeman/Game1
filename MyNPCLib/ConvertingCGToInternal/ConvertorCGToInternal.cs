using MyNPCLib.CG;
using MyNPCLib.InternalCG;
using System;
using System.Collections.Generic;
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

            var context = new ContextOfConvertingCGToInternal();
            return ConvertConceptualGraph(source, context);
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
