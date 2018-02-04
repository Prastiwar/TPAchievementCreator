using UnityEngine;
using UnityEditor;
using TP.Achievement;

namespace TP.AchievementEditor
{
    [CustomEditor(typeof(TPAchievementGUIData))]
    public class TPSoundManagerGUIDataEditor : ScriptlessAchievementEditor
    {
        TPAchievementGUIData TPAchievementData;

        void OnEnable()
        {
            TPAchievementData = (TPAchievementGUIData)target;
            if (serializedObject.targetObject.hideFlags != HideFlags.NotEditable)
                serializedObject.targetObject.hideFlags = HideFlags.NotEditable;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Container for editor data");
            if (!TPAchievementCreator.DebugMode)
                return;

            EditorGUILayout.LabelField("GUI Skin");
            TPAchievementData.GUISkin =
                (EditorGUILayout.ObjectField(TPAchievementData.GUISkin, typeof(GUISkin), true) as GUISkin);

            EditorGUILayout.LabelField("New achievement files - Path");
            EditorGUILayout.BeginHorizontal();
            TPAchievementData.AchievementsPath = EditorGUILayout.TextField(TPAchievementData.AchievementsPath);
            EditorGUILayout.EndHorizontal();

            if (GUI.changed)
                EditorUtility.SetDirty(TPAchievementData);
        }
    }
}