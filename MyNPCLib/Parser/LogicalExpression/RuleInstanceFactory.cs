using MyNPCLib.CGStorage;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.VariantsConverting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public static class RuleInstanceFactory
    {
        public static ASTNodeOfLogicalQuery ConvertStringToASTNodeOfLogicalQuery(string queryStr, IEntityDictionary entityDictionary, params QueryParam[] paramsCollection)
        {
#if DEBUG
            //LogInstance.Log($"paramsCollection.Length = {paramsCollection.Length}");
#endif

            var contextOfParser = new ContextOfLogicalExpressionParser(queryStr, entityDictionary);

            if(paramsCollection.Length > 0)
            {
                foreach(var queryParam in paramsCollection)
                {
#if DEBUG
                    //LogInstance.Log($"queryParam = {queryParam}");
#endif

                    var variant = VariantsConvertor.ConvertObjectToVariant(queryParam.Value, entityDictionary);

#if DEBUG
                    //LogInstance.Log($"variant = {variant}");
#endif

                    contextOfParser.QueryParamsDict[queryParam.Name] = variant;
                }
            }

            var parser = new FactParser(contextOfParser);
            parser.Run();

            var result = parser.Result;

            return result;
        }

        public static RuleInstancePackage ConvertStringToRuleInstancePackage(string queryStr, IEntityDictionary entityDictionary, params QueryParam[] paramsCollection)
        {
            var result = ConvertStringToASTNodeOfLogicalQuery(queryStr, entityDictionary, paramsCollection);

            var ruleInstancesPackage = ConvertorASTNodeOfLogicalQueryToRuleInstance.Convert(result, entityDictionary);

            return ruleInstancesPackage;
        }

        public static ICGStorage ConvertStringToQueryCGStorage(string queryStr, IEntityDictionary entityDictionary, params QueryParam[] paramsCollection)
        {
            var ruleInstancesPackage = ConvertStringToRuleInstancePackage(queryStr, entityDictionary, paramsCollection);

            var storage = new QueryCGStorage(entityDictionary, ruleInstancesPackage);
            return storage;
        }

        public static PassiveListGCStorage ConvertStringToPassiveListGCStorage(string queryStr, IEntityDictionary entityDictionary, params QueryParam[] paramsCollection)
        {
            var ruleInstancesPackage = ConvertStringToRuleInstancePackage(queryStr, entityDictionary, paramsCollection);

            var ruleInstancesList = ruleInstancesPackage.AllRuleInstances;

            var result = new PassiveListGCStorage(entityDictionary, ruleInstancesList);

            return result;
        }
    }
}
