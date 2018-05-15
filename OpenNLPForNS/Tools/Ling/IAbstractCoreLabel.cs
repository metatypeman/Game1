//OpenNLPForNS is based on AlexPoint/OpenNlp
//I just need OpenNLP what is based on Net. Standard 1.6.

using System;
using System.Collections.Generic;
using System.Text;
using OpenNLP.Tools.Util;

namespace OpenNLP.Tools.Ling
{
    public interface IAbstractCoreLabel : ILabel, IHasWord, IHasIndex, IHasTag, IHasLemma, IHasOffset, ITypesafeMap
    {
        string Ner();

        void SetNer(string ner);

        string OriginalText();

        void SetOriginalText(string originalText);

        string GetString(Type key);
    }
}
