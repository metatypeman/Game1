using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.Variants;
using System;
using System.Collections.Generic;
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
                    return ConvertResultOfVarToVariantAsFact(foundExpression.AsFact);

                case KindOfExpressionNode.EntityCondition:
                    return ConvertResultOfVarToVariantAsEntityCondition(foundExpression.AsEntityCondition);

                default:
                    return null;
            }
        }

        private static BaseVariant ConvertResultOfVarToVariantAsConcept(ConceptExpressionNode expressionNode)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            throw new NotImplementedException();
        }

        private static BaseVariant ConvertResultOfVarToVariantAsEntity(EntityRefExpressionNode expressionNode)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            throw new NotImplementedException();
        }

        private static BaseVariant ConvertResultOfVarToVariantAsValue(ValueExpressionNode expressionNode)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            throw new NotImplementedException();
        }

        private static BaseVariant ConvertResultOfVarToVariantAsFact(FactExpressionNode expressionNode)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            throw new NotImplementedException();
        }

        private static BaseVariant ConvertResultOfVarToVariantAsEntityCondition(EntityConditionExpressionNode expressionNode)
        {
#if DEBUG
            LogInstance.Log($"expressionNode = {expressionNode}");
#endif

            throw new NotImplementedException();
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
