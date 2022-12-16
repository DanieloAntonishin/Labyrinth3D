using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gates1 : MonoBehaviour
{
    private const float timeout = 10;  // больше чем у чекпоинта
    private float timeleft;
    private System.Random rand;

    void Start()
    {
        timeleft = timeout;
        rand = new System.Random();
    }

    void Update()
    {
        if (timeleft < 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {

            transform.position = new Vector3(
                transform.position.x,
                transform.localScale.y * (-1f / 2 + timeleft / timeout),
                transform.position.z);

            timeleft -= Time.deltaTime - (rand.Next(0, 800) == 0 ? Time.deltaTime+0.4f :0);
        }
    }
}