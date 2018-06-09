using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class ImperativeFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Imperative;
    }
}
