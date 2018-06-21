using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    public static class QueryExecutingCardAboutKnownInfoHelper
    {
        public static MergingResultOfTwoQueryExecutingCardAboutKnownInfoLists Merge(IList<QueryExecutingCardAboutKnownInfo> internalKnownInfoList, IList<QueryExecutingCardAboutVar> internalVarsInfoList, IList<QueryExecutingCardAboutKnownInfo> externalKnownInfoList, bool inPartFromRelationForProduction)
        {
            var result = new MergingResultOfTwoQueryExecutingCardAboutKnownInfoLists();
            var targetKnownInfoList = new List<QueryExecutingCardAboutKnownInfo>();

#if DEBUG
            LogInstance.Log($"inPartFromRelationForProduction = {inPartFromRelationForProduction}");
#endif

            if (ListHelper.IsEmpty(externalKnownInfoList))
            {
                targetKnownInfoList = internalKnownInfoList.ToList();
            }
            else
            {
                var currentKnownInfoDict = internalKnownInfoList.ToDictionary(p => p.Position, p => p);
                var targetRelationVarsInfoDictByPosition = internalVarsInfoList.ToDictionary(p => p.Position, p => p);
                var targetRelationVarsInfoDictByKeyOfVar = internalVarsInfoList.ToDictionary(p => p.KeyOfVar, p => p);

#if DEBUG
                LogInstance.Log($"currentKnownInfoDict.Count = {currentKnownInfoDict.Count}");
                LogInstance.Log($"externalKnownInfoList.Count = {externalKnownInfoList.Count}");
#endif

                foreach (var initialKnownInfo in externalKnownInfoList)
                {
#if DEBUG
                    LogInstance.Log($"initialKnownInfo = {initialKnownInfo}");
#endif
                    if(inPartFromRelationForProduction)
                    {
                        var position = initialKnownInfo.Position;

#if DEBUG
                        LogInstance.Log($"position = {position}");
#endif

                        if (position.HasValue)
                        {
                            var existingVar = targetRelationVarsInfoDictByPosition[position.Value];

#if DEBUG
                            LogInstance.Log($"existingVar = {existingVar}");
#endif

                            var resultKnownInfo = initialKnownInfo.Clone();
                            resultKnownInfo.KeyOfVar = existingVar.KeyOfVar;
                            targetKnownInfoList.Add(resultKnownInfo);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        var keyOfVar = initialKnownInfo.KeyOfVar;

#if DEBUG
                        LogInstance.Log($"keyOfVar = {keyOfVar}");
#endif

                        if(keyOfVar.HasValue)
                        {
                            var keyOfVarValue = keyOfVar.Value;

                            if(targetRelationVarsInfoDictByKeyOfVar.ContainsKey(keyOfVarValue))
                            {
                                var existingVar = targetRelationVarsInfoDictByKeyOfVar[keyOfVarValue];

#if DEBUG
                                LogInstance.Log($"existingVar = {existingVar}");
#endif

                                var resultKnownInfo = initialKnownInfo.Clone();
                                resultKnownInfo.KeyOfVar = keyOfVar;
                                resultKnownInfo.Position = existingVar.Position;
                                targetKnownInfoList.Add(resultKnownInfo);
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }                
                    }
//                    if (currentKnownInfoDict.Count == 0)
//                    {
//                        var position = initialKnownInfo.Position;

//                        if(position.HasValue)
//                        {
//                            var existingVar = targetRelationVarsInfoDictByPosition[position.Value];

//#if DEBUG
//                            LogInstance.Log($"existingVar = {existingVar}");
//#endif

//                            var resultKnownInfo = initialKnownInfo.Clone();
//                            resultKnownInfo.KeyOfVar = existingVar.KeyOfVar;
//                            targetKnownInfoList.Add(resultKnownInfo);
//                        }
//                        else
//                        {
//                            throw new NotImplementedException();
//                        }
//                    }
//                    else
//                    {
//                        throw new NotImplementedException();
//                    }
                }
            }

            result.KnownInfoList = targetKnownInfoList;
            result.IsSuccess = true;
            return result;
        }
    }
}
