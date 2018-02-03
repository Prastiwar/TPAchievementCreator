using System.Collections;
using System.Collections.Generic;
using TP_Achievement;
using UnityEditor;

namespace TP_AchievementEditor
{
    [CustomEditor(typeof(TPNotification))]
    public class TPNotificationEditor : ScriptlessAchievementEditor
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