using System.Collections.Generic;
using Framework;
using UnityEngine;

public class Darius_Q_CollisionHandler : AB2D_CollisionHandler
{
    public override void HandleCollisionStart(Unit a, Unit b)
    {
        B2D_ColliderComponent aColliderComponent = a.GetComponent<B2D_ColliderComponent>();
        B2D_RoleCastComponent aRole = aColliderComponent.BelongToUnit.GetComponent<B2D_RoleCastComponent>();

        B2D_ColliderComponent bColliderComponent = b.GetComponent<B2D_ColliderComponent>();
        B2D_RoleCastComponent bRole = bColliderComponent.BelongToUnit.GetComponent<B2D_RoleCastComponent>();

        RoleCast roleCast = aRole.GetRoleCastToTarget(bColliderComponent.BelongToUnit);
        RoleTag roleTag = bRole.RoleTag;
        B2D_CollisionRelationConfig serverB2SCollisionRelationConfig =
            B2D_CollisionRelationConfigFactory.Instance.Get(aColliderComponent
                .B2D_CollisionRelationConfigId);

        switch (roleCast)
        {
            case RoleCast.Adverse:
                switch (roleTag)
                {
                    case RoleTag.Hero:
                        if (serverB2SCollisionRelationConfig.EnemyHero)
                        {
                            //获取目标SkillCanvas
                            List<NP_RuntimeTree> targetSkillCanvas = aColliderComponent.GetParent<Unit>()
                                .GetComponent<SkillCanvasManagerComponent>()
                                .GetSkillCanvas(
                                    SkillCanvasDataFactory.Instance.Get(10006).BelongToSkillId);

                            //敌方英雄
                            if (Vector3.Distance(aColliderComponent.BelongToUnit.Position,
                                    bColliderComponent.BelongToUnit.Position) >= 2.3f)
                            {
                                //Log.Info("Q技能打到了诺克，外圈，开始添加Buff");

                                foreach (var skillCanvas in targetSkillCanvas)
                                {
                                    skillCanvas.GetBlackboard().Set("Darius_QOutIsHitUnit", true);
                                    skillCanvas.GetBlackboard().Get<List<long>>("Darius_QOutHitUnitIds")
                                        ?.Add(bColliderComponent.BelongToUnit.Id);
                                }
                            }
                            else
                            {
                                //Log.Info("Q技能打到了诺克，内圈，开始添加Buff");

                                foreach (var skillCanvas in targetSkillCanvas)
                                {
                                    skillCanvas.GetBlackboard().Set("Darius_QInnerIsHitUnit", true);
                                    skillCanvas.GetBlackboard().Get<List<long>>("Darius_QInnerHitUnitIds")
                                        ?.Add(bColliderComponent.BelongToUnit.Id);
                                }
                            }
                        }

                        break;
                }

                break;
            case RoleCast.Friendly:
                switch (roleTag)
                {
                    case RoleTag.Hero:
                        if (serverB2SCollisionRelationConfig.FriendlyHero)
                        {
                        }

                        break;
                }

                break;
        }
    }

    public override void HandleCollisionStay(Unit a, Unit b)
    {
    }

    public override void HandleCollisionEnd(Unit a, Unit b)
    {
    }
}