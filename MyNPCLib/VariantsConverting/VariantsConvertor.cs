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
        public static BaseVariant ConvertObjectToVariant(object source, IEntityDictionary entityDictionary)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif

            var type = source.GetType();

            if(type == typeof(string))
            {
                return ConvertStringToVariant((string)source, entityDictionary);
            }

            if(type == typeof(bool))
            {
                return ConvertLogicalValueToVariant((bool)source);
            }

            if(type.IsSubclassOf(typeof(BaseVariant)))
            {
                return (BaseVariant)source;
            }

            if (type.IsSubclassOf(typeof(RuleInstance)))
            {
                return ConvertRuleInstanceToVariant((RuleInstance)source);
            }

            return ConvertObjectValueToVariant(source);
        }

        private static BaseVariant ConvertStringToVariant(string source, IEntityDictionary entityDictionary)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif
            var key = entityDictionary.GetKey(source);
            var kindOfKey = entityDictionary.GetKindOfKey(key);

            switch(kindOfKey)
            {
                case KindOfKey.Concept:
                    {
                        var result = new ConceptVariant(key, source);
                        return result;
                    }

                case KindOfKey.Entity:
                    {
                        var result = new EntityVariant(key, source);
                        return result;
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfKey), kindOfKey, null);
            }

            throw new NotImplementedException();
        }

        private static BaseVariant ConvertRuleInstanceToVariant(RuleInstance ruleInstance)
        {
#if DEBUG
            //LogInstance.Log($"ruleInstance = {ruleInstance}");
#endif

            if (ruleInstance.Kind == KindOfRuleInstance.EntityCondition)
            {
                var result = new EntityConditionVariant(ruleInstance);
                return result;
            }

            {
                var result = new FactVariant(ruleInstance);
                return result;
            }
        }

        private static BaseVariant ConvertObjectValueToVariant(object source)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif

            var result = new ValueVariant(source);
            return result;
        }

        private static BaseVariant ConvertLogicalValueToVariant(bool source)
        {
            float fuzzySource = 0;

            if(source)
            {
                fuzzySource = 1;
            }

            var result = new FuzzyLogicalValueVariant(fuzzySource);
            return result;
        }

        public static BaseVariant ConvertResultOfVarToVariant(ResultOfVarOfQueryToRelation source)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
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

                case KindOfExpressionNode.FuzzyLogicValue:
                    return ConvertResultOfVarToVariantAsLogicalValue(foundExpression.AsFuzzyLogicValue);

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
            //LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            var result = new ConceptVariant(expressionNode.Key, expressionNode.Name, expressionNode.Annotations);
            return result;
        }

        private static BaseVariant ConvertResultOfVarToVariantAsEntity(EntityRefExpressionNode expressionNode)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            var result = new EntityVariant(expressionNode.Key, expressionNode.Name, expressionNode.Annotations);
            return result;
        }

        private static BaseVariant ConvertResultOfVarToVariantAsValue(ValueExpressionNode expressionNode)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            var result = new ValueVariant(expressionNode.Value, expressionNode.Annotations);
            return result;
        }

        private static BaseVariant ConvertResultOfVarToVariantAsLogicalValue(FuzzyLogicValueExpressionNode expressionNode)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            var result = new FuzzyLogicalValueVariant(expressionNode.Value, expressionNode.Annotations);
            return result;
        }

        private static BaseVariant ConvertResultOfVarToVariantAsFact(FactExpressionNode expressionNode, ResultOfVarOfQueryToRelation source)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            var factRuleInstance = source.Parent.Storage.GetRuleInstanceByKey(expressionNode.Key);

            var result = new FactVariant(factRuleInstance);
            return result;
        }

        private static BaseVariant ConvertResultOfVarToVariantAsEntityCondition(EntityConditionExpressionNode expressionNode, ResultOfVarOfQueryToRelation source)
        {
#if DEBUG
            //LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            //var entityConditionRec = source.Parent.RuleInstance.EntitiesConditions.Items.FirstOrDefault(p => p.VariableKey == expressionNode.VariableKey);

            //if (entityConditionRec == null)
            //{
            //    return null;
            //}

#if DEBUG
            //LogInstance.Log($"entityConditionRec = {entityConditionRec}");
#endif
            //var keyOfEntityConditionFact = entityConditionRec.Key;
            var keyOfEntityConditionFact = expressionNode.Key;

#if DEBUG
            //LogInstance.Log($"keyOfEntityConditionFact = {keyOfEntityConditionFact}");
#endif
            var entityConditionRuleInstance = source.Parent.Storage.GetRuleInstanceByKey(keyOfEntityConditionFact);

#if DEBUG
            //LogInstance.Log($"entityConditionRuleInstance = {entityConditionRuleInstance}");
#endif

            var result = new EntityConditionVariant(entityConditionRuleInstance);
            return result;
        }

        public static BaseExpressionNode ConvertVariantToExpressionNode(BaseVariant source)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif
            var kind = source.Kind;

            switch(kind)
            {
                case KindOfVariant.Concept:
                    return ConvertConceptVariant(source.AsConcept);

                case KindOfVariant.Entity:
                    return ConvertEntityVariant(source.AsEntity);

                case KindOfVariant.Value:
                    return ConvertValueVariant(source.AsValue);

                case KindOfVariant.FuzzyLogicalValue:
                    return ConvertFuzzyLogicalValueVariant(source.AsFuzzyLogicalValue);

                case KindOfVariant.Fact:
                    return ConvertFactVariant(source.AsFact);

                case KindOfVariant.EntityCondition:
                    return ConvertEntityConditionVariant(source.AsEntityCondition);

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static BaseExpressionNode ConvertConceptVariant(ConceptVariant source)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif

            var result = new ConceptExpressionNode();
            result.Key = source.Key;
            result.Name = source.Name;
            result.Annotations = source.Annotations;
            return result;
        }

        private static BaseExpressionNode ConvertEntityVariant(EntityVariant source)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif

            var result = new EntityRefExpressionNode();
            result.Key = source.Key;
            result.Name = source.Name;
            result.Annotations = source.Annotations;
            return result;
        }

        private static BaseExpressionNode ConvertValueVariant(ValueVariant source)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif

            var result = new ValueExpressionNode();
            result.Value = source.Value;
            result.Annotations = source.Annotations;
            return result;
        }

        private static BaseExpressionNode ConvertFuzzyLogicalValueVariant(FuzzyLogicalValueVariant source)
        {
            var result = new FuzzyLogicValueExpressionNode();
            result.Value = source.Value;
            result.Annotations = source.Annotations;
            return result;
        }

        private static BaseExpressionNode ConvertFactVariant(FactVariant source)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif

            var ruleInstance = source.RuleInstance;

            var result = new FactExpressionNode();
            result.Key = ruleInstance.Key;
            result.Name = ruleInstance.Name;
            return result;
        }

        private static BaseExpressionNode ConvertEntityConditionVariant(EntityConditionVariant source)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif

            var ruleInstance = source.RuleInstance;

            var result = new EntityConditionExpressionNode();
            result.Key = ruleInstance.Key;
            result.Name = ruleInstance.Name;
            return result;
        }
    }
}
