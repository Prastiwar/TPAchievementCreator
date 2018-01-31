using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP_Achievement
{
    public class TPAchievementCreator : MonoBehaviour
    {
        public static bool DebugMode;
        public List<TPAchievement> Achievements = new List<TPAchievement>();
        public delegate void NotifyEventHandler(GameObject notification, bool toActive);
        NotifyEventHandler OnNotifyActive;
        List<GameObject> _NotifyGO = new List<GameObject>();
        Queue<TPNotification> Notifications = new Queue<TPNotification>();


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

        GameObject GetNotificationObject(TPAchievement achievement)
        {
            int length = _NotifyGO.Count;
            for (int i = 0; i < length; i++)
            {
                if (_NotifyGO[i] == achievement.Notification.gameObject)
                    return _NotifyGO[i];
            }
            return null;
        }

        void SetActive(GameObject notification, bool toActive)
        {
            notification.SetActive(toActive);
        }

        public void ShowNotification(TPAchievement achievement, bool Active)
        {
            GameObject go = GetNotificationObject(achievement);
            if (go == null)
            {
                go = Instantiate(achievement.Notification.gameObject);
                go.SetActive(false);
                _NotifyGO.Add(go);
            }

            if (OnNotifyActive == null)
                OnNotifyActive = SetActive;

            achievement.Notification.SetNotification(achievement);
            StartCoroutine(IEShowNotification(go, achievement, Active));
        }

        IEnumerator IEShowNotification(GameObject go, TPAchievement achievement, bool Active)
        {
            OnNotifyActive(go, Active);
            if (Active)
                Notifications.Enqueue(achievement.Notification);
            else
                Notifications.Dequeue();

            foreach (var item in Notifications)
            {
                //ShowNotification();
            }

            yield return null;
        }

        public void AddPointTo(TPAchievement achievement, bool showNotification)
        {
            achievement.Points++;
            if (achievement.Points >= achievement.MaxPoints)
            {
                CompleteAchievement(achievement, showNotification);
                return;
            }

            if (showNotification)
                ShowNotification(achievement, true);
        }

        public void CompleteAchievement(TPAchievement achievement, bool showNotification)
        {
            achievement.Points = achievement.MaxPoints;
            achievement.IsCompleted = true;
            ShowNotification(achievement, true);
        }



#if UNITY_EDITOR
        public void Refresh()
        {
            Achievements = FindAssetsByType<TPAchievement>();
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
    }
}