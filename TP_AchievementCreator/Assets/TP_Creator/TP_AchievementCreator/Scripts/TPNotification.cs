using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TPNotification : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;

    public void SetNotification(TPAchievement achievement)
    {
        titleText.text = achievement.Title;
        descriptionText.text = achievement.Description;
        iconImage.sprite = achievement.Icon;
    }
}
