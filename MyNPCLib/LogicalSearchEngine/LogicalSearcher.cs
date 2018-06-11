﻿using MyNPCLib.CGStorage;
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

            var context = new LogicalSearchContext();
            context.QueryExpression = options.QueryExpression;
            context.DataSourcesSettings = options.DataSourcesSettings.GroupBy(p => p.Priority).ToDictionary(p => p.Key, p => (IList<SettingsOfStorageForSearchingInThisSession>)p.ToList());

#if DEBUG
            LogInstance.Log($"context = {context}");
#endif
            
#if DEBUG
            LogInstance.Log("End");
#endif
            return result;
        }
    }
}
