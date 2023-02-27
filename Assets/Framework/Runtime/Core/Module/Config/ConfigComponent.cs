using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// Config组件会扫描所有的有ConfigAttribute标签的配置,加载进来
    /// </summary>
    public class ConfigComponent : Singleton<ConfigComponent>
    {

        public struct GetOneConfigBytes
        {
            public string ConfigName;
        }

        private readonly Dictionary<Type, ISingleton> allConfig = new Dictionary<Type, ISingleton>();

        public override void Dispose()
        {
            foreach (var kv in this.allConfig)
            {
                kv.Value.Destroy();
            }
        }

        public object LoadOneConfig(Type configType)
        {
            this.allConfig.TryGetValue(configType, out ISingleton oneConfig);
            if (oneConfig != null)
            {
                oneConfig.Destroy();
            }

            byte[] oneConfigBytes = EventSystem.Instance.Invoke<GetOneConfigBytes, byte[]>(new GetOneConfigBytes()
                { ConfigName = configType.FullName });

            object category = SerializeHelper.Deserialize(configType, oneConfigBytes, 0, oneConfigBytes.Length);
            ISingleton singleton = category as ISingleton;
            singleton.Register();

            this.allConfig[configType] = singleton;
            return category;
        }

        public void Load()
        {
            this.allConfig.Clear();
            var typeAndAttribute = EventSystem.Instance.GetTypesAndAttribute(typeof(ConfigAttribute));
            using RecyclableList<Task> recyclableListTasks = RecyclableList<Task>.Create();

            foreach ((BaseAttribute attribute, Type type) item in typeAndAttribute)
            {
                var oneConfigBytes =
                    Res.Default.LoadAsset<TextAsset>((item.attribute as ConfigAttribute).Path).bytes;
                LoadOneInThread(item.type, oneConfigBytes);
            }
        }

        public async ETTask LoadAsync()
        {
            this.allConfig.Clear();
            var typeAndAttribute = EventSystem.Instance.GetTypesAndAttribute(typeof(ConfigAttribute));
            using RecyclableList<Task> recyclableListTasks = RecyclableList<Task>.Create();

            foreach ((BaseAttribute attribute, Type type) item in typeAndAttribute)
            {
                var oneConfigBytes =
                    (await Res.Default.LoadAssetAsync<TextAsset>((item.attribute as ConfigAttribute).Path)).bytes;
                Task task = Task.Run(() => LoadOneInThread(item.type, oneConfigBytes));
                recyclableListTasks.Add(task);
            }

            await Task.WhenAll(recyclableListTasks.ToArray());
        }

        private void LoadOneInThread(Type configType, byte[] oneConfigBytes)
        {
            object category = SerializeHelper.Deserialize(configType, oneConfigBytes, 0, oneConfigBytes.Length);

            lock (this)
            {
                ISingleton singleton = category as ISingleton;
                singleton.Register();
                this.allConfig[configType] = singleton;
            }
        }
    }
}