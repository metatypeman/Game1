using SymOntoClay;
using SymOntoClay.Scriptables;
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
            _target.HostFile = (HostFile)EditorGUILayout.ObjectField("Host File", _target.HostFile, typeof(HostFile), false);
            GUILayout.EndVertical();
        }
    }
#endif
}
