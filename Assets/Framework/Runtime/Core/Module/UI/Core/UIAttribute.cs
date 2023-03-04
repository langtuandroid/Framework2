using System;

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
        /// 是否是全屏ui，如果是的话就会隐藏其下所有ui
        /// </summary>
        public bool IsFullScreen { get; }

        public UIAttribute(string path, bool isSingle, bool isFullScreen)
        {
            Path = path;
            IsSingle = isSingle;
            IsFullScreen = isFullScreen;
        }
    }
}