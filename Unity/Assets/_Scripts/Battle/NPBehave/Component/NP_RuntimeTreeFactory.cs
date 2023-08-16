using System;
using System.Collections.Generic;
using NPBehave;
using Exception = NPBehave.Exception;

namespace Framework
{
    public class NP_RuntimeTreeFactory
    {
        public static NPBehave.Root LoadExtraTree(Unit unit, NP_RuntimeTree runtimeTree, int behaveConfigId,
            ExtraBehave outBehave)
        {
            Log.Msg("创建了行为树", behaveConfigId);
            NP_DataSupportor npDataSupportor = unit.DomainScene().GetComponent<NP_TreeDataRepositoryComponent>()
                .GetNPTreeDataDeepCopyBBValuesOnly(BehaveConfigFactory.Instance.Get(behaveConfigId).ConfigPath);
            outBehave.DataSupportor = npDataSupportor;
            long rootId = npDataSupportor.NPBehaveTreeDataId;

            using RecyclableList<ExtraBehave> extraBehaves = RecyclableList<ExtraBehave>.Create();
            using RecyclableList<long> taskIds = RecyclableList<long>.Create();
            //配置节点数据
            foreach (KeyValuePair<long, NP_NodeDataBase> nodeDateBase in npDataSupportor.NP_DataSupportorDic)
            {
                switch (nodeDateBase.Value.BelongNodeType)
                {
                    case NodeType.Task:
                        try
                        {
                            Task node = nodeDateBase.Value.CreateTask(unit, runtimeTree);
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
                            Decorator node = nodeDateBase.Value.CreateDecoratorNode(unit, runtimeTree,
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
                    case NodeType.CombineNode:
                        try
                        {
                            Node node = nodeDateBase.Value.CreateCombineNode(unit, runtimeTree);
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
                    case NodeType.Tree:
                        try
                        {
                            taskIds.Add(nodeDateBase.Key);
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
            //配置黑板数据
            Dictionary<string, ANP_BBValue> bbvaluesManager = root.Blackboard.GetDatas();
            foreach (KeyValuePair<string, ANP_BBValue> bbValues in npDataSupportor.NP_BBValueManager)
            {
                bbvaluesManager.Add(bbValues.Key, bbValues.Value);
            }

            // 把字典放到这里赋值，下面create tree需要在字典赋值之后才能创建
            foreach (long taskId in taskIds)
            {
                NP_NodeDataBase nodeDateBase = npDataSupportor.NP_DataSupportorDic[taskId];
                ExtraBehave behave = nodeDateBase.CreateTree(unit, runtimeTree);
                behave.LinkedNodeId = taskId;
                extraBehaves.Add(behave);
            }

            foreach (KeyValuePair<IBlackboardOrValue, NP_OtherTreeBBKeyData> passItem in outBehave.PassValue)
            {
                object val = passItem.Key.GetObjValue(runtimeTree.GetBlackboard());
                root.Blackboard.Set(passItem.Value.BBKey,
                    NP_BBValueHelper.AutoCreateNPBBValueFromTValue(val, val.GetType()));
            }

            foreach (KeyValuePair<NP_BlackBoardKeyData, NP_OtherTreeBBKeyData> getItem in outBehave.GetValue)
            {
                runtimeTree.GetBlackboard().Set(getItem.Key.BBKey, root.Blackboard.Get(getItem.Value.BBKey));
            }

            // 额外的行为树需要用另外一个行为树的MainNode替换临时节点
            foreach (ExtraBehave extraBehave in extraBehaves)
            {
                Composite extraNode =
                    (Composite)npDataSupportor.NP_DataSupportorDic[extraBehave.LinkedNodeId].NP_GetNode();
                extraNode.ChangeChild(extraNode.GetChild(1), extraBehave.Root.MainNode);

                NP_DataSupportor tmpData = extraBehave.DataSupportor;
                foreach (KeyValuePair<string, ANP_BBValue> bbValue in extraBehave.Root.Blackboard.GetDatas())
                {
                    if (npDataSupportor.NP_BBValueManager.ContainsKey(bbValue.Key))
                    {
                        Log.Error(
                            $"配置id为{behaveConfigId}的行为树的字典key和子行为树{extraBehave.DataSupportor.NPBehaveTreeDataId}的字典key({bbValue.Key})重复了");
                    }
                    else
                    {
                        npDataSupportor.NP_BBValueManager.Add(bbValue.Key, bbValue.Value);
                        runtimeTree.GetBlackboard().GetDatas().Add(bbValue.Key, bbValue.Value);
                    }
                }

                foreach (KeyValuePair<long, BuffNodeDataBase> buffNode in tmpData.BuffNodeDataDic)
                {
                    if (npDataSupportor.BuffNodeDataDic.ContainsKey(buffNode.Key))
                    {
                        Log.Error(
                            $"配置id为{behaveConfigId}的行为树的buff id和子行为树{extraBehave.DataSupportor.NPBehaveTreeDataId}的buff id({buffNode.Key})重复了");
                    }
                    else
                    {
                        npDataSupportor.BuffNodeDataDic[buffNode.Key] = buffNode.Value;
                    }
                }
            }

            return root;
        }

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

            using RecyclableList<ExtraBehave> extraBehaves = RecyclableList<ExtraBehave>.Create();
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
                    case NodeType.Tree:
                        try
                        {
                            taskIds.Add(nodeDateBase.Key);
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

            foreach (long taskId in taskIds)
            {
                NP_NodeDataBase nodeDateBase = npDataSupportor.NP_DataSupportorDic[taskId];
                ExtraBehave behave = nodeDateBase.CreateTree(unit, tempTree);
                behave.LinkedNodeId = taskId;
                extraBehaves.Add(behave);
            }

            // 额外的行为树需要用另外一个行为树的MainNode替换临时节点
            // 把子行为树的数据添加到父行为树中
            foreach (ExtraBehave extraBehave in extraBehaves)
            {
                // 默认子行为树会变成一个Sequence，下面有两个节点，第一个节点是PassValue和GetValue赋值
                // 第二个节点是一个空节点来替换子行为树的MainNode
                Composite extraNode =
                    (Composite)npDataSupportor.NP_DataSupportorDic[extraBehave.LinkedNodeId].NP_GetNode();
                // 需要把子行为树的Root改成父行为树的Root
                extraBehave.Root.MainNode.SetRoot(root);
                extraNode.ChangeChild(extraNode.GetChild(1), extraBehave.Root.MainNode);

                NP_DataSupportor tmpData = extraBehave.DataSupportor;
                foreach (KeyValuePair<string, ANP_BBValue> bbValue in tmpData.NP_BBValueManager)
                {
                    if (npDataSupportor.NP_BBValueManager.ContainsKey(bbValue.Key))
                    {
                        Log.Error(
                            $"配置id为{npDataSupportor.NPBehaveTreeDataId}的行为树的字典key和子行为树{extraBehave.DataSupportor.ExcelId}的字典key({bbValue.Key})重复了");
                    }
                    else
                    {
                        npDataSupportor.NP_BBValueManager.Add(bbValue.Key, bbValue.Value);
                        root.Blackboard.GetDatas().Add(bbValue.Key, bbValue.Value);
                    }
                }

                foreach (KeyValuePair<long, BuffNodeDataBase> buffNode in tmpData.BuffNodeDataDic)
                {
                    if (npDataSupportor.BuffNodeDataDic.ContainsKey(buffNode.Key))
                    {
                        Log.Error(
                            $"配置id为{npDataSupportor.NPBehaveTreeDataId}的行为树的buff id和子行为树{extraBehave.DataSupportor.ExcelId}的buff id({buffNode.Key})重复了");
                    }
                    else
                    {
                        npDataSupportor.BuffNodeDataDic[buffNode.Key] = buffNode.Value;
                    }
                }
            }

            return tempTree;
        }
    }
}