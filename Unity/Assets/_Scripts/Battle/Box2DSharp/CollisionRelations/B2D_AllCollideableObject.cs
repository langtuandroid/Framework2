using System;
using Sirenix.OdinInspector;

namespace ET
{
    [Flags]
    public enum B2D_AllCollideableObject
    {
        [LabelText("己方小兵")]
        FirendSoldier = 1 << 1,

        [LabelText("敌方小兵")]
        EnemySoldier = 1 << 2,

        [LabelText("小兵")]
        Soldier = FirendSoldier | EnemySoldier,

        [LabelText("自己")]
        Self = 1 << 3,

        [LabelText("队友（英雄）")]
        Teammate = 1 << 4,

        [LabelText("敌人（英雄）")]
        EnemyHeros = 1 << 5,

        [LabelText("英雄")]
        Hero = Self | Teammate | EnemyHeros,
    }
}