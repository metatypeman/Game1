using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class ProbabilityFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Probability;

        public ProbabilityFuzzyModality Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new ProbabilityFuzzyModality();
            FillForClone(result, context);
            return result;
        }
    }
}
