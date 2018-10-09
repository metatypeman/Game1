using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class IndexedProbabilityFuzzyModality : IndexedFuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Probability;
    }
}
