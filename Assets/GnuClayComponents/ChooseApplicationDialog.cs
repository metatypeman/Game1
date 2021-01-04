using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Assets.GnuClayComponents
{
#if UNITY_EDITOR
    public class ChooseApplicationDialog: EditorWindow
    {
        void OnGUI()
        {
            titleContent = new GUIContent("chose");

            GUILayout.BeginVertical();



            GUILayout.EndVertical();

            //if (GUILayout.Button("Close"))
            //{
            //    Close();
            //}
        }

        public void Open()
        {
            ShowAuxWindow();
        }
    }
#endif
}
