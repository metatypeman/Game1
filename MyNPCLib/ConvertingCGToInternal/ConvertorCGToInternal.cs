using MyNPCLib.CG;
using MyNPCLib.CommonServiceGrammaticalElements;
using MyNPCLib.InternalCG;
using MyNPCLib.NLToCGParsing;
using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.ConvertingCGToInternal
{
    public static class ConvertorCGToInternal
    {
        public static InternalConceptualGraph Convert(ConceptualGraph source, IEntityDictionary entityDictionary)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            var context = new ContextOfConvertingCGToInternal();
            context.EntityDictionary = entityDictionary;

            while (IsWrapperGraph(source))
            {
                context.WrappersList.Add(source);
                source = (ConceptualGraph)source.Children.SingleOrDefault(p => p.Kind == KindOfCGNode.Graph);
            }

#if DEBUG
            LogInstance.Log($"source = {source}");
#endif


            return ConvertConceptualGraph(source, null, context);
        }

        private static bool IsWrapperGraph(ConceptualGraph source)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            var countOfConceptualGraphs = source.Children.Count(p => p.Kind == KindOfCGNode.Graph);

#if DEBUG
            LogInstance.Log($"countOfConceptualGraphs = {countOfConceptualGraphs}");
#endif

            var kindOfRelationsList = source.Children.Where(p => p.Kind == KindOfCGNode.Relation).Select(p => GrammaticalElementsHeper.GetKindOfGrammaticalRelationFromName(p.Name));

            var isGrammaticalRelationsOnly = !kindOfRelationsList.Any(p => p == KindOfGrammaticalRelation.Undefined);

#if DEBUG
            LogInstance.Log($"isGrammaticalRelationsOnly = {isGrammaticalRelationsOnly}");
#endif

            if(countOfConceptualGraphs == 1 && isGrammaticalRelationsOnly)
            {
                return true;
            }

            return false;
        }

        private static InternalConceptualGraph ConvertConceptualGraph(ConceptualGraph source, InternalConceptualGraph targetParent, ContextOfConvertingCGToInternal context)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            if(context.WrappersList.Contains(source))
            {
                return null;
            }

            if (context.ConceptualGraphsDict.ContainsKey(source))
            {
                return context.ConceptualGraphsDict[source];
            }

            var result = new InternalConceptualGraph();

            context.ConceptualGraphsDict[source] = result;

            if(targetParent == null)
            {
                if (source.Parent != null)
                {
                    var parentForResult = ConvertConceptualGraph(source.Parent, null, context);
                    result.Parent = parentForResult;
                }
            }
            else
            {
                result.Parent = targetParent;
            }

            FillName(source, result, context);

            SetGrammaticalOptions(source, result);

            CreateChildren(source, result, context);

            return result;
        }

        private static void FillName(BaseCGNode source, BaseInternalCGNode result, ContextOfConvertingCGToInternal context)
        {
            result.Name = source.Name;

            if (!string.IsNullOrWhiteSpace(source.Name))
            {
                result.Key = context.EntityDictionary.GetKey(source.Name);
            }
        }

        private static void CreateChildren(ConceptualGraph source, InternalConceptualGraph result, ContextOfConvertingCGToInternal context)
        {
            var childrenList = source.Children;

#if DEBUG
            LogInstance.Log($"childrenList.Count = {childrenList.Count}");
#endif

            var entitiesConditionsMarksRelationsList = childrenList.Where(p => GrammaticalElementsHeper.IsEntityCondition(p.Name)).ToList();

#if DEBUG
            LogInstance.Log($"entitiesConditionsMarksRelationsList.Count = {entitiesConditionsMarksRelationsList.Count}");
#endif

            if(entitiesConditionsMarksRelationsList.Count == 0)
            {
                CreateChildrenByAllNodes(childrenList, null, context);
                return;
            }

            var notDirectlyClonedNodesList = GetNotDirectlyClonedNodesList(entitiesConditionsMarksRelationsList);

#if DEBUG
            LogInstance.Log($"notDirectlyClonedNodesList.Count = {notDirectlyClonedNodesList.Count}");
            foreach(var notDirectlyClonedNode in notDirectlyClonedNodesList)
            {
                LogInstance.Log($"notDirectlyClonedNode = {notDirectlyClonedNode}");
            }
#endif

            var clustersOfLinkedNodesDict = GetClastersOfLinkedNodes(notDirectlyClonedNodesList);

#if DEBUG
            LogInstance.Log($"clustersOfLinkedNodesDict.Count = {clustersOfLinkedNodesDict.Count}");
#endif
            foreach (var clustersOfLinkedNodesKVPItem in clustersOfLinkedNodesDict)
            {
#if DEBUG
                LogInstance.Log($"clustersOfLinkedNodesKVPItem.Key = {clustersOfLinkedNodesKVPItem.Key}");
#endif
                CreateEntityCondition(result, clustersOfLinkedNodesKVPItem.Value, context);
            }

            var nodesForDirectlyClonningList = childrenList.Where(p => !notDirectlyClonedNodesList.Contains(p)).ToList();

            CreateChildrenByAllNodes(nodesForDirectlyClonningList, null, context);
        }

        private static void CreateEntityCondition(InternalConceptualGraph parent, List<BaseCGNode> sourceItems, ContextOfConvertingCGToInternal context)
        {
#if DEBUG
            LogInstance.Log($"sourceItems.Count = {sourceItems.Count}");
#endif

            var entityCondition = new InternalConceptualGraph();
            entityCondition.Parent = parent;
            entityCondition.IsEntityCondition = true;
            var entityConditionName = NamesHelper.CreateEntityName();
            entityCondition.Name = entityConditionName;
            entityCondition.Key = context.EntityDictionary.GetKey(entityConditionName);

            var entityConditionsDict = context.EntityConditionsDict;

            foreach (var sourceItem in sourceItems)
            {
#if DEBUG
                LogInstance.Log($"sourceItem = {sourceItem}");
#endif

                entityConditionsDict[sourceItem] = entityCondition;
            }

            CreateChildrenByAllNodes(sourceItems, entityCondition, context);

            var conceptsSourceItemsList = sourceItems.Where(p => p.Kind == KindOfCGNode.Concept).Select(p => ()p);

            foreach (var sourceItem in conceptsSourceItemsList)
            {
#if DEBUG
                LogInstance.Log($"sourceItem = {sourceItem}");
#endif

                var kind = sourceItem.Kind;

                switch(kind)
                {
                    case KindOfCGNode.Relation:
                        {
                            var resultItem = context.RelationsDict[(RelationCGNode)sourceItem];

#if DEBUG
                            LogInstance.Log($"resultItem = {resultItem}");
#endif


                        }
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }
            }
        }

        private static List<BaseCGNode> GetNotDirectlyClonedNodesList(List<BaseCGNode> entitiesConditionsMarksRelationsList)
        {
            var notDirectlyClonedNodesList = new List<BaseCGNode>();

            foreach (var entityConditionMarkRelation in entitiesConditionsMarksRelationsList)
            {
#if DEBUG
                LogInstance.Log($"entityConditionMarkRelation = {entityConditionMarkRelation}");
#endif

                notDirectlyClonedNodesList.Add(entityConditionMarkRelation);

                var firstOrdersRelationsList = entityConditionMarkRelation.Outputs.Where(p => p.Kind == KindOfCGNode.Relation).Select(p => (RelationCGNode)p).ToList();

#if DEBUG
                LogInstance.Log($"firstOrdersRelationsList.Count = {firstOrdersRelationsList.Count}");
#endif

                foreach (var firstOrderRelation in firstOrdersRelationsList)
                {
#if DEBUG
                    LogInstance.Log($"firstOrderRelation = {firstOrderRelation}");
#endif

                    if (!notDirectlyClonedNodesList.Contains(firstOrderRelation))
                    {
                        notDirectlyClonedNodesList.Add(firstOrderRelation);
                    }

                    var inputConceptsList = firstOrderRelation.Inputs.Where(p => p.Kind == KindOfCGNode.Concept).Select(p => (ConceptCGNode)p).ToList();

#if DEBUG
                    LogInstance.Log($"inputConceptsList.Count = {inputConceptsList.Count}");
#endif

                    foreach (var inputConcept in inputConceptsList)
                    {
#if DEBUG
                        LogInstance.Log($"inputConcept = {inputConcept}");
#endif

                        if (!notDirectlyClonedNodesList.Contains(inputConcept))
                        {
                            notDirectlyClonedNodesList.Add(inputConcept);
                        }
                    }

                    var outputConceptsList = firstOrderRelation.Outputs.Where(p => p.Kind == KindOfCGNode.Concept).Select(p => (ConceptCGNode)p).ToList();

#if DEBUG
                    LogInstance.Log($"outputConceptsList.Count = {outputConceptsList.Count}");
#endif

                    foreach (var outputConcept in outputConceptsList)
                    {
#if DEBUG
                        LogInstance.Log($"outputConcept = {outputConcept}");
#endif

                        if (!notDirectlyClonedNodesList.Contains(outputConcept))
                        {
                            notDirectlyClonedNodesList.Add(outputConcept);
                        }
                    }
                }
            }

            return notDirectlyClonedNodesList;
        }

        private static Dictionary<int, List<BaseCGNode>> GetClastersOfLinkedNodes(List<BaseCGNode> source)
        {
            var result = new Dictionary<int, List<BaseCGNode>>();

            var currentTargetNodesList = source.ToList();
            var n = 0;
            while(currentTargetNodesList.Count > 0)
            {
                n++;
#if DEBUG
                LogInstance.Log($"currentTargetNodesList.Count = {currentTargetNodesList.Count} n = {n}");
#endif

                var nodesForThisN = GetLinkedNodes(currentTargetNodesList);

                if(nodesForThisN.Count == 0)
                {
                    break;
                }

                result[n] = nodesForThisN;

                currentTargetNodesList = currentTargetNodesList.Where(p => !nodesForThisN.Contains(p)).ToList();
            }

            return result;
        }

        private static List<BaseCGNode> GetLinkedNodes(List<BaseCGNode> source)
        {
            var result = new List<BaseCGNode>();

            foreach(var sourceItem in source)
            {
                NGetLinkedNodes(source, sourceItem, ref result);
            }

            return result;
        }

        private static void NGetLinkedNodes(List<BaseCGNode> source, BaseCGNode targetNode, ref List<BaseCGNode> result)
        {
#if DEBUG
            LogInstance.Log($"targetNode = {targetNode}");
#endif

            if(result.Contains(targetNode))
            {
                return;
            }

            result.Add(targetNode);

            var inputNodesList = targetNode.Inputs;

#if DEBUG
            LogInstance.Log($"inputNodesList.Count = {inputNodesList.Count}");
#endif

            if(inputNodesList.Count > 0)
            {
                var tmpNodesList = inputNodesList.Where(p => source.Contains(p)).ToList();

#if DEBUG
                LogInstance.Log($"tmpNodesList.Count = {tmpNodesList.Count}");
#endif

                foreach (var tmpNode in tmpNodesList)
                {
                    NGetLinkedNodes(source, tmpNode, ref result);
                }
            }

            var outputNodesList = targetNode.Outputs;

#if DEBUG
            LogInstance.Log($"outputNodesList.Count = {outputNodesList.Count}");
#endif

            if(outputNodesList.Count > 0)
            {
                var tmpNodesList = outputNodesList.Where(p => source.Contains(p)).ToList();

#if DEBUG
                LogInstance.Log($"tmpNodesList.Count = {tmpNodesList.Count}");
#endif

                foreach(var tmpNode in tmpNodesList)
                {
                    NGetLinkedNodes(source, tmpNode, ref result);
                }
            }
        }

        private static void CreateChildrenByAllNodes(IList<BaseCGNode> childrenList, InternalConceptualGraph targetParent, ContextOfConvertingCGToInternal context)
        {
#if DEBUG
            LogInstance.Log($"childrenList.Count = {childrenList.Count}");
#endif

            foreach (var child in childrenList)
            {
#if DEBUG
                LogInstance.Log($"child = {child}");
#endif

                var kind = child.Kind;

                switch (kind)
                {
                    case KindOfCGNode.Graph:
                        ConvertConceptualGraph((ConceptualGraph)child, targetParent, context);
                        break;

                    case KindOfCGNode.Concept:
                        ConvertConcept((ConceptCGNode)child, targetParent, context);
                        break;

                    case KindOfCGNode.Relation:
                        ConvertRelation((RelationCGNode)child, targetParent, context);
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }
            }
        }

        private static void SetGrammaticalOptions(ConceptualGraph source, InternalConceptualGraph result)
        {
            var outputNodesGroupedDict = source.Outputs.GroupBy(p => GrammaticalElementsHeper.GetKindOfGrammaticalRelationFromName(p.Name)).ToDictionary(p => p.Key, p => p.ToList());

#if DEBUG
            LogInstance.Log($"outputNodesGroupedDict.Count = {outputNodesGroupedDict.Count}");
#endif

            foreach (var outputNodesGroupedKVPItem in outputNodesGroupedDict)
            {
                var kindOfGrammaticalRelation = outputNodesGroupedKVPItem.Key;
                var relationsList = outputNodesGroupedKVPItem.Value;

                switch (kindOfGrammaticalRelation)
                {
                    case KindOfGrammaticalRelation.Undefined:
                        continue;

                    case KindOfGrammaticalRelation.Aspect:
                        {
                            if (relationsList.Count > 1)
                            {
                                throw new NotSupportedException();
                            }

                            var relation = relationsList.Single();

                            var outputNodesOfTheRelation = relation.Outputs;

                            if (outputNodesOfTheRelation.Count != 1)
                            {
                                throw new NotSupportedException();
                            }

                            var outputNodeOfTheRelation = outputNodesOfTheRelation.Single();

#if DEBUG
                            LogInstance.Log($"relation = {relation}");
                            LogInstance.Log($"outputNodeOfTheRelation = {outputNodeOfTheRelation}");
#endif

                            var aspect = GrammaticalElementsHeper.GetAspectFromName(outputNodeOfTheRelation.Name);

#if DEBUG
                            LogInstance.Log($"aspect = {aspect}");
#endif

                            if (aspect == GrammaticalAspect.Undefined)
                            {
                                throw new NotSupportedException();
                            }

                            result.Aspect = aspect;
                        }
                        break;

                    case KindOfGrammaticalRelation.Tense:
                        {
                            if (relationsList.Count > 1)
                            {
                                throw new NotSupportedException();
                            }

                            var relation = relationsList.Single();

                            var outputNodesOfTheRelation = relation.Outputs;

                            if (outputNodesOfTheRelation.Count != 1)
                            {
                                throw new NotSupportedException();
                            }

                            var outputNodeOfTheRelation = outputNodesOfTheRelation.Single();

#if DEBUG
                            LogInstance.Log($"relation = {relation}");
                            LogInstance.Log($"outputNodeOfTheRelation = {outputNodeOfTheRelation}");
#endif

                            var tense = GrammaticalElementsHeper.GetTenseFromName(outputNodeOfTheRelation.Name);

#if DEBUG
                            LogInstance.Log($"tense = {tense}");
#endif

                            if (tense == GrammaticalTenses.Undefined)
                            {
                                throw new NotSupportedException();
                            }

                            result.Tense = tense;
                        }
                        break;

                    case KindOfGrammaticalRelation.Voice:
                        {
                            if (relationsList.Count > 1)
                            {
                                throw new NotSupportedException();
                            }

                            var relation = relationsList.Single();

                            var outputNodesOfTheRelation = relation.Outputs;

                            if (outputNodesOfTheRelation.Count != 1)
                            {
                                throw new NotSupportedException();
                            }

                            var outputNodeOfTheRelation = outputNodesOfTheRelation.Single();

#if DEBUG
                            LogInstance.Log($"relation = {relation}");
                            LogInstance.Log($"outputNodeOfTheRelation = {outputNodeOfTheRelation}");
#endif

                            var voice = GrammaticalElementsHeper.GetVoiceFromName(outputNodeOfTheRelation.Name);

#if DEBUG
                            LogInstance.Log($"voice = {voice}");
#endif

                            if (voice == GrammaticalVoice.Undefined)
                            {
                                throw new NotSupportedException();
                            }

                            result.Voice = voice;
                        }
                        break;

                    case KindOfGrammaticalRelation.Mood:
                        {
                            if (relationsList.Count > 1)
                            {
                                throw new NotSupportedException();
                            }

                            var relation = relationsList.Single();

                            var outputNodesOfTheRelation = relation.Outputs;

                            if (outputNodesOfTheRelation.Count != 1)
                            {
                                throw new NotSupportedException();
                            }

                            var outputNodeOfTheRelation = outputNodesOfTheRelation.Single();

#if DEBUG
                            LogInstance.Log($"relation = {relation}");
                            LogInstance.Log($"outputNodeOfTheRelation = {outputNodeOfTheRelation}");
#endif

                            var mood = GrammaticalElementsHeper.GetMoodFromName(outputNodeOfTheRelation.Name);

#if DEBUG
                            LogInstance.Log($"mood = {mood}");
#endif

                            if (mood == GrammaticalMood.Undefined)
                            {
                                throw new NotSupportedException();
                            }

                            result.Mood = mood;
                        }
                        break;

                    case KindOfGrammaticalRelation.Modal:
                        {
                            if (relationsList.Count > 1)
                            {
                                throw new NotSupportedException();
                            }

                            var relation = relationsList.Single();

                            var outputNodesOfTheRelation = relation.Outputs;

                            if (outputNodesOfTheRelation.Count != 1)
                            {
                                throw new NotSupportedException();
                            }

                            var outputNodeOfTheRelation = outputNodesOfTheRelation.Single();

#if DEBUG
                            LogInstance.Log($"relation = {relation}");
                            LogInstance.Log($"outputNodeOfTheRelation = {outputNodeOfTheRelation}");
#endif

                            var modal = GrammaticalElementsHeper.GetModalFromName(outputNodeOfTheRelation.Name);

#if DEBUG
                            LogInstance.Log($"modal = {modal}");
#endif

                            if (modal == KindOfModal.Undefined)
                            {
                                throw new NotSupportedException();
                            }

                            result.Modal = modal;
                        }
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(kindOfGrammaticalRelation), kindOfGrammaticalRelation, null);
                }
            }
        }

        private static InternalConceptCGNode ConvertConcept(ConceptCGNode source, InternalConceptualGraph targetParent, ContextOfConvertingCGToInternal context)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            if (context.ConceptsDict.ContainsKey(source))
            {
                return context.ConceptsDict[source];
            }

            var result = new InternalConceptCGNode();

            context.ConceptsDict[source] = result;

            if(targetParent == null)
            {
                var parentForResult = ConvertConceptualGraph(source.Parent, null, context);
                result.Parent = parentForResult;
            }
            else
            {
                result.Parent = targetParent;
            }

            FillName(source, result, context);

            return result;
        }

        private static InternalRelationCGNode ConvertRelation(RelationCGNode source, InternalConceptualGraph targetParent, ContextOfConvertingCGToInternal context)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            if (context.RelationsDict.ContainsKey(source))
            {
                return context.RelationsDict[source];
            }

            var result = new InternalRelationCGNode();

            context.RelationsDict[source] = result;

            if(targetParent == null)
            {
                var parentForResult = ConvertConceptualGraph(source.Parent, null, context);
                result.Parent = parentForResult;
            }
            else
            {
                result.Parent = targetParent;
            }

            FillName(source, result, context);

            return result;
        }
    }
}
