using Assets.Scripts;
using MyNPCLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IMessengerDialog
{
    void Show();
}

public class MessengerDialog : MonoBehaviour, IMessengerDialog
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

    public void Awake()
    {
        lock(mInstanceLockObj)
        {
            mInstance = this;
        }
    }

    public void Show()
    {
        LogInstance.Log("Show");
        GetComponent<Canvas>().enabled = true;
        mUserClientCommonHost.UserClientMode = UserClientMode.Window;
    }

    public GameObject CloseButton;
    public GameObject SendMessageButton;
    public GameObject InputField;
    public GameObject ScrollView;
    private IUserClientCommonHost mUserClientCommonHost;

    // Use this for initialization
    void Start ()
    {
        mUserClientCommonHost = UserClientCommonHostFactory.Get();
        var closeBtn = CloseButton.GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);
        var sendMessgeBtn = SendMessageButton.GetComponent<Button>();
        sendMessgeBtn.onClick.AddListener(SendMessage);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnCloseClick()
    {
        LogInstance.Log("OnCloseClick");
        GetComponent<Canvas>().enabled = false;
        mUserClientCommonHost.UserClientMode = UserClientMode.Character;
    }

    private void SendMessage()
    {
        LogInstance.Log("SendMessage");
    }
}
