using System;
using UnityEngine;

public class TriggerCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print($"{other.name} {Time.frameCount}");
    }
}