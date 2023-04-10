 using System;
 using System.IO;
 using Framework;
 using MongoDB.Bson;
 using Sirenix.OdinInspector;

 public class SkillGraph : NPBehaveGraph
 {

     [Button("保存技能树信息为二进制文件", 25), GUIColor(0.4f, 0.8f, 1)]
     public void Save()
     {
         if (string.IsNullOrEmpty(SavePathClient) ||
             string.IsNullOrEmpty(Name))
         {
             Log.Error($"保存路径或文件名不能为空，请检查配置");
             return;
         }

         AutoSetCanvasDatas();
         File.WriteAllText($"{SavePathClient}/{this.Name}.bytes", NpDataSupportor_Client.ToJson());

         Log.Msg($"保存 {SavePathClient}/{this.Name}.bytes 成功");
     }

     [Button("测试技能树反序列化", 25), GUIColor(0.4f, 0.8f, 1)]
     public void TestDe()
     {
         try
         {
             var data = File.ReadAllText($"{SavePathClient}/{Name}.bytes");
             this.NpDataSupportor_Client_Des = SerializeHelper.Deserialize<NP_DataSupportor>(data);
             Log.Msg($"反序列化 {SavePathClient}/{this.Name}.bytes 成功");
         }
         catch (Exception e)
         {
             Log.Msg(e.ToString());
             throw;
         }
     }

     protected override void AutoSetCanvasDatas()
     {
         base.AutoSetCanvasDatas();
         AutoSetSkillData_NodeData(NpDataSupportor_Client);
     }
     
     private void AutoSetSkillData_NodeData(NP_DataSupportor npDataSupportor)
     {
         if (npDataSupportor.BuffNodeDataDic == null) return;
         npDataSupportor.BuffNodeDataDic.Clear();

         foreach (var node in this.nodes)
         {
             if (node is BuffNodeBase mNode)
             {
                 mNode.AutoAddLinkedBuffs();
                 BuffNodeDataBase buffNodeDataBase = mNode.GetBuffNodeData();
                 // if (buffNodeDataBase is NormalBuffNodeData normalBuffNodeData)
                 // {
                 //     normalBuffNodeData.BuffData.BelongToBuffDataSupportorId =
                 //         npDataSupportor.NpDataSupportorBase.NPBehaveTreeDataId;
                 // }

                 npDataSupportor.BuffNodeDataDic.Add(buffNodeDataBase.NodeId.Value, buffNodeDataBase);
             }
         }
     }
 }