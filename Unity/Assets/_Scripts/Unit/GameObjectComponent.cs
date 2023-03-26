using Framework;
using UnityEngine;

public class GameObjectComponent: Entity, IAwake, IDestroy
{
    public GameObject GameObject { get; set; }
}