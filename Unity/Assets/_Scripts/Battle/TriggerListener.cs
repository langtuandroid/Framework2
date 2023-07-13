using System;
using UnityEngine;

public class TriggerListener : MonoBehaviour
{
    public event Action<GameObject> TriggerEnter;
    public event Action<GameObject> TriggerExit;
    public event Action<GameObject> TriggerStay;
    private void OnTriggerEnter(Collider other)
    {
        TriggerEnter?.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerExit?.Invoke(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        TriggerStay?.Invoke(other.gameObject);
    }
}