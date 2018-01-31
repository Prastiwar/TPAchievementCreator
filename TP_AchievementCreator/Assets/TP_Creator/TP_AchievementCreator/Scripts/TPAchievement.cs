using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class TPAchievement : ScriptableObject
{
    public string Title;
    [Multiline]
    public string Description;
    public bool IsCompleted;
    public Sprite Icon;
    public float Points;
    public float MaxPoints;

    public void SetNotification(Image image)
    {
        image.sprite = Icon;

    }
}