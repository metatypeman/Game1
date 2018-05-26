using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Dot
{
    public class DotConverter
    {
        public static string ConvertToString(ICGNode node)
        {
            var tmpContext = new DotContext();

            var tmpMainLeaf = tmpContext.CreateRootLeaf(node);

            tmpMainLeaf.Run();

            return tmpMainLeaf.Text;
        }
    }
}
