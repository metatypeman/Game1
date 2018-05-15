//OpenNLPForNS is based on https://github.com/AlexPoint/OpenNlp
//I just need OpenNLP what is based on Net. Standard 1.6. 

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.PosTagger
{
    /// <summary> 
    /// The interface for part of speech taggers.
    /// </summary>
    public interface IPosTagger
    {

        /// <summary>Assigns the sentence of tokens pos tags</summary>
        /// <param name="tokens">The sentence of tokens to be tagged</param>
        /// <returns>An array of pos tags for each token provided in sentence</returns>
        string[] Tag(string[] tokens);

        /// <summary> Assigns pos tags to the sentence of space-delimited tokens</summary>
        /// <param name="sentence">The sentence of space-delimited tokens to be tagged</param>
        /// <returns>A collection of tagged words (word + pos tag + index in sentence)</returns>
        List<TaggedWord> TagSentence(string sentence);
    }
}
