using MyNPCLib.CGStorage;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public static class RuleInstanceFactory
    {
        public static ASTNodeOfLogicalQuery ConvertStringToASTNodeOfLogicalQuery(string queryStr, IEntityDictionary entityDictionary)
        {
            var contextOfParser = new ContextOfLogicalExpressionParser(queryStr, entityDictionary);

            var parser = new FactParser(contextOfParser);
            parser.Run();

            var result = parser.Result;

            return result;
        }

        public static RuleInstancePackage ConvertStringToRuleInstancePackage(string queryStr, IEntityDictionary entityDictionary)
        {
            var result = ConvertStringToASTNodeOfLogicalQuery(queryStr, entityDictionary);

            var ruleInstancesPackage = ConvertorASTNodeOfLogicalQueryToRuleInstance.Convert(result, entityDictionary);

            return ruleInstancesPackage;
        }

        public static PassiveListGCStorage ConvertStringToPassiveListGCStorage(string queryStr, ContextOfCGStorage context)
        {
            var entityDictionary = context.EntityDictionary;

            var ruleInstancesPackage = RuleInstanceFactory.ConvertStringToRuleInstancePackage(queryStr, entityDictionary);

            var ruleInstancesList = ruleInstancesPackage.AllRuleInstances;

            var result = new PassiveListGCStorage(context, ruleInstancesList);

            return result;
        }
    }
}
