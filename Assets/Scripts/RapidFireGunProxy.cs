using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class RapidFireGunProxy: IRapidFireGun
    {
        public RapidFireGunProxy()
        {
        }

        public RapidFireGunProxy(IRapidFireGun instance)
        {
            Instance = instance;
        }

        private object mLockObj = new object();
        private IRapidFireGun mInstance;

        public IRapidFireGun Instance
        {
            get
            {
                lock(mLockObj)
                {
                    return mInstance;
                }
            }

            set
            {
                lock (mLockObj)
                {
                    if(mInstance == value)
                    {
                        return;
                    }

                    var oldInstance = mInstance;
                    mInstance = value;

                    if(oldInstance != null)
                    {
                        oldInstance.OnFire -= OnFireHandler;
                    }

                    if(mInstance != null)
                    {
                        mInstance.OnFire += OnFireHandler;
                    }
                }
            }
        }

        public bool IsReady
        {
            get
            {
                lock (mLockObj)
                {
                    if (mInstance != null)
                    {
                        return mInstance.IsReady;
                    }

                    return false;
                }
            }
        }

        public bool UseDebugLine
        {
            get
            {
                lock (mLockObj)
                {
                    if(mInstance != null)
                    {
                        return mInstance.UseDebugLine;
                    }

                    return false;
                }
            }

            set
            {
                lock (mLockObj)
                {
                    if (mInstance != null)
                    {
                        mInstance.UseDebugLine = value;
                    }
                }
            }
        }

        public FireMode FireMode
        {
            get
            {
                lock (mLockObj)
                {
                    if (mInstance != null)
                    {
                        return mInstance.FireMode;
                    }

                    return FireMode.Single;
                }
            }

            set
            {
                lock (mLockObj)
                {
                    if (mInstance != null)
                    {
                        mInstance.FireMode = value;
                    }
                }
            }
        }
        public TurnState TurnState
        {
            get
            {
                lock (mLockObj)
                {
                    if (mInstance != null)
                    {
                        return mInstance.TurnState;
                    }

                    return TurnState.Off;
                }
            }

            set
            {
                lock (mLockObj)
                {
                    if (mInstance != null)
                    {
                        mInstance.TurnState = value;
                    }
                }
            }
        }

        private void OnFireHandler()
        {
            OnFire?.Invoke();
        }

        public event Action OnFire;

        public float GetCorrectingAngle(Vector3 targetPos)
        {
            lock (mLockObj)
            {
                if (mInstance != null)
                {
                    return mInstance.GetCorrectingAngle(targetPos);
                }

                return 0f;
            }
        }

        public bool SetToHandsOfHumanoid(IHumanoid humanoid)
        {
            lock (mLockObj)
            {
                if (mInstance != null)
                {
                    return mInstance.SetToHandsOfHumanoid(humanoid);
                }

                return false;           
            }
        }

        public bool SetAsAloneAndHide()
        {
            lock (mLockObj)
            {
                if (mInstance != null)
                {
                    return mInstance.SetAsAloneAndHide();
                }

                return false;
            }
        }

        public bool ThrowOutToSurface()
        {
            lock (mLockObj)
            {
                if (mInstance != null)
                {
                    return mInstance.ThrowOutToSurface();
                }

                return false;
            }
        }
    }
}
