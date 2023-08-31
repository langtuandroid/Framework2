using System;
using System.Collections.Generic;
using NPBehave;
using Exception = NPBehave.Exception;

namespace Framework
{
    public class NP_RuntimeTreeFactory
    {
        public static NP_RuntimeTree CreateSkillRuntimeTree(Unit unit, int skillConfigId)
        {
            NP_DataSupportor npDataSupportor = unit.DomainScene().GetComponent<NP_TreeDataRepositoryComponent>()
                .GetNPTreeDataDeepCopyBBValuesOnly(SkillBehaveConfigFactory.Instance.Get(skillConfigId).ConfigPath);

            return CreateNpRuntimeTree(unit, npDataSupportor);
        }

        public static NP_RuntimeTree CreateBehaveRuntimeTree(Unit unit, int behaveConfigId)
        {
            NP_DataSupportor npDataSupportor = unit.DomainScene().GetComponent<NP_TreeDataRepositoryComponent>()
                .GetNPTreeDataDeepCopyBBValuesOnly(BehaveConfigFactory.Instance.Get(behaveConfigId).ConfigPath);

            return CreateNpRuntimeTree(unit, npDataSupportor);
        }

        /// <summary>
        /// 创建一个行为树实例,默认存入Unit的NP_RuntimeTreeManager中
        /// </summary>
        /// <param name="unit">行为树所归属unit</param>
        /// <param name="behaveConfigId">行为树数据id</param>
        /// <returns></returns>
        private static NP_RuntimeTree CreateNpRuntimeTree(Unit unit, NP_DataSupportor npDataSupportor)
        {
            NP_RuntimeTreeManager npRuntimeTreeManager = unit.GetComponent<NP_RuntimeTreeManager>();
            long rootId = npDataSupportor.NPBehaveTreeDataId;

            NP_RuntimeTree tempTree =
                npRuntimeTreeManager.AddChildWithId<NP_RuntimeTree, NP_DataSupportor, NP_SyncComponent, Unit>(
                    rootId + unit.Id, npDataSupportor,
                    npRuntimeTreeManager.GetComponent<NP_SyncComponent>(), unit);

            npRuntimeTreeManager.AddTree(tempTree.Id, rootId, tempTree);

            using RecyclableList<long> taskIds = RecyclableList<long>.Create();
            //Log.Info($"运行时id为{theRuntimeTreeID}");
            //配置节点数据
            foreach (KeyValuePair<long, NP_NodeDataBase> nodeDateBase in npDataSupportor.NP_DataSupportorDic)
            {
                switch (nodeDateBase.Value.BelongNodeType)
                {
                    case NodeType.Task:
                        try
                        {
                            try
                            {
                                Task node = nodeDateBase.Value.CreateTask(unit, tempTree);
                                node.SetDebugData(nodeDateBase.Value);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw;
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error($"{e}-----{nodeDateBase.Value.NodeDes}");
                            throw;
                        }

                        break;
                    case NodeType.CombineNode:
                        try
                        {
                            Node node = nodeDateBase.Value.CreateCombineNode(unit, tempTree);
                            node.SetDebugData(nodeDateBase.Value);
                        }
                        catch (Exception e)
                        {
                            Log.Error($"{e}-----{nodeDateBase.Value.NodeDes}");
                            throw;
                        }

                        break;
                    case NodeType.Decorator:
                        try
                        {
                            Decorator node = nodeDateBase.Value.CreateDecoratorNode(unit, tempTree,
                                npDataSupportor.NP_DataSupportorDic[nodeDateBase.Value.LinkedIds[0]]
                                    .NP_GetNode());
                            node.SetDebugData(nodeDateBase.Value);
                        }
                        catch (Exception e)
                        {
                            Log.Error($"{e}-----{nodeDateBase.Value.NodeDes}");
                            throw;
                        }

                        break;
                    case NodeType.Composite:
                        try
                        {
                            List<Node> temp = new();
                            foreach (long linkedId in nodeDateBase.Value.LinkedIds)
                            {
                                temp.Add(npDataSupportor.NP_DataSupportorDic[linkedId]
                                    .NP_GetNode());
                            }

                            Composite node = nodeDateBase.Value.CreateComposite(temp.ToArray());
                            node.SetDebugData(nodeDateBase.Value);
                        }
                        catch (Exception e)
                        {
                            Log.Error($"{e}-----{nodeDateBase.Value.NodeDes}");
                            throw;
                        }

                        break;
                }
            }

            NPBehave.Root root = npDataSupportor.NP_DataSupportorDic[rootId].NP_GetNode() as NPBehave.Root;
            //配置根结点
            tempTree.SetRootNode(root);

            //配置黑板数据
            Dictionary<string, ANP_BBValue> bbvaluesManager = tempTree.GetBlackboard().GetDatas();
            foreach (KeyValuePair<string, ANP_BBValue> bbValues in npDataSupportor.NP_BBValueManager)
            {
                bbvaluesManager.Add(bbValues.Key, bbValues.Value);
            }
            
            return tempTree;
        }
    }
}