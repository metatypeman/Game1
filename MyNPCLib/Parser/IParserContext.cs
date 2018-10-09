using MyNPCLib.Variants;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public interface IParserContext
    {
        Token GetToken();
        void Recovery(Token token);
        bool IsEmpty();
        int Count { get; }
        ulong GetKey(string name);
        string TailOfString { get; }
        BaseVariant GetVariantByParamName(string nameOfParam);
    }
}
