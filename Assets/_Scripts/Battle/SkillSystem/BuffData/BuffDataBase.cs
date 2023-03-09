using System.Collections.Generic;
using Framework;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;
using UnityEngine;

[Title("Buff数据块", TitleAlignment = TitleAlignments.Centered)]
[HideLabel]
[HideReferenceObjectPicker]
[BsonDeserializerRegister]
public class BuffDataBase
{
    /// <summary>
    /// Buff归属的数据块的Id,不是buff的Id，是buff数据所在的数据块Id(也就是NP_DataSupportor的Id)
    /// </summary>
    [HideInInspector] [BoxGroup("必填项")] [LabelText("Buff归属的数据块Id")]
    public long BelongToBuffDataSupportorId;

    /// <summary>
    /// 用于区分Buff，每个Buff Id都是独一无二的
    /// 因为我们不能，也不应该关心具体Buff的Id，所以这里直接自动生成
    /// </summary>
    [HideInInspector] [LabelText("Buff的Id")] [BoxGroup("必填项")]
    public long BuffId = IdGenerater.Instance.GenerateId();

    [LabelText("Buff的添加目标")] [BoxGroup("必填项")]
    public BuffTargetTypes BuffTargetTypes;

    [LabelText("Buff的基本特征")] [BoxGroup("必填项")]
    public BuffBaseType BuffBaseType;

    /// <summary>
    /// 如果为false则不会纳入网络同步范畴
    /// 比如状态BuffA（true）和其所连接的BuffB（false），A会考虑进网络同步的范畴，但B不会
    /// 所以服务端只会将BuffA同步过来，客户端在添加BuffA的时候再去添加B，这样就保证了这种连锁类型的Buff不会重复添加
    /// </summary>
    [Space(50)] [ShowIf("Base_isVisualable")] [LabelText("Buff图标的名称")] [BoxGroup("选填项")]
    public string SpriteABInfo;

    [BoxGroup("选填项")] [LabelText("Buff是否状态栏可见")]
    public bool Base_isVisualable;

    [Tooltip("是否可以叠加(不能叠加就刷新，叠加满也刷新)")] [LabelText("是否叠加")] [BoxGroup("选填项")]
    public bool CanOverlay;

    [ShowIf("CanOverlay")] [LabelText("叠加层数")] [MinValue(1)] [BoxGroup("选填项")]
    public int TargetOverlay = 1;

    [ShowIf("CanOverlay")] [LabelText("最大叠加数")] [BoxGroup("选填项")]
    public int MaxOverlay;
    
    [LabelText("要抛出的事件ID，如果有的话")]
    [BoxGroup("选填项")]
    public List<VTD_EventId> EventIds = new List<VTD_EventId>(); 

    [LabelText("Buff持续时间")] [Tooltip("-1代表永久,0代表此处设置无效,1000 = 1s")] [BoxGroup("选填项")]
    public long SustainTime;

    [LabelText("Buff效果为")] [BoxGroup("选填项")]
    public BuffWorkTypes BuffWorkType;

    [LabelText("Buff基础数值影响者")] [BoxGroup("选填项")]
    public BuffBaseDataEffectTypes BaseBuffBaseDataEffectTypes;

    [LabelText("计算公式")] [BoxGroup("选填项")] [Tooltip("计算公式(level*3+4)")]
    public string Compute;

    [Tooltip("具体的加成(可能会一个效果多种加成方式)，例如法强加成")]
    [BoxGroup("选填项")]
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<BuffAdditionTypes, float> AdditionValue = new Dictionary<BuffAdditionTypes, float>();
}