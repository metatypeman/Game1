using MyNPCLib.CGStorage;
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
            var queryExpression = options.QueryExpression;

            var result = new LogicalSearchResult();
            result.QueryExpression = queryExpression;

            var entityDictionary = mContextOfCGStorage.EntityDictionary;

            var context = new LogicalSearchContext();
            context.QueryExpression = queryExpression;
            context.EntityDictionary = entityDictionary;
            context.DataSourcesSettingsOrderedByPriorityList = options.DataSourcesSettings.OrderBy(p => p.Priority).ToList();
            context.DataSourcesSettingsOrderedByPriorityAndUseProductionsList = context.DataSourcesSettingsOrderedByPriorityList.Where(p => p.UseProductions).ToList();

            var resultItemsList = new List<LogicalSearchResultItem>();

            var queryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExpression.FillExecutingCard(queryExecutingCard, context);

            foreach (var resultOfQueryToRelation in queryExecutingCard.ResultsOfQueryToRelationList)
            {
                var resultItem = new LogicalSearchResultItem(entityDictionary);
                resultItem.QueryExpression = queryExpression;
                resultItem.ResultOfVarOfQueryToRelationList = resultOfQueryToRelation.ResultOfVarOfQueryToRelationList;
                resultItemsList.Add(resultItem);
            }

            result.Items = resultItemsList;
            return result;
        }
    }
}
