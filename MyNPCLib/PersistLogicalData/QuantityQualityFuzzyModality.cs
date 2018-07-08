using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class QuantityQualityFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.QuantityQuality;

        public QuantityQualityFuzzyModality Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new QuantityQualityFuzzyModality();
            FillForClone(result, context);
            return result;
        }
    }
}
