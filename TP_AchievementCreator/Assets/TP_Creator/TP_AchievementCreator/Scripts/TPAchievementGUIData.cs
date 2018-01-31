using UnityEngine;

namespace TP_AchievementEditor
{
    public class TPAchievementGUIData : ScriptableObject
    {
        [HideInInspector] public GUISkin GUISkin;
        [HideInInspector] public string AchievementsPath;
    }
}