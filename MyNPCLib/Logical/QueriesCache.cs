using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class QueriesCache
    {
        public QueriesCache(IEntityDictionary entityDictionary)
        {
            mEntityDictionary = entityDictionary;
        }

        private IEntityDictionary mEntityDictionary;
        private readonly object mQueriesDictionaryLockObj = new object();
        private Dictionary<string, BaseQueryResolverASTNode> mQueriesDictionary = new Dictionary<string, BaseQueryResolverASTNode>();

        public BaseQueryResolverASTNode CreatePlan(string query)
        {
            var keyQuery = query.Replace(" ", string.Empty).ToLower().Trim();

            lock(mQueriesDictionaryLockObj)
            {
                if(mQueriesDictionary.ContainsKey(keyQuery))
                {
                    return mQueriesDictionary[keyQuery];
                }

                var rootQueryNode = QueryASTNodeFactory.CreateASTNode(query, mEntityDictionary);

#if DEBUG
                //LogInstance.Log($"QueriesCache CreatePlan rootQueryNode = {rootQueryNode}");
#endif

                var plan = QueryResolverASTNodeFactory.CreatePlan(rootQueryNode);

                return plan;
            }
        }
    }
}
