using MyNPCLib.ConvertingPersistLogicalDataToIndexing;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.Variants;
using MyNPCLib.VariantsConverting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.CGStorage.Helpers
{
    public static class PropertiesOfCGStorageHelper
    {
        public static RuleInstancePackage CreateRuleInstanceForSetQuery(ulong entityId, ulong propertyId, BaseVariant variant, IEntityDictionary entityDictionary)
        {
            return CreateRuleInstanceForSetQuery(entityId, propertyId, variant, entityDictionary, new List<KindOfAccessPolicyToFact>() { KindOfAccessPolicyToFact.Public });
        }

        public static RuleInstancePackage CreateRuleInstanceForSetQuery(ulong entityId, ulong propertyId, BaseVariant variant, IEntityDictionary entityDictionary, List<KindOfAccessPolicyToFact> kindOfAccessPolicyToFactList)
        {
            kindOfAccessPolicyToFactList = kindOfAccessPolicyToFactList.Distinct().ToList();

            var kind = variant.Kind;

            switch (kind)
            {
                case KindOfVariant.Concept:
                case KindOfVariant.Entity:
                case KindOfVariant.Value:
                case KindOfVariant.FuzzyLogicalValue:
                    return CreateUsualRuleInstanceForSetQuery(entityId, propertyId, variant, entityDictionary, kindOfAccessPolicyToFactList);

                case KindOfVariant.EntityCondition:
                    return CreateRuleInstanceWithEntityConditionForSetQuery(entityId, propertyId, variant.AsEntityCondition, entityDictionary, kindOfAccessPolicyToFactList);

                case KindOfVariant.Fact:
                    return CreateRuleInstanceWithFactForSetQuery(entityId, propertyId, variant.AsFact, entityDictionary, kindOfAccessPolicyToFactList);

                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static RuleInstancePackage CreateUsualRuleInstanceForSetQuery(ulong entityId, ulong propertyId, BaseVariant variant, IEntityDictionary entityDictionary, List<KindOfAccessPolicyToFact> kindOfAccessPolicyToFactList)
        {
            var keyOfClass = entityDictionary.GetKey("class");

            if(propertyId == keyOfClass)
            {
                return CreateUsualRuleInstanceForSetQueryForClass(entityId, propertyId, variant, entityDictionary, kindOfAccessPolicyToFactList);
            }

            var expressionOfVariant = VariantsConvertor.ConvertVariantToExpressionNode(variant);

#if DEBUG
            //LogInstance.Log($"expressionOfVariant = {expressionOfVariant}");
#endif

            var result = new RuleInstancePackage();
            var allRuleInstancesList = new List<RuleInstance>();
            result.AllRuleInstances = allRuleInstancesList;

            var relationKey = propertyId;
            var relationName = entityDictionary.GetName(relationKey);

            var entityName = entityDictionary.GetName(entityId);

            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = entityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;

            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = entityDictionary.GetKey(ruleInstance.Name);

            allRuleInstancesList.Add(ruleInstance);
            result.MainRuleInstance = ruleInstance;

            var accessPolicyToFactModalityList = new List<AccessPolicyToFactModality>();

            if(kindOfAccessPolicyToFactList.IsEmpty())
            {
                var accessPolicyToFactModality = new AccessPolicyToFactModality();
                accessPolicyToFactModality.Kind = KindOfAccessPolicyToFact.Public;
                accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
            }
            else
            {
                foreach(var kindOfAccessPolicyToFact in kindOfAccessPolicyToFactList)
                {
                    var accessPolicyToFactModality = new AccessPolicyToFactModality();
                    accessPolicyToFactModality.Kind = kindOfAccessPolicyToFact;
                    accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
                }
            }

            ruleInstance.AccessPolicyToFactModality = accessPolicyToFactModalityList;

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();
            expr3.Annotations = new List<LogicalAnnotation>();

            expr3.Name = relationName;
            expr3.Key = relationKey;

            if (entityDictionary.IsEntity(entityId))
            {
                var param_1 = new EntityRefExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }
            else
            {
                var param_1 = new ConceptExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }

            var param_2 = expressionOfVariant;
            expr3.Params.Add(param_2);
#if DEBUG
            //var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

            //LogInstance.Log($"debugStr (yyyyyyyyyyyyyyyyy) = {debugStr}");
#endif

            return result;
        }

        private static RuleInstancePackage CreateUsualRuleInstanceForSetQueryForClass(ulong entityId, ulong propertyId, BaseVariant variant, IEntityDictionary entityDictionary, List<KindOfAccessPolicyToFact> kindOfAccessPolicyToFactList)
        {
            var expressionOfVariant = VariantsConvertor.ConvertVariantToExpressionNode(variant);

#if DEBUG
            //LogInstance.Log($"expressionOfVariant = {expressionOfVariant}");
#endif

            var result = new RuleInstancePackage();
            var allRuleInstancesList = new List<RuleInstance>();
            result.AllRuleInstances = allRuleInstancesList;

            var relationKey = propertyId;
            var relationName = entityDictionary.GetName(relationKey);

            var entityName = entityDictionary.GetName(entityId);

            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = entityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;

            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = entityDictionary.GetKey(ruleInstance.Name);

            allRuleInstancesList.Add(ruleInstance);
            result.MainRuleInstance = ruleInstance;

            var accessPolicyToFactModalityList = new List<AccessPolicyToFactModality>();

            if (kindOfAccessPolicyToFactList.IsEmpty())
            {
                var accessPolicyToFactModality = new AccessPolicyToFactModality();
                accessPolicyToFactModality.Kind = KindOfAccessPolicyToFact.Public;
                accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
            }
            else
            {
                foreach (var kindOfAccessPolicyToFact in kindOfAccessPolicyToFactList)
                {
                    var accessPolicyToFactModality = new AccessPolicyToFactModality();
                    accessPolicyToFactModality.Kind = kindOfAccessPolicyToFact;
                    accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
                }
            }

            ruleInstance.AccessPolicyToFactModality = accessPolicyToFactModalityList;

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();
            expr3.Annotations = new List<LogicalAnnotation>();

            var baseRef = expressionOfVariant.AsBaseRef;

            expr3.Name = baseRef.Name;
            expr3.Key = baseRef.Key;

            if (entityDictionary.IsEntity(entityId))
            {
                var param_1 = new EntityRefExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }
            else
            {
                var param_1 = new ConceptExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }

#if DEBUG
            //var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

            //LogInstance.Log($"debugStr (yyyyyyyyyyyyyyyyy) = {debugStr}");
#endif

            return result;
        }

        private static RuleInstancePackage CreateRuleInstanceWithEntityConditionForSetQuery(ulong entityId, ulong propertyId, EntityConditionVariant variant, IEntityDictionary entityDictionary, List<KindOfAccessPolicyToFact> kindOfAccessPolicyToFactList)
        {
            var result = new RuleInstancePackage();
            var allRuleInstancesList = new List<RuleInstance>();
            result.AllRuleInstances = allRuleInstancesList;

            var addedRuleInstance = variant.RuleInstance;
            allRuleInstancesList.Add(addedRuleInstance);

            var relationKey = propertyId;
            var relationName = entityDictionary.GetName(relationKey);

            var entityName = entityDictionary.GetName(entityId);

            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = entityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;

            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = entityDictionary.GetKey(ruleInstance.Name);

            allRuleInstancesList.Add(ruleInstance);
            result.MainRuleInstance = ruleInstance;

            var accessPolicyToFactModalityList = new List<AccessPolicyToFactModality>();

            if (kindOfAccessPolicyToFactList.IsEmpty())
            {
                var accessPolicyToFactModality = new AccessPolicyToFactModality();
                accessPolicyToFactModality.Kind = KindOfAccessPolicyToFact.Public;
                accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
            }
            else
            {
                foreach (var kindOfAccessPolicyToFact in kindOfAccessPolicyToFactList)
                {
                    var accessPolicyToFactModality = new AccessPolicyToFactModality();
                    accessPolicyToFactModality.Kind = kindOfAccessPolicyToFact;
                    accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
                }
            }

            ruleInstance.AccessPolicyToFactModality = accessPolicyToFactModalityList;

            var entityConditionVarName = "#@X1";
            var entityConditionVarKey = entityDictionary.GetKey(entityConditionVarName);

            var entitiesConditions = new EntitiesConditions();
            ruleInstance.EntitiesConditions = entitiesConditions;
            entitiesConditions.Items = new List<EntityConditionItem>();

            var entityConditionName = addedRuleInstance.Name;
            var entityConditionKey = addedRuleInstance.Key;

            var entityCondition_1 = new EntityConditionItem();
            entitiesConditions.Items.Add(entityCondition_1);
            entityCondition_1.Name = entityConditionName;
            entityCondition_1.Key = entityConditionKey;
            entityCondition_1.VariableName = entityConditionVarName;
            entityCondition_1.VariableKey = entityConditionVarKey;

            var expressionOfVariant = new EntityConditionExpressionNode();
            expressionOfVariant.VariableKey = entityConditionVarKey;
            expressionOfVariant.VariableName = entityConditionVarName;
            expressionOfVariant.Name = entityConditionName;
            expressionOfVariant.Key = entityConditionVarKey;

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();
            expr3.Annotations = new List<LogicalAnnotation>();

            expr3.Name = relationName;
            expr3.Key = relationKey;

            if (entityDictionary.IsEntity(entityId))
            {
                var param_1 = new EntityRefExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }
            else
            {
                var param_1 = new ConceptExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }

            var param_2 = expressionOfVariant;
            expr3.Params.Add(param_2);
#if DEBUG
            //var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

            //LogInstance.Log($"debugStr (yyyyyyyyyyyyyyyyy) = {debugStr}");
#endif

            return result;
        }

        private static RuleInstancePackage CreateRuleInstanceWithFactForSetQuery(ulong entityId, ulong propertyId, FactVariant variant, IEntityDictionary entityDictionary, List<KindOfAccessPolicyToFact> kindOfAccessPolicyToFactList)
        {
            var expressionOfVariant = VariantsConvertor.ConvertVariantToExpressionNode(variant);

#if DEBUG
            //LogInstance.Log($"expressionOfVariant = {expressionOfVariant}");
#endif

            var result = new RuleInstancePackage();
            var allRuleInstancesList = new List<RuleInstance>();
            result.AllRuleInstances = allRuleInstancesList;

            var addedRuleInstance = variant.RuleInstance;
            allRuleInstancesList.Add(addedRuleInstance);

            var relationKey = propertyId;
            var relationName = entityDictionary.GetName(relationKey);

            var entityName = entityDictionary.GetName(entityId);

            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = entityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;

            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = entityDictionary.GetKey(ruleInstance.Name);

            allRuleInstancesList.Add(ruleInstance);
            result.MainRuleInstance = ruleInstance;

            var accessPolicyToFactModalityList = new List<AccessPolicyToFactModality>();

            if (kindOfAccessPolicyToFactList.IsEmpty())
            {
                var accessPolicyToFactModality = new AccessPolicyToFactModality();
                accessPolicyToFactModality.Kind = KindOfAccessPolicyToFact.Public;
                accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
            }
            else
            {
                foreach (var kindOfAccessPolicyToFact in kindOfAccessPolicyToFactList)
                {
                    var accessPolicyToFactModality = new AccessPolicyToFactModality();
                    accessPolicyToFactModality.Kind = kindOfAccessPolicyToFact;
                    accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
                }
            }

            ruleInstance.AccessPolicyToFactModality = accessPolicyToFactModalityList;

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();
            expr3.Annotations = new List<LogicalAnnotation>();

            expr3.Name = relationName;
            expr3.Key = relationKey;

            if (entityDictionary.IsEntity(entityId))
            {
                var param_1 = new EntityRefExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }
            else
            {
                var param_1 = new ConceptExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }

            var param_2 = expressionOfVariant;
            expr3.Params.Add(param_2);
#if DEBUG
            //var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

            //LogInstance.Log($"debugStr (yyyyyyyyyyyyyyyyy) = {debugStr}");
#endif

            return result;
        }

        public static IndexedRuleInstance CreateGetQuery(ulong entityId, ulong propertyId, IEntityDictionary entityDictionary, string nameOfVarForProperty, ulong keyOfVarForProperty)
        {
            return CreateGetQuery(entityId, propertyId, entityDictionary, nameOfVarForProperty, keyOfVarForProperty, new List<KindOfAccessPolicyToFact>() { KindOfAccessPolicyToFact.Public });
        }

        public static IndexedRuleInstance CreateGetQuery(ulong entityId, ulong propertyId, IEntityDictionary entityDictionary, string nameOfVarForProperty, ulong keyOfVarForProperty, List<KindOfAccessPolicyToFact> kindOfAccessPolicyToFactList)
        {
            kindOfAccessPolicyToFactList = kindOfAccessPolicyToFactList.Distinct().ToList();

            var keyOfClass = entityDictionary.GetKey("class");

            if(keyOfClass == propertyId)
            {
                return CreateGetQueryForClass(entityId, propertyId, entityDictionary, nameOfVarForProperty, keyOfVarForProperty, kindOfAccessPolicyToFactList);
            }

            var relationKey = propertyId;
            var relationName = entityDictionary.GetName(relationKey);

            var entityName = entityDictionary.GetName(entityId);

            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = entityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.QuestionVars;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = entityDictionary.GetKey(ruleInstance.Name);

            var accessPolicyToFactModalityList = new List<AccessPolicyToFactModality>();

            if (kindOfAccessPolicyToFactList.IsEmpty())
            {
                var accessPolicyToFactModality = new AccessPolicyToFactModality();
                accessPolicyToFactModality.Kind = KindOfAccessPolicyToFact.Public;
                accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
            }
            else
            {
                foreach (var kindOfAccessPolicyToFact in kindOfAccessPolicyToFactList)
                {
                    var accessPolicyToFactModality = new AccessPolicyToFactModality();
                    accessPolicyToFactModality.Kind = kindOfAccessPolicyToFact;
                    accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
                }
            }

            ruleInstance.AccessPolicyToFactModality = accessPolicyToFactModalityList;

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();
            expr3.Annotations = new List<LogicalAnnotation>();

            expr3.Name = relationName;
            expr3.Key = relationKey;

            if (entityDictionary.IsEntity(entityId))
            {
                var param_1 = new EntityRefExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }
            else
            {
                var param_1 = new ConceptExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }

            var param_2 = new QuestionVarExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = nameOfVarForProperty;
            param_2.Key = keyOfVarForProperty;

#if DEBUG
            //var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

            //LogInstance.Log($"debugStr (yyyyyyyyyyyyyyyyy) = {debugStr}");
#endif
            var indexedRuleInstance = ConvertorToIndexed.ConvertRuleInstance(ruleInstance);

            return indexedRuleInstance;
        }

        public static IndexedRuleInstance CreateGetQueryForClass(ulong entityId, ulong propertyId, IEntityDictionary entityDictionary, string nameOfVarForProperty, ulong keyOfVarForProperty, List<KindOfAccessPolicyToFact> kindOfAccessPolicyToFactList)
        {
            var relationKey = propertyId;
            var relationName = entityDictionary.GetName(relationKey);

            var entityName = entityDictionary.GetName(entityId);

            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = entityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.QuestionVars;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = entityDictionary.GetKey(ruleInstance.Name);

            var accessPolicyToFactModalityList = new List<AccessPolicyToFactModality>();

            if (kindOfAccessPolicyToFactList.IsEmpty())
            {
                var accessPolicyToFactModality = new AccessPolicyToFactModality();
                accessPolicyToFactModality.Kind = KindOfAccessPolicyToFact.Public;
                accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
            }
            else
            {
                foreach (var kindOfAccessPolicyToFact in kindOfAccessPolicyToFactList)
                {
                    var accessPolicyToFactModality = new AccessPolicyToFactModality();
                    accessPolicyToFactModality.Kind = kindOfAccessPolicyToFact;
                    accessPolicyToFactModalityList.Add(accessPolicyToFactModality);
                }
            }

            ruleInstance.AccessPolicyToFactModality = accessPolicyToFactModalityList;

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();
            expr3.Annotations = new List<LogicalAnnotation>();

            expr3.Name = nameOfVarForProperty;
            expr3.Key = keyOfVarForProperty;

            if (entityDictionary.IsEntity(entityId))
            {
                var param_1 = new EntityRefExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }
            else
            {
                var param_1 = new ConceptExpressionNode();
                expr3.Params.Add(param_1);
                param_1.Name = entityName;
                param_1.Key = entityId;
            }

#if DEBUG
            //var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

            //LogInstance.Log($"debugStr (yyyyyyyyyyyyyyyyy) = {debugStr}");
#endif
            var indexedRuleInstance = ConvertorToIndexed.ConvertRuleInstance(ruleInstance);

            return indexedRuleInstance;
        }
    }
}
