using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TP_Achievement;

public class DemoAchievementScript : MonoBehaviour
{
    TPAchievementCreator creator;
    TPAchievement a_Space;
    TPAchievement a_Space10;
    WaitForSeconds waitForSec = new WaitForSeconds(0.02f);

    void Awake()
    {
        creator = FindObjectOfType<TPAchievementCreator>();

        creator.SetOnNotifySet(ChangeNotificationBehavior);
        creator.SetOnNotifyActive(ChangeNotifyActiveBehavior);

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

    void ChangeNotifyActiveBehavior(GameObject notification, bool toActive)
    {
        StartCoroutine(Fading(notification, toActive));
    }

    IEnumerator Fading(GameObject notification, bool toActive)
    {
        CanvasGroup group = notification.GetComponent<CanvasGroup>();

        if (toActive)
        {
            group.alpha = 0;
            notification.SetActive(toActive);

            while (group.alpha <= 0.5f)
            {
                group.alpha += 0.1f;
                yield return waitForSec;
            }
            group.alpha = 1;
        }
        else
        {
            group.alpha = 1;

            while (group.alpha >= 0.5f)
            {
                group.alpha -= 0.1f;
                yield return waitForSec;
            }
            group.alpha = 0;
            notification.SetActive(toActive);
        }
    }

    void ChangeNotificationBehavior(TPNotification notification, TPAchievement achievement)
    {
        if (achievement.IsCompleted)
        {
            notification.descriptionText.enabled = true;
            notification.maxPointsText.gameObject.SetActive(false);
            notification.pointsText.gameObject.SetActive(false);
        }
        else
        {
            notification.descriptionText.enabled = false;
            notification.maxPointsText.gameObject.SetActive(true);
            notification.pointsText.gameObject.SetActive(true);
        }
        notification.SetNotification(achievement);
    }
}