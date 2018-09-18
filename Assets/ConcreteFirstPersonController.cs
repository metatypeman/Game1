using Assets.Scripts;
using MyNPCLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteFirstPersonController : MonoBehaviour
{
    private InputKeyHelper mInputKeyHelper;

    // Use this for initialization
    void Start ()
    {
        mInputKeyHelper = new InputKeyHelper();
        mInputKeyHelper.AddListener(KeyCode.Y, OnYPressAction);
    }
	
	// Update is called once per frame
	void Update ()
    {
        mInputKeyHelper.Update();
    }

    private void OnYPressAction(KeyCode key)
    {
        //var canvasComponent = GameObject.Find("Canvas");

        //LogInstance.Log($"canvasComponent != null = {canvasComponent != null}");

        MessengerDialog.Instance.Show();
    }
}
