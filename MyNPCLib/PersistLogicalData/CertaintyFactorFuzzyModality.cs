using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class CertaintyFactorFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.CertaintyFactor;

        public CertaintyFactorFuzzyModality Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new CertaintyFactorFuzzyModality();
            FillForClone(result, context);
            return result;
        }
    }
}
