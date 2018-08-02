using MyNPCLib.CommonServiceGrammaticalElements;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.InternalCG;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.ConvertingInternalCGToPersistLogicalData
{
    public static class ConvertorInternalCGToPersistLogicalData
    {
        public static IList<RuleInstance> ConvertConceptualGraph(InternalConceptualGraph source, IEntityDictionary entityDictionary)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            var context = new ContextOfConvertingInternalCGToPersistLogicalData();
            context.EntityDictionary = entityDictionary;
            return ConvertConceptualGraph(source, context);
        }

        private static IList<RuleInstance> ConvertConceptualGraph(InternalConceptualGraph source, ContextOfConvertingInternalCGToPersistLogicalData context)
        {
            PreliminaryСreation(source, context);

#if DEBUG
            LogInstance.Log($"context.RuleInstancesDict.Count = {context.RuleInstancesDict.Count}");
#endif

            foreach(var ruleInstancesDictKVPItem in context.RuleInstancesDict)
            {
                FillRuleInstances(ruleInstancesDictKVPItem.Key, ruleInstancesDictKVPItem.Value, context);
            }

            return context.RuleInstancesDict.Values.ToList();
        }

        private static void PreliminaryСreation(InternalConceptualGraph source, ContextOfConvertingInternalCGToPersistLogicalData context)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            if (context.RuleInstancesDict.ContainsKey(source))
            {
                return;
            }

            var ruleInstance = new RuleInstance();
            context.RuleInstancesDict[source] = ruleInstance;
            var name = source.Name;
            if(string.IsNullOrWhiteSpace(name))
            {
                name = NamesHelper.CreateEntityName();
            }
            ruleInstance.Name = name;
            ruleInstance.Key = context.EntityDictionary.GetKey(name);
            ruleInstance.DictionaryName = context.EntityDictionary.Name;

            if(source.KindOfGraphOrConcept == KindOfInternalGraphOrConceptNode.EntityCondition)
            {
                ruleInstance.Kind = KindOfRuleInstance.EntityCondition;
            }
            else
            {
                ruleInstance.Kind = KindOfRuleInstance.Fact;
            }

            var graphsChildrenList = source.Children.Where(p => p.IsConceptualGraph).Select(p => p.AsConceptualGraph).ToList();

            foreach(var graphsChild in graphsChildrenList)
            {
                PreliminaryСreation(graphsChild, context);
            }
        }

        private static void FillRuleInstances(InternalConceptualGraph source, RuleInstance dest, ContextOfConvertingInternalCGToPersistLogicalData context)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
            LogInstance.Log($"dest = {dest}");
#endif

            var contextForSingleRuleInstance = new ContextForSingleRuleInstanceOfConvertingInternalCGToPersistLogicalData();
            contextForSingleRuleInstance.CurrentRuleInstance = dest;

            var part = new RulePart();
            dest.Part_1 = part;
            part.Parent = dest;
            part.IsActive = true;

            var kindOfGraphOrConcept = source.KindOfGraphOrConcept;

#if DEBUG
            LogInstance.Log($"kindOfGraphOrConcept = {kindOfGraphOrConcept}");
#endif

            switch(kindOfGraphOrConcept)
            {
                case KindOfInternalGraphOrConceptNode.Graph:
                    PrepareForGraphConditionExpression(source, dest, context);
                    break;

                case KindOfInternalGraphOrConceptNode.EntityCondition:
                    PrepareForEntityConditionExpression(source, dest, context);
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(kindOfGraphOrConcept), kindOfGraphOrConcept, null);
            }

            var expression = CreateExpressionByWholeGraph(source, context, contextForSingleRuleInstance);

#if DEBUG
            LogInstance.Log($"expression = {expression}");
            var debugStr = DebugHelperForRuleInstance.ToString(expression);
            LogInstance.Log($"debugStr = {debugStr}");
#endif

            //throw new NotImplementedException();

            part.Expression = expression;

            var etityConditionsDict = contextForSingleRuleInstance.EntityConditionsDict;

#if DEBUG
            LogInstance.Log($"etityConditionsDict.Count = {etityConditionsDict.Count}");
#endif

            if(etityConditionsDict.Count > 0)
            {
                var entitiesConditions = new EntitiesConditions();
                dest.EntitiesConditions = entitiesConditions;
                entitiesConditions.Items = new List<EntityConditionItem>();

                foreach(var etityConditionsKVPItem in etityConditionsDict)
                {
#if DEBUG
                    LogInstance.Log($"etityConditionsKVPItem.Key = {etityConditionsKVPItem.Key}");
                    LogInstance.Log($"etityConditionsKVPItem.Value = {etityConditionsKVPItem.Value}");
#endif

                    var entityConditionItem = new EntityConditionItem();
                    entitiesConditions.Items.Add(entityConditionItem);
                    entityConditionItem.Name = etityConditionsKVPItem.Key;
                    entityConditionItem.Key = context.EntityDictionary.GetKey(entityConditionItem.Name);
                    entityConditionItem.VariableName = etityConditionsKVPItem.Value;
                    entityConditionItem.VariableKey = context.EntityDictionary.GetKey(entityConditionItem.VariableName);
                }

                //throw new NotImplementedException();
            }   
        }

        private static void PrepareForGraphConditionExpression(InternalConceptualGraph source, RuleInstance dest, ContextOfConvertingInternalCGToPersistLogicalData context)
        {
            var processedSpecialConceptsList = new List<BaseInternalConceptCGNode>();
            var processedRelationsList = new List<InternalRelationCGNode>();

            var initRelationsList = source.Children.Where(p => p.IsRelationNode).Select(p => p.AsRelationNode).ToList();

#if DEBUG
            //LogInstance.Log($"initRelationsList.Count = {initRelationsList.Count}");
#endif

            foreach (var initRelation in initRelationsList)
            {
#if DEBUG
                //LogInstance.Log($"initRelation = {initRelation}");
#endif

                var kindOfSpecialRelation = SpecialElementsHeper.GetKindOfSpecialRelation(initRelation.Name);

#if DEBUG
                //LogInstance.Log($"kindOfSpecialRelation = {kindOfSpecialRelation}");
#endif

                BaseInternalConceptCGNode firstConcept = null;
                BaseInternalConceptCGNode targetConceptToRelation = null;
                BaseInternalConceptCGNode secondConcept = null;

                switch (kindOfSpecialRelation)
                {
                    case KindOfSpecialRelation.Undefinded:
                        break;

                    case KindOfSpecialRelation.Object:
                        if(!processedRelationsList.Contains(initRelation))
                        {
                            processedRelationsList.Add(initRelation);
                            secondConcept = initRelation.Outputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();

#if DEBUG
                            //LogInstance.Log($"secondConcept = {secondConcept}");
#endif

                            var inputNode = initRelation.Inputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();
                            targetConceptToRelation = inputNode;
#if DEBUG
                            //LogInstance.Log($"inputNode = {inputNode}");
#endif

                            if(inputNode != null && !processedSpecialConceptsList.Contains(inputNode))
                            {
                                processedSpecialConceptsList.Add(inputNode);

                                var stateRelation = inputNode.Inputs.Where(p => p.IsRelationNode && SpecialElementsHeper.GetKindOfSpecialRelation(p.Name) == KindOfSpecialRelation.State).Select(p => p.AsRelationNode).FirstOrDefault();

#if DEBUG
                                LogInstance.Log($"stateRelation = {stateRelation}");
#endif

                                if(stateRelation != null && !processedRelationsList.Contains(stateRelation))
                                {
                                    processedRelationsList.Add(stateRelation);

                                    firstConcept = stateRelation.Inputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();

#if DEBUG
                                    LogInstance.Log($"firstConcept = {firstConcept}");
#endif
                                }
                            }
                        }
                        initRelation.Destroy();
                        break;

                    case KindOfSpecialRelation.Experiencer:
                        if (!processedRelationsList.Contains(initRelation))
                        {
                            processedRelationsList.Add(initRelation);

                            firstConcept = initRelation.Outputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();

#if DEBUG
                            //LogInstance.Log($"firstConcept = {firstConcept}");
#endif

                            var inputNode = initRelation.Inputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();
                            targetConceptToRelation = inputNode;
#if DEBUG
                            //LogInstance.Log($"inputNode = {inputNode}");
#endif

                            if (inputNode != null && !processedSpecialConceptsList.Contains(inputNode))
                            {
                                processedSpecialConceptsList.Add(inputNode);

                                var objectRelation = inputNode.Outputs.Where(p => p.IsRelationNode && SpecialElementsHeper.GetKindOfSpecialRelation(p.Name) == KindOfSpecialRelation.Object).Select(p => p.AsRelationNode).FirstOrDefault();

#if DEBUG
                                //LogInstance.Log($"objectRelation = {objectRelation}");
#endif
                                if (objectRelation != null && !processedRelationsList.Contains(objectRelation))
                                {
                                    processedRelationsList.Add(objectRelation);
                                    secondConcept = objectRelation.Outputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();

#if DEBUG
                                    //LogInstance.Log($"secondConcept = {secondConcept}");
#endif
                                }
                            }
                        }
                        initRelation.Destroy();
                        //throw new NotImplementedException();
                        break;

                    case KindOfSpecialRelation.State:
                        if (!processedRelationsList.Contains(initRelation))
                        {
                            processedRelationsList.Add(initRelation);

                            firstConcept = initRelation.Inputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();

#if DEBUG
                            //LogInstance.Log($"firstConcept = {firstConcept}");
#endif

                            var outputNode = initRelation.Outputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();
                            targetConceptToRelation = outputNode;
#if DEBUG
                            //LogInstance.Log($"outputNode = {outputNode}");
#endif
                            if (outputNode != null && !processedSpecialConceptsList.Contains(outputNode))
                            {
                                processedSpecialConceptsList.Add(outputNode);

                                var objectRelation = outputNode.Outputs.Where(p => p.IsRelationNode && SpecialElementsHeper.GetKindOfSpecialRelation(p.Name) == KindOfSpecialRelation.Object).Select(p => p.AsRelationNode).FirstOrDefault();

#if DEBUG
                                //LogInstance.Log($"objectRelation = {objectRelation}");
#endif

                                if (objectRelation != null && !processedRelationsList.Contains(objectRelation))
                                {
                                    processedRelationsList.Add(objectRelation);
                                    secondConcept = objectRelation.Outputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();

#if DEBUG
                                    //LogInstance.Log($"secondConcept = {secondConcept}");
#endif
                                }
                            }
                        }
                        initRelation.Destroy();
                        //throw new NotImplementedException();
                        break;

                    case KindOfSpecialRelation.Agent:
                        if (!processedRelationsList.Contains(initRelation))
                        {
                            processedRelationsList.Add(initRelation);
                        }
                        initRelation.Destroy();
                        throw new NotImplementedException();
                        break;

                    case KindOfSpecialRelation.Action:
                        if (!processedRelationsList.Contains(initRelation))
                        {
                            processedRelationsList.Add(initRelation);
                        }
                        initRelation.Destroy();
                        throw new NotImplementedException();
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(kindOfSpecialRelation), kindOfSpecialRelation, null);
                }

                if(firstConcept != null && targetConceptToRelation != null && secondConcept != null)
                {
#if DEBUG
                    LogInstance.Log($"firstConcept = {firstConcept}");
                    LogInstance.Log($"targetConceptToRelation = {targetConceptToRelation}");
                    LogInstance.Log($"secondConcept = {secondConcept}");
#endif

                    var parent = firstConcept.Parent;

                    var relation = new InternalRelationCGNode();
                    relation.Parent = parent;
                    relation.Name = targetConceptToRelation.Name;
                    relation.Key = targetConceptToRelation.Key;
                    relation.AddInputNode(firstConcept);
                    relation.AddOutputNode(secondConcept);
                }
            }
        }

        private static void PrepareForEntityConditionExpression(InternalConceptualGraph source, RuleInstance dest, ContextOfConvertingInternalCGToPersistLogicalData context)
        {
            var n = 0;
            var conceptsNamesDict = new Dictionary<string, string>();

            var variablesQuantification = new VariablesQuantificationPart();
            dest.VariablesQuantification = variablesQuantification;
            variablesQuantification.Items = new List<VarExpressionNode>();

            var initRelationsList = source.Children.Where(p => p.IsRelationNode).Select(p => p.AsRelationNode).ToList();

#if DEBUG
            LogInstance.Log($"initRelationsList.Count = {initRelationsList.Count}");
#endif

            foreach(var initRelation in initRelationsList)
            {
#if DEBUG
                LogInstance.Log($"initRelation = {initRelation}");
#endif

                var inputNode = initRelation.Inputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();

#if DEBUG
                LogInstance.Log($"inputNode = {inputNode}");
#endif

                var kindOfInputNode = inputNode.KindOfGraphOrConcept;

                switch(kindOfInputNode)
                {
                    case KindOfInternalGraphOrConceptNode.Concept:
                        {
                            var conceptName = inputNode.Name;

#if DEBUG
                            LogInstance.Log($"conceptName = {conceptName}");
#endif
                            var varName = string.Empty;

                            if(conceptsNamesDict.ContainsKey(conceptName))
                            {
                                varName = conceptsNamesDict[conceptName];
                            }
                            else
                            {
                                n++;
                                varName = $"@X{n}";
                                conceptsNamesDict[conceptName] = varName;

                                var varQuant_1 = new VarExpressionNode();
                                varQuant_1.Quantifier = KindOfQuantifier.Existential;
                                varQuant_1.Name = varName;
                                varQuant_1.Key = context.EntityDictionary.GetKey(varQuant_1.Name);
                                variablesQuantification.Items.Add(varQuant_1);

                                var isaRealtion = new InternalRelationCGNode();
                                isaRealtion.Name = "is_a";
                                isaRealtion.Key = context.EntityDictionary.GetKey(isaRealtion.Name);
                                isaRealtion.Parent = inputNode.Parent;

                                var varNode = new InternalConceptCGNode();
                                varNode.KindOfGraphOrConcept = KindOfInternalGraphOrConceptNode.Variable;
                                varNode.Name = varName;
                                varNode.Key = context.EntityDictionary.GetKey(varName);

                                isaRealtion.AddInputNode(varNode);
                                inputNode.AddInputNode(isaRealtion);                   
                            }

#if DEBUG
                            LogInstance.Log($"varName = {varName}");
#endif
                            {
                                var varNode = new InternalConceptCGNode();
                                varNode.KindOfGraphOrConcept = KindOfInternalGraphOrConceptNode.Variable;
                                varNode.Name = varName;
                                varNode.Key = context.EntityDictionary.GetKey(varName);

                                initRelation.Inputs.Remove(inputNode);
                                initRelation.Inputs.Add(varNode);
                            }

                            //throw new NotImplementedException();
                        }
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(kindOfInputNode), kindOfInputNode, null);
                }
            }

            //throw new NotImplementedException();
        }

        private static BaseExpressionNode CreateExpressionByWholeGraph(InternalConceptualGraph source, ContextOfConvertingInternalCGToPersistLogicalData context, ContextForSingleRuleInstanceOfConvertingInternalCGToPersistLogicalData contextForSingleRuleInstance)
        {
            var relationsList = source.Children.Where(p => p.IsRelationNode).Select(p => p.AsRelationNode).ToList();

#if DEBUG
            LogInstance.Log($"relationsList.Count = {relationsList.Count}");
#endif

            if(relationsList.Count == 0)
            {
                return null;
            }

            if(relationsList.Count == 1)
            {
                return CreateExpressionByRelation(relationsList.Single(), context, contextForSingleRuleInstance);
            }

            var result = new OperatorAndExpressionNode();

            var relationsListEnumerator = relationsList.GetEnumerator();

            OperatorAndExpressionNode currentAndNode = result;
            BaseExpressionNode prevRelation = null;

            var n = 0;

            while (relationsListEnumerator.MoveNext())
            {
                n++;
                var relation = relationsListEnumerator.Current;

                var relationExpr = CreateExpressionByRelation(relation, context, contextForSingleRuleInstance);

#if DEBUG
                LogInstance.Log($"n = {n} relationExpr = {relationExpr}");
#endif

                if(prevRelation == null)
                {
                    currentAndNode.Left = relationExpr;
                }
                else
                {
                    if(n == relationsList.Count)
                    {
                        currentAndNode.Right = relationExpr;
                    }
                    else
                    {
                        var tmpAndNode = new OperatorAndExpressionNode();
                        currentAndNode.Right = tmpAndNode;
                        currentAndNode = tmpAndNode;
                        currentAndNode.Left = relationExpr;
                    }            
                }

                prevRelation = relationExpr;
            }

            return result;
        }

        private static BaseExpressionNode CreateExpressionByRelation(InternalRelationCGNode relation, ContextOfConvertingInternalCGToPersistLogicalData context, ContextForSingleRuleInstanceOfConvertingInternalCGToPersistLogicalData contextForSingleRuleInstance)
        {
#if DEBUG
            LogInstance.Log($"relation = {relation}");
#endif

            var relationExpr = new RelationExpressionNode();
            relationExpr.Name = relation.Name;
            relationExpr.Key = context.EntityDictionary.GetKey(relation.Name);
            relationExpr.Params = new List<BaseExpressionNode>();

            var inputNode = relation.Inputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();

#if DEBUG
            LogInstance.Log($"inputNode = {inputNode}");
#endif
            var inputNodeExpr = CreateExpressionByGraphOrConceptNode(inputNode, context, contextForSingleRuleInstance);

            relationExpr.Params.Add(inputNodeExpr);

            var outputNode = relation.Outputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();

#if DEBUG
            LogInstance.Log($"outputNode = {outputNode}");
#endif

            var outputNodeExpr = CreateExpressionByGraphOrConceptNode(outputNode, context, contextForSingleRuleInstance);

            relationExpr.Params.Add(outputNodeExpr);

            //throw new NotImplementedException();

            return relationExpr;
        }

        private static BaseExpressionNode CreateExpressionByGraphOrConceptNode(BaseInternalConceptCGNode graphOrConcept, ContextOfConvertingInternalCGToPersistLogicalData context, ContextForSingleRuleInstanceOfConvertingInternalCGToPersistLogicalData contextForSingleRuleInstance)
        {
#if DEBUG
            LogInstance.Log($"graphOrConcept = {graphOrConcept}");
#endif

            var kindOfGraphOrConcept = graphOrConcept.KindOfGraphOrConcept;

            switch(kindOfGraphOrConcept)
            {
                case KindOfInternalGraphOrConceptNode.Concept:
                    {
                        var conceptExpression = new ConceptExpressionNode();
                        conceptExpression.Name = graphOrConcept.Name;
                        conceptExpression.Key = context.EntityDictionary.GetKey(graphOrConcept.Name);
                        return conceptExpression;
                    }

                case KindOfInternalGraphOrConceptNode.EntityCondition:
                    {
                        var aliasOfEntityCondition = string.Empty;

                        if(contextForSingleRuleInstance.EntityConditionsDict.ContainsKey(graphOrConcept.Name))
                        {
                            aliasOfEntityCondition = contextForSingleRuleInstance.EntityConditionsDict[graphOrConcept.Name];
                        }
                        else
                        {
                            aliasOfEntityCondition = $"#@X{contextForSingleRuleInstance.EntityConditionsDict.Count + 1}";
                            contextForSingleRuleInstance.EntityConditionsDict[graphOrConcept.Name] = aliasOfEntityCondition;
                        }

#if DEBUG
                        LogInstance.Log($"aliasOfEntityCondition = {aliasOfEntityCondition}");
#endif

                        //throw new NotImplementedException();

                        var entityCondition = new EntityConditionExpressionNode();
                        entityCondition.Name = aliasOfEntityCondition;
                        entityCondition.Key = context.EntityDictionary.GetKey(aliasOfEntityCondition);
                        return entityCondition;
                    }

                case KindOfInternalGraphOrConceptNode.Variable:
                    {
                        var varExpr = new VarExpressionNode();
                        varExpr.Name = graphOrConcept.Name;
                        varExpr.Key = context.EntityDictionary.GetKey(graphOrConcept.Name);
                        return varExpr;
                    }

                default: throw new ArgumentOutOfRangeException(nameof(kindOfGraphOrConcept), kindOfGraphOrConcept, null);
            }
        }
    }
}
