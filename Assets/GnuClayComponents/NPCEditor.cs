using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.GnuClayComponents
{
    //[CustomEditor(typeof(NPC))]
    //public class NPCEditor : Editor
    //{
    //    private NPC mTarget;

    //    void OnEnable()
    //    {
    //        mTarget = (NPC)target;
    //    }

    //    public override void OnInspectorGUI()
    //    {
    //        GUILayout.BeginVertical();
    //        GUILayout.BeginHorizontal();
    //        GUI.enabled = false;
    //        mTarget.App = EditorGUILayout.TextField(new GUIContent("App"), mTarget.App);
    //        GUI.enabled = true;
    //        /*GUILayout.Label("App");
    //        mTarget.App = GUILayout.TextField(mTarget.App);*/
    //        if (GUILayout.Button("⊙", new GUIStyle()
    //        {
    //            fixedWidth = 15
    //        }))
    //        {
    //            var path = EditorUtility.OpenFilePanel("Open NPC application", mTarget.AppPath, "gcapp");

    //            //Debug.Log($"OnInspectorGUI path = {path}");

    //            if(!string.IsNullOrWhiteSpace(path))
    //            {
    //                var fileInfo = new FileInfo(path);

    //                //Debug.Log($"OnInspectorGUI fileInfo.Name = {fileInfo.Name}");

    //                mTarget.AppPath = path;
    //                mTarget.App = fileInfo.Name;

    //                mTarget.AppPath = EditorGUILayout.TextField(new GUIContent("AppPath"), mTarget.AppPath);

    //                EditorUtility.SetDirty(mTarget);
    //            }
    //        }

    //        GUILayout.EndHorizontal();

    //        mTarget.AppPath = EditorGUILayout.TextField(new GUIContent("AppPath"), mTarget.AppPath);

    //        GUILayout.EndVertical();

    //        if (GUI.changed)
    //        {              
    //            EditorUtility.SetDirty(mTarget);
    //        }
    //    }
    //}
}
