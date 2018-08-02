using MyNPCLib.CG;
using MyNPCLib.CommonServiceGrammaticalElements;
using MyNPCLib.NLToCGParsing.DependencyTree;
using MyNPCLib.NLToCGParsing.PhraseTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class VerbPhraseNodeOfSemanticAnalyzer : BaseNodeOfSemanticAnalyzer
    {
        public VerbPhraseNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context, VerbDTNode verbPhrase)
            : base(context)
        {
            mVerbPhrase = verbPhrase;
        }

        private VerbDTNode mVerbPhrase;
        private ConceptCGNode mConcept;
        
        public ResultOfNodeOfSemanticAnalyzer Run()
        {
#if DEBUG
            //LogInstance.Log($"mVerbPhrase = {mVerbPhrase}");
#endif

            var result = new ResultOfNodeOfSemanticAnalyzer();
            var resultPrimaryRolesDict = result.PrimaryRolesDict;
            var resultSecondaryRolesDict = result.SecondaryRolesDict;
            var verb = mVerbPhrase.ExtendedToken;
            var conceptualGraph = Context.ConceptualGraph;
            mConcept = new ConceptCGNode();
            result.RootConcept = mConcept;
            mConcept.Parent = conceptualGraph;

            mConcept.Name = GetName(verb);

#if DEBUG
            //LogInstance.Log($"verb = {verb}");
#endif

            var verbsRolesStorage = new RolesStorageOfSemanticAnalyzer();

            var verbFullLogicalMeaning = verb.FullLogicalMeaning;

            foreach (var logicalMeaning in verbFullLogicalMeaning)
            {
#if DEBUG
                //LogInstance.Log($"logicalMeaning = {logicalMeaning}");
#endif

                PrimaryRolesDict.Add(logicalMeaning, mConcept);
                resultPrimaryRolesDict.Add(logicalMeaning, mConcept);
                verbsRolesStorage.Add(logicalMeaning, mConcept);
            }

            if (verbFullLogicalMeaning.IsEmpty())
            {
                return result;
            }

#if DEBUG
            //LogInstance.Log($"verbsRolesStorage = {verbsRolesStorage}");
#endif

            var nounSubjectsList = mVerbPhrase.NounSubjectsList;

            if(nounSubjectsList.IsEmpty())
            {
                var isAct = verb.FullLogicalMeaning.Contains("act");
                var isState = verb.FullLogicalMeaning.Contains("state");

#if DEBUG
                //LogInstance.Log($"isAct = {isAct}");
                //LogInstance.Log($"isState = {isState}");
#endif

                if(isAct)
                {
                    var relationName = SpecialNamesOfRelations.ActionRelationName;
                    var relation = new RelationCGNode();
                    relation.Parent = conceptualGraph;
                    relation.Name = relationName;

                    relation.AddOutputNode(mConcept);
                }
                else
                {
                    if(isState)
                    {
                        var relationName = SpecialNamesOfRelations.StateRelationName;
                        var relation = new RelationCGNode();
                        relation.Parent = conceptualGraph;
                        relation.Name = relationName;

                        relation.AddOutputNode(mConcept);
                    }
                }
            }
            else
            {
                var subjectsRolesStorage = new RolesStorageOfSemanticAnalyzer();

#if DEBUG
                //LogInstance.Log($"nounSubjectsList.Count = {nounSubjectsList.Count}");
#endif

                foreach (var nounSubject in nounSubjectsList)
                {
#if DEBUG
                    //LogInstance.Log($"nounSubject = {nounSubject}");
#endif

                    var nounPhraseNode = new NounPhraseNodeOfSemanticAnalyzer(Context, nounSubject);
                    var nounResult = nounPhraseNode.Run();

#if DEBUG
                    //LogInstance.Log($"nounResult = {nounResult}");
#endif

                    PrimaryRolesDict.Assing(nounResult.PrimaryRolesDict);
                    subjectsRolesStorage.Assing(nounResult.PrimaryRolesDict);
                }

                var verbAndSubjectsMixRolesStorage = new RolesStorageOfSemanticAnalyzer();
                verbAndSubjectsMixRolesStorage.Assing(subjectsRolesStorage);
                verbAndSubjectsMixRolesStorage.Assing(verbsRolesStorage);

#if DEBUG
                //LogInstance.Log($"subjectsRolesStorage = {subjectsRolesStorage}");
                //LogInstance.Log($"verbAndSubjectsMix = {verbAndSubjectsMixRolesStorage}");
#endif

                var primaryAnimatesList = verbAndSubjectsMixRolesStorage.GetByRole("animate");

                if (!primaryAnimatesList.IsEmpty())
                {
                    //state -> experiencer -> animate

                    var primaryStatesList = verbAndSubjectsMixRolesStorage.GetByRole("state");

                    if (!primaryStatesList.IsEmpty())
                    {
                        foreach (var state in primaryStatesList)
                        {
                            foreach (var animate in primaryAnimatesList)
                            {
                                CreateExperiencerRelation(state, animate);
                                CreateStateRelation(state, animate);
                            }
                        }
                    }
                    //act -> agent -> animate

                    var primaryActsList = verbAndSubjectsMixRolesStorage.GetByRole("act");

                    if (!primaryActsList.IsEmpty())
                    {
                        foreach (var act in primaryActsList)
                        {
                            foreach (var animate in primaryAnimatesList)
                            {
                                CreateAgentRelation(act, animate);
                                CreateActionRelation(act, animate);
                            }
                        }
                    }
                }
            }

#if DEBUG
            //LogInstance.Log($"PrimaryRolesDict (1) = {PrimaryRolesDict}");
#endif

            var nounObjectsList = mVerbPhrase.NounObjectsList;
            var prepositionalObjectsList = mVerbPhrase.PrepositionalObjectsList;

            if(!nounObjectsList.IsEmpty() || !prepositionalObjectsList.IsEmpty())
            {
                var objectsRolesStorage = new RolesStorageOfSemanticAnalyzer();

                if (!nounObjectsList.IsEmpty())
                {
#if DEBUG
                    //LogInstance.Log($"nounObjectsList.Count = {nounObjectsList.Count}");
#endif

                    foreach (var nounObject in nounObjectsList)
                    {
#if DEBUG
                        //LogInstance.Log($"nounObject = {nounObject}");
#endif

                        var nounPhraseNode = new NounPhraseNodeOfSemanticAnalyzer(Context, nounObject);
                        var nounResult = nounPhraseNode.Run();

#if DEBUG
                        //LogInstance.Log($"nounResult = {nounResult}");
#endif

                        //PrimaryRolesDict.Assing(nounResult.PrimaryRolesDict);
                        objectsRolesStorage.Assing(nounResult.PrimaryRolesDict);
                    }
                }

                if(!prepositionalObjectsList.IsEmpty())
                {
#if DEBUG
                    //LogInstance.Log($"prepositionalObjectsList.Count = {prepositionalObjectsList.Count}");
#endif

                    var isMoving = verb.FullLogicalMeaning.Contains("moving");

#if DEBUG
                    //LogInstance.Log($"isMoving = {isMoving}");
#endif

                    foreach (var prepositional in prepositionalObjectsList)
                    {
#if DEBUG
                        //LogInstance.Log($"prepositional = {prepositional}");
#endif

                        var isTo = prepositional.ExtendedToken.RootWord == "to";

#if DEBUG
                        //LogInstance.Log($"isTo = {isTo}");
#endif

                        if(isTo && isMoving)
                        {
                            var nounOfPrepositional = prepositional.NounObject;

                            var nodeOfNounOfPrepositional = new NounPhraseNodeOfSemanticAnalyzer(Context, nounOfPrepositional);
                            var nounOfPrepositionalResult = nodeOfNounOfPrepositional.Run();

#if DEBUG
                            //LogInstance.Log($"nounOfPrepositionalResult = {nounOfPrepositionalResult}");
#endif

                            var phisobjList = nounOfPrepositionalResult.PrimaryRolesDict.GetByRole("phisobj");

#if DEBUG
                            //LogInstance.Log($"phisobjList.Count = {phisobjList.Count}");
#endif

                            foreach(var phisobj in phisobjList)
                            {
#if DEBUG
                                //LogInstance.Log($"phisobj = {phisobj}");
#endif

                                var directionRelation = new RelationCGNode();
                                directionRelation.Parent = conceptualGraph;
                                directionRelation.Name = "direction";

                                directionRelation.AddInputNode(mConcept);
                                directionRelation.AddOutputNode(phisobj);
                            }
                        }
                    }
                }

                var verbAndObjectsMixRolesStorage = new RolesStorageOfSemanticAnalyzer();
                verbAndObjectsMixRolesStorage.Assing(objectsRolesStorage);
                verbAndObjectsMixRolesStorage.Assing(verbsRolesStorage);

#if DEBUG
                //LogInstance.Log($"objectsRolesStorage = {objectsRolesStorage}");
                //LogInstance.Log($"verbAndObjectsMix = {verbAndObjectsMixRolesStorage}");
#endif

                if (verbFullLogicalMeaning.Contains("event") || verbFullLogicalMeaning.Contains("state"))
                {
                    var entitiesList = verbAndObjectsMixRolesStorage.GetByRole("entity");

                    if (!entitiesList.IsEmpty())
                    {
                        foreach (var entityConcept in entitiesList)
                        {
#if DEBUG
                            //LogInstance.Log($"entityConcept = {entityConcept}");
#endif

                            CreateObjectRelation(mConcept, entityConcept);
                        }
                    }
                }
            }

#if DEBUG
            //LogInstance.Log($"PrimaryRolesDict (2) = {PrimaryRolesDict}");
#endif

#if DEBUG
            //LogInstance.Log("End");
#endif

            return result;
        }

        private void CreateObjectRelation(ConceptCGNode verbConcept, ConceptCGNode objectConcept)
        {
            var relationName = SpecialNamesOfRelations.ObjectRelationName;

            if (Context.RelationStorage.ContainsRelation(verbConcept.Name, objectConcept.Name, relationName))
            {
                return;
            }

            var conceptualGraph = Context.ConceptualGraph;

            var relation = new RelationCGNode();
            relation.Parent = conceptualGraph;
            relation.Name = relationName;

            verbConcept.AddOutputNode(relation);
            relation.AddOutputNode(objectConcept);

            Context.RelationStorage.AddRelation(verbConcept.Name, objectConcept.Name, relationName);
        }

        private void CreateAgentRelation(ConceptCGNode verbConcept, ConceptCGNode nounConcept)
        {
            var relationName = SpecialNamesOfRelations.AgentRelationName;

            if (Context.RelationStorage.ContainsRelation(verbConcept.Name, nounConcept.Name, relationName))
            {
                return;
            }

            var conceptualGraph = Context.ConceptualGraph;

            var relation = new RelationCGNode();
            relation.Parent = conceptualGraph;
            relation.Name = relationName;

            verbConcept.AddOutputNode(relation);
            relation.AddOutputNode(nounConcept);

            Context.RelationStorage.AddRelation(verbConcept.Name, nounConcept.Name, relationName);
        }

        private void CreateActionRelation(ConceptCGNode verbConcept, ConceptCGNode nounConcept)
        {
            var relationName = SpecialNamesOfRelations.ActionRelationName;

            if (Context.RelationStorage.ContainsRelation(nounConcept.Name, verbConcept.Name, relationName))
            {
                return;
            }

            var conceptualGraph = Context.ConceptualGraph;

            var relation = new RelationCGNode();
            relation.Parent = conceptualGraph;
            relation.Name = relationName;

            nounConcept.AddOutputNode(relation);
            relation.AddOutputNode(verbConcept);

            Context.RelationStorage.AddRelation(nounConcept.Name, verbConcept.Name, relationName);
        }

        private void CreateExperiencerRelation(ConceptCGNode verbConcept, ConceptCGNode nounConcept)
        {
#if DEBUG
            //LogInstance.Log($"verbConcept = {verbConcept}");
            //LogInstance.Log($"nounConcept = {nounConcept}");
#endif

            var relationName = SpecialNamesOfRelations.ExperiencerRelationName;

            if (Context.RelationStorage.ContainsRelation(verbConcept.Name, nounConcept.Name, relationName))
            {
                return;
            }

            var conceptualGraph = Context.ConceptualGraph;

            var relation = new RelationCGNode();
            relation.Parent = conceptualGraph;
            relation.Name = relationName;

            verbConcept.AddOutputNode(relation);
            relation.AddOutputNode(nounConcept);

            Context.RelationStorage.AddRelation(verbConcept.Name, nounConcept.Name, relationName);
        }

        private void CreateStateRelation(ConceptCGNode verbConcept, ConceptCGNode nounConcept)
        {
            var relationName = SpecialNamesOfRelations.StateRelationName;

            if (Context.RelationStorage.ContainsRelation(nounConcept.Name, verbConcept.Name, relationName))
            {
                return;
            }

            var conceptualGraph = Context.ConceptualGraph;

            var relation = new RelationCGNode();
            relation.Parent = conceptualGraph;
            relation.Name = relationName;

            nounConcept.AddOutputNode(relation);
            relation.AddOutputNode(verbConcept);

            Context.RelationStorage.AddRelation(verbConcept.Name, nounConcept.Name, relationName);
        }
    }
}
