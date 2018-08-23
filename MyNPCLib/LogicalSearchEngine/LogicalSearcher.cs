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
            //LogInstance.Log($"options = {options}");
#endif
            var queryDataSource = options.QuerySource;
            var queryExpression = queryDataSource.MainIndexedRuleInstance;

            var result = new LogicalSearchResult();
            result.QueryExpression = queryExpression;

            var entityDictionary = mContextOfCGStorage.EntityDictionary;

            //var context = new LogicalSearchContext();
            //context.QueryExpression = queryExpression;
            //context.EntityDictionary = entityDictionary;

            var externalDataSource = options.DataSource;

            var searchOptions = new LogicalSearchOptions();
            var globalStorageOptions = new SettingsOfStorageForSearchingInThisSession();
            globalStorageOptions.Storage = externalDataSource;
            globalStorageOptions.MaxDeph = null;
            globalStorageOptions.UseFacts = true;
            globalStorageOptions.UseProductions = true;
            globalStorageOptions.UseAdditionalInstances = true;
            globalStorageOptions.Priority = 1;

            var globalStorageOptions_1 = new SettingsOfStorageForSearchingInThisSession();
            globalStorageOptions_1.Storage = queryDataSource;
            globalStorageOptions_1.MaxDeph = null;
            globalStorageOptions_1.UseFacts = false;
            globalStorageOptions_1.UseAdditionalInstances = true;
            globalStorageOptions_1.UseProductions = false;
            globalStorageOptions_1.Priority = 1;

            var dataSourcesSettings = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions, globalStorageOptions_1 };

            var dataSource = new ConsolidatedCGStorage(externalDataSource.Context, dataSourcesSettings);

            //context.DataSource = dataSource;

#if DEBUG
            //LogInstance.Log($"context = {context}");
#endif

            var resultItemsList = new List<LogicalSearchResultItem>();

            var queryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExpression.FillExecutingCard(queryExecutingCard, dataSource);

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
