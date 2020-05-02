using SymOntoClay.UnityAsset.Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SymOntoClay.Unity3D
{
    public class BipedNPC : MonoBehaviour, IHostListener
    {
        public TextAsset LogicFile;
        public TextAsset HostFile;

        private IBipedNPC _logicAgent;

        protected virtual void Awake()
        {
            var settings = new BipedNPCSettings();

            if (LogicFile != null)
            {
                settings.LogicFile = FileHelper.GetFullPath(LogicFile);
            }

            if (HostFile != null)
            {
                settings.HostFile = FileHelper.GetFullPath(HostFile);
            }

            settings.HostListener = this;

            Debug.Log($"Awake settings = {settings}");

            _logicAgent = WorldCore.Instance.GetBipedNPC(settings);
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        ICommandCallResult IHostListener.CallCommand(ICommand command)
        {
            throw new System.NotImplementedException();
        }

        ICommandInfo IHostListener.GetCommandInfo(ICommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
