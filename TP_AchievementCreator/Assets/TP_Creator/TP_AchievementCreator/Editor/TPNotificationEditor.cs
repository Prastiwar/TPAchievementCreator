using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TP_AchievementEditor
{
    //[CustomEditor(typeof(TPNotification))]
    public class TPNotificationEditor : ScriptlessAchievementEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Script Managing layout of notification");

            OpenCreator();
        }
    }
}