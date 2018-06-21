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
#if DEBUG
            LogInstance.Log($"options = {options}");
#endif

            var queryExpression = options.QueryExpression;

            var result = new LogicalSearchResult();
            result.QueryExpression = queryExpression;

            var context = new LogicalSearchContext();
            context.QueryExpression = queryExpression;
            context.DataSourcesSettingsOrderedByPriorityList = options.DataSourcesSettings.OrderBy(p => p.Priority).ToList();
            context.DataSourcesSettingsOrderedByPriorityAndUseProductionsList = context.DataSourcesSettingsOrderedByPriorityList.Where(p => p.UseProductions).ToList();

#if DEBUG
            LogInstance.Log($"context = {context}");
#endif

            var resultItemsList = new List<LogicalSearchResultItem>();

            var queryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExpression.FillExecutingCard(queryExecutingCard, context);

#if DEBUG
            LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif

            result.Items = resultItemsList;
#if DEBUG
            LogInstance.Log("End");
#endif
            return result;
        }

        private IList<LogicalSearchResultItem> RunSearchingFactsInStorage(LogicalSearchContext context, SettingsOfStorageForSearchingInThisSession dataSourceSettings)
        {
#if DEBUG
            LogInstance.Log("Begin");
            LogInstance.Log($"dataSourceSettings = {dataSourceSettings}");
#endif
            var queryExpression = context.QueryExpression;
            var storage = dataSourceSettings.Storage;

            var contextForQueryExecutingCard = new ContextOfQueryExecutingCardForIndexedPersistLogicalData();
            contextForQueryExecutingCard.QueryExpression = queryExpression;
            contextForQueryExecutingCard.UseProductions = dataSourceSettings.UseProductions;
            contextForQueryExecutingCard.MaxDeph = dataSourceSettings.MaxDeph;

            var queryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();

            queryExpression.FillExecutingCardForFacts(queryExecutingCard, storage, contextForQueryExecutingCard);

#if DEBUG
            LogInstance.Log($"queryExecutingCard = {queryExecutingCard}");
#endif
            var entityDictionary = mContextOfCGStorage.EntityDictionary;
            var result = new List<LogicalSearchResultItem>();

            foreach (var resultOfQueryToRelation in queryExecutingCard.ResultsOfQueryToRelationList)
            {
                var resultItem = new LogicalSearchResultItem(entityDictionary);
                resultItem.QueryExpression = queryExpression;
                resultItem.ResultOfVarOfQueryToRelationList = resultOfQueryToRelation.ResultOfVarOfQueryToRelationList;
                result.Add(resultItem);
            }

#if DEBUG
            LogInstance.Log("End");
#endif

            return result;
        }
    }
}
