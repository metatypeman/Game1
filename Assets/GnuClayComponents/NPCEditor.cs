using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.GnuClayComponents
{
    [CustomEditor(typeof(NPC))]
    public class NPCEditor : Editor
    {
        private NPC mTarget;

        void OnEnable()
        {
            mTarget = (NPC)target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.Label("App");
            mTarget.App = GUILayout.TextField(mTarget.App);

            if (GUILayout.Button("DO CAKE"))
            {
                CreateInstance<ChooseApplicationDialog>().Open();
            }

            GUILayout.EndVertical();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(mTarget);
            }
        }
    }
}
