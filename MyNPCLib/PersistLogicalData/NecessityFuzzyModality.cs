using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class NecessityFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Necessity;

        public NecessityFuzzyModality Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new NecessityFuzzyModality();
            FillForClone(result, context);
            return result;
        }
    }
}
