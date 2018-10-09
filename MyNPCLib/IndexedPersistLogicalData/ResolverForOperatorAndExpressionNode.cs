using MyNPCLib.CGStorage;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class ResolverForOperatorAndExpressionNode: ResolverForBinaryOperatorExpressionNode
    {
        public OperatorAndExpressionNode ConcreteOrigin { get; set; }
        public override BaseExpressionNode Origin => ConcreteOrigin;
        public override KindOfExpressionNode Kind => KindOfExpressionNode.And;

        public override void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
            var senderIndexedRuleInstance = queryExecutingCard.SenderIndexedRuleInstance;
            var senderIndexedRulePart = queryExecutingCard.SenderIndexedRulePart;
            
            var leftQueryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
            leftQueryExecutingCard.SenderIndexedRuleInstance = senderIndexedRuleInstance;
            leftQueryExecutingCard.SenderIndexedRulePart = senderIndexedRulePart;
            leftQueryExecutingCard.SenderExpressionNode = Origin;
            leftQueryExecutingCard.KnownInfoList = queryExecutingCard.KnownInfoList;
            Left.FillExecutingCard(leftQueryExecutingCard, dataSource, options);

#if DEBUG
            //LogInstance.Log($"leftQueryExecutingCard = {leftQueryExecutingCard}");
#endif

            var leftQueryExecutingCardResultsOfQueryToRelationList = leftQueryExecutingCard.ResultsOfQueryToRelationList;

            if (leftQueryExecutingCardResultsOfQueryToRelationList.Count == 0)
            {
                return;
            }

            var resultsOfQueryToRelationList = queryExecutingCard.ResultsOfQueryToRelationList;

            foreach (var leftResultOfQueryToRelation in leftQueryExecutingCardResultsOfQueryToRelationList)
            {
                var rightQueryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
                rightQueryExecutingCard.SenderIndexedRuleInstance = senderIndexedRuleInstance;
                rightQueryExecutingCard.SenderIndexedRulePart = senderIndexedRulePart;
                rightQueryExecutingCard.SenderExpressionNode = Origin;
                rightQueryExecutingCard.KnownInfoList = queryExecutingCard.KnownInfoList;
                Right.FillExecutingCard(rightQueryExecutingCard, dataSource, options);

#if DEBUG
                //LogInstance.Log($"rightQueryExecutingCard = {rightQueryExecutingCard}");
#endif

                var rightQueryExecutingCardResultsOfQueryToRelationList = rightQueryExecutingCard.ResultsOfQueryToRelationList;

                if (rightQueryExecutingCardResultsOfQueryToRelationList.Count == 0)
                {
                    continue;
                }

                var leftVarsList = leftResultOfQueryToRelation.ResultOfVarOfQueryToRelationList;
                var leftVarsKeysList = leftVarsList.Select(p => p.KeyOfVar).Distinct().ToList();

                foreach (var rightResultOfQueryToRelation in rightQueryExecutingCardResultsOfQueryToRelationList)
                {
                    var rightVarsList = rightResultOfQueryToRelation.ResultOfVarOfQueryToRelationList;
                    var rightVarsKeysList = rightVarsList.Select(p => p.KeyOfVar).Distinct().ToList();
                    var intersectOfVarsKeysList = leftVarsKeysList.Intersect(rightVarsKeysList).ToList();

#if DEBUG
                    //LogInstance.Log($"intersectOfVarsKeysList.Count = {intersectOfVarsKeysList.Count}");
#endif

                    var isFit = true;

                    if (intersectOfVarsKeysList.Count == 0)
                    {
                        var resultItem = new ResultOfQueryToRelation();
                        foreach(var varItem in leftVarsList)
                        {
                            resultItem.ResultOfVarOfQueryToRelationList.Add(varItem);
                        }

                        foreach(var varItem in rightVarsList)
                        {
                            resultItem.ResultOfVarOfQueryToRelationList.Add(varItem);
                        }

                        queryExecutingCard.ResultsOfQueryToRelationList.Add(resultItem);
                    }
                    else
                    {
                        var leftVarsDict = new Dictionary<ulong, ResultOfVarOfQueryToRelation>();
                        var resultItem = new ResultOfQueryToRelation();
                        foreach (var varItem in leftVarsList)
                        {
                            var keyOfVars = varItem.KeyOfVar;
                            if (intersectOfVarsKeysList.Contains(keyOfVars))
                            {
                                leftVarsDict[keyOfVars] = varItem;
                            }
                            else
                            { 
                                resultItem.ResultOfVarOfQueryToRelationList.Add(varItem);
                                continue;
                            }
                        }

                        foreach (var varItem in rightVarsList)
                        {
                            var keyOfVars = varItem.KeyOfVar;
                            if (intersectOfVarsKeysList.Contains(keyOfVars))
                            {
                                var leftVarItem = leftVarsDict[keyOfVars];
                                var resultOfComparison = ExpressionNodeHelper.Compare(varItem.FoundExpression, leftVarItem.FoundExpression);

                                if(resultOfComparison)
                                {
                                    var originItemsDict = varItem.OriginDict;
                                    var leftVarOriginItemsDict = leftVarItem.OriginDict;

                                    foreach (var originItems in originItemsDict)
                                    {
                                        var tmpKeyOfOrigin = originItems.Key;

                                        if(!leftVarOriginItemsDict.ContainsKey(tmpKeyOfOrigin))
                                        {
                                            leftVarOriginItemsDict[tmpKeyOfOrigin] = originItems.Value;
                                        }
                                    }

                                    resultItem.ResultOfVarOfQueryToRelationList.Add(leftVarItem);
                                }
                                else
                                {
                                    isFit = false;
                                    break;
                                }                           
                            }
                            else
                            { 
                                resultItem.ResultOfVarOfQueryToRelationList.Add(varItem);
                                continue;
                            }
                        }

                        if(isFit)
                        {
                            resultsOfQueryToRelationList.Add(resultItem);
                        }
                    }
                }
            }
        }

        public override void FillExecutingCardForAnnotation(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, ICGStorage dataSource, OptionsOfFillExecutingCard options)
        {
#if DEBUG
            //LogInstance.Log("Begin");
#endif

#if DEBUG
            throw new NotImplementedException();
            //LogInstance.Log("End");
#endif
        }
    }
}
