using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class IndexedRealityFuzzyModality : IndexedFuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Reality;
    }
}
