using MyNPCLib.CG;
using MyNPCLib.CommonServiceGrammaticalElements;
using MyNPCLib.Dot;
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
            //LogInstance.Log($"source = {source}");
#endif

            var context = new ContextOfConvertingCGToInternal();
            context.EntityDictionary = entityDictionary;

            while (IsWrapperGraph(source))
            {
                context.WrappersList.Add(source);
                source = (ConceptualGraph)source.Children.SingleOrDefault(p => p.Kind == KindOfCGNode.Graph);
            }

#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif

            return ConvertConceptualGraph(source, null, context);
        }

        private static bool IsWrapperGraph(ConceptualGraph source)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
#endif

            var countOfConceptualGraphs = source.Children.Count(p => p.Kind == KindOfCGNode.Graph);

#if DEBUG
            //LogInstance.Log($"countOfConceptualGraphs = {countOfConceptualGraphs}");
#endif

            var kindOfRelationsList = source.Children.Where(p => p.Kind == KindOfCGNode.Relation).Select(p => GrammaticalElementsHeper.GetKindOfGrammaticalRelationFromName(p.Name));

            var isGrammaticalRelationsOnly = !kindOfRelationsList.Any(p => p == KindOfGrammaticalRelation.Undefined);

#if DEBUG
            //LogInstance.Log($"isGrammaticalRelationsOnly = {isGrammaticalRelationsOnly}");
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
            //LogInstance.Log($"source = {source}");
#endif

            if(context.WrappersList.Contains(source))
            {
                return null;
            }

            if (context.ConceptualGraphsDict.ContainsKey(source))
            {
                return context.ConceptualGraphsDict[source];
            }

#if DEBUG
            //LogInstance.Log($"NEXT source = {source}");
#endif

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

            result.KindOfGraphOrConcept = KindOfInternalGraphOrConceptNode.Graph;

            FillName(source, result, context);

            SetGrammaticalOptions(source, result);

#if DEBUG
            //LogInstance.Log($"before result = {result}");
#endif

            CreateChildren(source, result, context);

#if DEBUG
            //LogInstance.Log($"after result = {result}");
            //var dotStr = DotConverter.ConvertToString(result);
            //LogInstance.Log($"dotStr = {dotStr}");
#endif

            TransformResultToCanonicalView(result, context);

#if DEBUG
            //LogInstance.Log($"NEXT after result = {result}");
            //dotStr = DotConverter.ConvertToString(result);
            //LogInstance.Log($"dotStr = {dotStr}");
#endif

            return result;
        }

        private static void TransformResultToCanonicalView(InternalConceptualGraph dest, ContextOfConvertingCGToInternal context)
        {
#if DEBUG
            //LogInstance.Log($"dest = {dest}");
            //var dotStr = DotConverter.ConvertToString(dest);
            //LogInstance.Log($"dotStr = {dotStr}");
#endif

            TransformResultToCanonicalViewForActionConcepts(dest, context);
            TransformResultToCanonicalViewForStateConcepts(dest, context);
        }

        private static void TransformResultToCanonicalViewForActionConcepts(InternalConceptualGraph dest, ContextOfConvertingCGToInternal context)
        {
            var entityDictionary = context.EntityDictionary;
            var actionsConceptsList = dest.Children.Where(p => p.IsConceptNode && !p.Inputs.IsEmpty() && p.Inputs.Any(x => x.Name == SpecialNamesOfRelations.ActionRelationName)).Select(p => p.AsConceptNode).ToList();

#if DEBUG
            //LogInstance.Log($"actionsConceptsList.Count = {actionsConceptsList.Count}");
#endif

            foreach (var actionConcept in actionsConceptsList)
            {
#if DEBUG
                //LogInstance.Log($"actionConcept = {actionConcept}");
#endif

                var actionRelationsList = actionConcept.Inputs.Where(p => p.Name == SpecialNamesOfRelations.ActionRelationName).Select(p => p.AsRelationNode).ToList();

#if DEBUG
                //LogInstance.Log($"actionRelationsList.Count = {actionRelationsList.Count}");
#endif

                foreach (var actionRelation in actionRelationsList)
                {
#if DEBUG
                    //LogInstance.Log($"actionRelation = {actionRelation}");
#endif

                    if (!actionRelation.Inputs.IsEmpty())
                    {
                        continue;
                    }

#if DEBUG
                    //LogInstance.Log("Add stub of subject !!!!");
#endif

                    var stubOfOfSubject = new InternalConceptCGNode();
                    stubOfOfSubject.KindOfGraphOrConcept = KindOfInternalGraphOrConceptNode.Concept;
                    stubOfOfSubject.Parent = dest;
                    stubOfOfSubject.AddOutputNode(actionRelation);
                    stubOfOfSubject.Name = SpecialNamesOfConcepts.SomeOne;
                    stubOfOfSubject.Key = entityDictionary.GetKey(SpecialNamesOfConcepts.SomeOne);

                    var agentRelation = new InternalRelationCGNode();
                    agentRelation.Parent = dest;
                    agentRelation.Name = SpecialNamesOfRelations.AgentRelationName;
                    agentRelation.Key = entityDictionary.GetKey(SpecialNamesOfRelations.AgentRelationName);
                    agentRelation.AddInputNode(actionConcept);
                    agentRelation.AddOutputNode(stubOfOfSubject);
                }

                TransformResultToCanonicalViewTryFillObjectNode(actionConcept, context);
            }
        }

        private static void TransformResultToCanonicalViewForStateConcepts(InternalConceptualGraph dest, ContextOfConvertingCGToInternal context)
        {
            var entityDictionary = context.EntityDictionary;
            var statesConceptsList = dest.Children.Where(p => p.IsConceptNode && !p.Inputs.IsEmpty() && p.Inputs.Any(x => x.Name == SpecialNamesOfRelations.StateRelationName)).Select(p => p.AsConceptNode).ToList();

#if DEBUG
            //LogInstance.Log($"statesConceptsList.Count = {statesConceptsList.Count}");
#endif

            foreach(var stateConcept in statesConceptsList)
            {
#if DEBUG
                //LogInstance.Log($"stateConcept = {stateConcept}");
#endif

                var stateRelationsList = stateConcept.Inputs.Where(p => p.Name == SpecialNamesOfRelations.StateRelationName).Select(p => p.AsRelationNode).ToList();

#if DEBUG
                //LogInstance.Log($"stateRelationsList.Count = {stateRelationsList.Count}");
#endif

                foreach(var stateRelation in stateRelationsList)
                {
                    if (!stateRelation.Inputs.IsEmpty())
                    {
                        continue;
                    }

#if DEBUG
                    //LogInstance.Log("Add stub of subject !!!!");
#endif

                    var stubOfOfSubject = new InternalConceptCGNode();
                    stubOfOfSubject.KindOfGraphOrConcept = KindOfInternalGraphOrConceptNode.Concept;
                    stubOfOfSubject.Parent = dest;
                    stubOfOfSubject.AddOutputNode(stateRelation);
                    stubOfOfSubject.Name = SpecialNamesOfConcepts.SomeOne;
                    stubOfOfSubject.Key = entityDictionary.GetKey(SpecialNamesOfConcepts.SomeOne);
                }

                TransformResultToCanonicalViewTryFillObjectNode(stateConcept, context);
            }
        }

        private static void TransformResultToCanonicalViewTryFillObjectNode(InternalConceptCGNode concept, ContextOfConvertingCGToInternal context)
        {
            var objectRelationsList = concept.Outputs.Where(p => p.Name == SpecialNamesOfRelations.ObjectRelationName).Select(p => p.AsRelationNode).ToList();
            var parent = concept.Parent;
            var entityDictionary = context.EntityDictionary;

#if DEBUG
            //LogInstance.Log($"objectRelationsList.Count = {objectRelationsList.Count}");
#endif

            if (objectRelationsList.Count == 0)
            {
#if DEBUG
                //LogInstance.Log("Add stub of object !!!!");
#endif

                var objectRelation = new InternalRelationCGNode();
                objectRelation.Parent = parent;
                objectRelation.Name = SpecialNamesOfRelations.ObjectRelationName;
                objectRelation.Key = entityDictionary.GetKey(SpecialNamesOfRelations.ObjectRelationName);
                concept.AddOutputNode(objectRelation);

                var stubOfObject = new InternalConceptCGNode();
                stubOfObject.KindOfGraphOrConcept = KindOfInternalGraphOrConceptNode.Concept;
                stubOfObject.Parent = parent;
                stubOfObject.Name = SpecialNamesOfConcepts.Self;
                stubOfObject.Key = entityDictionary.GetKey(SpecialNamesOfConcepts.Self);
                objectRelation.AddOutputNode(stubOfObject);
            }
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
            //LogInstance.Log($"childrenList.Count = {childrenList.Count}");
#endif

            var entitiesConditionsMarksRelationsList = childrenList.Where(p => GrammaticalElementsHeper.IsEntityCondition(p.Name)).ToList();

#if DEBUG
            //LogInstance.Log($"entitiesConditionsMarksRelationsList.Count = {entitiesConditionsMarksRelationsList.Count}");
#endif

            if(entitiesConditionsMarksRelationsList.Count == 0)
            {
                CreateChildrenByAllNodes(childrenList, null, context);
                return;
            }

            var notDirectlyClonedNodesList = GetNotDirectlyClonedNodesList(entitiesConditionsMarksRelationsList);

#if DEBUG
            //LogInstance.Log($"notDirectlyClonedNodesList.Count = {notDirectlyClonedNodesList.Count}");
            //foreach (var notDirectlyClonedNode in notDirectlyClonedNodesList)
            //{
            //    LogInstance.Log($"notDirectlyClonedNode = {notDirectlyClonedNode}");
            //}
#endif

            var clustersOfLinkedNodesDict = GetClastersOfLinkedNodes(notDirectlyClonedNodesList);

#if DEBUG
            //LogInstance.Log($"clustersOfLinkedNodesDict.Count = {clustersOfLinkedNodesDict.Count}");
#endif
            foreach (var clustersOfLinkedNodesKVPItem in clustersOfLinkedNodesDict)
            {
#if DEBUG
                //LogInstance.Log($"clustersOfLinkedNodesKVPItem.Key = {clustersOfLinkedNodesKVPItem.Key}");
#endif
                CreateEntityCondition(result, clustersOfLinkedNodesKVPItem.Value, context);
            }

            var nodesForDirectlyClonningList = childrenList.Where(p => !notDirectlyClonedNodesList.Contains(p)).ToList();

            CreateChildrenByAllNodes(nodesForDirectlyClonningList, null, context);

            var conceptsSourceItemsList = nodesForDirectlyClonningList.Where(p => p.Kind == KindOfCGNode.Concept || p.Kind == KindOfCGNode.Graph).Select(p => (BaseConceptCGNode)p).ToList();

            var relationStorage = new RelationStorageOfSemanticAnalyzer();

            foreach(var sourceItem in conceptsSourceItemsList)
            {
#if DEBUG
                //LogInstance.Log($"sourceItem = {sourceItem}");
#endif

                BaseInternalConceptCGNode resultItem = null;

                var kind = sourceItem.Kind;

                switch(kind)
                {
                    case KindOfCGNode.Graph:
                        resultItem = context.ConceptualGraphsDict[(ConceptualGraph)sourceItem];
                        break;

                    case KindOfCGNode.Concept:
                        resultItem = context.ConceptsDict[(ConceptCGNode)sourceItem];
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }
          
#if DEBUG
                //LogInstance.Log($"resultItem = {resultItem}");
#endif

                var inputsNodesList = sourceItem.Inputs.Select(p => (RelationCGNode)p).ToList();

#if DEBUG
                //LogInstance.Log($"inputsNodesList.Count = {inputsNodesList.Count}");
#endif

                foreach (var inputNode in inputsNodesList)
                {
#if DEBUG
                    //LogInstance.Log($"Begin inputNode (Relation) = {inputNode}");
#endif

                    var resultRelationItem = context.RelationsDict[inputNode];

#if DEBUG
                    //LogInstance.Log($"resultRelationItem = {resultRelationItem}");
#endif

                    var relationsInputsNodesList = inputNode.Inputs.Where(p => p.Kind == KindOfCGNode.Concept || p.Kind == KindOfCGNode.Graph).Select(p => (BaseConceptCGNode)p).ToList();

#if DEBUG
                    //LogInstance.Log($"relationsInputsNodesList.Count = {relationsInputsNodesList.Count}");
#endif

                    if(relationsInputsNodesList.Count == 0)
                    {
                        if(!relationStorage.ContainsRelation(resultRelationItem.Name, resultItem.Name))
                        {
                            resultItem.AddInputNode(resultRelationItem);
                            relationStorage.AddRelation(resultRelationItem.Name, resultItem.Name);
                        }               
                    }
                    else
                    {
                        foreach (var relationInputNode in relationsInputsNodesList)
                        {
#if DEBUG
                            //LogInstance.Log($"relationInputNode = {relationInputNode}");
#endif

                            var resultRelationInputNode = GetBaseConceptCGNodeForMakingCommonRelation(relationInputNode, context);

#if DEBUG
                            //LogInstance.Log($"resultRelationInputNode = {resultRelationInputNode}");
#endif
                            if (!relationStorage.ContainsRelation(resultRelationInputNode.Name, resultRelationItem.Name, resultItem.Name))
                            {
                                resultItem.AddInputNode(resultRelationItem);
                                resultRelationItem.AddInputNode(resultRelationInputNode);

                                relationStorage.AddRelation(resultRelationInputNode.Name, resultRelationItem.Name, resultItem.Name);
                            }
                        }
                    }

#if DEBUG
                    //LogInstance.Log($"End inputNode (Relation) = {inputNode}");
#endif
                }

                //throw new NotImplementedException();

                var outputsNodesList = sourceItem.Outputs.Select(p => (RelationCGNode)p).ToList();

#if DEBUG
                //LogInstance.Log($"outputsNodesList.Count = {outputsNodesList.Count}");
#endif

                foreach (var outputNode in outputsNodesList)
                {
#if DEBUG
                    //LogInstance.Log($"Begin outputNode (Relation) = {outputNode}");
#endif

                    var resultRelationItem = context.RelationsDict[outputNode];

#if DEBUG
                    //LogInstance.Log($"resultRelationItem = {resultRelationItem}");
#endif

                    var relationsOutputsNodesList = outputNode.Outputs.Where(p => p.Kind == KindOfCGNode.Concept || p.Kind == KindOfCGNode.Graph).Select(p => (BaseConceptCGNode)p).ToList();

#if DEBUG
                    //LogInstance.Log($"relationsOutputsNodesList.Count = {relationsOutputsNodesList.Count}");
#endif

                    foreach (var relationOutputNode in relationsOutputsNodesList)
                    {
#if DEBUG
                        //LogInstance.Log($"relationOutputNode = {relationOutputNode}");
#endif
                        var resultRelationOutputNode = GetBaseConceptCGNodeForMakingCommonRelation(relationOutputNode, context);

#if DEBUG
                        //LogInstance.Log($"resultRelationOutputNode = {resultRelationOutputNode}");
#endif

                        if (!relationStorage.ContainsRelation(resultItem.Name, resultRelationItem.Name, resultRelationOutputNode.Name))
                        {
                            resultItem.AddOutputNode(resultRelationItem);
                            resultRelationItem.AddOutputNode(resultRelationOutputNode);

                            relationStorage.AddRelation(resultItem.Name, resultRelationItem.Name, resultRelationOutputNode.Name);
                        }
                    }
#if DEBUG
                    //LogInstance.Log($"End outputNode (Relation) = {outputNode}");
#endif
                }
            }

            //throw new NotImplementedException();
        }

        public static BaseInternalConceptCGNode GetBaseConceptCGNodeForMakingCommonRelation(BaseConceptCGNode sourceNode, ContextOfConvertingCGToInternal context)
        {
            if(context.EntityConditionsDict.ContainsKey(sourceNode))
            {
                return context.EntityConditionsDict[sourceNode];
            }

            var kind = sourceNode.Kind;

            switch (kind)
            {
                case KindOfCGNode.Graph:
                    return context.ConceptualGraphsDict[(ConceptualGraph)sourceNode];

                case KindOfCGNode.Concept:
                    return context.ConceptsDict[(ConceptCGNode)sourceNode];

                default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private static void CreateEntityCondition(InternalConceptualGraph parent, List<BaseCGNode> sourceItems, ContextOfConvertingCGToInternal context)
        {
#if DEBUG
            //LogInstance.Log($"sourceItems.Count = {sourceItems.Count}");
#endif

            var entityCondition = new InternalConceptualGraph();
            entityCondition.Parent = parent;
            entityCondition.KindOfGraphOrConcept = KindOfInternalGraphOrConceptNode.EntityCondition;
            var entityConditionName = NamesHelper.CreateEntityName();
            entityCondition.Name = entityConditionName;
            entityCondition.Key = context.EntityDictionary.GetKey(entityConditionName);

            var entityConditionsDict = context.EntityConditionsDict;

            sourceItems = sourceItems.Where(p => !GrammaticalElementsHeper.IsEntityCondition(p.Name)).ToList();

            foreach (var sourceItem in sourceItems)
            {
#if DEBUG
                //LogInstance.Log($"sourceItem = {sourceItem}");
#endif

                entityConditionsDict[sourceItem] = entityCondition;
            }

            CreateChildrenByAllNodes(sourceItems, entityCondition, context);

            var conceptsSourceItemsList = sourceItems.Where(p => p.Kind == KindOfCGNode.Concept || p.Kind == KindOfCGNode.Graph).Select(p => (BaseConceptCGNode)p).ToList();

            var relationStorage = new RelationStorageOfSemanticAnalyzer();

            foreach (var sourceItem in conceptsSourceItemsList)
            {
#if DEBUG
                //LogInstance.Log($"sourceItem (2) = {sourceItem}");
#endif

                BaseInternalConceptCGNode resultItem = null;

                var kind = sourceItem.Kind;

                switch (kind)
                {
                    case KindOfCGNode.Graph:
                        resultItem = context.ConceptualGraphsDict[(ConceptualGraph)sourceItem];
                        break;

                    case KindOfCGNode.Concept:
                        resultItem = context.ConceptsDict[(ConceptCGNode)sourceItem];
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
                }

#if DEBUG
                //LogInstance.Log($"resultItem = {resultItem}");
#endif

                var inputsNodesList = sourceItem.Inputs.Where(p => sourceItems.Contains(p)).Select(p => (RelationCGNode)p).ToList();

#if DEBUG
                //LogInstance.Log($"inputsNodesList.Count = {inputsNodesList.Count}");
#endif

                foreach(var inputNode in inputsNodesList)
                {
#if DEBUG
                    //LogInstance.Log($"inputNode = {inputNode}");
#endif

                    var resultRelationItem = context.RelationsDict[inputNode];

#if DEBUG
                    //LogInstance.Log($"resultRelationItem = {resultRelationItem}");
#endif

                    var relationsInputsNodesList = inputNode.Inputs.Where(p => (p.Kind == KindOfCGNode.Concept || p.Kind == KindOfCGNode.Graph) && sourceItems.Contains(p)).Select(p => (BaseConceptCGNode)p).ToList();

#if DEBUG
                    //LogInstance.Log($"relationsInputsNodesList.Count = {relationsInputsNodesList.Count}");
#endif

                    foreach (var relationInputNode in relationsInputsNodesList)
                    {
#if DEBUG
                        //LogInstance.Log($"relationInputNode = {relationInputNode}");
#endif

                        BaseInternalConceptCGNode resultRelationInputNode = null;

                        var relationInputNodeKind = relationInputNode.Kind;

                        switch(relationInputNodeKind)
                        {
                            case KindOfCGNode.Graph:
                                resultRelationInputNode = context.ConceptualGraphsDict[(ConceptualGraph)relationInputNode];
                                break;

                            case KindOfCGNode.Concept:
                                resultRelationInputNode = context.ConceptsDict[(ConceptCGNode)relationInputNode];
                                break;

                            default: throw new ArgumentOutOfRangeException(nameof(relationInputNodeKind), relationInputNodeKind, null);
                        }

#if DEBUG
                        //LogInstance.Log($"resultRelationInputNode = {resultRelationInputNode}");
#endif

                        if (relationStorage.ContainsRelation(resultRelationInputNode.Name, resultRelationItem.Name, resultItem.Name))
                        {
                            continue;
                        }

                        resultItem.AddInputNode(resultRelationItem);
                        resultRelationItem.AddInputNode(resultRelationInputNode);

                        relationStorage.AddRelation(resultRelationInputNode.Name, resultRelationItem.Name, resultItem.Name);
                    }
                }

                var outputsNodesList = sourceItem.Outputs.Where(p => sourceItems.Contains(p)).Select(p => (RelationCGNode)p).ToList();

#if DEBUG
                //LogInstance.Log($"outputsNodesList.Count = {outputsNodesList.Count}");
#endif

                foreach(var outputNode in outputsNodesList)
                {
#if DEBUG
                    //LogInstance.Log($"outputNode = {outputNode}");
#endif

                    var resultRelationItem = context.RelationsDict[outputNode];

#if DEBUG
                    //LogInstance.Log($"resultRelationItem = {resultRelationItem}");
#endif

                    var relationsOutputsNodesList = outputNode.Outputs.Where(p => (p.Kind == KindOfCGNode.Concept || p.Kind == KindOfCGNode.Graph) && sourceItems.Contains(p)).Select(p => (BaseConceptCGNode)p).ToList();

#if DEBUG
                    //LogInstance.Log($"relationsOutputsNodesList.Count = {relationsOutputsNodesList.Count}");
#endif

                    foreach(var relationOutputNode in relationsOutputsNodesList)
                    {
#if DEBUG
                        //LogInstance.Log($"relationOutputNode = {relationOutputNode}");
#endif
                        BaseInternalConceptCGNode resultRelationOutputNode = null;

                        var relationOutputNodeKind = relationOutputNode.Kind;

                        switch(relationOutputNodeKind)
                        {
                            case KindOfCGNode.Graph:
                                resultRelationOutputNode = context.ConceptualGraphsDict[(ConceptualGraph)relationOutputNode];
                                break;

                            case KindOfCGNode.Concept:
                                resultRelationOutputNode = context.ConceptsDict[(ConceptCGNode)relationOutputNode];
                                break;


                            default: throw new ArgumentOutOfRangeException(nameof(relationOutputNodeKind), relationOutputNodeKind, null);
                        }

#if DEBUG
                        //LogInstance.Log($"resultRelationOutputNode = {resultRelationOutputNode}");
#endif

                        if(relationStorage.ContainsRelation(resultItem.Name, resultRelationItem.Name, resultRelationOutputNode.Name))
                        {
                            continue;
                        }

                        resultItem.AddOutputNode(resultRelationItem);
                        resultRelationItem.AddOutputNode(resultRelationOutputNode);

                        relationStorage.AddRelation(resultItem.Name, resultRelationItem.Name, resultRelationOutputNode.Name);
                    }
                }
            }
        }

        private static List<BaseCGNode> GetNotDirectlyClonedNodesList(List<BaseCGNode> entitiesConditionsMarksRelationsList)
        {
            var notDirectlyClonedNodesList = new List<BaseCGNode>();

            foreach (var entityConditionMarkRelation in entitiesConditionsMarksRelationsList)
            {
#if DEBUG
                //LogInstance.Log($"entityConditionMarkRelation = {entityConditionMarkRelation}");
#endif

                notDirectlyClonedNodesList.Add(entityConditionMarkRelation);

                var firstOrdersRelationsList = entityConditionMarkRelation.Outputs.Where(p => p.Kind == KindOfCGNode.Relation).Select(p => (RelationCGNode)p).ToList();

#if DEBUG
                //LogInstance.Log($"firstOrdersRelationsList.Count = {firstOrdersRelationsList.Count}");
#endif

                foreach (var firstOrderRelation in firstOrdersRelationsList)
                {
#if DEBUG
                    //LogInstance.Log($"firstOrderRelation = {firstOrderRelation}");
#endif

                    if (!notDirectlyClonedNodesList.Contains(firstOrderRelation))
                    {
                        notDirectlyClonedNodesList.Add(firstOrderRelation);
                    }

                    var inputConceptsList = firstOrderRelation.Inputs.Where(p => p.Kind == KindOfCGNode.Concept).Select(p => (ConceptCGNode)p).ToList();

#if DEBUG
                    //LogInstance.Log($"inputConceptsList.Count = {inputConceptsList.Count}");
#endif

                    foreach (var inputConcept in inputConceptsList)
                    {
#if DEBUG
                        //LogInstance.Log($"inputConcept = {inputConcept}");
#endif

                        if (!notDirectlyClonedNodesList.Contains(inputConcept))
                        {
                            notDirectlyClonedNodesList.Add(inputConcept);
                        }
                    }

                    var outputConceptsList = firstOrderRelation.Outputs.Where(p => p.Kind == KindOfCGNode.Concept).Select(p => (ConceptCGNode)p).ToList();

#if DEBUG
                    //LogInstance.Log($"outputConceptsList.Count = {outputConceptsList.Count}");
#endif

                    foreach (var outputConcept in outputConceptsList)
                    {
#if DEBUG
                        //LogInstance.Log($"outputConcept = {outputConcept}");
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
                //LogInstance.Log($"currentTargetNodesList.Count = {currentTargetNodesList.Count} n = {n}");
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
            //LogInstance.Log($"targetNode = {targetNode}");
#endif

            if(result.Contains(targetNode))
            {
                return;
            }

            result.Add(targetNode);

            var inputNodesList = targetNode.Inputs;

#if DEBUG
            //LogInstance.Log($"inputNodesList.Count = {inputNodesList.Count}");
#endif

            if(inputNodesList.Count > 0)
            {
                var tmpNodesList = inputNodesList.Where(p => source.Contains(p)).ToList();

#if DEBUG
                //LogInstance.Log($"tmpNodesList.Count = {tmpNodesList.Count}");
#endif

                foreach (var tmpNode in tmpNodesList)
                {
                    NGetLinkedNodes(source, tmpNode, ref result);
                }
            }

            var outputNodesList = targetNode.Outputs;

#if DEBUG
            //LogInstance.Log($"outputNodesList.Count = {outputNodesList.Count}");
#endif

            if(outputNodesList.Count > 0)
            {
                var tmpNodesList = outputNodesList.Where(p => source.Contains(p)).ToList();

#if DEBUG
                //LogInstance.Log($"tmpNodesList.Count = {tmpNodesList.Count}");
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
            //LogInstance.Log($"childrenList.Count = {childrenList.Count}");
#endif

            foreach (var child in childrenList)
            {
#if DEBUG
                //LogInstance.Log($"child = {child}");
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
            //LogInstance.Log($"outputNodesGroupedDict.Count = {outputNodesGroupedDict.Count}");
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
                            //LogInstance.Log($"relation = {relation}");
                            //LogInstance.Log($"outputNodeOfTheRelation = {outputNodeOfTheRelation}");
#endif

                            var aspect = GrammaticalElementsHeper.GetAspectFromName(outputNodeOfTheRelation.Name);

#if DEBUG
                            //LogInstance.Log($"aspect = {aspect}");
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
                            //LogInstance.Log($"relation = {relation}");
                            //LogInstance.Log($"outputNodeOfTheRelation = {outputNodeOfTheRelation}");
#endif

                            var tense = GrammaticalElementsHeper.GetTenseFromName(outputNodeOfTheRelation.Name);

#if DEBUG
                            //LogInstance.Log($"tense = {tense}");
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
                            //LogInstance.Log($"relation = {relation}");
                            //LogInstance.Log($"outputNodeOfTheRelation = {outputNodeOfTheRelation}");
#endif

                            var voice = GrammaticalElementsHeper.GetVoiceFromName(outputNodeOfTheRelation.Name);

#if DEBUG
                            //LogInstance.Log($"voice = {voice}");
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
                            //LogInstance.Log($"relation = {relation}");
                            //LogInstance.Log($"outputNodeOfTheRelation = {outputNodeOfTheRelation}");
#endif

                            var mood = GrammaticalElementsHeper.GetMoodFromName(outputNodeOfTheRelation.Name);

#if DEBUG
                            //LogInstance.Log($"mood = {mood}");
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
                            //LogInstance.Log($"relation = {relation}");
                            //LogInstance.Log($"outputNodeOfTheRelation = {outputNodeOfTheRelation}");
#endif

                            var modal = GrammaticalElementsHeper.GetModalFromName(outputNodeOfTheRelation.Name);

#if DEBUG
                            //LogInstance.Log($"modal = {modal}");
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
            //LogInstance.Log($"source = {source}");
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

            result.KindOfGraphOrConcept = KindOfInternalGraphOrConceptNode.Concept;

            FillName(source, result, context);

            return result;
        }

        private static InternalRelationCGNode ConvertRelation(RelationCGNode source, InternalConceptualGraph targetParent, ContextOfConvertingCGToInternal context)
        {
#if DEBUG
            //LogInstance.Log($"source = {source}");
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
