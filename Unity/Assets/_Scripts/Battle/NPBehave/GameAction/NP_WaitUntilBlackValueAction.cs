using Framework;
using NPBehave;
using Sirenix.OdinInspector;

public class NP_WaitUntilBlackValueAction : NP_CalssForStoreWaitUntilAction
{
    [InfoBox("@info",visibleIfMemberName: nameof(isShowInfoBox))]
    [LabelText("黑板值")]
    public NP_BlackBoardKeyData BlackBoardKey = new();

    private string info => $"当 {BlackBoardKey.BBKey} 的值不等于默认值 {(BlackBoardKey.EditorValue.GetType().DefaultForType())} 的时候会通过此节点";

    private bool isShowInfoBox
    {
        get
        {
            if (string.IsNullOrEmpty(BlackBoardKey.BBKey)) return false;
            return NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager.BBValues.ContainsKey(BlackBoardKey
                .BBKey);
        }
    }

    /// <summary>
    /// 上面黑板值运行时默认值，用来判断是不是跟行为树里面的值相等
    /// </summary>
    private ANP_BBValue bbValue;
    protected override bool UntilFunc()
    {
        if (bbValue == null)
        {
            var treeBBValue = BelongtoRuntimeTree.GetBlackboard().Get(BlackBoardKey.BBKey);
            bbValue = NP_BBValueHelper.AutoCreateNPBBValueFromTValue(treeBBValue.NP_BBValueType.DefaultForType(),
                treeBBValue.NP_BBValueType);
        }
        if (string.IsNullOrEmpty(BlackBoardKey.BBKey)) return true; 
        return NP_BBValueHelper.Compare(BelongtoRuntimeTree.GetBlackboard().Get(BlackBoardKey.BBKey), bbValue,
            Operator.IS_EQUAL);
    }
}