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
    private IUserClientCommonHost mUserClientCommonHost;

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

        mUserClientCommonHost = UserClientCommonHostFactory.Get();

        mInputKeyHelper = new InputKeyHelper(mUserClientCommonHost);
        mInputKeyHelper.AddPressListener(KeyCode.F, OnFPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.G, OnGPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.K, OnKPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.N, OnNPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.H, OnHPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.L, OnLPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.I, OnIPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.P, OnPPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.U, OnUPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.M, OnMPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.B, OnBPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.J, OnJPressAction);
        mInputKeyHelper.AddPressListener(KeyCode.Q, OnQPressAction);

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

    private void OnQPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.Q);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnJPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.J);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnBPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.B);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnMPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.M);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnFPressAction()
    {
        Log("Begin");
        var command = KeyToNPCCommandConverter.Convert(KeyCode.F);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnGPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.G);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnKPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.K);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnNPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.N);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnHPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.H);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command); 
    }

    private void OnLPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.L);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnIPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.I);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    private void OnPPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.P);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }
    
    private void OnUPressAction()
    {
        Log("Begin");

        var command = KeyToNPCCommandConverter.Convert(KeyCode.U);
        Log($"command = {command}");
        mNPCProcessesContext?.Send(command);
    }

    void OnDestroy()
    {
        //Log("Begin");
        mNPCProcessesContext?.Dispose();   
    }
}
