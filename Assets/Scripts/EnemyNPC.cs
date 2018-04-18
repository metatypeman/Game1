using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using MyNPCLib;

[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyRayScaner))]
public class EnemyNPC : MonoBehaviour
{
    public void Awake()
    {
        //Debug.Log("EnemyNPC Awake");

        var logInstance = new LogProxyForDebug();
        LogInstance.SetLogProxy(logInstance);
    }

    private EnemyController mEnemyController;
    private EnemyRayScaner mEnemyRayScaner;

    public float surfaceOffset = 1.5f;
    public GameObject setTargetOn;

    private OldTstConcreteNPCProcessesContextWithBlackBoard mOldNPCProcessesContext;
    private TestedNPCContext mNPCProcessesContext;

    public bool UseOldContext = false;

    //RapidFireGun _gun;

    private InputKeyHelper mInputKeyHelper;

    public Transform Head;
    public Animator mAnim;

    public Transform GunEnd;
    private GameObject mGunBody;

    private int mInstanceIdOfRifle;

    // Use this for initialization
    void Start()
    {
        mAnim = GetComponent<Animator>();

        mEnemyController = GetComponent<EnemyController>();
        
        mEnemyRayScaner = GetComponent<EnemyRayScaner>();

        if(UseOldContext)
        {
            mOldNPCProcessesContext = new OldTstConcreteNPCProcessesContextWithBlackBoard(mEnemyController);
            mOldNPCProcessesContext.RegisterInstance<INPCRayScaner>(mEnemyRayScaner);
        }
        else
        {
            var targetGun = FindObjectOfType<RapidFireGun>();
            var possibleInstanceIdOfRifle = 0;
            
            if (targetGun != null)
            {
                possibleInstanceIdOfRifle = targetGun.GetInstanceID();
            }
        
            var _target = GameObject.Find("Ethan");
            
            Vector3? ethanPosition = null;
            
            if(_target != null)
            {
                ethanPosition = _target.transform.position;
            }
        
            var npcProcessInfoCache = new NPCProcessInfoCache();
            var globalEntityDictionary = new EntityDictionary();
            //var  GetComponent<EnemyController>();

            var internalBodyHost = GetComponent <IInternalBodyHumanoidHost>();

            var hostContext = new TestedNPCHostContext(internalBodyHost);
            mNPCProcessesContext = new TestedNPCContext(globalEntityDictionary, npcProcessInfoCache, hostContext);
            var blackBoard = mNPCProcessesContext.BlackBoard;
            blackBoard.PossibleIdOfRifle = possibleInstanceIdOfRifle;
            blackBoard.EthanPosition = ethanPosition;
            mNPCProcessesContext.Bootstrap();
        }        
        
        //_gun = GetComponentInChildren<RapidFireGun>();

        //if(_gun != null)
        //{
        //    mEnemyController.SetAimCorrector(_gun);
        //    _gun.UseDebugLine = true;
            //_gun.FireMode = FireMode.Single;
        //}

        mInputKeyHelper = new InputKeyHelper();
        mInputKeyHelper.AddListener(KeyCode.F, OnFPressAction);
        mInputKeyHelper.AddListener(KeyCode.G, OnGPressAction);
        mInputKeyHelper.AddListener(KeyCode.K, OnKPressAction);
        mInputKeyHelper.AddListener(KeyCode.N, OnNPressAction);
        mInputKeyHelper.AddListener(KeyCode.H, OnHPressAction);
        mInputKeyHelper.AddListener(KeyCode.L, OnLPressAction);
        mInputKeyHelper.AddListener(KeyCode.I, OnIPressAction);
        mInputKeyHelper.AddListener(KeyCode.P, OnPPressAction);
        mInputKeyHelper.AddListener(KeyCode.U, OnUPressAction);
        mInputKeyHelper.AddListener(KeyCode.M, OnMPressAction);
        mInputKeyHelper.AddListener(KeyCode.B, OnBPressAction);
        mInputKeyHelper.AddListener(KeyCode.J, OnJPressAction);
        mInputKeyHelper.AddListener(KeyCode.Q, OnQPressAction);

        mGunBody = GameObject.Find("M4A1 Sopmod");

        var clas1 = new Class1();
        clas1.GetItems().Add(12);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("EnemyNPC Update");
        mInputKeyHelper.Update();
        //mGunBody.SetActive(false);
    }

    private void OnQPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnQPressAction key = {key}");
        Debug.Log($"EnemyNPC OnQPressAction mInstanceIdOfRifle = {mInstanceIdOfRifle}");

        if (UseOldContext)
        {
            if (mInstanceIdOfRifle > 0)
            {
                var tmpProcess = new OldTstThrowOutToSurfaceRifleToSurfaceProcess(mOldNPCProcessesContext, mInstanceIdOfRifle);
                tmpProcess.RunAsync();
            }
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnQPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        }
    }

    private void OnJPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnJPressAction key = {key}");
        Debug.Log($"EnemyNPC OnJPressAction mInstanceIdOfRifle = {mInstanceIdOfRifle}");

        if (UseOldContext)
        {
            if (mInstanceIdOfRifle > 0)
            {
                var tmpProcess = new OldTstHideRifleToBagPackProcess(mOldNPCProcessesContext, mInstanceIdOfRifle);
                tmpProcess.RunAsync();
            }
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnJPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        }
    }

    private void OnBPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnBPressAction key = {key}");

        var instanceId = 0;

        var targetGun = FindObjectOfType<RapidFireGun>();

        Debug.Log($"EnemyNPC OnBPressAction (targetGun == null) = {targetGun == null}");

        if (targetGun == null)
        {
            if(mInstanceIdOfRifle > 0)
            {
                instanceId = mInstanceIdOfRifle;
            }
        }
        else
        { 
            instanceId = targetGun.GetInstanceID();
            mInstanceIdOfRifle = instanceId;        
        }

        Debug.Log($"EnemyNPC OnBPressAction instanceId = {instanceId}");

        if (UseOldContext)
        {
            if (instanceId > 0)
            {
                var tmpProcess = new OldTSTTakeFromSurfaceProcess(mOldNPCProcessesContext, instanceId);
                tmpProcess.RunAsync();
            }
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnBPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        }
    }

    private void OnMPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnMPressAction key = {key}");

        if (UseOldContext)
        {
            var tmpProcess = new OldTSTMoveProcess(mOldNPCProcessesContext);
            tmpProcess.RunAsync();
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnMPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        }
    }

    private float? TargetAngle = null;
    //private float AngleSpeed = 0.5f;
    //private float? InitAngle = null;

    public bool isIkActive;

    private void OnFPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnFPressAction key = {key}");

        isIkActive = false;

        if (UseOldContext)
        {
            var tmpProcess = new OldTSTHeadToForvardProcess(mOldNPCProcessesContext);
            tmpProcess.RunAsync();
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnFPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        }
    }

    private void OnGPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnGPressAction key = {key}");

        var currHeadRotation = Head.rotation;
        //isIkActive = true;
        //Head.rotation = Quaternion.Euler(0, 12f, 0) * currHeadRotation;

        if (UseOldContext)
        {
            var tmpProcess = new OldTSTRotateHeadProcess(mOldNPCProcessesContext, 12f);
            tmpProcess.RunAsync();
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnGPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        }
    }

    private void OnKPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnKPressAction key = {key}");

        TargetAngle = 30f;

        if (UseOldContext)
        {
            var tmpProcess = new OldTSTRotateProcess(mOldNPCProcessesContext, TargetAngle.Value);
            tmpProcess.RunAsync();
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnKPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        }
    }

    private void OnNPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnNPressAction key = {key}");

        if (UseOldContext)
        {
            var _gun = mOldNPCProcessesContext.BlackBoard.RapidFireGunProxy;

            Debug.Log($"EnemyNPC OnNPressAction _gun.IsReady = {_gun.IsReady}");

            if (_gun != null && _gun.IsReady)
            {
                _gun.UseDebugLine = true;
                _gun.TurnState = TurnState.On;
            }
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnNPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        } 
    }

    private void OnHPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnHPressAction key = {key}");

        if (UseOldContext)
        {
            var _gun = mOldNPCProcessesContext.BlackBoard.RapidFireGunProxy;
            if (_gun != null)
            {
                _gun.TurnState = TurnState.Off;
            }
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnHPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        }  
    }

    private void OnLPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnLPressAction key = {key}");
        if (UseOldContext)
        {
            var tmpSimpleAimProcess = new OldTstSimpleAimProcess(mOldNPCProcessesContext);
            tmpSimpleAimProcess.RunAsync();
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnLPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        }
    }

    private void OnIPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnIPressAction key = {key}");        
        var _target = GameObject.Find("Ethan");

        if (UseOldContext)
        {
            var tmpTSTFireToEthanProcess = new OldTSTFireToEthanProcess(mOldNPCProcessesContext, _target.transform.position);
            tmpTSTFireToEthanProcess.RunAsync();
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnIPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        }
    }

    private void OnPPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnPPressAction key = {key}");
        if (UseOldContext)
        {
            var tmpProcess = new OldTstRunAtOurBaseProcess(mOldNPCProcessesContext);
            tmpProcess.RunAsync();
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnPPressAction command = {command}");
            
            mNPCProcessesContext.Send(command);
        }
    }
    
    private void OnUPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnUPressAction key = {key}");
        if (UseOldContext)
        {
            var tmpProcess = new OldTstRootProcess(mOldNPCProcessesContext);
            tmpProcess.RunAsync();
        }
        else
        {
            var command = KeyToNPCCommandConverter.Convert(key);

            Debug.Log($"EnemyNPC OnUPressAction command = {command}");

            mNPCProcessesContext.Send(command);
        }
    }

    void OnDestroy()
    {
        //Debug.Log("OnDestroy");
        if (UseOldContext)
        {
            mOldNPCProcessesContext?.Dispose();
        }
        else
        {
            mNPCProcessesContext?.Dispose();
        }    
    }
}
