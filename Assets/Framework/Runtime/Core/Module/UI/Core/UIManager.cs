using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public class UIManager : Singleton<UIManager>, ISingletonAwake, ISingletonUpdate
    {
        private IRes _res;
        public Canvas Canvas { get; private set; }
        private Dictionary<Type, UIAttribute> viewType2Attribute = new();
        private Dictionary<Type, View> openedSingleViews = new();
        private MultiMap<Type, (GameObject go, DateTime destroyTime)> _waitDestroyViews = new();
        private Dictionary<Type, IProgressResult<float, View>> loadingView = new();
        private Dictionary<UILevel, List<View>> uiLevel2View = new();
        private const double ViewDestroyTime = 5;

        public void Awake()
        {
            Canvas = Resources.Load<GameObject>("UIRoot").GetComponent<Canvas>();
            Object.DontDestroyOnLoad(Canvas);
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
                // 如果加载过程中就关闭了，直接放到销毁池里
                if (progressResult.IsCancelled)
                {
                    _waitDestroyViews.Add(type, (progressResult.Result.Go, DateTime.Now.AddSeconds(ViewDestroyTime)));
                    return;
                }
                Sort(progressResult.Result);
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

            if (!uiLevel2View.TryGetValue(view.UILevel, out var list))
            {
                list = new List<View>();
                uiLevel2View[view.UILevel] = list;
            }

            if (attribute.IsSingle)
            {
                openedSingleViews[type] = view;
                list.TryAddSingle(view);
            }
            else
            {
                list.Add(view);
            }
        }

        public IProgressResult<float, T> CreateViewAsync<T>(ViewModel vm) where T : View
        {
            ProgressResult<float, T> progressResult = new();
            var type = typeof(T);
            var view = Activator.CreateInstance(type) as View;
            var path = viewType2Attribute[type].Path;
            Executors.RunOnCoroutineNoReturn(CreateViewGo(progressResult, view, path, vm));
            return progressResult;
        }
        
        public IProgressResult<float, View> CreateViewAsync(Type type, ViewModel vm)
        {
            ProgressResult<float, View> progressResult = new();
            var view = Activator.CreateInstance(type) as View;
            var path = viewType2Attribute[type].Path;
            Executors.RunOnCoroutineNoReturn(CreateViewGo(progressResult, view, path, vm));
            return progressResult;
        }

        private IEnumerator CreateViewGo<T>(IProgressPromise<float, T> promise,View view,string path, ViewModel viewModel)
            where T : View
        {
            GameObject go = null;
            var type = typeof(T);
            if (_waitDestroyViews.TryGetValue(type, out var list) && list.Count > 0)
            {
                go = list.RemoveLast().go;
                if (list.Count <= 0)
                    _waitDestroyViews.Remove(type);
            }
            else
            {
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

                go = Object.Instantiate(viewTemplateGo);
            }

            view.SetGameObject(go);
            go.name = type.Name;
            promise.UpdateProgress(1f);
            promise.SetResult(view);
            view.SetVm(viewModel);
        }

        public T Open<T>(ViewModel viewModel = null) where T : View
        {
            var type = typeof(T);
            var view = CreateView(type, viewModel) as T;
            Sort(view);
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

            return view;
        }

        private View CreateView(Type type, ViewModel viewModel)
        {
            GameObject go = null;
            if (_waitDestroyViews.TryGetValue(type, out var list) && list.Count > 0)
            {
                go = list.RemoveLast().go;
                if (list.Count <= 0)
                    _waitDestroyViews.Remove(type);
            }
            else
            {
                var path = viewType2Attribute[type].Path;
                go = _res.Instantiate(path);
            }

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
            _waitDestroyViews.Add(view.GetType(), (view.Go, DateTime.Now.AddSeconds(ViewDestroyTime)));
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
        
        public void Close(Type type)
        {
            if (!openedSingleViews.TryGetValue(type, out var view))
                return;
            openedSingleViews.Remove(type);
            uiLevel2View[view.UILevel].Remove(view);
            view.Dispose();
            _waitDestroyViews.Add(type, (view.Go, DateTime.Now.AddSeconds(ViewDestroyTime)));
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

        private void Sort(View view)
        {
            var viewTransform = view.Go.transform;
            Transform lastTrans = null;
            int index = Int32.MaxValue;
            foreach (View openedView in openedViews)
            {
				if(openedView.Go == null) continue;
                if(openedView.UILevel <= view.UILevel)
                    continue;
                try
                {
                    if (openedView.Go.transform.GetSiblingIndex() < index)
                    {
                        lastTrans = openedView.Go.transform;
                        index = lastTrans.GetSiblingIndex();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
               
            }
            
            viewTransform.SetParent(Canvas.transform, false);
            if (lastTrans == null)
                viewTransform.SetAsLastSibling();
            else
                viewTransform.SetSiblingIndex(index);
        }

        public void Update()
        {
            
        }
    }
}