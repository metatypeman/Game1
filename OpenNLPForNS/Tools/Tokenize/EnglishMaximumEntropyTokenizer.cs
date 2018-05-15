//OpenNLPForNS is based on https://github.com/AlexPoint/OpenNlp
//I just need OpenNLP what is based on Net. Standard 1.6. 

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenNLP.Tools.Tokenize
{
    /// <summary> 
    /// A tokenizer which uses default English data for the maximum entropy model.
    /// </summary>
    public class EnglishMaximumEntropyTokenizer : MaximumEntropyTokenizer
    {
        public EnglishMaximumEntropyTokenizer(string name) : base(new SharpEntropy.GisModel(new SharpEntropy.IO.BinaryGisModelReader(name)))
        {
            AlphaNumericOptimization = true;
        }
    }
}
