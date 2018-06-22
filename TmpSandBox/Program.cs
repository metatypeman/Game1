using MyNPCLib;
using MyNPCLib.CG;
using MyNPCLib.CGStorage;
using MyNPCLib.ConvertingPersistLogicalDataToIndexing;
using MyNPCLib.DebugHelperForPersistLogicalData;
using MyNPCLib.Dot;
using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.Logical;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.Parser;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.PersistLogicalDataStorage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TmpSandBox.NPCBehaviour;
using TmpSandBox.TSTConceptualGraphs;

namespace TmpSandBox
{
    class Program
    {
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"CurrentDomain_UnhandledException e.ExceptionObject = {e.ExceptionObject}");
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; 

            var logProxy = new LogProxyForNLog();
            LogInstance.SetLogProxy(logProxy);

            TSTRuleInstance();
            //TSTEntityLogging();
            //TSTConceptualGraph_2();
            //TSTConceptualGraphs();
            //TSTRange();
            //TSTLexer();
            //TSTLogicalAST();
            //TSTCancelTask_2();
            //TSTCancelTask();
            //TSTMyNPCContext();
            //TSTStorageOfNPCProcesses();
            //TSTActivatorOfNPCProcessEntryPointInfo();
            //CreateContextAndProcessesCase1();
            //CreateInfoOfConcreteProcess();
        }

        private static void TSTRuleInstance()
        {
            LogInstance.Log("Begin");

            var globalEntityDictionary = new EntityDictionary();

            LogInstance.Log($"globalEntityDictionary.Name = {globalEntityDictionary.Name}");

            var context = new ContextOfCGStorage(globalEntityDictionary);
            context.Init();

            var commonPersistLogicalData = new CommonPersistLogicalData();
            commonPersistLogicalData.DictionaryName = globalEntityDictionary.Name;
            commonPersistLogicalData.RuleInstancesList = new List<RuleInstance>();

            var commonPersistIndexedLogicalData = new CommonPersistIndexedLogicalData();

            commonPersistIndexedLogicalData.IndexedRuleInstancesDict = new Dictionary<ulong, IndexedRuleInstance>();

            //var exampleRuleInstance = CreateFirstRuleInstance(globalEntityDictionary);

            //LogInstance.Log($"ruleInstance = {exampleRuleInstance}");

            //var debugStr = DebugHelperForRuleInstance.ToString(exampleRuleInstance);

            //LogInstance.Log($"debugStr = {debugStr}");

            //var indexedExampleRuleInstance = ConvertorToIndexed.ConvertRuleInstance(exampleRuleInstance);

            //LogInstance.Log($"indexedExampleRuleInstance = {indexedExampleRuleInstance}");

            //var ruleInstance = CreateFirstRuleInstance(globalEntityDictionary);
            var ruleInstance = CreateSimpleRule(globalEntityDictionary);

            //LogInstance.Log($"ruleInstance = {ruleInstance}");

            var debugStr = DebugHelperForRuleInstance.ToString(ruleInstance);

            LogInstance.Log($"debugStr = {debugStr}");

            var indexedRuleInstance = ConvertorToIndexed.ConvertRuleInstance(ruleInstance);

            context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedRuleInstance);

            var factInstance = CreateSimpleFact(globalEntityDictionary);
            commonPersistLogicalData.RuleInstancesList.Add(factInstance);

            //LogInstance.Log($"factInstance = {factInstance}");

            debugStr = DebugHelperForRuleInstance.ToString(factInstance);

            LogInstance.Log($"debugStr = {debugStr}");

            var indexedFactInstance = ConvertorToIndexed.ConvertRuleInstance(factInstance);

            commonPersistIndexedLogicalData.IndexedRuleInstancesDict[indexedFactInstance.Key] = indexedFactInstance;

            context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedFactInstance);

            //LogInstance.Log($"indexedFactInstance = {indexedFactInstance}");

            var fact_2 = CreateSimpleFact_2(globalEntityDictionary);

            debugStr = DebugHelperForRuleInstance.ToString(fact_2);

            LogInstance.Log($"debugStr = {debugStr}");

            var indexedFact_2 = ConvertorToIndexed.ConvertRuleInstance(fact_2);

            context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedFact_2);

            var fact_2_2 = CreateSimpleFact_2_2(globalEntityDictionary);

            debugStr = DebugHelperForRuleInstance.ToString(fact_2_2);

            LogInstance.Log($"debugStr = {debugStr}");

            var indexedFact_2_2 = ConvertorToIndexed.ConvertRuleInstance(fact_2_2);

            context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedFact_2_2);

            var fact_3 = CreateSimpleFact_3(globalEntityDictionary);

            debugStr = DebugHelperForRuleInstance.ToString(fact_3);

            LogInstance.Log($"debugStr = {debugStr}");

            var indexedFact_3 = ConvertorToIndexed.ConvertRuleInstance(fact_3);

            context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedFact_3);

            var fact_3_2 = CreateSimpleFact_3_2(globalEntityDictionary);

            debugStr = DebugHelperForRuleInstance.ToString(fact_3_2);

            LogInstance.Log($"debugStr = {debugStr}");

            var indexedFact_3_2 = ConvertorToIndexed.ConvertRuleInstance(fact_3_2);

            context.GlobalCGStorage.NSetIndexedRuleInstanceToIndexData(indexedFact_3_2);

            var query = CreateSimpleQuery(globalEntityDictionary);

            //LogInstance.Log($"query = {query}");

            debugStr = DebugHelperForRuleInstance.ToString(query);

            LogInstance.Log($"debugStr = {debugStr}");

            var indexedQuery = ConvertorToIndexed.ConvertRuleInstance(query);

            var searcher = new LogicalSearcher(context);

            var searchOptions = new LogicalSearchOptions();
            var globalStorageOptions = new SettingsOfStorageForSearchingInThisSession();
            globalStorageOptions.Storage = context.GlobalCGStorage;
            globalStorageOptions.MaxDeph = null;
            globalStorageOptions.UseFacts = true;
            globalStorageOptions.UseProductions = true;
            globalStorageOptions.Priority = 1;

            searchOptions.DataSourcesSettings = new List<SettingsOfStorageForSearchingInThisSession>() { globalStorageOptions };

            searchOptions.QueryExpression = indexedQuery;

            var rearchResult = searcher.Run(searchOptions);

            //LogInstance.Log($"rearchResult = {rearchResult}");

            var targetSearchResultItemsList = rearchResult.Items;

            foreach(var targetSearchResultItem in targetSearchResultItemsList)
            {
                var completeFoundRuleInstance = targetSearchResultItem.RuleInstance;

                //LogInstance.Log($"completeFoundRuleInstance = {completeFoundRuleInstance}");

                debugStr = DebugHelperForRuleInstance.ToString(completeFoundRuleInstance);

                LogInstance.Log($"debugStr = {debugStr}");
            }

            LogInstance.Log("End");
        }

        private static RuleInstance CreateSimpleRule(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Rule;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            var rulePart_2 = new RulePart();
            rulePart_2.Parent = ruleInstance;
            ruleInstance.Part_2 = rulePart_2;

            rulePart_1.NextPart = rulePart_2;
            rulePart_2.NextPart = rulePart_1;

            rulePart_1.IsActive = true;
            rulePart_2.IsActive = true;

            var expr_1_1 = new RelationExpressionNode();
            rulePart_1.Expression = expr_1_1;
            expr_1_1.Params = new List<BaseExpressionNode>();
            expr_1_1.Name = "son";
            expr_1_1.Key = globalEntityDictionary.GetKey(expr_1_1.Name);

            var param_1_1_1 = new VarExpressionNode();
            expr_1_1.Params.Add(param_1_1_1);
            param_1_1_1.Name = "@X";
            param_1_1_1.Key = globalEntityDictionary.GetKey(param_1_1_1.Name);

            var param_1_1_2 = new VarExpressionNode();
            expr_1_1.Params.Add(param_1_1_2);
            param_1_1_2.Name = "@Y";
            param_1_1_2.Key = globalEntityDictionary.GetKey(param_1_1_2.Name);

            var expr_2_1 = new OperatorAndExpressionNode();
            rulePart_2.Expression = expr_2_1;

            var expr_2_2 = new RelationExpressionNode();
            expr_2_1.Left = expr_2_2;
            expr_2_2.Params = new List<BaseExpressionNode>();
            expr_2_2.Name = "parent";
            expr_2_2.Key = globalEntityDictionary.GetKey(expr_2_2.Name);

            var param_2_2_1 = new VarExpressionNode();
            expr_2_2.Params.Add(param_2_2_1);
            param_2_2_1.Name = "@Y";
            param_2_2_1.Key = globalEntityDictionary.GetKey(param_2_2_1.Name);

            var param_2_2_2 = new VarExpressionNode();
            expr_2_2.Params.Add(param_2_2_2);
            param_2_2_2.Name = "@X";
            param_2_2_2.Key = globalEntityDictionary.GetKey(param_2_2_2.Name);

            var expr_2_3 = new RelationExpressionNode();
            expr_2_1.Right = expr_2_3;
            expr_2_3.Params = new List<BaseExpressionNode>();
            expr_2_3.Name = "male";
            expr_2_3.Key = globalEntityDictionary.GetKey(expr_2_3.Name);

            var param_2_3_1 = new VarExpressionNode();
            expr_2_3.Params.Add(param_2_3_1);
            param_2_3_1.Name = "@X";
            param_2_3_1.Key = globalEntityDictionary.GetKey(param_2_3_1.Name);

            return ruleInstance;
        }

        private static RuleInstance CreateSimpleFact(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();

            var relationName = "son";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#Piter";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new EntityRefExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "#Tom";
            param_2.Key = globalEntityDictionary.GetKey(param_2.Name);

            //son(#Piter,#Tom)

            return ruleInstance;
        }

        private static RuleInstance CreateSimpleFact_2(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();

            var relationName = "parent";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#Tom";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new EntityRefExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "#John";
            param_2.Key = globalEntityDictionary.GetKey(param_2.Name);

            //parent(#Tom, #John)

            return ruleInstance;
        }

        private static RuleInstance CreateSimpleFact_2_2(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();

            var relationName = "parent";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#George";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new EntityRefExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "#Bob";
            param_2.Key = globalEntityDictionary.GetKey(param_2.Name);

            //parent(#George, #Bob)

            return ruleInstance;
        }

        private static RuleInstance CreateSimpleFact_3(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();

            var relationName = "male";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#John";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            //male(#John)

            return ruleInstance;
        }

        private static RuleInstance CreateSimpleFact_3_2(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.Fact;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();

            var relationName = "male";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#Bob";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            //male(#Bob)

            return ruleInstance;
        }

        private static RuleInstance CreateSimpleQuery(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Kind = KindOfRuleInstance.QuestionVars;
            ruleInstance.Name = NamesHelper.CreateEntityName();
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            rulePart_1.IsActive = true;

            var expr3 = new RelationExpressionNode();
            rulePart_1.Expression = expr3;
            expr3.Params = new List<BaseExpressionNode>();

            var relationName = "son";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new QuestionVarExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "?X";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new EntityRefExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "#Tom";
            param_2.Key = globalEntityDictionary.GetKey(param_2.Name);

            //son(Piter,$X1)

            return ruleInstance;
        }

        private static RuleInstance CreateFirstRuleInstance(IEntityDictionary globalEntityDictionary)
        {
            var ruleInstance = new RuleInstance();
            ruleInstance.DictionaryName = globalEntityDictionary.Name;
            ruleInstance.Name = "#1";
            ruleInstance.Key = globalEntityDictionary.GetKey(ruleInstance.Name);
            ruleInstance.ModuleName = "#simple_module";
            ruleInstance.ModuleKey = globalEntityDictionary.GetKey(ruleInstance.ModuleName);

            var belongToEntityExpression = new EntityRefExpressionNode();
            ruleInstance.BelongToEntity = belongToEntityExpression;
            belongToEntityExpression.Name = "cat";
            belongToEntityExpression.Key = globalEntityDictionary.GetKey(belongToEntityExpression.Name);

            var entitiesConditions = new EntitiesConditions();
            ruleInstance.EntitiesConditions = entitiesConditions;
            entitiesConditions.Items = new List<EntityConditionItem>();

            var entityCondition_1 = new EntityConditionItem();
            entitiesConditions.Items.Add(entityCondition_1);
            entityCondition_1.Name = "#123";
            entityCondition_1.Key = globalEntityDictionary.GetKey(entityCondition_1.Name);
            entityCondition_1.VariableName = "#@R";
            entityCondition_1.VariableKey = globalEntityDictionary.GetKey(entityCondition_1.VariableName);

            var entityCondition_2 = new EntityConditionItem();
            entitiesConditions.Items.Add(entityCondition_2);
            entityCondition_2.Name = "#124";
            entityCondition_2.Key = globalEntityDictionary.GetKey(entityCondition_2.Name);
            entityCondition_2.VariableName = "#@T";
            entityCondition_2.VariableKey = globalEntityDictionary.GetKey(entityCondition_2.VariableName);

            var variablesQuantification = new VariablesQuantificationPart();
            ruleInstance.VariablesQuantification = variablesQuantification;
            variablesQuantification.Items = new List<VarExpressionNode>();

            var varQuant_1 = new VarExpressionNode();
            varQuant_1.Quantifier = KindOfQuantifier.Universal;
            varQuant_1.Name = "@X";
            varQuant_1.Key = globalEntityDictionary.GetKey(varQuant_1.Name);
            variablesQuantification.Items.Add(varQuant_1);

            var varQuant_2 = new VarExpressionNode();
            varQuant_2.Quantifier = KindOfQuantifier.Universal;
            varQuant_2.Name = "@Z";
            varQuant_2.Key = globalEntityDictionary.GetKey(varQuant_2.Name);
            variablesQuantification.Items.Add(varQuant_2);

            var rulePart_1 = new RulePart();
            rulePart_1.Parent = ruleInstance;
            ruleInstance.Part_1 = rulePart_1;

            var rulePart_2 = new RulePart();
            rulePart_2.Parent = ruleInstance;
            ruleInstance.Part_2 = rulePart_2;

            rulePart_1.NextPart = rulePart_2;
            rulePart_2.NextPart = rulePart_1;

            rulePart_1.IsActive = true;
            rulePart_2.IsActive = true;

            var variablesQuantification_1 = new VariablesQuantificationPart();
            rulePart_1.VariablesQuantification = variablesQuantification_1;
            variablesQuantification_1.Items = new List<VarExpressionNode>();

            var varQuant_3 = new VarExpressionNode();
            varQuant_3.Quantifier = KindOfQuantifier.Existential;
            varQuant_3.Name = "@X";
            varQuant_3.Key = globalEntityDictionary.GetKey(varQuant_3.Name);
            variablesQuantification_1.Items.Add(varQuant_3);

            var expr_1 = new OperatorNotExpressionNode();
            rulePart_1.Expression = expr_1;
            var expr_2 = new OperatorAndExpressionNode();
            expr_1.Left = expr_2;
            var expr3 = new RelationExpressionNode();
            expr_2.Left = expr3;
            expr3.Params = new List<BaseExpressionNode>();

            var relationName = "isa";
            var relationKey = globalEntityDictionary.GetKey(relationName);
            expr3.Name = relationName;
            expr3.Key = relationKey;

            var param_1 = new EntityRefExpressionNode();
            expr3.Params.Add(param_1);
            param_1.Name = "#2";
            param_1.Key = globalEntityDictionary.GetKey(param_1.Name);

            var param_2 = new ConceptExpressionNode();
            expr3.Params.Add(param_2);
            param_2.Name = "dog";
            param_2.Key = globalEntityDictionary.GetKey(param_1.Name);

            var relation2 = new RelationExpressionNode();
            expr_2.Right = relation2;
            relation2.Params = new List<BaseExpressionNode>();
            relation2.Name = "smile";
            relation2.Key = globalEntityDictionary.GetKey(relation2.Name);

            var param_3 = new ConceptExpressionNode();
            relation2.Params.Add(param_3);
            param_3.Name = "tree";
            param_3.Key = globalEntityDictionary.GetKey(param_3.Name);

            var relationForPart_2 = new RelationExpressionNode();
            rulePart_2.Expression = relationForPart_2;
            relationForPart_2.Params = new List<BaseExpressionNode>();
            relationForPart_2.Name = "slow";
            relationForPart_2.Key = globalEntityDictionary.GetKey(relationForPart_2.Name);

            var param_4 = new ConceptExpressionNode();
            relationForPart_2.Params.Add(param_4);
            param_4.Name = "car";
            param_4.Key = globalEntityDictionary.GetKey(param_4.Name);

            var notContradictPart = new NotContradictPart();
            ruleInstance.NotContradict = notContradictPart;
            notContradictPart.Parent = ruleInstance;

            var study_hard_relation = new RelationExpressionNode();
            notContradictPart.Expression = study_hard_relation;
            study_hard_relation.Params = new List<BaseExpressionNode>();

            study_hard_relation.Name = "study_hard";
            study_hard_relation.Key = globalEntityDictionary.GetKey(study_hard_relation.Name);

            var param_5 = new VarExpressionNode();
            study_hard_relation.Params.Add(param_5);

            param_5.Name = "@X";
            param_5.Key = globalEntityDictionary.GetKey(param_5.Name);

            var cF = new CertaintyFactorFuzzyModality();
            ruleInstance.CertaintyFactor = cF;
            cF.Parent = ruleInstance;

            var cFExpression = new ValueExpressionNode();
            cF.Expression = cFExpression;
            cFExpression.Value = 0.5f;

            cF.Annotations = new List<LogicalAnnotation>();

            var annotationForCF_1 = new LogicalAnnotation();
            cF.Annotations.Add(annotationForCF_1);
            annotationForCF_1.Name = "#annotation_1";
            annotationForCF_1.Key = globalEntityDictionary.GetKey(annotationForCF_1.Name);

            var annotationForCF_2 = new LogicalAnnotation();
            cF.Annotations.Add(annotationForCF_2);
            annotationForCF_2.Name = "#annotation_2";
            annotationForCF_2.Key = globalEntityDictionary.GetKey(annotationForCF_2.Name);

            return ruleInstance;
        }

        [MethodForLoggingSupport]
        private static string GetClassFullName()
        {
            var className = string.Empty;
            Type declaringType = null;
            //var framesToSkip = 2;
            var framesToSkip = 0;

            while (true)
            {
                var frame = new StackFrame(framesToSkip, false);

                var method = frame.GetMethod();

                var attribute = method?.GetCustomAttribute<MethodForLoggingSupportAttribute>();

                declaringType = method?.DeclaringType;

                LogInstance.Log($"method?.Name = {method?.Name} framesToSkip = {framesToSkip}");
                LogInstance.Log($"declaringType?.FullName = {declaringType?.FullName} framesToSkip = {framesToSkip}");
                LogInstance.Log($"(attribute == null) = {attribute == null}");

                if (declaringType == null)
                {
                    break;
                }

                if (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                framesToSkip++;
                className = declaringType.FullName;
            }

            return className;
        }

        private static void TSTEntityLogging()
        {
            var tmpName = GetClassFullName();

            LogInstance.Log($"tmpName = {tmpName}");

            var tmpCallInfo = DiagnosticsHelper.GetNotLoggingSupportCallInfo();

            LogInstance.Log($"tmpCallInfo = {tmpCallInfo}");

            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");

            entityLogger.Log("1 :)");

            entityLogger.Enabled = true;

            entityLogger.Log("2 :)");
        }

        private static void TSTConceptualGraph_2()
        {
            LogInstance.Log("Begin");

            var globalEntityDictionary = new EntityDictionary();

            var context = new ContextOfCGStorage(globalEntityDictionary);
            context.Init();

            var graph = new ConceptualGraph();
            graph.Name = "#1";

            LogInstance.Log($"graph = {graph}");

            var graph_2 = new ConceptualGraph();
            graph_2.Name = "#2";

            LogInstance.Log($"graph_2 = {graph_2}");

            var concept = new ConceptCGNode();    
            concept.Name = "dog";

            LogInstance.Log($"concept = {concept}");

            concept.Parent = graph;

            var relation = new RelationCGNode();
            relation.Name = "color";
            relation.Parent = graph;

            var concept_2 = new ConceptCGNode();
            concept_2.Name = "black";
            concept_2.Parent = graph;

            relation.AddInputNode(concept);
            concept_2.AddInputNode(relation);

            LogInstance.Log($"concept = {concept}");
            LogInstance.Log($"relation = {relation}");
            LogInstance.Log($"concept_2 = {concept_2}");
            LogInstance.Log($"graph = {graph}");
            LogInstance.Log($"graph_2 = {graph_2}");

            var dotStr = DotConverter.ConvertToString(graph);

            LogInstance.Log($"dotStr = {dotStr}");

            LogInstance.Log("End");
        }

        private static void TSTConceptualGraphs()
        {
            LogInstance.Log("Begin");

            var parser = new TSTConceptualGraphParser();
            var globalStorage = new TSTGlobalLogicalStorage();

            LogInstance.Log($"globalStorage = {globalStorage}");

            var nlText = "Go to far waypoint.";

            LogInstance.Log($"TSTConceptualGraphs nlText = {nlText}");

            //I get a conceptual graph by some text of natural language.
            var graph = parser.Parse(nlText);

            LogInstance.Log($"graph = {graph}");

            //I get a query by the conceptual graph.
            //The query is a special storage which contains this conceptual graph.
            //We can get information about this conceptual graph by making queries to the storage.
            //The global storage is as parent for the storage.
            //So read-queries can get information what is related with the storage but it contains only in global storage.
            var queryStorage = globalStorage.Query(graph);

            LogInstance.Log($"queryStorage = {queryStorage}");

            //I get a conceptual graph from storage.
            //In this case it is a query-storage.
            //But all of kinds of storages can return a conceptual graph by this way.
            //If the storage contains one graph it is that graph.
            //It the storage contains two or more graphs it is undetermined graph of contained in ths storage.
            //Check kind of storage before using this method.
            var conceptualGraphFromQueryStorage = queryStorage.GetConceptualGraph();

            LogInstance.Log($"conceptualGraphFromQueryStorage = {conceptualGraphFromQueryStorage}");

            //The information which is containde in the storage can be peresented in many different ways.
            //Olso as gnu clay sentence (my own format).
            var gnuClaySentenceFromQueryStorage = queryStorage.GetGnuClaySentence();

            LogInstance.Log($"gnuClaySentenceFromQueryStorage = {gnuClaySentenceFromQueryStorage}");

            //The information which is containde in the storage can be peresented in many different ways.
            //Olso as sdandard predicate sentence.
            var predicateSentenceFromQueryStorage = queryStorage.GetPredicateSentence();

            LogInstance.Log($"predicateSentenceFromQueryStorage = {predicateSentenceFromQueryStorage}");

            //Add all information from `queryStorage` to `globalStorage`.
            //This information remains in `queryStorage`.
            //I will have duplicating of this information in both storages.
            globalStorage.Accept(queryStorage);

            //I can add directly a conceptual graph, gnu clay sentence or sdandard predicate sentence to any storage.
            globalStorage.Accept(predicateSentenceFromQueryStorage);

            //I create an empty storage which is based on `globalStorage` as its parent.
            var fork_1 = globalStorage.Fork();

            LogInstance.Log($"fork_1 = {fork_1}");

            //I create an empty storage which is based on `fork_1` as its parent.
            var fork_2 = fork_1.Fork();

            LogInstance.Log($"fork_2 = {fork_2}");

            //I create an empty storage which is based on `queryStorage` as its parent.
            var fork_3 = queryStorage.Fork();

            LogInstance.Log($"fork_3 = {fork_3}");

            LogInstance.Log("End");
        }

        private static void TSTRange()
        {
            LogInstance.Log("Begin");

            var list = ListHelper.GetRange(0, 90, 5);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach(var item in list)
            {
                LogInstance.Log($"item = {item}");
            }
            list = ListHelper.GetRange(90, 0, 5);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach (var item in list)
            {
                LogInstance.Log($"item = {item}");
            }
            list = ListHelper.GetRange(-90, 0, 5);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach (var item in list)
            {
                LogInstance.Log($"item = {item}");
            }
            list = ListHelper.GetRange(90, 90, 5);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach (var item in list)
            {
                LogInstance.Log($"item = {item}");
            }
            list = ListHelper.GetRange(10, 90, 0);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach (var item in list)
            {
                LogInstance.Log($"item = {item}");
            }
            list = ListHelper.GetRange(0, -90, 5);
            LogInstance.Log($"list.Count = {list.Count}");
            foreach (var item in list)
            {
                LogInstance.Log($"item = {item}");
            }

            LogInstance.Log("End");
        }

        private static void TSTLexer()
        {
            var queryStr = "!((name='helen'|name='ann')&class='girl')";
            LogInstance.Log($"queryStr = {queryStr}");

            //var lexer = new Lexer(queryStr);
            //Token token = null;
            //while ((token = lexer.GetToken()) != null)
            //{
            //    LogInstance.Log($"token = {token}");
            //}
            var globalEntityDictionary = new EntityDictionary();
            var context = new ParserContext(queryStr, globalEntityDictionary);
            //Token token = null;
            //while ((token = context.GetToken()) != null)
            //{
            //    LogInstance.Log($"token = {token}");
            //}
            //var parser = new LogicalExpressionParser(context);
            //parser.Run();
            var node = LogicalExpressionParserHelper.CreateNode(context);
            LogInstance.Log($"node = {node}");
            
            queryStr = "!((name='helen'&name='ann')|class='girl')";
            LogInstance.Log($"queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            LogInstance.Log($"node = {node}");

            queryStr = "(name='helen'&name='ann')|class='girl'";
            LogInstance.Log($"queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);
           
            node = LogicalExpressionParserHelper.CreateNode(context);
            LogInstance.Log($"node = {node}");

            queryStr = "class='girl'|(name='helen'&name='ann')";
            LogInstance.Log($"queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            LogInstance.Log($"node = {node}");

            queryStr = "class='girl'&(name='helen'&name='ann')";
            LogInstance.Log($"queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            LogInstance.Log($"node = {node}");

            queryStr = "class='girl'&!(name='helen'&name='ann')";
            LogInstance.Log($"queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            LogInstance.Log($"node = {node}");

            queryStr = "class='girl'|!(name='helen'&name='ann')";
            LogInstance.Log($"queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            LogInstance.Log($"node = {node}");

            queryStr = "!class='girl'";
            LogInstance.Log($"queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            LogInstance.Log($"node = {node}");

            queryStr = "class='girl'";
            LogInstance.Log($"queryStr = {queryStr}");

            context = new ParserContext(queryStr, globalEntityDictionary);

            node = LogicalExpressionParserHelper.CreateNode(context);
            LogInstance.Log($"node = {node}");
        }

        private static void TSTLogicalAST()
        {
            LogInstance.Log("Begin");

            var globalEntityDictionary = new EntityDictionary();
            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var indexingStorage = new LogicalIndexStorage(entityLogger);

            var namePropertyId = globalEntityDictionary.GetKey("name");
            var classPropertyId = globalEntityDictionary.GetKey("class");

            var passiveLogicalObject = new PassiveLogicalObject(entityLogger, globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject);

            passiveLogicalObject[namePropertyId] = "helen";
            passiveLogicalObject[classPropertyId] = "girl";

            var passiveLogicalObject_2 = new PassiveLogicalObject(entityLogger, globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject_2);

            passiveLogicalObject_2[namePropertyId] = "ann";
            passiveLogicalObject_2[classPropertyId] = "girl";

            var passiveLogicalObject_3 = new PassiveLogicalObject(entityLogger, globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject_3);

            passiveLogicalObject_3[namePropertyId] = "Beatles";
            passiveLogicalObject_3[classPropertyId] = "band";

            //indexingStorage.PutPropertyValue(12, namePropertyId, "helen");

            var conditionNode = new ConditionOfQueryASTNode();
            conditionNode.PropertyId = namePropertyId;
            conditionNode.Value = "helen";

            var conditionNode_2 = new ConditionOfQueryASTNode();
            conditionNode_2.PropertyId = namePropertyId;
            conditionNode_2.Value = "ann";

            var orNode = new BinaryOperatorOfQueryASTNode();
            orNode.OperatorId = KindOfBinaryOperators.Or;
            orNode.Left = conditionNode;
            orNode.Right = conditionNode_2;

            var conditionNode_1 = new ConditionOfQueryASTNode();
            conditionNode_1.PropertyId = classPropertyId;
            conditionNode_1.Value = "girl";

            var andNode = new BinaryOperatorOfQueryASTNode();
            andNode.OperatorId = KindOfBinaryOperators.And;
            andNode.Left = orNode;
            andNode.Right = conditionNode_1;

            var notNode = new UnaryOperatorOfQueryASTNode();
            notNode.OperatorId = KindOfUnaryOperators.Not;
            notNode.Left = andNode;

            LogInstance.Log($"notNode = {notNode}");

            var queryCache = new QueriesCache(globalEntityDictionary);

            var systemPropertiesDictionary = new SystemPropertiesDictionary(globalEntityDictionary);

            var npcHostContext = new StubOfNPCHostContext(entityLogger);

            var storageOfSpecialEntities = new StorageOfSpecialEntities();
            storageOfSpecialEntities.SelfEntityId = npcHostContext.SelfEntityId;
            var visionObjectsStorage = new VisionObjectsStorage(entityLogger, globalEntityDictionary, npcHostContext, systemPropertiesDictionary, storageOfSpecialEntities);

            var queryStr = "!((name=helen|name=ann)&class=girl)";
            var logicalObject = new LogicalObject(entityLogger, queryStr, globalEntityDictionary, indexingStorage, queryCache, systemPropertiesDictionary, visionObjectsStorage);

            var entitiesIdList = logicalObject.CurrentEntitiesIdList;

            LogInstance.Log($"entitiesIdList.Count = {entitiesIdList.Count}");
            foreach (var entityId in entitiesIdList)
            {
                LogInstance.Log($"entityId = {entityId}");
            }

            passiveLogicalObject_2[classPropertyId] = "boy";

            Thread.Sleep(100);

            entitiesIdList = logicalObject.CurrentEntitiesIdList;

            LogInstance.Log($"(2) entitiesIdList.Count = {entitiesIdList.Count}");
            foreach (var entityId in entitiesIdList)
            {
                LogInstance.Log($"(2) entityId = {entityId}");
            }

            var logicalObject_2 = new LogicalObject(entityLogger, queryStr, globalEntityDictionary, indexingStorage, queryCache, systemPropertiesDictionary, visionObjectsStorage);

            entitiesIdList = logicalObject.CurrentEntitiesIdList;

            LogInstance.Log($"(3) entitiesIdList.Count = {entitiesIdList.Count}");
            foreach (var entityId in entitiesIdList)
            {
                LogInstance.Log($"(3) entityId = {entityId}");
            }

            var resultOfcomparsing = logicalObject == logicalObject_2;

            LogInstance.Log($"resultOfcomparsing = {resultOfcomparsing}");

            resultOfcomparsing = logicalObject_2 == logicalObject;

            LogInstance.Log($"(2) resultOfcomparsing = {resultOfcomparsing}");

            /*var list1 = new List<int>() { 1 };
            var list2 = new List<int>() { 1, 2 };

            var except1_2 = list1.Except(list2).ToList();
            LogInstance.Log($"except1_2.Count = {except1_2.Count}");
            foreach (var entityId in except1_2)
            {
                LogInstance.Log($"(1_2) entityId = {entityId}");
            }

            var except2_1 = list2.Except(list1).ToList();
            LogInstance.Log($"except2_1.Count = {except2_1.Count}");
            foreach (var entityId in except2_1)
            {
                LogInstance.Log($"(2_1) entityId = {entityId}");
            }*/
        }

        private static Dictionary<int, CancellationToken> mCancelationTokenDict = new Dictionary<int, CancellationToken>();

        private static void TSTCancelTask_2()
        {
            LogInstance.Log("Begin");

            var cs = new CancellationTokenSource();
            var token = cs.Token;
            var token2 = token;

            var tmpTask = new Task(() =>
            {
                try
                {
                    mCancelationTokenDict[Task.CurrentId.Value] = token;

                    LogInstance.Log("Task start");
                    LogInstance.Log($"Task.CurrentId = {Task.CurrentId}");

                    DoWork();
                }
                catch(OperationCanceledException)
                {
                    LogInstance.Log("catch(OperationCanceledException)");
                }
                catch(Exception e)
                {
                    LogInstance.Error($"Task e = {e}");
                }
                finally
                {
                    mCancelationTokenDict.Remove(Task.CurrentId.Value);
                    LogInstance.Log($"finally");
                }
            }, token);

            tmpTask.Start();

            LogInstance.Log("started");
            LogInstance.Log($"tmpTask.Id = {tmpTask.Id}");

            Thread.Sleep(1000);

            LogInstance.Log($"mCancelationTokenDict.Count = {mCancelationTokenDict.Count}");

            cs.Cancel();

            LogInstance.Log("Canceled");

            cs.Cancel();

            LogInstance.Log("Canceled twice");

            Thread.Sleep(1000);

            LogInstance.Log($"after mCancelationTokenDict.Count = {mCancelationTokenDict.Count}");
            LogInstance.Log("End");
        }

        private static void DoWork()
        {
            var token = mCancelationTokenDict[Task.CurrentId.Value];

            var n = 0;

            while (true)
            {
                LogInstance.Log($"n = {n}");
                n++;

                token.ThrowIfCancellationRequested();
            }
        }

        private static void TSTCancelTask()
        {
            LogInstance.Log("Begin");

            Thread tmpThread = null;

            var tmpTask = new Task(() => {
                tmpThread = Thread.CurrentThread;

                var n = 0;

                while(true)
                {
                    LogInstance.Log($"Task n = {n}");
                    n++;
                }
            });

            tmpTask.Start();

            LogInstance.Log("started");

            Thread.Sleep(1000);

            tmpThread.Abort();

            LogInstance.Log("aborted");

            Thread.Sleep(1000);

            LogInstance.Log("End");
        }

        private static void TSTMyNPCContext()
        {
            LogInstance.Log("Begin");

            var globalEntityDictionary = new EntityDictionary();

            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var stubOfHumanoidBodyController = new StubOfNPCHostContext(entityLogger, globalEntityDictionary);

            var indexingStorage = stubOfHumanoidBodyController.LogicalIndexStorageImpl;

            var context = new MyNPCContext(entityLogger, globalEntityDictionary, stubOfHumanoidBodyController);
            context.Bootstrap();

            Thread.Sleep(1000);

            var command = new NPCCommand();
            command.Name = "key press";
            command.Params.Add("key", "k");

            context.Send(command);

            var namePropertyId = globalEntityDictionary.GetKey("name");
            var classPropertyId = globalEntityDictionary.GetKey("class");

            var passiveLogicalObject = new PassiveLogicalObject(entityLogger, globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject);

            passiveLogicalObject[namePropertyId] = "helen";
            passiveLogicalObject[classPropertyId] = "girl";

            var passiveLogicalObject_2 = new PassiveLogicalObject(entityLogger, globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject_2);

            passiveLogicalObject_2[namePropertyId] = "ann";
            passiveLogicalObject_2[classPropertyId] = "girl";

            var passiveLogicalObject_3 = new PassiveLogicalObject(entityLogger, globalEntityDictionary, indexingStorage);

            indexingStorage.RegisterObject(passiveLogicalObject_3);

            passiveLogicalObject_3[namePropertyId] = "Beatles";
            passiveLogicalObject_3[classPropertyId] = "band";

            var queryStr = "!((name=helen|name=ann)&class=girl)";

            var logicalObject = context.GetLogicalObject(queryStr);

            var entitiesIdList = logicalObject.CurrentEntitiesIdList;

            LogInstance.Log($"entitiesIdList.Count = {entitiesIdList.Count}");
            foreach (var entityId in entitiesIdList)
            {
                LogInstance.Log($"entityId = {entityId}");
            }

            var logicalObject_2 = context.GetLogicalObject(queryStr);

            var entitiesIdList_2 = logicalObject_2.CurrentEntitiesIdList;

            LogInstance.Log($"entitiesIdList_2.Count = {entitiesIdList_2.Count}");
            foreach (var entityId in entitiesIdList)
            {
                LogInstance.Log($"entityId = {entityId}");
            }

            var resultOfcomparsing = logicalObject == logicalObject_2;

            LogInstance.Log($"resultOfcomparsing = {resultOfcomparsing}");

            var name = logicalObject["name"];

            LogInstance.Log($"name = {name}");

            logicalObject["name"] = 12;

            name = logicalObject["name"];

            LogInstance.Log($"name (2) = {name}");

            resultOfcomparsing = context.SelfLogicalObject == logicalObject;

            LogInstance.Log($"resultOfcomparsing (2) = {resultOfcomparsing}");

            var visibleItems = context.VisibleObjects;
            LogInstance.Log($"visibleItems.Count = {visibleItems.Count}");
            foreach (var visibleItem in visibleItems)
            {
                LogInstance.Log($"visibleItem = {visibleItem}");
                var posOfVisibleItem = visibleItem["global position"];
                LogInstance.Log($"posOfVisibleItem = {posOfVisibleItem}");
            }

            var pos = context.SelfLogicalObject["global position"];

            LogInstance.Log($"pos = {pos}");

            while (true)
            {
                Thread.Sleep(10000);
            }
        }

        private static void TSTStorageOfNPCProcesses()
        {
            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var idFactory = new IdFactory();
            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var testedContext = new TestedNPCContext(entityLogger);
            var storage = new StorageOfNPCProcesses(entityLogger, idFactory, globalEntityDictionary, npcProcessInfoCache, testedContext);

            var type = typeof(TmpConcreteNPCProcess);

            var result = storage.AddTypeOfProcess(type);

            LogInstance.Log($"result = {result}");

            var command = new NPCCommand();
            command.Name = "test";

            LogInstance.Log($"command = {command}");

            var internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            LogInstance.Log($"internalCommand = {internalCommand}");

            var process = storage.GetProcess(internalCommand);

            LogInstance.Log($"(process == null) = {process == null}");

            process = storage.GetProcess(internalCommand);

            LogInstance.Log($"(process == null) (2) = {process == null}");

            process.RunAsync();

            LogInstance.Log("-----------------------------------------------");

            //type = typeof(TestedNPCProcessInfoWithoutEntryPointsAndWithNameAndWithStartupModeNPCProcess);

            //result = storage.AddTypeOfProcess(type);

            //LogInstance.Log($"result = {result}");

            //command = new NPCCommand();
            //command.Name = "SomeName";

            //LogInstance.Log($"command = {command}");

            //internalCommand = NPCCommandHelper.ConvertICommandToInternalCommand(command, globalEntityDictionary);

            //LogInstance.Log($"internalCommand = {internalCommand}");

            //process = storage.GetProcess(internalCommand);

            //LogInstance.Log($"(process == null) = {process == null}");

            //process = storage.GetProcess(internalCommand);

            //LogInstance.Log($"(process == null) (2) = {process == null}");

            Thread.Sleep(10000);

            LogInstance.Log("End");
        }

        private static void TSTActivatorOfNPCProcessEntryPointInfo()
        {
            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var activator = new ActivatorOfNPCProcessEntryPointInfo(entityLogger);
            var rank = activator.GetRankByTypesOfParameters(typeof(int), typeof(string));

            LogInstance.Log($"rank = {rank}");

            LogInstance.Log($"typeof(int?).FullName = {typeof(int?).FullName}");
            LogInstance.Log($"System.Nullable = {typeof(int?).FullName.StartsWith("System.Nullable")}");
            LogInstance.Log($"typeof(int?).IsClass = {typeof(int?).IsClass}");
            LogInstance.Log($"typeof(string).IsClass = {typeof(string).IsClass}");

            rank = activator.GetRankByTypesOfParameters(typeof(int?), typeof(int));

            LogInstance.Log($"rank = {rank}");

            rank = activator.GetRankByTypesOfParameters(typeof(string), null);

            LogInstance.Log($"rank = {rank}");

            rank = activator.GetRankByTypesOfParameters(typeof(int?), null);

            LogInstance.Log($"rank = {rank}");

            rank = activator.GetRankByTypesOfParameters(typeof(int), null);

            LogInstance.Log($"rank = {rank}");

            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);

            var type = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributesNPCProcess);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            var arg1Key = globalEntityDictionary.GetKey("someArgument");
            var arg2Key = globalEntityDictionary.GetKey("secondArgument");

            var paramsDict = new Dictionary<ulong, object>() { { arg1Key, true }, { arg2Key, 12 } };
            var result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            LogInstance.Log($"result.Count = {result.Count}");
            foreach(var tmpItem in result)
            {
                LogInstance.Log($"tmpItem = {tmpItem}");
            }

            type = typeof(TestedNPCProcessInfoWithOneEntryPointWithArgsAndWithoutAttributesNPCProcess);
            npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            paramsDict = new Dictionary<ulong, object>() { { 1ul, true }, { 2ul, 12 } };
            result = activator.GetRankedEntryPoints(npcProcessInfo, paramsDict);

            LogInstance.Log($"result.Count = {result.Count}");
            foreach (var tmpItem in result)
            {
                LogInstance.Log($"tmpItem = {tmpItem}");
            }
        }

        private static void CreateContextAndProcessesCase1()
        {
            LogInstance.Log("Begin");

            var npcProcessInfoCache = new NPCProcessInfoCache();
            var globalEntityDictionary = new EntityDictionary();

            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            var tmpContext = new TmpConcreteNPCContext(entityLogger, globalEntityDictionary, npcProcessInfoCache);

            var command = new NPCCommand();
            command.Name = "SomeName";
            command.InitiatingProcessId = 1;

            var process = tmpContext.Send(command);

            process.Task?.Wait();

            //Thread.Sleep(10000);

            //try
            //{
            //    npcProcessInfoCache.Set(null);
            //}
            //catch(Exception e)
            //{
            //    LogInstance.Log($"e = {e}");
            //}

            //try
            //{
            //    npcProcessInfoCache.Get(null);
            //}
            //catch (Exception e)
            //{
            //    LogInstance.Log($"e = {e}");
            //}

            LogInstance.Log("End");
        }

        private static void CreateInfoOfConcreteProcess()
        {
            LogInstance.Log("Begin");

            var entityLogger = new EntityLogger();
            entityLogger.Marker = Guid.NewGuid().ToString("D");
            entityLogger.Enabled = true;

            //var type = typeof(TmpConcreteNPCProcess);
            var type = typeof(TestedNPCProcessInfoWithTwoEntryPointsAndWithoutAttributesNPCProcess);

            LogInstance.Log($"type.FullName = {type.FullName}");

            var globalEntityDictionary = new EntityDictionary();
            var npcProcessInfoFactory = new NPCProcessInfoFactory(entityLogger, globalEntityDictionary);
            var npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            LogInstance.Log($"npcProcessInfo = {npcProcessInfo}");

            var method_1 = npcProcessInfo.EntryPointsInfoList.Single(p => p.ParametersMap.Count == 0);

            LogInstance.Log($"method_1 = {method_1}");

            var method_2 = npcProcessInfo.EntryPointsInfoList.SingleOrDefault(p => p.ParametersMap.Count == 2 && p.ParametersMap.ContainsValue(typeof(int)) && p.ParametersMap.ContainsValue(typeof(bool)));
            LogInstance.Log($"method_2 = {method_2}");

            var method_3 = npcProcessInfo.EntryPointsInfoList.SingleOrDefault(p => p.ParametersMap.Count == 2 && p.ParametersMap.Values.Count(x => x == typeof(int)) == 2);
            LogInstance.Log($"method_3 = {method_3}");

            //type = typeof(Program);
            //LogInstance.Log($"type.FullName = {type.FullName}");

            //npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            //LogInstance.Log($"npcProcessInfo = {npcProcessInfo}");

            type = typeof(TestedNPCProcessInfoWithPointWithDefaultValueOfArgumentAndWithNameAndWithStartupModeNPCProcess);
            LogInstance.Log($"type.FullName = {type.FullName}");

            npcProcessInfo = npcProcessInfoFactory.CreateInfo(type);

            LogInstance.Log($"npcProcessInfo = {npcProcessInfo}");

            LogInstance.Log("End");
        }
    }
}
