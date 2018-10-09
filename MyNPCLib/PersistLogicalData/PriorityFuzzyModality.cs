using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class PriorityFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Priority;

        public PriorityFuzzyModality Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new PriorityFuzzyModality();
            FillForClone(result, context);
            return result;
        }
    }
}
