using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public enum StateOfATNParsing
    {
        Undefined,
        NP,
        /// <summary>
        /// She helps me; The Earth is round.
        /// </summary>
        NP_VP
    }
}
