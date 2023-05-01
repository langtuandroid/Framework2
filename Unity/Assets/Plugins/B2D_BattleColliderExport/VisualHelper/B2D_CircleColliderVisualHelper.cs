//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 21:01:36
//------------------------------------------------------------

#if UNITY_EDITOR


using System;
using System.IO;
using System.Numerics;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace ET
{
    /// <summary>
    /// 圆形可视化辅助
    /// </summary>
    public class B2D_CircleColliderVisualHelper : B2D_ColliderVisualHelperBase
    {
        [InlineEditor] [DisableInEditorMode] [Required("需要至少一个Unity2D圆形碰撞器")] [HideLabel] [BsonIgnore]
        public CircleCollider2D mCollider2D;

        [LabelText("碰撞体数据")]
        [ShowInInspector]
        public B2D_CircleColliderDataStructure MB2D_CircleColliderDataStructure
        {
            get => dataStructureBase as B2D_CircleColliderDataStructure;
            set => dataStructureBase = value;
        }

        [BsonIgnore] [LabelText("圆线段数")] public int Segments = 66;

        public override void InitColliderBaseInfo()
        {
            this.MB2D_CircleColliderDataStructure.B2D_ColliderType = B2D_ColliderType.CircleCollider;
        }

        [Button("重新绘制圆形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            var step = Mathf.RoundToInt(360f / Segments);

            Vector3 startPoint = new Vector3(
                MB2D_CircleColliderDataStructure.radius *
                Mathf.Cos(0 * Mathf.Deg2Rad) + MB2D_CircleColliderDataStructure.finalOffset.x, 0,
                MB2D_CircleColliderDataStructure.radius *
                Mathf.Sin(0 * Mathf.Deg2Rad) + MB2D_CircleColliderDataStructure.finalOffset.y);
            for (int i = step; i <= 360; i += step)
            {
                var nextPoint = new Vector3(
                    MB2D_CircleColliderDataStructure.radius *
                    Mathf.Cos(i * 1.0f * Mathf.Deg2Rad) + MB2D_CircleColliderDataStructure.finalOffset.x, 0,
                    MB2D_CircleColliderDataStructure.radius *
                    Mathf.Sin(i * 1.0f * Mathf.Deg2Rad) + MB2D_CircleColliderDataStructure.finalOffset.y);
                Gizmos.DrawLine(startPoint, nextPoint);
                startPoint = nextPoint;
            }
        }

        public override void FillDataStructure()
        {
            if(mCollider2D == null) return;
            MB2D_CircleColliderDataStructure.radius = mCollider2D.radius;
            MB2D_CircleColliderDataStructure.finalOffset = mCollider2D.offset;
            MB2D_CircleColliderDataStructure.isSensor = mCollider2D.isTrigger;
        }

        [Button("保存圆形碰撞体信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SaveColliderData()
        {
            var objTrans = theObjectWillBeEdited.transform;
            if (objTrans.position != Vector3.zero || objTrans.eulerAngles != Vector3.zero ||
                objTrans.localScale != Vector3.one)
            {
                colliderEditor.ShowTips("物体的旋转位移缩放不是默认值");
                return;
            } 
            SavePrefab();
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null && MB2D_CircleColliderDataStructure.id != 0)
            {
                if (!this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2D_CircleColliderDataStructure.id))
                {
                    B2D_CircleColliderDataStructure b2DCircleColliderDataStructure =
                        new B2D_CircleColliderDataStructure();
                    b2DCircleColliderDataStructure.id = MB2D_CircleColliderDataStructure.id;
                    b2DCircleColliderDataStructure.finalOffset.x = MB2D_CircleColliderDataStructure.finalOffset.x;
                    b2DCircleColliderDataStructure.finalOffset.y = MB2D_CircleColliderDataStructure.finalOffset.y;
                    b2DCircleColliderDataStructure.isSensor = MB2D_CircleColliderDataStructure.isSensor;
                    b2DCircleColliderDataStructure.B2D_ColliderType = MB2D_CircleColliderDataStructure.B2D_ColliderType;
                    b2DCircleColliderDataStructure.radius = MB2D_CircleColliderDataStructure.radius;
                    this.MColliderDataSupporter.colliderDataDic.Add(this.MB2D_CircleColliderDataStructure.id,
                        b2DCircleColliderDataStructure);
                }
                else
                {
                    this.MColliderDataSupporter.colliderDataDic[this.MB2D_CircleColliderDataStructure.id] =
                        this.MB2D_CircleColliderDataStructure;
                }
            }

            if (SavecolliderNameAndIdInflect())
            {
                using (FileStream file =
                       File.Create(
                           $"{B2D_BattleColliderExportPathDefine.ClientColliderDataSavePath}"))
                {
                    BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
                }

                colliderEditor.OnSaveColliderData(dataStructureBase.id);
                colliderEditor.ShowTips("保存成功");
            }
        }

        [Button("清除此圆形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
        public override void DeletecolliderData()
        {
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
            {
                base.DeletecolliderData();
            }
        }

        public override void OnUpdate()
        {
            if (CachedGameObject != theObjectWillBeEdited)
            {
                if (theObjectWillBeEdited != null)
                    CachedGameObject = theObjectWillBeEdited;
                this.ResetData();
                return;
            }

            if (theObjectWillBeEdited == null)
            {
                this.ResetData();
                return;
            }

            if (this.MB2D_CircleColliderDataStructure.id == 0)
            {
                if (this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(
                    this.theObjectWillBeEdited.name,
                    out this.MB2D_CircleColliderDataStructure.id))
                {
                    Debug.Log($"自动设置圆形碰撞体ID成功，ID为{MB2D_CircleColliderDataStructure.id}");
                }

                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2D_CircleColliderDataStructure.id))
                {
                    this.MB2D_CircleColliderDataStructure =
                        (B2D_CircleColliderDataStructure) this.MColliderDataSupporter.colliderDataDic[
                            this.MB2D_CircleColliderDataStructure.id];
                }
            }

            if (mCollider2D == null)
            {
                mCollider2D = theObjectWillBeEdited.GetComponent<CircleCollider2D>();
                if (mCollider2D == null)
                {
                    this.canDraw = false;
                }
            }
            FillDataStructure();
        }

        private void ResetData()
        {
            mCollider2D = null;
            this.canDraw = false;
            this.MB2D_CircleColliderDataStructure.id = 0;
            this.MB2D_CircleColliderDataStructure.radius = 0;
            MB2D_CircleColliderDataStructure.finalOffset = new float2(0);
            this.MB2D_CircleColliderDataStructure.isSensor = false;
        }

        public B2D_CircleColliderVisualHelper(B2D_ColliderEditor colliderEditor) : base(colliderEditor)
        {
            dataStructureBase = new B2D_CircleColliderDataStructure();
        }
    }
}

#endif