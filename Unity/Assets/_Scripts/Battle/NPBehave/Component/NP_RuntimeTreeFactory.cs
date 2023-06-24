using System;
using System.Collections.Generic;
using NPBehave;
using Exception = NPBehave.Exception;

namespace Framework
{
    public class NP_RuntimeTreeFactory
    {
        /// <summary>
        /// 创建一个行为树实例,默认存入Unit的NP_RuntimeTreeManager中
        /// </summary>
        /// <param name="unit">行为树所归属unit</param>
        /// <param name="nPDataId">行为树数据id</param>
        /// <returns></returns>
        public static NP_RuntimeTree CreateNpRuntimeTree(Unit unit, long nPDataId)
        {
            NP_DataSupportor npDataSupportor = unit.DomainScene().GetComponent<NP_TreeDataRepositoryComponent>()
                .GetNPSkillTreeDataDeepCopyBBValuesOnly(nPDataId);

            NP_RuntimeTreeManager npRuntimeTreeManager = unit.GetComponent<NP_RuntimeTreeManager>();
            long rootId = npDataSupportor.NPBehaveTreeDataId;

            NP_RuntimeTree tempTree =
                npRuntimeTreeManager.AddChildWithId<NP_RuntimeTree, NP_DataSupportor, NP_SyncComponent, Unit>(
                    rootId + unit.Id, npDataSupportor,
                    npRuntimeTreeManager.GetComponent<NP_SyncComponent>(), unit);


            npRuntimeTreeManager.AddTree(tempTree.Id, rootId, tempTree);

            //Log.Info($"运行时id为{theRuntimeTreeID}");
            //配置节点数据
            foreach (var nodeDateBase in npDataSupportor.NP_DataSupportorDic)
            {
                switch (nodeDateBase.Value.BelongNodeType)
                {
                    case NodeType.Task:
                        try
                        {
                            nodeDateBase.Value.CreateTask(unit, tempTree);
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
                            nodeDateBase.Value.CreateDecoratorNode(unit, tempTree,
                                npDataSupportor.NP_DataSupportorDic[nodeDateBase.Value.LinkedIds[0]]
                                    .NP_GetNode());
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
                            List<Node> temp = new List<Node>();
                            foreach (var linkedId in nodeDateBase.Value.LinkedIds)
                            {
                                temp.Add(npDataSupportor.NP_DataSupportorDic[linkedId]
                                    .NP_GetNode());
                            }

                            nodeDateBase.Value.CreateComposite(temp.ToArray());
                        }
                        catch (Exception e)
                        {
                            Log.Error($"{e}-----{nodeDateBase.Value.NodeDes}");
                            throw;
                        }

                        break;
                }
            }

            //配置根结点
            tempTree.SetRootNode(npDataSupportor.NP_DataSupportorDic[rootId].NP_GetNode() as NPBehave.Root);

            //配置黑板数据
            Dictionary<string, ANP_BBValue> bbvaluesManager = tempTree.GetBlackboard().GetDatas();
            foreach (var bbValues in npDataSupportor.NP_BBValueManager)
            {
                bbvaluesManager.Add(bbValues.Key, bbValues.Value);
            }

            return tempTree;
        }

        /// <summary>
        /// 创建一个技能树实例,默认存入Unit的SkillCanvasManagerComponentComponent中
        /// </summary>
        /// <param name="unit">行为树所归属unit</param>
        /// <param name="nPDataId">行为树数据id</param>
        /// <param name="belongToSkillId">归属的SkillId,一般来说需要从excel表中读取</param>
        /// <returns></returns>
        public static NP_RuntimeTree CreateSkillNpRuntimeTree(Unit unit, long nPDataId)
        {
            NP_RuntimeTree result = CreateNpRuntimeTree(unit, nPDataId);
            return result;
        }
    }
}