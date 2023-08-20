using Framework;
using UnityEngine;

public class CameraComponent : Singleton<CameraComponent>, ISingletonAwake
{
    public Camera CurrentCamera { get; private set; }

    public void Awake()
    {
        CurrentCamera = Camera.main;
    }
}