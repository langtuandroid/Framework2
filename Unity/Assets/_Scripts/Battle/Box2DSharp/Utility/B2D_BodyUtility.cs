using Box2DSharp.Dynamics;

/// <summary>
/// Box2D刚体实用函数集
/// </summary>
public static class B2D_BodyUtility
{
    /// <summary>
    /// 创造一个动态刚体
    /// </summary>
    public static Body CreateDynamicBody(this B2D_WorldComponent self)
    {
        return self.GetWorld().CreateBody(new BodyDef() { BodyType = BodyType.DynamicBody, AllowSleep = false });
    }

    /// <summary>
    /// 创造一个静态刚体
    /// </summary>
    public static Body CreateStaticBody(this B2D_WorldComponent self)
    {
        return self.GetWorld().CreateBody(new BodyDef() { BodyType = BodyType.StaticBody });
    }
}