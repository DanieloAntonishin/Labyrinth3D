using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGate : MonoBehaviour
{
    private const float timeout = 3;  // ?????? ??? ? ?????????
    private float timeleft;
    public static bool isActivated = false;
    void Start()
    {
        timeleft = 0;
    }
    void Update()
    {
        if (transform.localScale.y * (-1f / 2 + timeleft / timeout) < transform.localScale.y * (1f / 2) && isActivated)
        {
            transform.position = new Vector3(
                    transform.position.x,
                    transform.localScale.y * (-1f / 2 + timeleft / timeout),
                    transform.position.z);
            timeleft += Time.deltaTime;
        }
    }
}
