using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public enum KindOfQuantifier
    {
        Undefined,
        /// <summary>
        /// ∀ (for all)
        /// </summary>
        Universal,
        /// <summary>
        /// ∃ (there exists, there is at least one, for some)
        /// </summary>
        Existential
    }
}
