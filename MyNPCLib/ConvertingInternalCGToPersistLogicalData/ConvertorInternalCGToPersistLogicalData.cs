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

            var part = new RulePart();
            dest.Part_1 = part;
            part.Parent = dest;
            part.IsActive = true;

            var expression = CreateExpressionByWholeGraph(source, context);

#if DEBUG
            LogInstance.Log($"expression = {expression}");
#endif

            part.Expression = expression;
        }

        private static BaseExpressionNode CreateExpressionByWholeGraph(InternalConceptualGraph source, ContextOfConvertingInternalCGToPersistLogicalData context)
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
                return CreateExpressionByRelation(relationsList.Single(), context);
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

                var relationExpr = CreateExpressionByRelation(relation, context);

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

        private static BaseExpressionNode CreateExpressionByRelation(InternalRelationCGNode relation, ContextOfConvertingInternalCGToPersistLogicalData context)
        {
#if DEBUG
            LogInstance.Log($"relation = {relation}");
#endif

            var relationExpr = new RelationExpressionNode();
            relationExpr.Name = relation.Name;
            relationExpr.Params = new List<BaseExpressionNode>();

            var inputNode = relation.Inputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();

#if DEBUG
            LogInstance.Log($"inputNode = {inputNode}");
#endif
            var inputNodeExpr = CreateExpressionByGraphOrConceptNode(inputNode, context);


            relationExpr.Params.Add(inputNodeExpr);

            var outputNode = relation.Outputs.Where(p => p.IsGraphOrConceptNode).Select(p => p.AsGraphOrConceptNode).FirstOrDefault();

#if DEBUG
            LogInstance.Log($"outputNode = {outputNode}");
#endif

            var outputNodeExpr = CreateExpressionByGraphOrConceptNode(outputNode, context);

            relationExpr.Params.Add(outputNodeExpr);

            //throw new NotImplementedException();

            return relationExpr;
        }

        private static BaseExpressionNode CreateExpressionByGraphOrConceptNode(BaseInternalConceptCGNode graphOrConcept, ContextOfConvertingInternalCGToPersistLogicalData context)
        {
#if DEBUG
            LogInstance.Log($"graphOrConcept = {graphOrConcept}");
#endif

            var kindOfGraphOrConcept = graphOrConcept.KindOfGraphOrConcept;

            switch(kindOfGraphOrConcept)
            {
                default: throw new ArgumentOutOfRangeException(nameof(kindOfGraphOrConcept), kindOfGraphOrConcept, null);
            }
        }
    }
}
