using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    public class ResComponent : Singleton<ResComponent> , ISingletonAwake
    {
        private IRes res;
        public void Awake()
        {
            res = new YooRes();
        }

        public IAsyncResult Init()
        {
            return res.Init();
        }

        public string HostServerURL
        {
            get => res.HostServerURL;
            set => res.HostServerURL = value;
        }

        public string FallbackHostServerURL
        {
            get => res.FallbackHostServerURL;
            set => res.FallbackHostServerURL = value;
        }

        public IProgressResult<float, T> LoadAssetAsync<T>(string key) where T : Object
        {
            return res.LoadAssetAsync<T>(key);
        }

        public IProgressResult<float, T> InstantiateAsync<T>(string key, Transform parent = null,
            bool instantiateInWorldSpace = false)
        {
            return res.InstantiateAsync<T>(key, parent, instantiateInWorldSpace);
        }

        public IProgressResult<float, T> InstantiateAsync<T>(string key, Vector3 localPosition, Quaternion localRotation,
            Transform parent = null)
        {
            return res.InstantiateAsync<T>(key, localPosition, localRotation, parent);
        }

        public IProgressResult<float, GameObject> InstantiateAsync(string key, Vector3 localPosition,
            Quaternion localRotation,
            Transform parent = null)
        {
            return res.InstantiateAsync(key, localPosition, localRotation, parent);
        }

        public IProgressResult<float, GameObject> InstantiateAsync(string key, Transform parent = null,
            bool instantiateInWorldSpace = false)
        {
            return res.InstantiateAsync(key, parent, instantiateInWorldSpace);
        }

        public IProgressResult<float, string> LoadScene(string path, LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            bool allowSceneActivation = true)
        {
            return res.LoadScene(path, loadSceneMode, allowSceneActivation);
        }

        public IProgressResult<float, string> CheckDownloadSize()
        {
            return res.CheckDownloadSize();
        }

        public IProgressResult<DownloadProgress> DownloadAssets()
        {
            return res.DownloadAssets();
        }

        public GameObject Instantiate(string key, Transform parent = null, bool instantiateInWorldSpace = false)
        {
            return res.Instantiate(key, parent, instantiateInWorldSpace);
        }

        public T LoadAsset<T>(string key) where T : Object
        {
            return res.LoadAsset<T>(key);
        }

        public void Release()
        {
            res.Release();
        }
    }
}