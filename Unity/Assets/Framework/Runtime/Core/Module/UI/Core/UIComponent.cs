using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public class UIComponent : Entity, IAwakeSystem
    {
        private IRes _res;
        private Transform destroyPoolContent;
        private readonly Dictionary<Type, UIAttribute> viewType2Attribute = new();
        private readonly Dictionary<Type, View> openedSingleViews = new();
        private readonly Dictionary<Type, IProgressResult<float, View>> loadingView = new();
        private readonly Dictionary<UILevel, List<View>> uiLevel2View = new();
        private readonly Dictionary<Type, List<DelayDestroyGo>> delayDestroyViewDic = new();

        private class DelayDestroyGo : IReference
        {
            public float DestroyTime;
            public GameObject Go;
            
            public void Clear()
            {
                DestroyTime = 0;
                Go = null;
            }
        }

        public void Awake()
        {
            destroyPoolContent = new GameObject("DestroyPool").transform;
            var canvas = this.RootScene().GetComponent<GlobalReferenceComponent>().UICanvas;
            destroyPoolContent.SetParent(canvas.transform);
            var canvasGroup = destroyPoolContent.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = true;
            Object.DontDestroyOnLoad(canvas.gameObject);
            _res = Res.Create();
            foreach (var tuple in EventSystem.Instance.GetTypesAndAttribute(typeof(UIAttribute)))
            {
                viewType2Attribute[tuple.type] = tuple.attribute as UIAttribute;
            }
        }

        public IProgressResult<float, View> OpenAsync<T>(ViewModel viewModel = null) where T : View
        {
            var type = typeof(T);
            if (viewType2Attribute[type].IsSingle && loadingView.TryGetValue(type, out var result))
                return result;
            ProgressResult<float, View> result1 = new();
            InternalOpenAsync<T>(type, result1, viewModel);
            return result1;
        }

        private void InternalOpenAsync<T>(Type type, ProgressResult<float, View> promise, ViewModel viewModel)
            where T : View
        {
            var attribute = viewType2Attribute[type];
            loadingView[type] = promise;
            promise.Callbackable().OnCallback(progressResult =>
            {
                // 如果加载过程中就关闭了，直接销毁
                if (progressResult.IsCancelled)
                {
                    progressResult.Result.Dispose();
                    return;
                }

                Sort(progressResult.Result);
                AddOpenView(progressResult.Result);
                progressResult.Result.Show();
                loadingView.Remove(type);
            });
            if (openedSingleViews.TryGetValue(type, out var view))
            {
                promise.UpdateProgress(1);
                promise.SetResult(view);
            }
            else
            {
                view = Activator.CreateInstance(type) as View;
                Executors.RunOnCoroutineNoReturn(CreateViewGo(promise, view, attribute.Path, viewModel));
            }
        }

        internal IProgressResult<float, View> CreateViewAsync(Type type, ViewModel vm)
        {
            ProgressResult<float, View> progressResult = new();
            var view = Activator.CreateInstance(type) as View;
                Executors.RunOnCoroutineNoReturn(CreateViewGo(progressResult, view, viewType2Attribute[type].Path, vm));
            return progressResult;
        }

        private IEnumerator CreateViewGo<T>(IProgressPromise<float, T> promise, View view, string path,
            ViewModel viewModel)
            where T : View
        {
            var type = typeof(T);

            var request = _res.LoadAssetAsync<GameObject>(path);
            while (!request.IsDone)
            {
                promise.UpdateProgress(request.Progress);
                yield return null;
            }

            GameObject viewTemplateGo = request.Result;
            if (viewTemplateGo == null)
            {
                promise.UpdateProgress(1f);
                Log.Error($"Not found the window path = \"{path}\".");
                promise.SetException(new FileNotFoundException(path));
                yield break;
            }

            var go = Object.Instantiate(viewTemplateGo);

            view.SetGameObject(go);
            go.name = type.Name;
            promise.SetResult(view);
            view.SetVm(viewModel);
        }

        public T Open<T>(ViewModel viewModel = null) where T : View
        {
            var type = typeof(T);
                View view = CreateView(type, viewModel) as T;
            Sort(view);
            AddOpenView(view);

            return (T)view;
        }

        private void AddOpenView(View view)
        {
            var type = view.GetType();
            if (!uiLevel2View.TryGetValue(view.UILevel, out var list))
            {
                list = new List<View>();
                uiLevel2View[view.UILevel] = list;
            }

            if (viewType2Attribute[type].IsSingle)
            {
                openedSingleViews[type] = view;
                list.TryAddSingle(view);
            }
            else
            {
                list.Add(view);
            }
        }

        private View CreateView(Type type, ViewModel viewModel)
        {
            var path = viewType2Attribute[type].Path;
            var go = _res.Instantiate(path);
            View view = Activator.CreateInstance(type) as View;
            view.SetGameObject(go);
            view.SetVm(viewModel);
            return view;
        }

        /// <summary>
        /// close isSingle=true的窗口
        /// </summary>
        public void Close<T>() where T : View
        {
            Close(typeof(T));
        }

        public void Close(View view)
        {
            if (viewType2Attribute[view.GetType()].IsSingle)
            {
                Close(view.GetType());
                return;
            }

            uiLevel2View[view.UILevel].Remove(view);
            view.Dispose();
            MaskViews(view, true);
        }

        public void Close(Type type)
        {
            if (!openedSingleViews.TryGetValue(type, out var view))
                return;
            openedSingleViews.Remove(type);
            uiLevel2View[view.UILevel].Remove(view);
            view.Dispose();
            MaskViews(view, true);
        }

        public T Get<T>() where T : View
        {
            var view = Get(typeof(T));
            return view as T;
        }

        public View Get(Type type)
        {
            if (openedSingleViews.TryGetValue(type, out var view))
            {
                return view;
            }

            return null;
        }

        public void CloseAll()
        {
            using RecyclableList<View> views = RecyclableList<View>.Create();
            uiLevel2View.Values.ForEach(v => views.AddRange(v));
            foreach (var view in views)
            {
                Close(view);
            }
        }

        public IAsyncResult<GameObject> CreateViewGameObjectAsync(Type type)
        {
            IAsyncResult<GameObject> result;
            var gos = GetDelayDestroyGoes(type);
            if (gos.Count > 0)
            {
                var asyncResult = new AsyncResult<GameObject>();
                asyncResult.SetResult(gos.RemoveLast().Go);
                result = asyncResult;
            }
            else
            {
                var path = viewType2Attribute[type].Path;
                result = _res.InstantiateAsync<GameObject>(path);
            }

            return result;
        }

        public IAsyncResult<GameObject> CreateViewGameObjectAsync<T>()
        {
            return CreateViewGameObjectAsync(typeof(T));
        }

        public GameObject CreateViewGameObject(Type type)
        {
            var gos = GetDelayDestroyGoes(type);
            if (gos.Count > 0)
            {
                return gos.RemoveLast().Go;
            }
            var path = viewType2Attribute[type].Path;
            var go = _res.Instantiate(path);
            return go;
        }

        public GameObject CreateViewGameObject<T>()
        {
            return CreateViewGameObject(typeof(T));
        }

        public void FreeViewGameObject<T>(T view) where T : View
        {
            if(view.Go == null) return;
            var gos = GetDelayDestroyGoes(view.GetType());
            var delayDestroyGo = ReferencePool.Allocate<DelayDestroyGo>();
            // 5s后销毁
            delayDestroyGo.DestroyTime = TimeInfo.Instance.ClientNow() + 5000;
            delayDestroyGo.Go = view.Go;
            view.Go.transform.SetParent(destroyPoolContent);
            gos.Add(delayDestroyGo);
        }

        private List<DelayDestroyGo> GetDelayDestroyGoes(Type type)
        {
            if (!delayDestroyViewDic.TryGetValue(type, out var result))
            {
                result = new List<DelayDestroyGo>();
                delayDestroyViewDic[type] = result;
            }

            return result;
        }

        private void Sort(View view)
        {
            var viewTransform = view.Go.transform;
            Transform lastTrans = null;
            int index = Int32.MaxValue;

            for (int i = (int)view.UILevel + 1; i < (int)UILevel.Max; i++)
            {
                UILevel level = (UILevel)i;
                if (uiLevel2View.TryGetValue(level, out var views) && views.Count > 0)
                {
                    lastTrans = views.Last().Go.transform;
                    break;
                }
            }

            viewTransform.SetParent(this.RootScene().GetComponent<GlobalReferenceComponent>().UICanvas.transform, false);
            if (lastTrans == null)
                viewTransform.SetAsLastSibling();
            else
                viewTransform.SetSiblingIndex(index);
            MaskViews(view, true);
        }

        private void MaskViews(View view, bool open)
        {
            bool isMaskBottomView = viewType2Attribute[view.GetType()].IsMaskBottomView;
            // 如果不会挡住下面的界面，则直接返回
            if (!isMaskBottomView) return;
            if (open)
            {
                // 打开则隐藏下面的所有ui
                for (int i = (int)view.UILevel; i > (int)UILevel.None; i--)
                {
                    UILevel level = (UILevel)i;
                    if (!uiLevel2View.TryGetValue(level, out var views) || views.Count <= 0) continue;
                    foreach (var openedView in views)
                    {
                        if (openedView != view)
                            openedView.Hide();
                    }
                }
            }
            else
            {
                // 打开下面的ui，直到碰到一个遮挡下面ui的ui
                for (int i = (int)view.UILevel; i > (int)UILevel.None; i--)
                {
                    UILevel level = (UILevel)i;
                    if (!uiLevel2View.TryGetValue(level, out var views) || views.Count <= 0) continue;
                    foreach (var openedView in views)
                    {
                        if (openedView == view) continue;
                        openedView.Show();
                        var type = openedView.GetType();
                        if (viewType2Attribute[type].IsMaskBottomView)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}