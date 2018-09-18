using Assets.Scripts;
using MyNPCLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IMessengerDialog: IBaseDialog
{
}

public class MessengerDialog : BaseDialog, IMessengerDialog
{
    private static object mInstanceLockObj = new object();
    private static IMessengerDialog mInstance;

    public static IMessengerDialog Instance
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
    public GameObject ScrollView;
    
    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        var sendMessgeBtn = SendMessageButton.GetComponent<Button>();
        sendMessgeBtn.onClick.AddListener(SendMessage);
        var scrollView = ScrollView.GetComponent<ScrollRect>();
    }
	
	// Update is called once per frame
	void Update ()
    {		
	}

    private void SendMessage()
    {
        LogInstance.Log("SendMessage");
    }
}
