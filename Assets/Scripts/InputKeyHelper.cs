using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public delegate void KeyPressAction(KeyCode key);

    public class InputKeyHelper
    {
        public class InputKeyHandlers
        {
            public InputKeyHandlers(KeyCode keyCode)
            {
                mKeyCode = keyCode;
            }

            private KeyCode mKeyCode;

            private object mLockObj = new object();

            private event KeyPressAction mHandlers;
            private bool mIsPressed;

            public void Update()
            {
                lock(mLockObj)
                {
                    var tmpKeyDownValue = Input.GetKeyDown(mKeyCode);
                    var tmpKeyUpValue = Input.GetKeyUp(mKeyCode);

                    if(tmpKeyDownValue)
                    {
                        if(mIsPressed)
                        {
                            return;
                        }

                        mIsPressed = true;
                        mHandlers?.Invoke(mKeyCode);
                    }
                    else
                    {
                        if (tmpKeyUpValue)
                        {
                            if(!mIsPressed)
                            {
                                return;
                            }

                            mIsPressed = false;
                        }
                    }            
                }
            }

            public void AddHandler(KeyPressAction action)
            {
                lock (mLockObj)
                {
                    mHandlers += action;
                }
            }
        }

        private object mLockObj = new object();
        private Dictionary<KeyCode, InputKeyHandlers> mHandlersDict = new Dictionary<KeyCode, InputKeyHandlers>();

        public void Update()
        {
            lock (mLockObj)
            {
                foreach(var handlerKVPItem in mHandlersDict)
                {
                    handlerKVPItem.Value.Update();
                }
            }
        }

        public void AddListener(KeyCode key, KeyPressAction action)
        {
            InputKeyHandlers mHandler = null;

            lock (mLockObj)
            {
                if (mHandlersDict.ContainsKey(key))
                {
                    mHandler = mHandlersDict[key];
                }
                else
                {
                    mHandler = new InputKeyHandlers(key);
                    mHandlersDict[key] = mHandler;
                }
            }

            mHandler.AddHandler(action);
        }
    }
}
