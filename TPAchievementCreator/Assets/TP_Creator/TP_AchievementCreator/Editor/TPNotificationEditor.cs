using System.Collections;
using System.Collections.Generic;
using TP.Achievement;
using UnityEditor;

namespace TP.AchievementEditor
{
    [CustomEditor(typeof(TPNotification))]
    internal class TPNotificationEditor : ScriptlessAchievementEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Script Managing layout of notification");

            if (TPAchievementCreator.DebugMode)
                DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }
    }
}