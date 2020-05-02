using SymOntoClay.UnityAsset.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SymOntoClay.Unity3D
{
    public class World : MonoBehaviour
    {
        public TextAsset ModulesDirLocator;
        public TextAsset DictionariesDirLocator;
        public TextAsset HostFile;

        protected virtual void Awake()
        {
            var settings = new WorldSettings();

            settings.ImagesRootDir = Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "Game1", "Images");

            if(ModulesDirLocator != null)
            {
                settings.SourceFilesDirs = new List<string>() { FileHelper.GetFullPathByLocator(ModulesDirLocator) };
            }

            if(DictionariesDirLocator != null)
            {
                settings.DictionariesDirs = new List<string>() { FileHelper.GetFullPathByLocator(DictionariesDirLocator) };
            }

            if (HostFile != null)
            {
                settings.HostFile = FileHelper.GetFullPath(HostFile);
            }

            var loggingSettings = new LoggingSettings();
            settings.Logging = loggingSettings;

            loggingSettings.Enable = true;
            loggingSettings.LogDir = Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "Game1", "Logging");
            loggingSettings.PlatformLoggers = new List<IPlatformLogger>() { PlatformLogger.Instance };
            
            loggingSettings.EnableRemoteConnection = true;
            loggingSettings.RootContractName = "Game1";

            Debug.Log($"Awake settings = {settings}");

            WorldCore.Instance.SetSettings(settings);
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            Debug.Log("Start");
        }
    }
}
