using Framework;

public class PlayAnimComponent : Entity, IAwakeSystem
{
    public void PlayAnim(string animName)
    {
  
    }

    public bool IsArriveTargetFrame(string animName, int frame)
    {
        return true;
    }

    public void Awake()
    {
    }
}