namespace Framework
{
    public enum BuffState
    {
        /// <summary>
        /// 等待执行
        /// </summary>
        Waiting,

        /// <summary>
        /// 正在执行
        /// </summary>
        Running,

        /// <summary>
        /// Buff使命完成
        /// </summary>
        Finished,

        /// <summary>
        /// 永恒的Buff
        /// </summary>
        Forever
    }
}