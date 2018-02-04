using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP.Achievement
{
    public class TPAchievement : ScriptableObject
    {
        public string Title;
        [Multiline]
        public string Description;
        public bool IsCompleted;
        public Sprite Icon;
        public float Points;
        public float MaxPoints;
        [Space]
        public TPNotification Notification;
        public float NotifyLong;
    }
}