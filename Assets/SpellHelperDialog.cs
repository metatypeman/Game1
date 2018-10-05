using Assets.Scripts;
using MyNPCLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellHelperDialog : BaseDialog, ISpellHelperDialog
{
    private static object mInstanceLockObj = new object();
    private static ISpellHelperDialog mInstance;

    public static ISpellHelperDialog Instance
    {
        get
        {
            lock(mInstanceLockObj)
            {
                return mInstance;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        lock(mInstanceLockObj)
        {
            mInstance = this;
        }
    }

    public GameObject SendMessageButton;
    public GameObject InputField;
    public GameObject RepitedInputField;
    private InputField mInputField;
    private InputField mRepitedInputField;

    public event Action<string> OnSpellMessage;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        var sendMessgeBtn = SendMessageButton.GetComponent<Button>();
        sendMessgeBtn.onClick.AddListener(SendMessage);
        
        mInputField = InputField.GetComponent<InputField>();
        mRepitedInputField = RepitedInputField.GetComponent<InputField>();
    }
	
	// Update is called once per frame
	void Update ()
    {		
	}

    private void SendMessage()
    {
        //LogInstance.Log("Begin");
        var text = mInputField.text;
        //LogInstance.Log($"text = {text}");
        mInputField.text = string.Empty;
        mRepitedInputField.text = text;

        OnSpellMessage?.Invoke(text);
    }
}
