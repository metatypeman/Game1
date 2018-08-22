﻿using MyNPCLib.CGStorage;
using MyNPCLib.IndexedPersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class LogicalSearcher
    {
        public LogicalSearcher(ContextOfCGStorage contextOfCGStorage)
        {
            mContextOfCGStorage = contextOfCGStorage;
        }

        private ContextOfCGStorage mContextOfCGStorage;

        public LogicalSearchResult Run(LogicalSearchOptions options)
        {
#if DEBUG
            //LogInstance.Log($"options = {options}");
#endif
            var queryExpression = options.QueryExpression;

            var result = new LogicalSearchResult();
            result.QueryExpression = queryExpression;

            var entityDictionary = mContextOfCGStorage.EntityDictionary;

            var context = new LogicalSearchContext();
            context.QueryExpression = queryExpression;
            context.EntityDictionary = entityDictionary;
            context.DataSource = options.DataSource;

#if DEBUG
            //LogInstance.Log($"context = {context}");
#endif

            var resultItemsList = new List<LogicalSearchResultItem>();

            var queryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExpression.FillExecutingCard(queryExecutingCard, context);

#if DEBUG
            //LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

            foreach (var resultOfQueryToRelation in queryExecutingCard.ResultsOfQueryToRelationList)
            {
                var resultItem = new LogicalSearchResultItem(entityDictionary);
                resultItem.QueryExpression = queryExpression;
                resultItem.ResultOfVarOfQueryToRelationList = resultOfQueryToRelation.ResultOfVarOfQueryToRelationList;
                resultItemsList.Add(resultItem);
                resultItem.Ready();
            }

            result.Items = resultItemsList;

#if DEBUG
            //LogInstance.Log("End");
#endif

            return result;
        }
    }
}
