using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class DesirableFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.Desirable;
    }
}
