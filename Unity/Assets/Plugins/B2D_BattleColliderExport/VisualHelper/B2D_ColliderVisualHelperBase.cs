//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 20:58:09
//------------------------------------------------------------

#if UNITY_EDITOR

using System;
using System.IO;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public abstract class B2D_ColliderVisualHelperBase
    {
        [InfoBox("将想要编辑的游戏对象拖放到此处")]
        [HideLabel]
        [BsonIgnore]
        [InfoBox("物体的旋转位移缩放只能是默认值！！！", InfoMessageType.Error,
            VisibleIf =
                "@theObjectWillBeEdited != null && (theObjectWillBeEdited.transform.position != UnityEngine.Vector3.zero || theObjectWillBeEdited.transform.eulerAngles != UnityEngine.Vector3.zero || theObjectWillBeEdited.transform.localScale != UnityEngine.Vector3.one)")]
        public GameObject theObjectWillBeEdited;

        /// <summary>
        /// 缓存的游戏对象，用于对比更新
        /// </summary>
        [HideInEditorMode] public GameObject CachedGameObject;

        private Color mDrawColor = Color.red;

        [BsonIgnore] [HideInEditorMode] public bool canDraw;

        [HideInEditorMode] public ColliderNameAndIdInflectSupporter MColliderNameAndIdInflectSupporter;

        [HideInEditorMode] public ColliderDataSupporter MColliderDataSupporter;

        protected B2D_ColliderDataStructureBase dataStructureBase;

        protected B2D_ColliderEditor colliderEditor;

        public B2D_ColliderVisualHelperBase(B2D_ColliderEditor colliderEditor)
        {
            this.colliderEditor = colliderEditor;
            this.MColliderNameAndIdInflectSupporter = colliderEditor.ColliderNameAndIdInflectSupporter;
            this.MColliderDataSupporter = this.colliderEditor.ColliderDataSupporter;
        }

        /// <summary>
        /// 设置碰撞体基础信息
        /// </summary>
        public abstract void InitColliderBaseInfo();

        /// <summary>
        /// 重新绘制Box2DSharp
        /// </summary>
        public abstract void InitPointInfo();

        /// <summary>
        /// 重新绘制Box2DSharp
        /// </summary>
        public abstract void DrawCollider();

        /// <summary>
        /// 保存名称Id映射信息
        /// </summary>
        public bool SavecolliderNameAndIdInflect()
        {
            var objTrans = theObjectWillBeEdited.transform;
            if (objTrans.position != Vector3.zero || objTrans.eulerAngles != Vector3.zero ||
                objTrans.localScale != Vector3.one)
            {
                Debug.LogError("物体的旋转位移缩放不是默认值");
                return false;
            }

            if (dataStructureBase.id == 0)
            {
                colliderEditor.ShowTips("碰撞器id不能为0");
                return false;
            }
            if (!this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.ContainsKey(
                    this.theObjectWillBeEdited.name))
            {
                foreach (var value in MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic)
                {
                    if (value.Value == dataStructureBase.id)
                    {
                        colliderEditor.ShowTips($"id重复，已有{value.Key}");
                        return false;
                    }
                }
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Add(this.theObjectWillBeEdited.name,
                    this.dataStructureBase.id);
            }
            else
            {
                if (MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic[theObjectWillBeEdited.name] !=
                    dataStructureBase.id)
                {
                    MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic[this.theObjectWillBeEdited.name] =
                        this.dataStructureBase.id;
                    Debug.LogWarning($"修改了{theObjectWillBeEdited.name}的id为{dataStructureBase.id}");
                }
            }

            using (FileStream file =
                   File.Create($"{B2D_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
            }

            return true;
        }

        public abstract void FillDataStructure();

        /// <summary>
        /// 保存碰撞体信息
        /// </summary>
        public abstract void SaveColliderData();

        protected void SavePrefab()
        {
            PrefabUtility.SaveAsPrefabAsset(theObjectWillBeEdited,
                $"{B2D_BattleColliderExportPathDefine.ColliderPrefabSavePath}/{theObjectWillBeEdited.name}.prefab");
        }

        public abstract void OnUpdate();

        public void OnDrawGizmos()
        {
            Gizmos.color = this.mDrawColor;
            DrawCollider();
        }

        /// <summary>
        /// 删除此碰撞体相关所有信息
        /// </summary>
        public virtual void DeletecolliderData()
        {
            if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.dataStructureBase.id))
            {
                this.MColliderDataSupporter.colliderDataDic.Remove(this.dataStructureBase.id);
            }

            if (this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.ContainsKey(
                    this.theObjectWillBeEdited.name))
            {
                this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Remove(
                    this.theObjectWillBeEdited.name);
            }

            colliderEditor.OnDeleteColliderData(dataStructureBase.id);
            AssetDatabase.DeleteAsset(
                $"{B2D_BattleColliderExportPathDefine.ColliderPrefabSavePath}/{theObjectWillBeEdited.name}.prefab");
            theObjectWillBeEdited = null;
            using (FileStream file =
                   File.Create(
                       $"{B2D_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
            }

            using (FileStream file =
                   File.Create(
                       $"{B2D_BattleColliderExportPathDefine.ClientColliderDataSavePath}"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
            }
            colliderEditor.ShowTips("删除完成");
        }
    }
}
#endif