using System.Collections.Generic;

namespace Framework
{
    public class NP_RuntimeTreeManager : Entity, IBattleUpdateSystem, IAwakeSystem
    {
        public Dictionary<long, NP_RuntimeTree> RuntimeTrees = new Dictionary<long, NP_RuntimeTree>();

        /// <summary>
        /// 已经添加过的行为树，第一个id为root id，第二个id为运行时id
        /// </summary>
        private Dictionary<long, long> m_HasAddedTrees = new Dictionary<long, long>();

        /// <summary>
        /// 添加行为树
        /// </summary>
        /// <param name="runTimeID">行为树运行时ID</param>
        /// <param name="rootId">行为树在预配置中的id，即根节点id</param>
        /// <param name="npRuntimeTree">要添加的行为树</param>
        public void AddTree(long runTimeID, long rootId, NP_RuntimeTree npRuntimeTree)
        {
            RuntimeTrees.Add(runTimeID, npRuntimeTree);
            this.m_HasAddedTrees.Add(rootId, runTimeID);
        }

        /// <summary>
        /// 通过运行时ID请求行为树
        /// </summary>
        /// <param name="runtimeid">运行时ID</param>
        /// <returns></returns>
        public NP_RuntimeTree GetTreeByRuntimeID(long runtimeid)
        {
            if (RuntimeTrees.TryGetValue(runtimeid, out var id))
            {
                return id;
            }

            Log.Error($"通过运行时ID请求行为树请求的ID不存在，id是{runtimeid}");
            return null;
        }

        /// <summary>
        /// 通过root id请求行为树
        /// </summary>
        /// <param name="rootId">预制id</param>
        /// <returns></returns>
        public NP_RuntimeTree GetTreeByPrefabID(long rootId)
        {
            if (this.m_HasAddedTrees.TryGetValue(rootId, out var tree))
            {
                return RuntimeTrees[tree];
            }

            Log.Error($"通过预制id请求行为树,请求的ID不存在，id是{rootId}");
            return null;
        }

        public void RemoveTree(long id)
        {
            if (RuntimeTrees.ContainsKey(id))
            {
                RuntimeTrees[id].Dispose();
                RuntimeTrees.Remove(id);
            }
            else
            {
                Log.Error($"请求删除的ID不存在，id是{id}");
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
            foreach (var runtimeTree in RuntimeTrees)
            {
                runtimeTree.Value.Dispose();
            }

            RuntimeTrees.Clear();
            this.m_HasAddedTrees.Clear();
        }

        public void BattleUpdate(float deltaTime)
        {
            GetComponent<NP_SyncComponent>().SyncContext.GetClock().Update(deltaTime);
        }

        public void Awake()
        {
            AddComponent<NP_SyncComponent>();
        }
    }
}