using Framework;
using MongoDB.Bson.Serialization;
using UnityEngine;

namespace ET
{

    /// <summary>
    /// 碰撞体数据仓库，从二进制文件读取数据
    /// </summary>
    public class B2D_ColliderDataRepositoryComponent : Entity ,IAwakeSystem
    {
        public ColliderDataSupporter ColliderDatas = new ColliderDataSupporter();

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// 读取所有碰撞数据
        /// </summary>
        private void ReadcolliderData()
        {
            byte[] bytes = ResComponent.Instance.LoadAsset<TextAsset>(B2D_BattleColliderExportPathDefine.ClientColliderDataSavePath).bytes;
            if (bytes.Length > 0)
                ColliderDatas = BsonSerializer.Deserialize<ColliderDataSupporter>(bytes);
        }

        public B2D_ColliderDataStructureBase GetDataByColliderId(long id)
        {
            if (ColliderDatas.colliderDataDic.TryGetValue(id, out var data))
            {
                return data;
            }

            Log.Error($"未找到碰撞体数据，所查找的ID：{id}");
            return null;
        }

        public void Awake()
        {
            this.ReadcolliderData();
        }
    }
}