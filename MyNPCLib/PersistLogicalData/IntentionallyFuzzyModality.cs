using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class IntentionallyFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Intentionally;

        public IntentionallyFuzzyModality Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new IntentionallyFuzzyModality();
            FillForClone(result, context);
            return result;
        }
    }
}
