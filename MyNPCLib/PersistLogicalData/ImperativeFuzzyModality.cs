using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class ImperativeFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Imperative;

        public ImperativeFuzzyModality Clone(CloneContextOfPersistLogicalData context)
        {
            var result = new ImperativeFuzzyModality();
            FillForClone(result, context);
            return result;
        }
    }
}
