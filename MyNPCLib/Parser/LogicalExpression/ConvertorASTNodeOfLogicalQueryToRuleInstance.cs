using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser.LogicalExpression
{
    public static class ConvertorASTNodeOfLogicalQueryToRuleInstance
    {
        public static List<RuleInstance> Convert(ASTNodeOfLogicalQuery node, IEntityDictionary entityDictionary)
        {
            var context = new ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance();
            context.EntityDictionary = entityDictionary;

            NConvertFact(node, context);

            return context.ResultsList;
        }

        private static void NConvertFact(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            var kindOfNode = node.Kind;

            switch(kindOfNode)
            {
                case KindOfASTNodeOfLogicalQuery.Fact:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfNode), kindOfNode, null);
            }

            var entityDictionary = context.EntityDictionary;

            var result = new RuleInstance();
            var nameOfFact = NamesHelper.CreateEntityName();
            result.Name = nameOfFact;
            result.Key = entityDictionary.GetKey(nameOfFact);


            
            //throw new NotImplementedException();

            if(!node.AnnotationsList.IsEmpty())
            {
                var completeAnnotationsList = new List<LogicalAnnotation>();

                foreach (var initAnnotation in node.AnnotationsList)
                {
                    var annotationsList = Convert(initAnnotation, entityDictionary);
                    completeAnnotationsList.AddRange(annotationsList);
                }

                result.Annotations = completeAnnotationsList;
            }

            context.ResultsList.Add(result);
        }

        private static void NConvertRulePart(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
#if DEBUG
            LogInstance.Log($"node = {node}");
#endif

            throw new NotImplementedException();
        }

        private static List<LogicalAnnotation> NConvertAnnotation(ASTNodeOfLogicalQuery node, ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance context)
        {
            var resultsList = new List<LogicalAnnotation>();

            var newContext = new ContextOfConvertorASTNodeOfLogicalQueryToRuleInstance();
            newContext.EntityDictionary = context.EntityDictionary;

            NConvertFact(node, newContext);

            var initFactsList = newContext.ResultsList;

            foreach(var initFact in initFactsList)
            {

            }
            
            return resultsList;
        }
    }
}
