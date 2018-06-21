using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public static class QueryExecutingCardAboutKnownInfoHelper
    {
        public static MergingResultOfTwoQueryExecutingCardAboutKnownInfoLists Merge(IList<QueryExecutingCardAboutKnownInfo> internalKnownInfoList, IList<QueryExecutingCardAboutVar> internalVarsInfoList, IList<QueryExecutingCardAboutKnownInfo> externalKnownInfoList)
        {
            var result = new MergingResultOfTwoQueryExecutingCardAboutKnownInfoLists();
            var targetKnownInfoList = new List<QueryExecutingCardAboutKnownInfo>();

            if(ListHelper.IsEmpty(externalKnownInfoList))
            {
                targetKnownInfoList = internalKnownInfoList.ToList();
            }
            else
            {
                var currentKnownInfoDict = internalKnownInfoList.ToDictionary(p => p.Position, p => p);
                var targetRelationVarsInfoDictByPosition = internalVarsInfoList.ToDictionary(p => p.Position, p => p.KeyOfVar);

#if DEBUG
                LogInstance.Log($"currentKnownInfoDict.Count = {currentKnownInfoDict.Count}");
                LogInstance.Log($"externalKnownInfoList.Count = {externalKnownInfoList.Count}");
#endif

                foreach (var initialKnownInfo in externalKnownInfoList)
                {
#if DEBUG
                    LogInstance.Log($"initialKnownInfo = {initialKnownInfo}");
#endif

                    if (currentKnownInfoDict.Count == 0)
                    {
                        var existingKeyOfVar = targetRelationVarsInfoDictByPosition[initialKnownInfo.Position];

#if DEBUG
                        LogInstance.Log($"existingKeyOfVar = {existingKeyOfVar}");
#endif

                        targetKnownInfoList.Add(initialKnownInfo);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            result.KnownInfoList = targetKnownInfoList;
            result.IsSuccess = true;
            return result;
        }
    }
}
