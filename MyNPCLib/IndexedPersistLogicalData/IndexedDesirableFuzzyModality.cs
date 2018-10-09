using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class IndexedDesirableFuzzyModality : IndexedFuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Desirable;
    }
}
