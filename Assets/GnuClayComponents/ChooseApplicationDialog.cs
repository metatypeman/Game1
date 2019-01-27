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
    public class ChooseApplicationDialog: EditorWindow
    {
        void OnGUI()
        {
            titleContent = new GUIContent("chose");

            if (GUILayout.Button("Close"))
            {
                Close();
            }
        }

        public void Open()
        {
            ShowAuxWindow();
        }
    }
}
