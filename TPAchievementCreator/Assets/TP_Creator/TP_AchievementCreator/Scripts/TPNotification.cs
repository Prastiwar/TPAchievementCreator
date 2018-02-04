using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TP.Achievement
{
    public class TPNotification : MonoBehaviour
    {
        public Image iconImage;
        public TextMeshProUGUI pointsText;
        public TextMeshProUGUI maxPointsText;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public TPAchievement OnAchievement { get; private set; }

        public void SetNotification(TPAchievement achievement)
        {
            OnAchievement = achievement;
            iconImage.sprite = achievement.Icon;
            titleText.text = achievement.Title;
            descriptionText.text = achievement.Description;
            pointsText.text = achievement.Points.ToString();
            maxPointsText.text = achievement.MaxPoints.ToString();
        }
    }
}