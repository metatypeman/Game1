using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    [CustomEditor(typeof(DoCake))]
    public class Editor_DoCake : Editor
    {
        // There is a variable called 'target' that comes from the Editor, its the script we are extending but to
        // make it easy to use we will decalre a new variable called '_target' that will cast this 'target' to our script type
        // otherwise you will need to cast it everytime you use it like this: int i = (ourType)target;

        DoCake _target;

        void OnEnable()
        {
            _target = (DoCake)target;
        }

        // Here is where the magic begins! You can use any GUI command here (As far as i know)
        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("The Cake Maker Script!", EditorStyles.boldLabel);
            _target.isDelicious = EditorGUILayout.Toggle("Is it Delicous Cake?", _target.isDelicious); // Our bool
            _target.amountOfChocolate = EditorGUILayout.Slider("How much Chocolate?", _target.amountOfChocolate, 0.0f, 10.0f); // A slider to make thing better looking

            // There is now ay to have a cake without chocolate
            if (_target.amountOfChocolate == 0)
            {
                EditorGUILayout.HelpBox("THERE IS NO CHOCOLATE IN THIS CAKE", MessageType.Error);
            }

            _target.randomNumber = EditorGUILayout.IntField("Just a number", _target.randomNumber); // Common INT field
            _target.cakeColor = EditorGUILayout.ColorField("Color", _target.cakeColor); // Color Field
            _target.cakeT = (DoCake.cakeTypes)EditorGUILayout.EnumPopup("Cake type", _target.cakeT); // Enum Field - It needs proper casting

            if (GUILayout.Button("DO CAKE"))
            {
                _target.BakeTheCake();
            }
            GUILayout.EndVertical();

            //If we changed the GUI aply the new values to the script
            if (GUI.changed)
            {
                EditorUtility.SetDirty(_target);
            }
        }
    }
}
