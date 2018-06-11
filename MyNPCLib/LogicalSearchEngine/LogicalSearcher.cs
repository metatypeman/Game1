using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
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
