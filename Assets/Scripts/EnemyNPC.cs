﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
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

    [MethodForLoggingSupport]
    protected void Log(string message)
    {
        mEntityLogger?.Log(message);
    }

    [MethodForLoggingSupport]
    protected void Error(string message)
    {
        mEntityLogger?.Error(message);
    }

    [MethodForLoggingSupport]
    protected void Warning(string message)
    {
        mEntityLogger?.Warning(message);
    }

    // Use this for initialization
    void Start()
    {
        var commonLevelHost = LevelCommonHostFactory.Get();

        Log($"(commonLevelHost == null) = {commonLevelHost == null}");

        var internalBodyHost = GetComponent<IInternalBodyHumanoidHost>();

        var hostContext = new TestedNPCHostContext(internalBodyHost);
        mNPCProcessesContext = new TestedNPCContext(commonLevelHost.EntityDictionary, commonLevelHost.NPCProcessInfoCache, hostContext, commonLevelHost.QueriesCache);

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

        Task.Run(() => {
            try
            {
                var gameObj = ThreadSafeGameObj();

                Debug.Log($"EnemyNPC Start gameObj = {gameObj}");
            }
            catch(Exception e)
            {
                Debug.LogError($"EnemyNPC Start e = {e}");
            }
        });
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
            Debug.Log($"EnemyNPC fun = () gunBody.name = {gunBody.name}");
            var position = gunBody.transform.position;
            Debug.Log($"EnemyNPC End fun = () position = {position}");
        }, this);

        invocableWithoutResult.Run();

        var invocable = new InvocableInMainThreadObj<string>(() => {
            var gunBody = GameObject.Find("M4A1 Sopmod");
            Debug.Log($"EnemyNPC End fun = () gunBody.name = {gunBody.name}");
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
        //Debug.Log("EnemyNPC Update");
        mInputKeyHelper.Update();
        ProcessInvocable();
        //mGunBody.SetActive(false);
    }

    private void OnQPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnQPressAction key = {key}");
        
        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnQPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    private void OnJPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnJPressAction key = {key}");
        
        var command = KeyToNPCCommandConverter.Convert(key);
        Debug.Log($"EnemyNPC OnJPressAction command = {command}");
        mNPCProcessesContext.Send(command);
    }

    private void OnBPressAction(KeyCode key)
    {
        Debug.Log($"EnemyNPC OnBPressAction key = {key}");

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
