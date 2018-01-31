using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Space]
    public TPNotification Notification;
}