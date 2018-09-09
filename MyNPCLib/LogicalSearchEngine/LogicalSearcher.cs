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
        public LogicalSearcher(IEntityDictionary entityDictionary)
        {
            mEntityDictionary = entityDictionary;
        }

        private IEntityDictionary mEntityDictionary;

        public LogicalSearchResult Run(LogicalSearchOptions options)
        {
#if DEBUG
            //LogInstance.Log($"options = {options}");
#endif
            var queryDataSource = options.QuerySource;
            IndexedRuleInstance queryExpression = null;

            var optionsOfFillExecutingCard = new OptionsOfFillExecutingCard();
            optionsOfFillExecutingCard.EntityIdOnly = options.EntityIdOnly;
            optionsOfFillExecutingCard.UseAccessPolicy = !options.IgnoreAccessPolicy;

            if (queryDataSource == null)
            {
                queryExpression = options.QueryExpression;
                optionsOfFillExecutingCard.AccessPolicyToFactModalityList = queryExpression.AccessPolicyToFactModality.ToList();
            }
            else
            {
                queryExpression = queryDataSource.MainIndexedRuleInstance;
                optionsOfFillExecutingCard.AccessPolicyToFactModalityList = queryExpression.AccessPolicyToFactModality.ToList();
            }

#if DEBUG
            //LogInstance.Log($"NEXT");
#endif

            var externalDataSource = options.DataSource;

            var result = new LogicalSearchResult(externalDataSource);
            result.QueryExpression = queryExpression;

            ICGStorage dataSource = null;

            if (queryDataSource == null)
            {
                dataSource = externalDataSource;
            }
            else
            {
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

                dataSource = new ConsolidatedCGStorage(externalDataSource.EntityDictionary, dataSourcesSettings);
            }

            var resultItemsList = new List<LogicalSearchResultItem>();

            var queryExecutingCard = new QueryExecutingCardForIndexedPersistLogicalData();
            queryExpression.FillExecutingCard(queryExecutingCard, dataSource, optionsOfFillExecutingCard);

#if DEBUG
            //LogInstance.Log($"@!@!@!@!@!@!@! queryExecutingCard = {queryExecutingCard}");
#endif

            foreach (var resultOfQueryToRelation in queryExecutingCard.ResultsOfQueryToRelationList)
            {
                var resultItem = new LogicalSearchResultItem(mEntityDictionary, externalDataSource, result);
                resultItem.QueryExpression = queryExpression;
                var resultOfVarOfQueryToRelationList = resultOfQueryToRelation.ResultOfVarOfQueryToRelationList;
                foreach(var resultOfVarOfQueryToRelation in resultOfVarOfQueryToRelationList)
                {
                    resultOfVarOfQueryToRelation.Parent = resultItem;
                    resultOfVarOfQueryToRelation.Storage = externalDataSource;
                }
                resultItem.ResultOfVarOfQueryToRelationList = resultOfVarOfQueryToRelationList;
                resultItemsList.Add(resultItem);
                resultItem.Ready();
            }

            result.Items = resultItemsList;

#if DEBUG
            //LogInstance.Log("End");
#endif

            return result;
        }

        public IList<ulong> GetEntitiesIdList(LogicalSearchOptions options)
        {
            options.EntityIdOnly = true;

            var searchResult = Run(options);
            
#if DEBUG
            //LogInstance.Log($"searchResult = {searchResult}");
#endif

            var result = searchResult.Items.Select(p => p.ResultOfVarOfQueryToRelationList.Single()).Select(p => p.FoundExpression.AsEntityRef.Key).Distinct().ToList();

#if DEBUG
            //LogInstance.Log($"result.Count = {result.Count}");
#endif

            return result;
        }
    }
}
