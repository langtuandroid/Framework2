//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 21:02:18
//------------------------------------------------------------

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET
{
    /// <summary>
    /// 多边形可视化辅助
    /// </summary>
    public class B2S_PolygonColliderVisualHelper : B2S_ColliderVisualHelperBase
    {

        [LabelText("请设置每个多边形最大顶点数")] [Range(3, 8)]
        public int MaxPointLimit = 3;

        [InlineEditor] [Required("需要至少一个Unity2D多边形碰撞器")] [HideLabel] [DisableInEditorMode]
        public PolygonCollider2D mCollider2D;

        [LabelText("碰撞体数据")]
        [ShowInInspector]
        public B2S_PolygonColliderDataStructure MB2S_PolygonColliderDataStructure
        {
            get => dataStructureBase as B2S_PolygonColliderDataStructure;
            set => dataStructureBase = value;
        }

        public override void InitColliderBaseInfo()
        {
            this.MB2S_PolygonColliderDataStructure.b2SColliderType = B2S_ColliderType.PolygonCollider;
        }

        [Button("重新绘制多边形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            InitBox2dPolygonFromUnityCollider();
            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            foreach (var b2sColliderPoints in this.MB2S_PolygonColliderDataStructure.finalPoints)
            {
                for (int i = 0; i < b2sColliderPoints.Count; i++)
                {
                    if (i < b2sColliderPoints.Count - 1)
                        Gizmos.DrawLine(new Vector3(
                                b2sColliderPoints[i].X,
                                0,
                                b2sColliderPoints[i].Y),
                            new Vector3(
                                b2sColliderPoints[i + 1].X, 0,
                                b2sColliderPoints[i + 1].Y));
                    else
                    {
                        Gizmos.DrawLine(new Vector3(
                                b2sColliderPoints[i].X,
                                0,
                                b2sColliderPoints[i].Y),
                            new Vector3(
                                b2sColliderPoints[0].X, 0,
                                b2sColliderPoints[0].Y));
                    }
                }
            }
        }

        public override void FillDataStructure()
        {
            if (mCollider2D == null) return;
            InitBox2dPolygonFromUnityCollider();
            MB2S_PolygonColliderDataStructure.finalOffset = mCollider2D.offset;
            MB2S_PolygonColliderDataStructure.isSensor = mCollider2D.isTrigger;
        }

        private void InitBox2dPolygonFromUnityCollider()
        {
            MB2S_PolygonColliderDataStructure.pointCount = 0;
            this.MB2S_PolygonColliderDataStructure.finalPoints.Clear();

            //对多边形进行分割操作
            List<Vector2> tempPoints = new List<Vector2>();
            //必须进行放缩操作，不然在很近的时候运算误差会导致出错
            foreach (var unityColliderPoints in this.mCollider2D.points)
            {
                Vector2 tempVector2 = new Vector2(unityColliderPoints.x * 30, unityColliderPoints.y * 30);
                tempPoints.Add(tempVector2);
            }

            List<List<Vector2>> tempFinalPolygons = Separator.CalcShapes(tempPoints);

            foreach (var b2sColliderPoints in tempFinalPolygons)
            {
                for (int i = 0; i < b2sColliderPoints.Count; i++)
                {
                    b2sColliderPoints[i] = new Vector2(b2sColliderPoints[i].X / 30, b2sColliderPoints[i].Y / 30);
                }
            }

            // 因为Box2d的多边形默认不提供偏移设置，所以这里在存储的时候直接将偏移累加到顶点位置上
            MB2S_PolygonColliderDataStructure.finalOffset = 0;
            
            List<List<Vector2>> FinalPolygons = Separator.SplitPolygonUntilLessX(this.MaxPointLimit, tempFinalPolygons);

            int pointCount = 0;
            for (int i = 0; i < FinalPolygons.Count; i++)
            {
                this.MB2S_PolygonColliderDataStructure.finalPoints.Add(new List<Vector2>());
                for (int j = 0; j < FinalPolygons[i].Count; j++, pointCount++)
                {
                    // 因为Box2d的多边形默认不提供偏移设置，所以这里在存储的时候直接将偏移累加到顶点位置上
                    Vector3 finalPint = new Vector3(FinalPolygons[i][j].X + mCollider2D.offset.x, 0,
                        FinalPolygons[i][j].Y + mCollider2D.offset.y);
                    this.MB2S_PolygonColliderDataStructure.finalPoints[i].Add(new Vector2(finalPint.x, finalPint.z));
                }
            }

            MB2S_PolygonColliderDataStructure.pointCount = pointCount;
        }

        [Button("保存多边形碰撞体信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
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
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null && MB2S_PolygonColliderDataStructure.id != 0)
            {
                if (!this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_PolygonColliderDataStructure.id))
                {
                    B2S_PolygonColliderDataStructure temp = new B2S_PolygonColliderDataStructure();
                    temp.id = MB2S_PolygonColliderDataStructure.id;
                    temp.finalOffset.x = MB2S_PolygonColliderDataStructure.finalOffset.x;
                    temp.finalOffset.y = MB2S_PolygonColliderDataStructure.finalOffset.y;
                    temp.isSensor = MB2S_PolygonColliderDataStructure.isSensor;
                    temp.b2SColliderType = MB2S_PolygonColliderDataStructure.b2SColliderType;
                    for (int i = 0; i < this.MB2S_PolygonColliderDataStructure.finalPoints.Count; i++)
                    {
                        temp.finalPoints.Add(new List<Vector2>());
                        for (int j = 0; j < this.MB2S_PolygonColliderDataStructure.finalPoints[i].Count; j++)
                        {
                            Vector2 costumVector2 = new Vector2(
                                this.MB2S_PolygonColliderDataStructure.finalPoints[i][j].X,
                                this.MB2S_PolygonColliderDataStructure.finalPoints[i][j].Y);
                            temp.finalPoints[i].Add(costumVector2);
                        }
                    }

                    temp.pointCount = this.MB2S_PolygonColliderDataStructure.pointCount;
                    this.MColliderDataSupporter.colliderDataDic.Add(this.MB2S_PolygonColliderDataStructure.id,
                        temp);
                }
                else
                {
                    this.MColliderDataSupporter.colliderDataDic[this.MB2S_PolygonColliderDataStructure.id] =
                        this.MB2S_PolygonColliderDataStructure;
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

        [Button("清除此多边形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
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
                CachedGameObject = null;
                return;
            }

            if (mCollider2D == null)
            {
                mCollider2D = theObjectWillBeEdited.GetComponent<PolygonCollider2D>();
                if (mCollider2D == null)
                {
                    this.canDraw = false;
                }
            }
            FillDataStructure();

            if (this.MB2S_PolygonColliderDataStructure.id == 0)
            {
                this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(
                    this.theObjectWillBeEdited.name,
                    out this.MB2S_PolygonColliderDataStructure.id);

                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_PolygonColliderDataStructure.id))
                {
                    this.MB2S_PolygonColliderDataStructure =
                        (B2S_PolygonColliderDataStructure) this.MColliderDataSupporter.colliderDataDic[
                            this.MB2S_PolygonColliderDataStructure.id];
                }
            }
        }

        private void ResetData()
        {
            mCollider2D = null;
            this.canDraw = false;
            this.MB2S_PolygonColliderDataStructure.id = 0;
            this.MB2S_PolygonColliderDataStructure.isSensor = false;
            this.MB2S_PolygonColliderDataStructure.finalOffset = 0;
        }

        public B2S_PolygonColliderVisualHelper(B2S_ColliderEditor colliderEditor) : base(colliderEditor)
        {
            dataStructureBase = new B2S_PolygonColliderDataStructure();
        }
    }
}

#endif