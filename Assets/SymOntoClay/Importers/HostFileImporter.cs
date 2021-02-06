using SymOntoClay.Scriptables;
using SymOntoClay.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AssetImporters;
#endif

namespace SymOntoClay.Importers
{
#if UNITY_EDITOR
    [ScriptedImporter(0, ".host")]
    public class HostFileImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var fileName = FileHelper.RemoveCommonFragment(ctx.assetPath);
            var obj = ObjectFactory.CreateInstance<HostFile>();
            obj.name = Path.GetFileNameWithoutExtension(fileName);
            obj.FullName = fileName;
            ctx.AddObjectToAsset("main", obj);
            ctx.SetMainObject(obj);
        }
    }
#endif
}
