using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class InputMouseKeyHelper
    {
        public class InputMouseKeyHandlers
        {
            public InputMouseKeyHandlers(int button, IUserClientCommonHost userClientCommonHost)
            {
                mButton = button;
                mUserClientCommonHost = userClientCommonHost;
            }

            private IUserClientCommonHost mUserClientCommonHost;
            private int mButton;

            private readonly object mLockObj = new object();

            private event Action mPressHandlers;
            private event Action mUpHandlers;
            private bool mIsPressed;

            public void Update()
            {
                lock (mLockObj)
                {
                    var tmpKeyDownValue = mUserClientCommonHost.GetMouseButtonDown(mButton);
                    var tmpKeyUpValue = mUserClientCommonHost.GetMouseButtonUp(mButton);

                    if (tmpKeyDownValue)
                    {
                        if (mIsPressed)
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
                            if (!mIsPressed)
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

        public InputMouseKeyHelper(IUserClientCommonHost userClientCommonHost)
        {
            mUserClientCommonHost = userClientCommonHost;
        }

        private IUserClientCommonHost mUserClientCommonHost;
        private readonly object mLockObj = new object();
        private Dictionary<int, InputMouseKeyHandlers> mHandlersDict = new Dictionary<int, InputMouseKeyHandlers>();

        public void Update()
        {
            lock (mLockObj)
            {
                foreach (var handlerKVPItem in mHandlersDict)
                {
                    handlerKVPItem.Value.Update();
                }
            }
        }

        public void AddPressListener(int button, Action action)
        {
            InputMouseKeyHandlers mHandler = null;

            lock (mLockObj)
            {
                if (mHandlersDict.ContainsKey(button))
                {
                    mHandler = mHandlersDict[button];
                }
                else
                {
                    mHandler = new InputMouseKeyHandlers(button, mUserClientCommonHost);
                    mHandlersDict[button] = mHandler;
                }
            }

            mHandler.AddPressHandler(action);
        }

        public void AddUpListener(int button, Action action)
        {
            InputMouseKeyHandlers mHandler = null;

            lock (mLockObj)
            {
                if (mHandlersDict.ContainsKey(button))
                {
                    mHandler = mHandlersDict[button];
                }
                else
                {
                    mHandler = new InputMouseKeyHandlers(button, mUserClientCommonHost);
                    mHandlersDict[button] = mHandler;
                }
            }

            mHandler.AddUpHandler(action);
        }
    }
}
