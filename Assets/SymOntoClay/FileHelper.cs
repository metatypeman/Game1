using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace SymOntoClay.Unity3D
{
    public static class FileHelper
    {
        public static string GetFullPath(UnityEngine.Object assetObject)
        {
            var localPath = AssetDatabase.GetAssetPath(assetObject);
            var rootPath = Directory.GetParent(UnityEngine.Windows.Directory.localFolder).FullName;
            var fullPath = Path.Combine(rootPath, localPath);
            return fullPath;
        }

        public static string GetFullPathByLocator(UnityEngine.Object assetObject)
        {
            var localPath = AssetDatabase.GetAssetPath(assetObject);
            var rootPath = Directory.GetParent(UnityEngine.Windows.Directory.localFolder).FullName;
            var fullPath = Path.Combine(rootPath, localPath);
            var fileInfo = new FileInfo(localPath);
            fullPath = fileInfo.Directory.FullName;
            return fullPath;
        }
    }
}
