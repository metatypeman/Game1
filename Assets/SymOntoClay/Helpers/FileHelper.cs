using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

namespace SymOntoClay.Helpers
{
    public static class FileHelper
    {
        public static string RemoveCommonFragment(string fileName)
        {
            if (fileName.StartsWith("Assets/"))
            {
                return fileName.Replace("Assets/", string.Empty).Trim();
            }

            return fileName;
        }
    }
}
