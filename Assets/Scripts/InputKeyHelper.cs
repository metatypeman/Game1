using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class InputKeyHelper
    {
        public class InputKeyHandlers
        {
            public InputKeyHandlers(KeyCode keyCode, IUserClientCommonHost userClientCommonHost)
            {
                mKeyCode = keyCode;
                mUserClientCommonHost = userClientCommonHost;
            }

            private IUserClientCommonHost mUserClientCommonHost;
            private KeyCode mKeyCode;

            private readonly object mLockObj = new object();

            private event Action mPressHandlers;
            private event Action mUpHandlers;
            private bool mIsPressed;

            public void Update()
            {
                lock(mLockObj)
                {
                    var tmpKeyDownValue = mUserClientCommonHost.GetKeyDown(mKeyCode);
                    var tmpKeyUpValue = mUserClientCommonHost.GetKeyUp(mKeyCode);

                    if(tmpKeyDownValue)
                    {
                        if(mIsPressed)
                        {
                            return;
                        }

                        mIsPressed = true;
                        mPressHandlers?.Invoke();
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
                            mUpHandlers?.Invoke();
                        }
                    }            
                }
            }

            public void AddPressHandler(Action action)
            {
                lock (mLockObj)
                {
                    mPressHandlers += action;
                }
            }

            public void AddUpHandler(Action action)
            {
                lock (mLockObj)
                {
                    mUpHandlers += action;
                }
            }
        }

        public InputKeyHelper(IUserClientCommonHost userClientCommonHost)
        {
            mUserClientCommonHost = userClientCommonHost;
        }

        private IUserClientCommonHost mUserClientCommonHost;
        private readonly object mLockObj = new object();
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

        public void AddPressListener(KeyCode key, Action action)
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
                    mHandler = new InputKeyHandlers(key, mUserClientCommonHost);
                    mHandlersDict[key] = mHandler;
                }
            }

            mHandler.AddPressHandler(action);
        }

        public void AddUpListener(KeyCode key, Action action)
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
                    mHandler = new InputKeyHandlers(key, mUserClientCommonHost);
                    mHandlersDict[key] = mHandler;
                }
            }

            mHandler.AddUpHandler(action);
        }
    }
}
