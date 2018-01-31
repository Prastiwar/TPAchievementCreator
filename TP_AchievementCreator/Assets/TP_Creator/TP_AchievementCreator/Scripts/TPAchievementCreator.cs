using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP_Achievement
{
    public class TPAchievementCreator : MonoBehaviour
    {
        public static bool DebugMode;
        [SerializeField] List<TPAchievement> Achievements = new List<TPAchievement>();
        [SerializeField] List<TPNotification> Notifications = new List<TPNotification>();

#if UNITY_EDITOR
        public void Refresh()
        {
            Achievements = FindAssetsByType<TPAchievement>();
            Notifications = FindAssetsByType<TPNotification>();
        }
        List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = UnityEditor.AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
#endif

        public TPAchievement GetAchievement(string name)
        {
            int length = Achievements.Count;
            for (int i = 0; i < length; i++)
            {
                if (name == Achievements[i].name)
                    return Achievements[i];
            }
            return null;
        }

        public void ShowNotification(TPAchievement achievement)
        {
            //achievement.SetNotification();
        }

        public void AddPointTo(TPAchievement achievement, bool showNotification)
        {
            achievement.Points++;
            if (achievement.Points >= achievement.MaxPoints)
            {
                achievement.Points = achievement.MaxPoints;
                CompleteAchievement(achievement);
                return;
            }

            if (showNotification)
                ShowNotification(achievement);
        }

        public void CompleteAchievement(TPAchievement achievement)
        {
            achievement.IsCompleted = true;
            ShowNotification(achievement);
        }
    }
}