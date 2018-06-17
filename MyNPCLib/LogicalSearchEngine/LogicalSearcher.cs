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
            LogInstance.Log("Begin");
            LogInstance.Log($"options = {options}");
#endif

            var result = new LogicalSearchResult();
            result.QueryExpression = options.QueryExpression;

            var context = new LogicalSearchContext();
            context.QueryExpression = options.QueryExpression;
            context.DataSourcesSettings = options.DataSourcesSettings.GroupBy(p => p.Priority).ToDictionary(p => p.Key, p => (IList<SettingsOfStorageForSearchingInThisSession>)p.ToList());

#if DEBUG
            LogInstance.Log($"context = {context}");
            LogInstance.Log($"context.DataSourcesSettings.Count = {context.DataSourcesSettings.Count}");
#endif

            var resultItemsList = new List<LogicalSearchResultItem>();

            foreach (var dataSourceSettingsGroup in context.DataSourcesSettings)
            {
#if DEBUG
                LogInstance.Log($"dataSourceSettingsGroup.Key = {dataSourceSettingsGroup.Key}");
                LogInstance.Log($"dataSourceSettingsGroup.Value.Count = {dataSourceSettingsGroup.Value.Count}");
#endif
                foreach (var dataSourceSettings in dataSourceSettingsGroup.Value)
                {
                    var resultItemsFromStorageList = RunStorage(context, dataSourceSettings);
                    resultItemsList.AddRange(resultItemsFromStorageList);
                }
            }

            result.Items = resultItemsList;
#if DEBUG
            LogInstance.Log("End");
#endif
            return result;
        }

        private IList<LogicalSearchResultItem> RunStorage(LogicalSearchContext context, SettingsOfStorageForSearchingInThisSession dataSourceSettings)
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

            queryExpression.FillExecutingCard(queryExecutingCard, storage, contextForQueryExecutingCard);

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
