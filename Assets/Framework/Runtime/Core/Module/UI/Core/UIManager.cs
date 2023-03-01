using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public class UIManager : Singleton<UIManager>, ISingletonAwake
    {
        private IRes _res;
        public Canvas Canvas { get; private set; }
        private Dictionary<Type, string> viewType2Path = new();


        public void Awake()
        {
            Canvas = Resources.Load<GameObject>("UIRoot").GetComponent<Canvas>();
            Object.DontDestroyOnLoad(Canvas);
            _res = Res.Create();
            foreach (var tuple in EventSystem.Instance.GetTypesAndAttribute(typeof(UIAttribute)))
            {
                viewType2Path[tuple.type] = (tuple.attribute as UIAttribute).Path;
            }
        }

        private List<View> openedViews = new List<View>();
        private Dictionary<Type, View> openedSingleViews = new Dictionary<Type, View>();

        public IProgressResult<float, View> OpenAsync<T>(ViewModel viewModel = null) where T : View
        {
            ProgressResult<float, View> result = new ProgressResult<float, View>();
            InternalOpenAsync(typeof(T), result, viewModel);
            return result;
        }

        public IProgressResult<float, View> OpenAsync(Type type, ViewModel viewModel = null)
        {
            ProgressResult<float, View> result = new ProgressResult<float, View>();
            InternalOpenAsync(type, result, viewModel);
            return result;
        }

        private void InternalOpenAsync<T>(Type type, ProgressResult<float, T> promise, ViewModel viewModel) where T : View
        {
            var path = viewType2Path[type];
            promise.Callbackable().OnCallback(progressResult =>
            {
                Sort(progressResult.Result);
                progressResult.Result.Show();
            });
            if (openedSingleViews.TryGetValue(type, out var view))
            {
                promise.UpdateProgress(1);
                promise.SetResult(view);
            }
            else
            {
                view = Activator.CreateInstance(type) as View;
                Executors.RunOnCoroutineNoReturn(CreateViewGo(promise, view, path, viewModel));
            }
            
            if (view.IsSingle)
            {
                openedSingleViews[type] = view;
                openedViews.TryAddSingle(view);
            }
            else
            {
                openedViews.Add(view);    
            }
        }

        public IProgressResult<float, T> CreateViewAsync<T>(ViewModel vm) where T : View
        {
            ProgressResult<float, T> progressResult = new ProgressResult<float, T>();
            var type = typeof(T);
            var view = Activator.CreateInstance(type) as View;
            var path = viewType2Path[type];
            Executors.RunOnCoroutineNoReturn(CreateViewGo(progressResult, view, path, vm));
            return progressResult;
        }
        
        public IProgressResult<float, View> CreateViewAsync(Type type, ViewModel vm)
        {
            ProgressResult<float, View> progressResult = new ProgressResult<float, View>();
            var view = Activator.CreateInstance(type) as View;
            var path = viewType2Path[type];
            Executors.RunOnCoroutineNoReturn(CreateViewGo(progressResult, view, path, vm));
            return progressResult;
        }

        private IEnumerator CreateViewGo<T>(IProgressPromise<float, T> promise,View view,string path, ViewModel viewModel)
            where T : View
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
            GameObject go = Object.Instantiate(viewTemplateGo);
            view.SetGameObject(go);
            go.name = viewTemplateGo.name;
            promise.UpdateProgress(1f);
            promise.SetResult(view);
            view.SetVm(viewModel);
        }

        public T Open<T>(ViewModel viewModel = null) where T : View
        {
            var view = CreateView(typeof(T), viewModel) as T;
            if (view.IsSingle)
            {
                openedSingleViews[typeof(T)] = view;
                openedViews.TryAddSingle(view);
            }
            else
            {
                openedViews.Add(view);    
            }
            return view;
        }

        private View CreateView(Type type, ViewModel viewModel)
        {
            var path = viewType2Path[type];
            var loadGo = _res.Instantiate(path);
            View view = Activator.CreateInstance(type) as View;
            view.SetGameObject(loadGo);
            view.SetVm(viewModel);
            Sort(view);
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
            if (view.IsSingle)
            {
                Close(view.GetType());
                return;
            }
            for (int i = 0; i < openedViews.Count; i++)
            {
                if (view == openedViews[i])
                {
                    openedViews.RemoveAt(i);
                    view.Dispose();
                    break;
                }
            }
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
            for (int i = 0; i < openedViews.Count; i++)
            {
                if (view == openedViews[i])
                {
                    openedViews.RemoveAt(i);
                    break;
                }
            }
            view.Dispose();
            //_waitDestroyViews[view] = DateTime.Now.AddSeconds(ViewDestroyTime);
        }
        
        public void CloseAll()
        {
            foreach (var openedView in openedViews)
            {
                openedView.Dispose();
            }
            openedViews.Clear();
            openedSingleViews.Clear();
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

    }
}