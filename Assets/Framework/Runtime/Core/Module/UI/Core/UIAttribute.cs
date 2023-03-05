namespace Framework
{
    public class UIAttribute : BaseAttribute
    {
        public string Path { get; }
        /// <summary>
        /// 是否是只能打开一个的ui，如果是，则在当前ui已经打开的情况打开第二次会把当前ui提到最上层显示，不会再额外创建新的ui
        /// </summary>
        public bool IsSingle { get; }
        /// <summary>
        /// 是否会隐藏其下所有ui
        /// </summary>
        public bool IsMaskBottomView { get; }

        public UIAttribute(string path, bool isSingle, bool isMaskBottomView)
        {
            Path = path;
            IsSingle = isSingle;
            IsMaskBottomView = isMaskBottomView;
        }
    }
}