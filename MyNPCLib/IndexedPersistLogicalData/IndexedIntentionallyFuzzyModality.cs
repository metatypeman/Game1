using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class IndexedIntentionallyFuzzyModality : IndexedFuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Intentionally;
    }
}
