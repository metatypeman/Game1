using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class QueryASTNodeResolver
    {
        public QueryASTNodeResolver(ILogicalStorage source)
        {

        }

        public IList<ulong> GetEntitiesIdListByAST(BaseQueryASTNode queryNode)
        {
#if DEBUG
            LogInstance.Log($"QueryASTNodeResolver GetEntitiesIdListByAST queryNode = {queryNode}");
#endif

            return new List<ulong>();//tmp
        }
    }
}
