using Framework;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject go;
    private void Start()
    {
        var triggerListener = Instantiate(go).GetOrAddComponent<TriggerListener>();
        Log.Msg(Time.frameCount);
        triggerListener.TriggerEnter += OnTriggerEnter1;
    }

    private void OnTriggerEnter1(GameObject other)
    {
        Log.Msg($"碰到了{other.name} {Time.frameCount}");
    } 
}