//OpenNLPForNS is based on https://github.com/AlexPoint/OpenNlp
//I just need OpenNLP what is based on Net. Standard 1.6. 

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Parser
{
    /// <summary>
    /// Interface for encoding the head rules associated with parsing.
    /// </summary>
    public interface IHeadRules
    {
        /// <summary>
        /// Returns the head constituent for the specified constituents of the specified type.
        /// </summary>
        /// <param name="constituents">
        /// The constituents which make up a constituent of the specified type.
        /// </param>
        /// <param name="type">
        /// The type of a constituent which is made up of the specifed constituents.
        /// </param>
        /// <returns>
        /// The constituent which is the head.
        /// </returns>
        Parse GetHead(Parse[] constituents, string type);
    }
}
