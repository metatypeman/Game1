﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    [Serializable]
    public class QuantityQualityFuzzyModality : FuzzyModality
    {
        public override KindOfModality Kind => KindOfModality.QuantityQuality;
    }
}