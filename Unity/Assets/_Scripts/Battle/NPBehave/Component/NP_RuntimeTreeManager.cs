using System.Collections.Generic;

namespace Framework
{
    public class NP_RuntimeTreeManager : Entity, IBattleUpdateSystem, IAwakeSystem
    {
        public Dictionary<long, NP_RuntimeTree> runtimeId2Tree = new();

        /// <summary>
        /// 已经添加过的行为树，第一个id为root id，第二个id为运行时id
        /// </summary>
        private Dictionary<long, long> rootId2TreeRuntimeId = new();

        /// <summary>
        /// 添加行为树
        /// </summary>
        /// <param name="runTimeID">行为树运行时ID</param>
        /// <param name="rootId">行为树在预配置中的id，即根节点id</param>
        /// <param name="npRuntimeTree">要添加的行为树</param>
        public void AddTree(long runTimeID, long rootId, NP_RuntimeTree npRuntimeTree)
        {
            runtimeId2Tree.Add(runTimeID, npRuntimeTree);
            rootId2TreeRuntimeId[rootId] = runTimeID;
        }

        /// <summary>
        /// 通过运行时ID请求行为树
        /// </summary>
        /// <param name="runtimeid">运行时ID</param>
        /// <returns></returns>
        public NP_RuntimeTree GetTreeByRuntimeID(long runtimeid)
        {
            if (runtimeId2Tree.TryGetValue(runtimeid, out NP_RuntimeTree id))
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
        public NP_RuntimeTree GetTreeByRootID(long rootId)
        {
            if (rootId2TreeRuntimeId.TryGetValue(rootId, out long tree))
            {
                return runtimeId2Tree[tree];
            }

            return null;
        }

        public void RemoveTree(long id)
        {
            if (runtimeId2Tree.ContainsKey(id))
            {
                runtimeId2Tree[id].Dispose();
                runtimeId2Tree.Remove(id);
                long removeId = -1;
                foreach (KeyValuePair<long, long> item in rootId2TreeRuntimeId)
                {
                    if (item.Value == id)
                    {
                        removeId = item.Key;
                        break;
                    }
                }

                if (removeId != -1)
                {
                    rootId2TreeRuntimeId.Remove(removeId);
                }
            }
            else
            {
                Log.Error($"请求删除的ID不存在，id是{id}");
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();
            foreach (KeyValuePair<long, NP_RuntimeTree> runtimeTree in runtimeId2Tree)
            {
                runtimeTree.Value.Finish();
            }

            runtimeId2Tree.Clear();
            rootId2TreeRuntimeId.Clear();
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