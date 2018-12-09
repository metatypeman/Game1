using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public delegate void OnKeyPressAction(KeyCode keyCode);

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

            private event OnKeyPressAction mPressHandlers;
            private event OnKeyPressAction mUpHandlers;
            private event Action mPressActionsHadnlers;
            private event Action mUpActionsHandlers;
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
                        mPressHandlers?.Invoke(mKeyCode);
                        mPressActionsHadnlers?.Invoke();
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
                            mUpHandlers?.Invoke(mKeyCode);
                            mUpActionsHandlers?.Invoke();
                        }
                    }            
                }
            }

            public void AddPressHandler(OnKeyPressAction action)
            {
                lock (mLockObj)
                {
                    mPressHandlers += action;
                }
            }

            public void AddUpHandler(OnKeyPressAction action)
            {
                lock (mLockObj)
                {
                    mUpHandlers += action;
                }
            }

            public void AddPressHandler(Action action)
            {
                lock (mLockObj)
                {
                    mPressActionsHadnlers += action;
                }
            }

            public void AddUpHandler(Action action)
            {
                lock (mLockObj)
                {
                    mUpActionsHandlers += action;
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

        private InputKeyHandlers GetHandler(KeyCode key)
        {
            lock (mLockObj)
            {
                if (mHandlersDict.ContainsKey(key))
                {
                    return mHandlersDict[key];
                }

                var handler = new InputKeyHandlers(key, mUserClientCommonHost);
                mHandlersDict[key] = handler;
                return handler;
            }
        }

        public void AddPressListener(KeyCode key, OnKeyPressAction action)
        {
            var handler = GetHandler(key);
            handler.AddPressHandler(action);
        }

        public void AddPressListener(KeyCode key, Action action)
        {
            var handler = GetHandler(key);
            handler.AddPressHandler(action);
        }

        public void AddUpListener(KeyCode key, OnKeyPressAction action)
        {
            var handler = GetHandler(key);
            handler.AddUpHandler(action);
        }

        public void AddUpListener(KeyCode key, Action action)
        {
            var handler = GetHandler(key);
            handler.AddUpHandler(action);
        }
    }
}
