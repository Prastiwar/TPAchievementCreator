using UnityEditor;
using TP.Achievement;

namespace TP.AchievementEditor
{
    [CustomEditor(typeof(TPAchievementCreator))]
    internal class TPAchievementCreatorEditor : ScriptlessAchievementEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Script which allows you managing persistance");

            if (TPAchievementCreator.DebugMode)
                DrawPropertiesExcluding(serializedObject, scriptField);

            OpenCreator();
        }

    }
}