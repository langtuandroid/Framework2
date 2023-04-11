using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public enum UILevel
    {
        //这个在第一个
        None,
        Bg,
        Common,
        Pop,
        Toast,
        Guide,
        FullScreen,
        //这个放到最下边
        Max,
    }
    
    public abstract class View : Entity
    {
        private List<View> _subViews;
        private CanvasGroup _canvasGroup;
        public GameObject Go { get; private set; }
        public ViewModel ViewModel { get; private set; }
        protected readonly UIBindFactory Binding;

        public View()
        {
            _subViews = new List<View>();
            Binding = ReferencePool.Allocate<UIBindFactory>();
            Binding.Init(this);
        }

        public void SetGameObject(GameObject obj)
        {
            Go = obj;
            _canvasGroup = Go.GetOrAddComponent<CanvasGroup>();
            Start();
        }

        public void SetVm(ViewModel vm)
        {
            if (vm == null || ViewModel == vm) return;
            ViewModel = vm;
            if (ViewModel != null)
            {
                Binding.Reset();
                OnVmChange();
            }
        }

        #region 界面显示隐藏的调用和回调方法

        protected virtual void Start()
        {
            
        }

        public void Show()
        {
            Visible(true);
            OnShow();
        }

        public void Hide()
        {
            Visible(false);
            ViewModel?.OnViewHide();
            OnHide();
        }

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }

        protected virtual void OnClose()
        {
        }

        public void Visible(bool visible)
        {
            if (Go == null) return;
            _canvasGroup.interactable = visible;
            _canvasGroup.alpha = visible ? 1 : 0;
            _canvasGroup.blocksRaycasts = visible;
        }

        #endregion

        public IProgressResult<float, View> AddSubView<T>(ViewModel viewModel = null) where T : View
        {
            var progressResult = this.RootScene().GetComponent<UIManager>().CreateViewAsync(typeof(T), viewModel);
            progressResult.Callbackable().OnCallback((result => AddSubView(result.Result)));
            return progressResult;
        }
        
        public IProgressResult<float, View> AddSubView(Type type, ViewModel viewModel = null)
        {
            var progressResult = this.RootScene().GetComponent<UIManager>().CreateViewAsync(type, viewModel);
            progressResult.Callbackable().OnCallback((result => AddSubView(result.Result)));
            return progressResult;
        }

        protected void RemoveSubView(View view)
        {
            _subViews.TryRemove(view);
        }
        
        public void AddSubView(View view)
        {
            view.Go.transform.SetParent(Go.transform, false);
            _subViews.Add(view);
        }

        protected T GetSubView<T>() where T : View
        {
            foreach (var subView in _subViews)
            {
                if (subView is T view)
                    return view;
            }
            return null;
        }

        protected void Close()
        {
            this.RootScene().GetComponent<UIManager>().Close(this);
            OnClose();
            for (int i = 0; i < _subViews.Count; i++)
            {
                _subViews[i].OnClose();
            }
            ViewModel?.OnViewDestroy();
        }

        public void Dispose()
        {
            Close();
            Binding.Clear();
            if(Go != null)
                Object.Destroy(Go);
        }

        protected abstract void OnVmChange();
        public virtual UILevel UILevel { get; } = UILevel.Common;
    }
}