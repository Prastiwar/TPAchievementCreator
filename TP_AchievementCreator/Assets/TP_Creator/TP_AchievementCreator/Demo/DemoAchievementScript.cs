using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TP_Achievement;

public class DemoAchievementScript : MonoBehaviour
{
    [SerializeField] TPAchievementCreator creator;
    [SerializeField] TPAchievement a_Space;
    [SerializeField] TPAchievement a_Space10;

    void Awake()
    {
        creator = FindObjectOfType<TPAchievementCreator>();
        a_Space = creator.GetAchievement("Space");
        a_Space10 = creator.GetAchievement("Space10");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            creator.AddPointTo(a_Space, 1, true, true);
            creator.AddPointTo(a_Space10, 1, true, true);
        }
    }
}