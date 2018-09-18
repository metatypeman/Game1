using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserClientCommonHost : MonoBehaviour, IUserClientCommonHost
{
    public void Awake()
    {
    }

    public UserClientMode UserClientMode { get; set; } = UserClientMode.Character;

    public float GetAxis(string name)
    {
        if(UserClientMode == UserClientMode.Window)
        {
            return 0f;
        }

        return Input.GetAxis(name);
    }

    public bool GetKeyUp(KeyCode key)
    {
        if (UserClientMode == UserClientMode.Window)
        {
            return false;
        }

        return Input.GetKeyUp(key);
    }

    public bool GetMouseButtonUp(int button)
    {
        if (UserClientMode == UserClientMode.Window)
        {
            return false;
        }

        return Input.GetMouseButtonUp(0);
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCursorLock();
    }

    public void UpdateCursorLock()
    {
        InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if(GetKeyUp(KeyCode.Escape))
        {
            UserClientMode = UserClientMode.Window;
        }
        else
        {
            //if(GetMouseButtonUp(0))
            //{
            //    UserClientMode = UserClientMode.Character;
            //}
        }

        if(UserClientMode == UserClientMode.Character)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        //if (GetKeyUp(KeyCode.Escape))
        //{
        //    mCursorIsLocked = false;
        //}
        //else
        //{
        //    if (GetMouseButtonUp(0))
        //    {
        //        mCursorIsLocked = true;
        //    }
        //}

        //if (mCursorIsLocked)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
        //}
        //else
        //{
        //    if (!mCursorIsLocked)
        //    {
        //        Cursor.lockState = CursorLockMode.None;
        //        Cursor.visible = true;
        //    }
        //}
    }
}
