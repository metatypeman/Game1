using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;
using MyNPCLib;
using System;
using System.Threading;
using System.Linq;

[RequireComponent(typeof(HumanoidBodyHost))]
[RequireComponent(typeof(EnemyRayScaner))]
public class EnemyNPC : MonoBehaviour, IInvokingInMainThread
{
    private TestedNPCContext mNPCProcessesContext;

    private InputKeyHelper mInputKeyHelper;
    private IInternalBodyHumanoidHost mInternalBodyHumanoidHost;

    private readonly object mEntityLoggerLockObj = new object();
    private IEntityLogger mEntityLogger;

    [MethodForLoggingSupport]
    protected void Log(string message)
    {
        lock (mEntityLoggerLockObj)
        {
            mEntityLogger?.Log(message);
        }
    }

    [MethodForLoggingSupport]
    protected void Error(string message)
    {
        lock (mEntityLoggerLockObj)
        {
            mEntityLogger?.Error(message);
        }
    }

    [MethodForLoggingSupport]
    protected void Warning(string message)
    {
        lock (mEntityLoggerLockObj)
        {
            mEntityLogger?.Warning(message);
        }
    }

    // Use this for initialization
    void Start()
    {
        var internalBodyHost = GetComponent<IInternalBodyHumanoidHost>();

        mInternalBodyHumanoidHost = internalBodyHost;

        internalBodyHost.OnReady += InternalBodyHost_OnReady;
  
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

        Task.Run(() => {
            try
            {
                var gameObj = ThreadSafeGameObj();

                Log($"gameObj = {gameObj}");
            }
            catch(Exception e)
            {
                Error($"e = {e}");
            }
        });
    }

    private void InternalBodyHost_OnReady()
    {
        CreateNPCHostContext();
    }

    public void CreateNPCHostContext()
    {
        lock (mEntityLoggerLockObj)
        {
            mEntityLogger = mInternalBodyHumanoidHost.EntityLogger;
        }

        CallInMainUI(() => {
            var commonLevelHost = LevelCommonHostFactory.Get();

            Log($"(commonLevelHost == null) = {commonLevelHost == null}");

            var hostContext = new TestedNPCHostContext(mEntityLogger, mInternalBodyHumanoidHost);
            mNPCProcessesContext = new TestedNPCContext(mEntityLogger, commonLevelHost.EntityDictionary, commonLevelHost.NPCProcessInfoCache, hostContext);

            mNPCProcessesContext.Bootstrap();
        });
    }

    private void CallInMainUI(Action function)
    {
        var invocable = new InvocableInMainThreadObj(function, this);
        invocable.Run();
    }

    private TResult CallInMainUI<TResult>(Func<TResult> function)
    {
        var invocable = new InvocableInMainThreadObj<TResult>(function, this);
        return invocable.Run();
    }

    public void SetInvocableObj(IInvocableInMainThreadObj invokableObj)
    {
        lock (mTmpQueueLockObj)
        {
            mTmpQueue.Enqueue(invokableObj);
        }
    }

    private string ThreadSafeGameObj()
    {
        var invocableWithoutResult = new InvocableInMainThreadObj(() =>
        {
            var gunBody = GameObject.Find("M4A1 Sopmod");
            Log($"fun = () gunBody.name = {gunBody.name}");
            var position = gunBody.transform.position;
            Log($"End fun = () position = {position}");
        }, this);

        invocableWithoutResult.Run();

        var invocable = new InvocableInMainThreadObj<string>(() => {
            var gunBody = GameObject.Find("M4A1 Sopmod");
            Log($"End fun = () gunBody.name = {gunBody.name}");
            return gunBody.name;
        }, this);

        return invocable.Run();
    }

    private object mTmpQueueLockObj = new object();
    private Queue<IInvocableInMainThreadObj> mTmpQueue = new Queue<IInvocableInMainThreadObj>();

    private void ProcessInvocable()
    {
        List<IInvocableInMainThreadObj> invocableList = null;

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
        //Log("Begin");
        mInputKeyHelper.Update();
        ProcessInvocable();
        //mGunBody.SetActive(false);
    }

    private void OnQPressAction(KeyCode key)
    {
        Log($"key = {key}");
        
        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnJPressAction(KeyCode key)
    {
        Log($"key = {key}");
        
        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnBPressAction(KeyCode key)
    {
        Log($"key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnMPressAction(KeyCode key)
    {
        Log($"key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnFPressAction(KeyCode key)
    {
        Log($"key = {key}");
        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnGPressAction(KeyCode key)
    {
        Log($"key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnKPressAction(KeyCode key)
    {
        Log($"key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnNPressAction(KeyCode key)
    {
        Log($"key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnHPressAction(KeyCode key)
    {
        Log($"key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command); 
    }

    private void OnLPressAction(KeyCode key)
    {
        Log($"key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnIPressAction(KeyCode key)
    {
        Log($"key = {key}");        

        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnPPressAction(KeyCode key)
    {
        Log($"key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }
    
    private void OnUPressAction(KeyCode key)
    {
        Log($"key = {key}");

        var command = KeyToNPCCommandConverter.Convert(key);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    void OnDestroy()
    {
        //Log("Begin");
        mNPCProcessesContext?.Dispose();   
    }
}
