using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Framework
{
    public class BindViewList<TVm, TView> : BaseBind where TVm : ViewModel where TView : View
    {
        private Transform _content;
        private List<View> _views;
        private ObservableList<TVm> _list;
        private List<ViewWrapper> _wrappers;
        private Type viewType;

        public void Reset(ObservableList<TVm> list, Transform root)
        {
            _views = new List<View>();
            _content = root;
            _list = list;
            InitEvent();
            InitCpntValue(); 
        }

        public void SetViewType(Type type)
        {
            viewType = type;
        }

        private void InitCpntValue()
        {
            for (var i = 0; i < _list.Count; i++)
            {
                var vm = _list[i];
                _wrappers.ForEach((wrapper) =>
                    ((IBindList<ViewModel>) wrapper).GetBindListFunc()(NotifyCollectionChangedAction.Add, vm, i));
            }
        }

        private void InitEvent()
        {
            var view = Activator.CreateInstance(viewType) as View;
            var wrapper = new ViewWrapper(view, _content);
            _list.AddListener(((IBindList<ViewModel>)wrapper).GetBindListFunc());
            _wrappers.Add(wrapper);
        }

        protected override void OnReset()
        {
            foreach (var wrapper in _wrappers)
            {
                wrapper.ClearView();
                _list.RemoveListener(((IBindList<ViewModel>)wrapper).GetBindListFunc());
            }
        }

        protected override void OnClear()
        {
            _content = default;
            _views = default;
            _list = default;
            _wrappers = default;
            viewType = default;
        }
    }

    public class BindIpairsViewList<TVm, TView> : BaseBind where TVm : ViewModel where TView : View
    {
        private ObservableList<TVm> _list;
        private List<View> _views;
        private Type viewType;

        public void SetViewType(Type type)
        {
            viewType = type;
        }

        public void Reset(ObservableList<TVm> list, string itemName, Transform root)
        {
            SetValue(list, itemName, root);
        }

        private void SetValue(ObservableList<TVm> list, string itemName, Transform root)
        {
            this._list = list;
            ParseItems(itemName, root);
            InitEvent();
        }

        private void ParseItems(string itemName, Transform root)
        {
            _views = new List<View>();
            var regex = new Regex(@"(\w+)?\[\?\]");
            var match = regex.Match(itemName);
            Log.Assert(match.Success, $"{itemName} not match (skill[?]) pattern.");
            itemName = match.Groups[1].Value;
            regex = new Regex(itemName + @"\[\d+\]");
            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                if (regex.IsMatch(child.name))
                {
                    var view = Activator.CreateInstance(viewType) as View;
                    view.SetGameObject(child.gameObject);
                    _views.Add(view);
                }
            }
        }

        private void InitEvent()
        {
            for (var i = 0; i < _views.Count; i++) _views[i].SetVm(_list[i]);
        }

        protected override void OnReset()
        {
            foreach (var view in _views)
            {
                view.Dispose();
            }
        }

        protected override void OnClear()
        {
            _list = default;
            _views = default;
            viewType = default;
        }
    }
}