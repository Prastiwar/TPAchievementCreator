using UnityEngine;

namespace TP.AchievementEditor
{
    public class TPAchievementGUIData : ScriptableObject
    {
        [HideInInspector] public GUISkin GUISkin;
        [HideInInspector] public string AchievementsPath;
    }
}