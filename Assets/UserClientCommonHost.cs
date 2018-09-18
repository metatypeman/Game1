using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserClientCommonHost : MonoBehaviour, IUserClientCommonHost
{
    public void Awake()
    {
    }

    private bool mNeedLockingCursor;
    private UserClientMode mUserClientMode = UserClientMode.Character;
    private int mCounter;
    private readonly object mLockObj = new object();

    public float GetAxis(string name)
    {
        if(mUserClientMode == UserClientMode.Window)
        {
            return 0f;
        }

        return Input.GetAxis(name);
    }

    public bool GetKeyUp(KeyCode key)
    {
        if (mUserClientMode == UserClientMode.Window)
        {
            return false;
        }

        return Input.GetKeyUp(key);
    }

    public bool GetKeyDown(KeyCode key)
    {
        if (mUserClientMode == UserClientMode.Window)
        {
            return false;
        }

        return Input.GetKeyDown(key);
    }

    public bool GetMouseButtonUp(int button)
    {
        if (mUserClientMode == UserClientMode.Window)
        {
            return false;
        }

        return Input.GetMouseButtonUp(0);
    }

    public void SetCharacterMode()
    {
        lock(mLockObj)
        {
            var oldValue = mUserClientMode;

            mUserClientMode = UserClientMode.Character;

            if (oldValue != mUserClientMode)
            {
                mCounter = 0;
            }

            mNeedLockingCursor = true;
        }
    }

    public void AddWindow()
    {
        lock (mLockObj)
        {
            if(!mNeedLockingCursor)
            {
                return;
            }

            mUserClientMode = UserClientMode.Window;
            mCounter++;
        }
    }

    public void ReleaseWindow()
    {
        lock (mLockObj)
        {
            if (!mNeedLockingCursor)
            {
                return;
            }

            mCounter--;

            if(mCounter == 0)
            {
                mUserClientMode = UserClientMode.Character;
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if(GetKeyUp(KeyCode.Escape))
        {
            mUserClientMode = UserClientMode.Window;
        }
        else
        {
            //if(GetMouseButtonUp(0))
            //{
            //    UserClientMode = UserClientMode.Character;
            //}
        }

        if(mUserClientMode == UserClientMode.Character)
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
