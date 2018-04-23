using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using MyNPCLib;
using System;
using System.Threading;
using System.Linq;

public interface IInvokingInMainThread
{
    void SetInvocableObj(IInvocableObj invokableObj);
}

public interface IInvocableObj
{
    void Invoke();
}

public class InvocableObj<TResult> : IInvocableObj
{
    public InvocableObj(Func<TResult> function, IInvokingInMainThread invokingInMainThread)
    {
        mFunction = function;
        mInvokingInMainThread = invokingInMainThread;
    }

    private Func<TResult> mFunction;
    private IInvokingInMainThread mInvokingInMainThread;
    private bool mHasResult;
    private TResult mResult;
    private readonly object mLockObj = new object();

    public TResult Run()
    {
        mInvokingInMainThread.SetInvocableObj(this);

        while (true)
        {
            lock (mLockObj)
            {
                if(mHasResult)
                {
                    break;
                }
            }

            Thread.Sleep(10);
        }

        return mResult;
    }

    public void Invoke()
    {
        mResult = mFunction.Invoke();

        lock(mLockObj)
        {
            mHasResult = true;
        }
    }
}

[RequireComponent(typeof(HumanoidBodyHost))]
[RequireComponent(typeof(EnemyRayScaner))]
public class EnemyNPC : MonoBehaviour, IInvokingInMainThread
{
    public void Awake()
    {
        //Debug.Log("EnemyNPC Awake");

        var logInstance = new LogProxyForDebug();
        LogInstance.SetLogProxy(logInstance);
    }

    private EnemyRayScaner mEnemyRayScaner;

    public float surfaceOffset = 1.5f;
    public GameObject setTargetOn;

    private TestedNPCContext mNPCProcessesContext;

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

        mEnemyRayScaner = GetComponent<EnemyRayScaner>();

        var targetGun = FindObjectOfType<RapidFireGun>();
        var possibleInstanceIdOfRifle = 0;

        if (targetGun != null)
        {
            possibleInstanceIdOfRifle = targetGun.GetInstanceID();
        }

        var _target = GameObject.Find("Ethan");

        Vector3? ethanPosition = null;

        if (_target != null)
        {
            ethanPosition = _target.transform.position;
        }

        var npcProcessInfoCache = new NPCProcessInfoCache();
        var globalEntityDictionary = new EntityDictionary();

        var internalBodyHost = GetComponent<IInternalBodyHumanoidHost>();

        var hostContext = new TestedNPCHostContext(internalBodyHost);
        mNPCProcessesContext = new TestedNPCContext(globalEntityDictionary, npcProcessInfoCache, hostContext);
        mNPCProcessesContext.RegisterInstance<INPCRayScaner>(mEnemyRayScaner);
        var blackBoard = mNPCProcessesContext.BlackBoard;
        blackBoard.PossibleIdOfRifle = possibleInstanceIdOfRifle;
        blackBoard.EthanPosition = ethanPosition;
        mNPCProcessesContext.Bootstrap();
        
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

        Task.Run(() => {
            var gameObj = ThreadSafeGameObj();

            Debug.Log($"EnemyNPC gameObj = {gameObj}");
        });
    }

    public void SetInvocableObj(IInvocableObj invokableObj)
    {
        lock (mTmpQueueLockObj)
        {
            mTmpQueue.Enqueue(invokableObj);
        }
    }

    private string ThreadSafeGameObj()
    {
        var invocable = new InvocableObj<string>(() => {
            var gunBody = GameObject.Find("M4A1 Sopmod");
            Debug.Log($"EnemyNPC End fun = () gunBody.name = {gunBody.name}");
            return gunBody.name;
        }, this);

        return invocable.Run();
    }

    private object mTmpQueueLockObj = new object();
    private Queue<IInvocableObj> mTmpQueue = new Queue<IInvocableObj>();

    private void ProcessInvocable()
    {
        List<IInvocableObj> invocableList = null;

        lock (mTmpQueueLockObj)
        {
            if (mTmpQueue.Count > 0)
            {
                invocableList = mTmpQueue.ToList();
                mTmpQueue.Clear();
            }
        }

        if(invocableList == null)
        {
            return;
        }

        foreach(var invocable in invocableList)
        {
            invocable.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("EnemyNPC Update");
        mInputKeyHelper.Update();
        ProcessInvocable();
        //mGunBody.SetActive(false);
    }

    private void OnQPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnQPressAction key = {key}");
        Debug.Log($"EnemyNPC OnQPressAction mInstanceIdOfRifle = {mInstanceIdOfRifle}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnQPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    private void OnJPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnJPressAction key = {key}");
        Debug.Log($"EnemyNPC OnJPressAction mInstanceIdOfRifle = {mInstanceIdOfRifle}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnJPressAction command = {command}");
        mNPCProcessesContext.Send(command);
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

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnBPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    private void OnMPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnMPressAction key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnMPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    private void OnFPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnFPressAction key = {key}");
        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnFPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    private void OnGPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnGPressAction key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnGPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    private void OnKPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnKPressAction key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnKPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    private void OnNPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnNPressAction key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnNPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    private void OnHPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnHPressAction key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnHPressAction command = {command}");
        mNPCProcessesContext.Send(command); 
    }

    private void OnLPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnLPressAction key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnLPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    private void OnIPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnIPressAction key = {key}");        
        var _target = GameObject.Find("Ethan");

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnIPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    private void OnPPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnPPressAction key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnPPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }
    
    private void OnUPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnUPressAction key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnUPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    void OnDestroy()
    {
        //Debug.Log("OnDestroy");
        mNPCProcessesContext?.Dispose();   
    }
}
