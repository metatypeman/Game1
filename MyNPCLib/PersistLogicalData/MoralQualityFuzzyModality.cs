using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class MoralQualityFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.MoralQuality;

        public MoralQualityFuzzyModality Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new MoralQualityFuzzyModality();
            FillForClone(result, context);
            return result;
        }
    }
}
