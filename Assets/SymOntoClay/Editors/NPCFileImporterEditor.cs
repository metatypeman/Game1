using SymOntoClay.Importers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
#endif

namespace SymOntoClay.Editors
{
#if UNITY_EDITOR
    [CustomEditor(typeof(NPCFileImporter))]
    [CanEditMultipleObjects]
    public class NPCFileImporterEditor : ScriptedImporterEditor
    {
        protected override bool needsApplyRevert => false;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Describes behaviour and information of NPC.", EditorStyles.boldLabel);
        }
    }
#endif
}
