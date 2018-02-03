using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TPNotification : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI maxPointsText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TPAchievement OnAchievement;

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
