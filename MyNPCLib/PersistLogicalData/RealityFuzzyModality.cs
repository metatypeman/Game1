using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class RealityFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Reality;

        public RealityFuzzyModality Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new RealityFuzzyModality();
            FillForClone(result, context);
            return result;
        }
    }
}
