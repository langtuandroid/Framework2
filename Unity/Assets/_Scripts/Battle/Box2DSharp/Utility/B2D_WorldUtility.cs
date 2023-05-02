using System.Numerics;
using Box2DSharp.Dynamics;

public class B2D_WorldUtility
{
    /// <summary>
    /// 创造一个物理世界
    /// </summary>
    /// <param name="gravity">重力加速度</param>
    /// <returns></returns>
    public static World CreateWorld(Vector2 gravity)
    {
        return new World(gravity);
    }
}