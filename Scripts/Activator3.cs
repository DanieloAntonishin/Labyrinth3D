using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator3 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CheckPoint3.isActivated = true;
    }
}
