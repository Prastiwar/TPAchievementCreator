﻿using UnityEngine;
using UnityEditor;
using TP_Achievement;

namespace TP_AchievementEditor
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

            if (GUI.changed)
                EditorUtility.SetDirty(TPAchievementData);
        }
    }
}