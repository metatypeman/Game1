using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BaseDialog: MonoBehaviour, IBaseDialog
    {
        protected virtual void Awake()
        {
        }

        public GameObject CloseButton;
        private Canvas mCanvas;
        private IUserClientCommonHost mUserClientCommonHost;

        protected virtual void Start()
        {
            mUserClientCommonHost = UserClientCommonHostFactory.Get();
            mCanvas = GetComponent<Canvas>();
            var closeBtn = CloseButton.GetComponent<Button>();
            closeBtn.onClick.AddListener(OnCloseClick);
        }

        public void ShowDialog()
        {
            LogInstance.Log("Begin");
            mCanvas.enabled = true;
            mUserClientCommonHost.AddWindow();
        }

        public void CloseDialog()
        {
            mCanvas.enabled = false;
            mUserClientCommonHost.ReleaseWindow();
        }

        private void OnCloseClick()
        {
            LogInstance.Log("Begin");
            CloseDialog();
        }
    }
}
