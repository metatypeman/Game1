using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.SimpleWordsDict
{
    public interface IWordsDict
    {
        WordFrame GetWordFrame(string word);
        bool IsName(string word);
    }
}
