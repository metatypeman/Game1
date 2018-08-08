using Assets.Scripts;
using MyNPCLib;
using MyNPCLib.LogicalSoundModeling;
using MyNPCLib.NLToCGParsing;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.SimpleWordsDict;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class TmpSmallBot : MonoBehaviour {
    private DictationRecognizer m_DictationRecognizer;
    private InputKeyHelper mInputKeyHelper;
    private LogicalSoundBus mLogicalSoundBus;
    private IEntityDictionary mEntityDictionary;
    private WordsDict mWordsDict;
    private CGParser mCGParser;

    // Use this for initialization
    void Start () {
        var commonLevelHost = LevelCommonHostFactory.Get();
        mEntityDictionary = commonLevelHost.EntityDictionary;
        mLogicalSoundBus = commonLevelHost.LogicalSoundBus;

        mWordsDict = new WordsDict();
        //mCGParser = new CGParser(mWordsDict);

        mInputKeyHelper = new InputKeyHelper();
        mInputKeyHelper.AddListener(KeyCode.Z, OnZPressAction);
    }
	
	// Update is called once per frame
	void Update () {
        mInputKeyHelper.Update();
    }

    private void OnZPressAction(KeyCode key)
    {
#if DEBUG
        LogInstance.Log($"key = {key}");
#endif

        var paragraph = "Go to Green Waypoint";

        //var result = mCGParser.Run(paragraph);
#if DEBUG
        //LogInstance.Log($"result = {result}");
#endif

        var tstFact = CreateSimpleFact(mEntityDictionary);

        var soundPackage = new InputLogicalSoundPackage(new System.Numerics.Vector3(1, 0, 0), 60, new List<string>() { "human_speech" }, new List<RuleInstance>() { tstFact });

        mLogicalSoundBus.PushSoundPackage(soundPackage);
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
}
