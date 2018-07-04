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

            while(IsWrapperGraph(source))
            {
                source = (ConceptualGraph)source.Children.SingleOrDefault(p => p.Kind == KindOfCGNode.Graph);
            }

#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            var context = new ContextOfConvertingCGToInternal();
            context.EntityDictionary = entityDictionary;
            return ConvertConceptualGraph(source, context);
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

        private static InternalConceptualGraph ConvertConceptualGraph(ConceptualGraph source, ContextOfConvertingCGToInternal context)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            if (context.ConceptualGraphsDict.ContainsKey(source))
            {
                return context.ConceptualGraphsDict[source];
            }

            var result = new InternalConceptualGraph();

            context.ConceptualGraphsDict[source] = result;

            if(source.Parent != null)
            {
                var parentForResult = ConvertConceptualGraph(source.Parent, context);
                result.Parent = parentForResult;
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

            foreach(var child in childrenList)
            {
#if DEBUG
                LogInstance.Log($"child = {child}");
#endif

                var kind = child.Kind;

                switch (kind)
                {
                    case KindOfCGNode.Graph:
                        ConvertConceptualGraph((ConceptualGraph)child, context);
                        break;

                    case KindOfCGNode.Concept:
                        ConvertConcept((ConceptCGNode)child, context);
                        break;

                    case KindOfCGNode.Relation:
                        ConvertRelation((RelationCGNode)child, context);
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

        private static InternalConceptCGNode ConvertConcept(ConceptCGNode source, ContextOfConvertingCGToInternal context)
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

            var parentForResult = ConvertConceptualGraph(source.Parent, context);
            result.Parent = parentForResult;

            FillName(source, result, context);

            return result;
        }

        private static InternalRelationCGNode ConvertRelation(RelationCGNode source, ContextOfConvertingCGToInternal context)
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

            var parentForResult = ConvertConceptualGraph(source.Parent, context);
            result.Parent = parentForResult;

            FillName(source, result, context);

            return result;
        }
    }
}
