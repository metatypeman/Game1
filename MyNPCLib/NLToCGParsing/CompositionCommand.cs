using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public enum CompositionCommand
    {
        Undefined,
        AddToNounPhraseOfSentence,
        AddToVerbPhraseOfSentence,
        AddToObjectOfVP,
        PutNounInNP,
        PutDeterminerInNP,
        PutVerbInVP
    }
}
