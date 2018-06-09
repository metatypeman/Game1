using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.PersistLogicalData
{
    /// <summary>
    /// Differences between Reality, Possibility and Probability (in my opinion):
    /// For instance, look at president of USA.
    /// President of USA is real in our world. For instance, fairy is not real in the world (not in a fantasy world).
    /// Each borned in USA citizen can be president of USA. It is possible for him.
    /// But each borned not in USA can not be president of USA. It is impossible for him.
    /// How much probability does for borned in USA citizen to be a president of USA?
    /// There is a big probability for some people borned in USA.
    /// There is a very small probability for other people borned in USA, but it is non zero.
    /// There is exist a relation between Possibility and Probability: the probability is small or equals zero if it is impossible.
    /// </summary>
    public enum KindOfModality
    {
        /// <summary>
        /// This fact (rule) is desirable (optative) or not.
        /// </summary>
        Desirable,

        /// <summary>
        /// This fact (rule) is necessary or not
        /// </summary>
        Necessity,

        /// <summary>
        /// Does NPC intentional to do it?
        /// </summary>
        Intentionally,

        /// <summary>
        /// Represents external power.
        /// </summary>
        Imperative,

        /// <summary>
        /// Represents priority of the fact (rule).
        /// </summary>
        Priority,

        /// <summary>
        /// This fact (rule) is real in the world or not.
        /// </summary>
        Reality,

        /// <summary>
        /// This fact (rule) is possible or not.
        /// </summary>
        Possibility,

        /// <summary>
        /// This fact (rule) is probable or not.
        /// </summary>
        Probability,

        /// <summary>
        /// 1 is good, 0 is bad.
        /// </summary>
        MoralQuality,

        /// <summary>
        /// 1 is much, 0 is few
        /// </summary>
        QuantityQuality
    }
}
