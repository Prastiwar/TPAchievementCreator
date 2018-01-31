using UnityEditor;
using TP_Achievement;

namespace TP_AchievementEditor
{
    [CustomEditor(typeof(TPAchievementCreator))]
    public class TPAchievementCreatorEditor : ScriptlessAchievementEditor
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