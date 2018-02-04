using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP.Achievement
{
    public class TPAchievementCreator : MonoBehaviour
    {
        public static bool DebugMode;

        public List<TPAchievement> Achievements = new List<TPAchievement>();
        Dictionary<string, GameObject> _NotifyGO = new Dictionary<string, GameObject>();
        Queue<TPAchievement> AchievementQueue = new Queue<TPAchievement>();

        WaitForSeconds Waiter;
        WaitUntil Until;
        float WaitSeconds;
        [SerializeField] bool IsNotify;

        public delegate void NotifyActivationEventHandler(GameObject notification, bool toActive);
        public delegate void NotifySettingEventHandler(TPNotification notification, TPAchievement achievement);
        NotifySettingEventHandler _onNotifySet;
        NotifyActivationEventHandler _onNotifyActive;

        NotifyActivationEventHandler OnNotifyActive
        {
            get
            {
                if (_onNotifyActive == null)
                    _onNotifyActive = SetActive;
                return _onNotifyActive;
            }
            set { _onNotifyActive = value; }
        }
        NotifySettingEventHandler OnNotifySet
        {
            get
            {
                if (_onNotifySet == null)
                    _onNotifySet = SetNotification;
                return _onNotifySet;
            }

            set{ _onNotifySet = value; }
        }


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
            string _name = achievement.Notification.gameObject.name;

            if (_NotifyGO.ContainsKey(_name))
            {
                return _NotifyGO[_name];
            }

            return null;
        }

        void SetActive(GameObject notification, bool toActive)
        {
            notification.SetActive(toActive);
        }

        void ShowNotification()
        {
            IsNotify = true;
            TPAchievement achievement = AchievementQueue.Dequeue();
            GameObject GO = achievement.Notification.gameObject;

            if (!_NotifyGO.ContainsKey(GO.name))
            {
                string _GOName = GO.name;
                GO = Instantiate(GO);
                GO.SetActive(false);
                _NotifyGO.Add(_GOName, GO);
            }
            else
            {
                GO = GetNotificationObject(achievement);
            }
            TPNotification notification = GO.GetComponent<TPNotification>();
            OnNotifySet(notification, achievement);

            StartCoroutine(IEShowNotification(GO, achievement));
        }

        void SetNotification(TPNotification notification, TPAchievement achievement)
        {
            notification.SetNotification(achievement);
        }

        IEnumerator IEShowNotification(GameObject go, TPAchievement achievement)
        {
            OnNotifyActive(go, true);

            if (WaitSeconds != achievement.NotifyLong)
            {
                WaitSeconds = achievement.NotifyLong;
                Waiter = new WaitForSeconds(achievement.NotifyLong);
            }

            yield return Waiter;

            OnNotifyActive(go, false);

            yield return Waiter;

            if (Until == null)
            {
                Until = new WaitUntil(() => !go.activeSelf);
            }

            yield return Until;

            if (AchievementQueue.Count > 0)
            {
                ShowNotification();
            }
            else
            {
                IsNotify = false;
            }
        }

        public void ShowNotification(TPAchievement achievement)
        {
            if (!AchievementQueue.Contains(achievement))
            {
                AchievementQueue.Enqueue(achievement);
                if (!IsNotify)
                    ShowNotification();
            }
        }
        
        /// <param name="achievement"></param>
        /// <param name="value">How many points add to achievement?</param>
        /// <param name="showProgressNotify">Should show notification with progress?</param>
        /// <param name="showNotifyAfterCompleted">Should show notification if achievement will'be completed after adding points?</param>
        public void AddPointTo(TPAchievement achievement, float value, bool showProgressNotify, bool showNotifyAfterCompleted)
        {
            if (achievement.IsCompleted)
                return;

            achievement.Points += value;
            if (achievement.Points >= achievement.MaxPoints)
            {
                CompleteAchievement(achievement, showNotifyAfterCompleted);
                return;
            }

            if (showProgressNotify)
                ShowNotification(achievement);
        }

        public void CompleteAchievement(TPAchievement achievement, bool showNotification)
        {
            achievement.Points = achievement.MaxPoints;
            achievement.IsCompleted = true;
            if(showNotification)
                ShowNotification(achievement);
        }

        public void SetOnNotifySet(NotifySettingEventHandler _NotifySettingEventHandler)
        {
            OnNotifySet = _NotifySettingEventHandler;
        }

        public void SetOnNotifyActive(NotifyActivationEventHandler _NotifyActivationEventHandler)
        {
            OnNotifyActive = _NotifyActivationEventHandler;
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