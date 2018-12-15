using Assets.Scripts;
using MyNPCLib;
using MyNPCLib.CGStorage;
using MyNPCLib.ConvertingCGToInternal;
using MyNPCLib.ConvertingInternalCGToPersistLogicalData;
using MyNPCLib.LogicalSoundModeling;
using MyNPCLib.NLToCGParsing;
using MyNPCLib.NLToCGParsing_v2;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class ConcreteFirstPersonController : MonoBehaviour
{
    private IUserClientCommonHost mUserClientCommonHost;
    private ISpellHelperDialog mSpellHelperDialog;

    private DictationRecognizer m_DictationRecognizer;
    private InputKeyHelper mInputKeyHelper;
    private InputMouseKeyHelper mInputMouseKeyHelper;
    private LogicalSoundBus mLogicalSoundBus;
    private IEntityDictionary mEntityDictionary;
    private WordsDict mWordsDict;
    private CGParser_v2 mCGParser;
    private ContextOfCGStorage mContextOfCGStorage;

    public Texture2D CrosshairImage;
    public GameObject Rifle;
    private IHandThing mRifleInstance;

    private GateOfMilitaryBase mGateOfMilitaryBase;

    // Use this for initialization
    void Start ()
    {
        var commonLevelHost = LevelCommonHostFactory.Get();
        mEntityDictionary = commonLevelHost.EntityDictionary;
        mLogicalSoundBus = commonLevelHost.LogicalSoundBus;

        mContextOfCGStorage = new ContextOfCGStorage(mEntityDictionary);
        //c:\Users\Sergey\Documents\GitHub\Game1\Assets\working.dict
        var pathOfWordsDict = @"c:\Users\Sergey\Documents\GitHub\Game1\Assets\Resources\";

        mWordsDict = new WordsDict(pathOfWordsDict);
        var cgParserOptions = new CGParserOptions_v2();
        cgParserOptions.WordsDict = mWordsDict;
        cgParserOptions.BasePath = @"c:\Users\Sergey\Documents\GitHub\Game1\Assets\";

        mCGParser = new CGParser_v2(cgParserOptions);

        mSpellHelperDialog = SpellHelperDialog.Instance;
        mSpellHelperDialog.OnSpellMessage += OnSpellMessage;

        mUserClientCommonHost = UserClientCommonHostFactory.Get();
        mInputKeyHelper = new InputKeyHelper(mUserClientCommonHost);
        mInputKeyHelper.AddPressListener(KeyCode.Y, OnYPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.Z, OnZPressAction);
        //mInputKeyHelper.AddPressListener(KeyCode.F, OnFPressAction);
        //mInputKeyHelper.AddUpListener(KeyCode.F, OnFUpAction);
        mInputKeyHelper.AddPressListener(KeyCode.C, OnCPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.X, OnXPressAction);

        mInputKeyHelper.AddPressListener(KeyCode.V, OnVPressAction);//Jonh
        mInputKeyHelper.AddPressListener(KeyCode.B, OnBPressAction);//Jake

        mInputKeyHelper.AddPressListener(KeyCode.F, ProcessGoToRedWaypoint);
        mInputKeyHelper.AddPressListener(KeyCode.G, ProcessGoToGreenWaypoint);
        mInputKeyHelper.AddPressListener(KeyCode.H, ProcessGoToBlueWaypoint);
        mInputKeyHelper.AddPressListener(KeyCode.J, ProcessGoToYellowWaypoint);
        mInputKeyHelper.AddPressListener(KeyCode.K, ProcessStop);
        mInputKeyHelper.AddPressListener(KeyCode.L, ProcessContinue);

        mInputMouseKeyHelper = new InputMouseKeyHelper(mUserClientCommonHost);
        mInputMouseKeyHelper.AddPressListener(0, OnFPressAction);
        mInputMouseKeyHelper.AddUpListener(0, OnFUpAction);

        m_DictationRecognizer = new DictationRecognizer();

        m_DictationRecognizer.InitialSilenceTimeoutSeconds = 120;
        m_DictationRecognizer.AutoSilenceTimeoutSeconds = 120;

        Debug.LogFormat($"AppDomain.CurrentDomain.BaseDirectory = {AppDomain.CurrentDomain.BaseDirectory}");

        Debug.LogFormat("Dictation m_DictationRecognizer.InitialSilenceTimeoutSeconds: {0}", m_DictationRecognizer.InitialSilenceTimeoutSeconds);
        Debug.LogFormat("Dictation m_DictationRecognizer.AutoSilenceTimeoutSeconds: {0}", m_DictationRecognizer.AutoSilenceTimeoutSeconds);

        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);

            //DispatchText(text);
        };

        m_DictationRecognizer.DictationHypothesis += (text) =>
        {
            Debug.LogFormat("Dictation hypothesis: {0}", text);
        };

        m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
            {
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
            }
        };

        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        //m_DictationRecognizer.Start();

        mRifleInstance = Rifle.GetComponent<IHandThing>();

        mGateOfMilitaryBase = GameObject.Find("GateOfMilitaryBase").GetComponent<GateOfMilitaryBase>();
    }

    // Update is called once per frame
    void Update ()
    {
        mInputKeyHelper.Update();
        mInputMouseKeyHelper.Update();
    }

    //void OnGUI()
    //{
    //    float xMin = (Screen.width / 2) - (CrosshairImage.width / 2);
    //    float yMin = (Screen.height / 2) - (CrosshairImage.height / 2);
    //    GUI.DrawTexture(new Rect(xMin, yMin, CrosshairImage.width, CrosshairImage.height), CrosshairImage);
    //}

    private void OnSpellMessage(string message)
    {
        LogInstance.Log($"message = {message}");
    }

    private void OnYPressAction()
    {
        //var canvasComponent = GameObject.Find("Canvas");

        //LogInstance.Log($"canvasComponent != null = {canvasComponent != null}");

        mSpellHelperDialog.ShowDialog();
    }

    //private void OnDPressAction(KeyCode key)
    //{
    //    var command = new NPCCommand();
    //    command.Name = "shoot on";

    //    mRifleInstance.Send(command);
    //}

    private void OnFPressAction()
    {
        var command = new NPCCommand();
        command.Name = "shoot on";

        mRifleInstance.Send(command);
    }

    private void OnFUpAction()
    {
        var command = new NPCCommand();
        command.Name = "shoot off";

        mRifleInstance.Send(command);
    }

    private void OnZPressAction()
    {
#if DEBUG
        LogInstance.Log("Begin");
#endif

        var paragraph = "Go to green place";

        DispatchText(paragraph);
    }

    private void OnCPressAction()
    {
#if DEBUG
        LogInstance.Log("Begin");
#endif

        mGateOfMilitaryBase.Open();
    }

    private void OnXPressAction()
    {
#if DEBUG
        LogInstance.Log("Begin");
#endif

        mGateOfMilitaryBase.Close();
    }

    /// <summary>
    /// Jonh
    /// </summary>
    private void OnVPressAction()
    {
#if DEBUG
        LogInstance.Log("Begin");
#endif

        var paragraph = "Jonh";

        DispatchText(paragraph);
    }

    /// <summary>
    /// Jake
    /// </summary>
    private void OnBPressAction()
    {
#if DEBUG
        LogInstance.Log("Begin");
#endif

        var paragraph = "Jake";

        DispatchText(paragraph);
    }

    private void ProcessGoToRedWaypoint()
    {
        var paragraph = "Go to red place";

        DispatchText(paragraph);
    }

    private void ProcessGoToGreenWaypoint()
    {
        var paragraph = "Go to green place";

        DispatchText(paragraph);
    }

    private void ProcessGoToBlueWaypoint()
    {
        var paragraph = "Go to blue place";

        DispatchText(paragraph);
    }

    private void ProcessGoToYellowWaypoint()
    {
        var paragraph = "Go to yellow place";

        DispatchText(paragraph);
    }

    private void ProcessStop()
    {
        var paragraph = "Stop";

        DispatchText(paragraph);
    }

    private void ProcessContinue()
    {
        var paragraph = "Continue";

        DispatchText(paragraph);
    }

    private void DispatchText(string text)
    {
        try
        {
#if DEBUG
            LogInstance.Log($"pre text = {text}");
#endif

            if (text.Contains("bay point"))
            {
                text = text.Replace("bay point", "waypoint");
            }

#if DEBUG
            LogInstance.Log($"after text = {text}");
#endif

            var result = mCGParser.Run(text);
#if DEBUG
            LogInstance.Log($"result = {result}");
#endif

            var ruleInstancesList = new List<RuleInstance>();

            var items = result.Items;

            foreach (var graph in items)
            {
                var internalCG = ConvertorCGToInternal.Convert(graph, mEntityDictionary);

                ruleInstancesList.AddRange(ConvertorInternalCGToPersistLogicalData.ConvertConceptualGraph(internalCG, mEntityDictionary));
            }

#if DEBUG
            LogInstance.Log($"ruleInstancesList.Count = {ruleInstancesList.Count}");
#endif

            //var tstFact = CreateSimpleFact(mEntityDictionary);

            var soundPackage = new InputLogicalSoundPackage(new System.Numerics.Vector3(1, 0, 0), 60, new List<string>() { "human_speech" }, new PassiveListGCStorage(mEntityDictionary, ruleInstancesList));

            mLogicalSoundBus.PushSoundPackage(soundPackage);
        }
        catch (Exception e)
        {
            LogInstance.Error(e.ToString());
        }
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
