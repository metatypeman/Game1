using SymOntoClay.Helpers;
using SymOntoClay.Scriptables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.Experimental.AssetImporters;
#endif
using UnityEngine;

namespace SymOntoClay.Importers
{
#if UNITY_EDITOR
    [ScriptedImporter(0, ".npc")]
    public class NPCFileImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var fileName = FileHelper.RemoveCommonFragment(ctx.assetPath);
            var obj = ObjectFactory.CreateInstance<NPCFile>();
            obj.name = Path.GetFileNameWithoutExtension(fileName);
            obj.FullName = fileName;
            ctx.AddObjectToAsset("main", obj);
            ctx.SetMainObject(obj);
        }
    }
#endif
}
