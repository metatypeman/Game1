using SymOntoClay;
using SymOntoClay.Scriptables;
using SymOntoClay.UnityAsset.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.SymOntoClay.Editors
{
#if UNITY_EDITOR
    [CustomEditor(typeof(HumanoidNPC))]
    [CanEditMultipleObjects]
    public class HumanoidNPCEditor : Editor
    {
        private HumanoidNPC _target;

        private void OnEnable()
        {
            _target = (HumanoidNPC)target;
        }

        //protected override void OnHeaderGUI()
        //{

        //}

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();
            _target.NPCFile = (NPCFile)EditorGUILayout.ObjectField("NPC File", _target.NPCFile, typeof(NPCFile), false);
            
            var newIdValue = EditorGUILayout.TextField("Id", _target.Id);

            if(_target.Id != newIdValue && IsValidId(newIdValue))
            {
                UniqueIdRegistry.RemoveId(_target.Id);
                UniqueIdRegistry.AddId(newIdValue);

                _target.Id = newIdValue;
            }

            GUILayout.EndVertical();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_target);
            }
        }

        private bool IsValidId(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            if(!id.StartsWith("#"))
            {
                return false;
            }

            if(UniqueIdRegistry.ContainsId(id))
            {
                return false;
            }

            return true;
        }
    }
#endif
}
