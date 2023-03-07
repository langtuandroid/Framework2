using System;

namespace Framework
{
    public class TimeInfo : Singleton<TimeInfo>, ISingletonUpdate
    {
        private int timeZone;

        public int TimeZone
        {
            get { return this.timeZone; }
            set
            {
                this.timeZone = value;
                dt = dt1970.AddHours(TimeZone);
            }
        }

        private DateTime dt1970;
        private DateTime dt;

        public long ServerMinusClientTime { private get; set; }

        public long FrameTime;

        public TimeInfo()
        {
            this.dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            this.FrameTime = this.ClientNow();
        }

        public void Update(float deltaTime)
        {
            this.FrameTime = this.ClientNow();
        }

        /// <summary> 
        /// 根据时间戳获取时间 
        /// </summary>  
        public DateTime ToDateTime(long timeStamp)
        {
            return dt.AddTicks(timeStamp * 10000);
        }

        // 线程安全
        /// <summary>
        /// 返回值是毫秒
        /// </summary>
        /// <returns></returns>
        public long ClientNow()
        {
            return (DateTime.UtcNow.Ticks - this.dt1970.Ticks) / 10000;
        }

        /// <summary>
        /// 返回值是毫秒
        /// </summary>
        /// <returns></returns>
        public long ServerNow()
        {
            return ClientNow() + Instance.ServerMinusClientTime;
        }

        public long ClientFrameTime()
        {
            return this.FrameTime;
        }

        public long ServerFrameTime()
        {
            return this.FrameTime + Instance.ServerMinusClientTime;
        }

        public long Transition(DateTime d)
        {
            return (d.Ticks - dt.Ticks) / 10000;
        }
    }
}