using System;
using System.Collections.Generic;
using Framework;

namespace ET
{
    public class CDInfo : IReference
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 时间间隔（CD）
        /// </summary>
        public float Interval { get; set; }

        /// <summary>
        /// 剩余CD时长
        /// </summary>
        public float RemainCDLength { get; set; }

        /// <summary>
        /// CD是否转好了
        /// </summary>
        public bool Finish => RemainCDLength <= 0;

        /// <summary>
        /// CD信息变化时的回调
        /// </summary>
        public Action<CDInfo> CDChangedCallBack;

        public void Init(string name, float cdLength, Action<CDInfo> cDChangedCallBack = null)
        {
            Name = name;
            Interval = cdLength;
            RemainCDLength = cdLength;
            CDChangedCallBack = cDChangedCallBack;
        }

        public void Clear()
        {
            Name = null;
            Interval = 0;
            RemainCDLength = 0;
            CDChangedCallBack = null;
        }
    }

    /// <summary>
    /// CD组件，用于统一管理所有的CD类型的数据，比如攻速CD，服务器上因试图攻击导致的循环MoveTo CD
    /// </summary>
    public class CDComponent : Entity, IDestroySystem, IUpdateSystem
    {
        /// <summary>
        /// 包含所有CD信息的字典
        /// 键为id，值为对应所有CD信息
        /// </summary>
        private Dictionary<long, Dictionary<string, CDInfo>> CDInfos = new();


        /// <summary>
        /// 新增一条CD数据
        /// </summary>
        public CDInfo AddCDData(long id, string name, float cDLength, Action<CDInfo> onCDChangedCallback = null)
        {
            if (GetCDData(id, name) != null)
            {
                Log.Error($"已注册id为：{id}，Name为：{name}的CD信息，请勿重复注册");
                return null;
            }

            CDInfo cdInfo = ReferencePool.Allocate<CDInfo>();
            cdInfo.Init(name, cDLength, onCDChangedCallback);
            AddCDData(id, cdInfo);
            return cdInfo;
        }

        /// <summary>
        /// 新增一条CD数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cdInfo"></param>
        public CDInfo AddCDData(long id, CDInfo cdInfo)
        {
            if (CDInfos.TryGetValue(id, out Dictionary<string, CDInfo> cdInfoDic))
            {
                cdInfoDic.Add(cdInfo.Name, cdInfo);
            }
            else
            {
                CDInfos.Add(id, new Dictionary<string, CDInfo>() { { cdInfo.Name, cdInfo } });
            }

            return cdInfo;
        }

        /// <summary>
        /// 触发某个CD，使其进入CD状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="cdLength">CD长度</param>
        public void TriggerCD(long id, string name, long cdLength = -1)
        {
            CDInfo cdInfo = GetCDData(id, name);
            cdInfo.RemainCDLength = cdLength == -1 ? cdInfo.Interval : cdLength;
        }

        /// <summary>
        /// 获取CD数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public CDInfo GetCDData(long id, string name)
        {
            if (CDInfos.TryGetValue(id, out Dictionary<string, CDInfo> cdInfoDic))
            {
                if (cdInfoDic.TryGetValue(name, out CDInfo cdInfo))
                {
                    return cdInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 增加CD时间到指定CD
        /// </summary>
        public void AddCD(long id, string name, float addedCDLength)
        {
            CDInfo cdInfo = GetCDData(id, name);
            cdInfo.RemainCDLength += addedCDLength;
            cdInfo.CDChangedCallBack?.Invoke(cdInfo);
        }

        /// <summary>
        /// 减少CD时间到指定CD
        /// </summary>
        public void ReduceCD(long id, string name, float reducedCDLength)
        {
            CDInfo cdInfo = GetCDData(id, name);
            cdInfo.RemainCDLength -= reducedCDLength;
            cdInfo.CDChangedCallBack?.Invoke(cdInfo);
        }

        /// <summary>
        /// 直接重设CD数据以及CD的剩余时长
        /// </summary>
        public void SetCD(long id, string name, float cDLength, float remainCdLength)
        {
            CDInfo cdInfo = GetCDData(id, name);

            if (cdInfo == null)
            {
                cdInfo = AddCDData(id, name, cDLength, null);
            }

            cdInfo.Interval = cDLength;
            cdInfo.RemainCDLength = remainCdLength;
            cdInfo.CDChangedCallBack?.Invoke(cdInfo);
        }

        /// <summary>
        /// 获取CD结果
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetCDResult(long id, string name)
        {
            if (CDInfos.TryGetValue(id, out Dictionary<string, CDInfo> cdInfoDic))
            {
                if (cdInfoDic.TryGetValue(name, out CDInfo cdInfo))
                {
                    return cdInfo.Finish;
                }
            }

            Log.Error($"尚未注册id为：{id}，Name为：{name}的CD信息");
            return false;
        }

        /// <summary>
        /// 移除一条CD数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void RemoveCDData(long id, string name)
        {
            if (CDInfos.TryGetValue(id, out Dictionary<string, CDInfo> cdInfoDic))
            {
                cdInfoDic.Remove(name);
            }
        }

        public void OnDestroy()
        {
            foreach (KeyValuePair<long, Dictionary<string, CDInfo>> cdInfoList in CDInfos)
            {
                foreach (KeyValuePair<string, CDInfo> cdInfo in cdInfoList.Value)
                {
                    ReferencePool.Free(cdInfo.Value);
                }

                cdInfoList.Value.Clear();
            }

            CDInfos.Clear();
        }

        public void Update(float deltaTime)
        {
            foreach (KeyValuePair<long, Dictionary<string, CDInfo>> cdInfoDic in CDInfos)
            {
                foreach (KeyValuePair<string, CDInfo> cdInfo in cdInfoDic.Value)
                {
                    if (cdInfo.Value.Finish)
                    {
                        continue;
                    }

                    cdInfo.Value.RemainCDLength -= deltaTime;
                    if (!cdInfo.Value.Finish)
                    {
                        cdInfo.Value.CDChangedCallBack?.Invoke(cdInfo.Value);
                    }
                }
            }
        }
    }
}