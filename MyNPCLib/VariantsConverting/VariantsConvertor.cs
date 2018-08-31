using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.Variants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.VariantsConverting
{
    public static class VariantsConvertor
    {
        public static BaseVariant ConvertObjectToVariant(object source)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            throw new NotImplementedException();
        }

        public static BaseVariant ConvertResultOfVarToVariant(ResultOfVarOfQueryToRelation source)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            var foundExpression = source.FoundExpression;

            if(foundExpression == null)
            {
                return null;
            }

            var kindOfFoundExpression = foundExpression.Kind;

            switch(kindOfFoundExpression)
            {
                case KindOfExpressionNode.Concept:
                    return ConvertResultOfVarToVariantAsConcept(foundExpression.AsConcept);

                case KindOfExpressionNode.EntityRef:
                    return ConvertResultOfVarToVariantAsEntity(foundExpression.AsEntityRef);

                case KindOfExpressionNode.Value:
                    return ConvertResultOfVarToVariantAsValue(foundExpression.AsValue);

                case KindOfExpressionNode.Fact:
                    return ConvertResultOfVarToVariantAsFact(foundExpression.AsFact, source);

                case KindOfExpressionNode.EntityCondition:
                    return ConvertResultOfVarToVariantAsEntityCondition(foundExpression.AsEntityCondition, source);

                default:
                    return null;
            }
        }

        private static BaseVariant ConvertResultOfVarToVariantAsConcept(ConceptExpressionNode expressionNode)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            var result = new ConceptVariant(expressionNode);
            return result;
        }

        private static BaseVariant ConvertResultOfVarToVariantAsEntity(EntityRefExpressionNode expressionNode)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            var result = new EntityVariant(expressionNode);
            return result;
        }

        private static BaseVariant ConvertResultOfVarToVariantAsValue(ValueExpressionNode expressionNode)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            var result = new ValueVariant(expressionNode);
            return result;
        }

        private static BaseVariant ConvertResultOfVarToVariantAsFact(FactExpressionNode expressionNode, ResultOfVarOfQueryToRelation source)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            var factRuleInstance = source.Parent.Storage.GetRuleInstanceByKey(expressionNode.Key);

            var result = new FactVariant(factRuleInstance);
            return result;
        }

        private static BaseVariant ConvertResultOfVarToVariantAsEntityCondition(EntityConditionExpressionNode expressionNode, ResultOfVarOfQueryToRelation source)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            var entityConditionRec = source.Parent.RuleInstance.EntitiesConditions.Items.FirstOrDefault(p => p.VariableKey == expressionNode.Key);

            if (entityConditionRec == null)
            {
                return null;
            }

#if DEBUG
            LogInstance.Log($"entityConditionRec = {entityConditionRec}");
#endif
            var keyOfEntityConditionFact = entityConditionRec.Key;

#if DEBUG
            LogInstance.Log($"keyOfEntityConditionFact = {keyOfEntityConditionFact}");
#endif
            var entityConditionRuleInstance = source.Parent.Storage.GetRuleInstanceByKey(keyOfEntityConditionFact);

#if DEBUG
            LogInstance.Log($"entityConditionRuleInstance = {entityConditionRuleInstance}");
#endif

            var result = new EntityConditionVariant(entityConditionRuleInstance);
            return result;
        }

        public static BaseExpressionNode ConvertVariantToExpressionNode(BaseVariant source)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            throw new NotImplementedException();
        }
    }
}
