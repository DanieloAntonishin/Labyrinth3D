using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint3 : MonoBehaviour
{
    public static bool isActivated;

    private UnityEngine.UI.Image image;
    private const float timeout = 20;
    private float timeleft;

    void Start()
    {
        isActivated = false;
        timeleft = timeout;
        image = GetComponentInChildren<UnityEngine.UI.Image>();
    }

    void Update()
    {
        if (isActivated)
        {
            if (timeleft < 0)
            {
                this.gameObject.SetActive(false);
                GameStat.CheckImage3Checked(false);
            }
            else
            {
                GameStat.Checkpoint3Fill = image.fillAmount = timeleft / timeout;
                timeleft -= Time.deltaTime;
                // меняем цвет изображению на Canvas
                image.color = new Color(0.8f - timeleft / timeout, timeleft / timeout, 0.1f);
            }
        }
    }
}
