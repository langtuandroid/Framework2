//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 21:01:06
//------------------------------------------------------------

#if UNITY_EDITOR


using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 矩形可视化辅助
    /// </summary>
    public class B2S_BoxColliderVisualHelper : B2S_ColliderVisualHelperBase
    {
        [InlineEditor] [DisableInEditorMode] [Required("需要至少一个Unity2D矩形碰撞器")] [BsonIgnore]
        public BoxCollider2D mCollider2D;

        [LabelText("碰撞体数据")]
        [ShowInInspector]
        public B2S_BoxColliderDataStructure MB2S_BoxColliderDataStructure
        {
            get => dataStructureBase as B2S_BoxColliderDataStructure;
            set => dataStructureBase = value;
        }

        private Vector3[] Points = new Vector3[4];

        public override void InitColliderBaseInfo()
        {
            this.MB2S_BoxColliderDataStructure.b2SColliderType = B2S_ColliderType.BoxColllider;
        }

        [Button("重新绘制矩形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            BoxCollider2D tempBox2D = this.mCollider2D;
            // 这里不需要再去根据Go的缩放计算了，因为Unity已经帮我们计算好了
            this.MB2S_BoxColliderDataStructure.hx = tempBox2D.bounds.size.x / 2;
            this.MB2S_BoxColliderDataStructure.hy = tempBox2D.bounds.size.y / 2;

            MB2S_BoxColliderDataStructure.finalOffset = mCollider2D.offset;

            // 从左上角开始顺时针计算顶点
            Points[0] = new Vector3(
                -MB2S_BoxColliderDataStructure.hx + MB2S_BoxColliderDataStructure.finalOffset.x, 0,
                MB2S_BoxColliderDataStructure.hy + MB2S_BoxColliderDataStructure.finalOffset.y);
            Points[1] = new Vector3(
                MB2S_BoxColliderDataStructure.hx + MB2S_BoxColliderDataStructure.finalOffset.x, 0,
                MB2S_BoxColliderDataStructure.hy + MB2S_BoxColliderDataStructure.finalOffset.y);
            Points[2] = new Vector3(
                MB2S_BoxColliderDataStructure.hx + MB2S_BoxColliderDataStructure.finalOffset.x, 0,
                -MB2S_BoxColliderDataStructure.hy + MB2S_BoxColliderDataStructure.finalOffset.y);
            Points[3] = new Vector3(
                -MB2S_BoxColliderDataStructure.hx + MB2S_BoxColliderDataStructure.finalOffset.x, 0,
                -MB2S_BoxColliderDataStructure.hy + MB2S_BoxColliderDataStructure.finalOffset.y);

            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(Points[i], Points[(i + 1) % 4]);
            }
        }

        public override void FillDataStructure()
        {
            if (mCollider2D == null) return;
            this.MB2S_BoxColliderDataStructure.hx = mCollider2D.bounds.size.x / 2;
            this.MB2S_BoxColliderDataStructure.hy = mCollider2D.bounds.size.y / 2;
            MB2S_BoxColliderDataStructure.finalOffset = mCollider2D.offset;
            MB2S_BoxColliderDataStructure.isSensor = mCollider2D.isTrigger;
        }

        [Button("保存矩形碰撞体信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
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
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null && MB2S_BoxColliderDataStructure.id != 0)
            {
                if (!this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_BoxColliderDataStructure.id))
                {
                    B2S_BoxColliderDataStructure b2SBoxColliderDataStructure = new B2S_BoxColliderDataStructure();
                    b2SBoxColliderDataStructure.id = MB2S_BoxColliderDataStructure.id;
                    b2SBoxColliderDataStructure.finalOffset.x = MB2S_BoxColliderDataStructure.finalOffset.x;
                    b2SBoxColliderDataStructure.finalOffset.y = MB2S_BoxColliderDataStructure.finalOffset.y;
                    b2SBoxColliderDataStructure.isSensor = MB2S_BoxColliderDataStructure.isSensor;
                    b2SBoxColliderDataStructure.b2SColliderType = MB2S_BoxColliderDataStructure.b2SColliderType;
                    b2SBoxColliderDataStructure.hx = MB2S_BoxColliderDataStructure.hx;
                    b2SBoxColliderDataStructure.hy = this.MB2S_BoxColliderDataStructure.hy;
                    this.MColliderDataSupporter.colliderDataDic.Add(this.MB2S_BoxColliderDataStructure.id,
                        b2SBoxColliderDataStructure);
                }
                else
                {
                    this.MColliderDataSupporter.colliderDataDic[this.MB2S_BoxColliderDataStructure.id] =
                        this.MB2S_BoxColliderDataStructure;
                }
            }

            if (SavecolliderNameAndIdInflect())
            {
                using (FileStream file =
                       File.Create(
                           $"{B2S_BattleColliderExportPathDefine.ClientColliderDataSavePath}/{B2S_BattleColliderExportPathDefine.ColliderDataFileName}.bytes"))
                {
                    BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
                }

                colliderEditor.OnSaveColliderData(dataStructureBase.id);
                colliderEditor.ShowTips("保存成功");
            }
        }

        [Button("清除此矩形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
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
                ResetData();
                return;
            }

            if (theObjectWillBeEdited == null)
            {
                ResetData();
                return;
            }

            if (mCollider2D == null)
            {
                mCollider2D = theObjectWillBeEdited.GetComponent<BoxCollider2D>();
                if (mCollider2D == null)
                {
                    this.canDraw = false;
                }
            }
            FillDataStructure();

            if (this.MB2S_BoxColliderDataStructure.id == 0)
            {
                this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(
                    this.theObjectWillBeEdited.name,
                    out this.MB2S_BoxColliderDataStructure.id);
                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_BoxColliderDataStructure.id))
                {
                    this.MB2S_BoxColliderDataStructure =
                        (B2S_BoxColliderDataStructure)this.MColliderDataSupporter.colliderDataDic[
                            this.MB2S_BoxColliderDataStructure.id];
                }
            }
        }

        private void ResetData()
        {
            mCollider2D = null;
            this.canDraw = false;
            this.MB2S_BoxColliderDataStructure.id = 0;
            this.MB2S_BoxColliderDataStructure.isSensor = false;
            MB2S_BoxColliderDataStructure.finalOffset = new float2(0);
        }


        public B2S_BoxColliderVisualHelper(B2S_ColliderEditor colliderEditor) : base(colliderEditor)
        {
            dataStructureBase = new B2S_BoxColliderDataStructure();
        }
    }
}

#endif