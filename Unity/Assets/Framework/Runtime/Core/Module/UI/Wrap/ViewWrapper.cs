using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public class ViewWrapper : BaseWrapper<View>, IBindList<ViewModel>
    {
        private readonly Transform _content;
        private readonly View _item;
        private readonly GameObject _template;
        private List<View> existViews = new List<View>();

        public ViewWrapper(View component, Transform root)
        {
            _item = component;
            _content = root;
            Log.Assert(_content.childCount == 1, "_content.childCount 只能有一个");
            _template = _content.GetChild(0).gameObject;
            _template.ActiveHide();
        }

        Action<NotifyCollectionChangedAction, ViewModel, int> IBindList<ViewModel>.GetBindListFunc()
        {
            return BindListFunc;
        }

        private void BindListFunc
            (NotifyCollectionChangedAction type, ViewModel newViewModel, int index)
        {
            switch (type)
            {
                case NotifyCollectionChangedAction.Add:
                    AddItem(index, newViewModel);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItem(index);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceItem(index, newViewModel);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Clear(index);
                    break;
                case NotifyCollectionChangedAction.Move: break;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void AddItem(int index, ViewModel vm)
        {
            var view = Activator.CreateInstance(_item.GetType()) as View;
            var go = Object.Instantiate(_template, _content);
            go.transform.SetSiblingIndex(index + 1);
            go.ActiveShow();
            view.SetGameObject(go);
            view.SetVm(vm);
            view.Show();
            existViews.Insert(index, view);
        }

        private void RemoveItem(int index)
        {
            Object.DestroyImmediate(_content.GetChild(index + 1).gameObject);
            existViews.RemoveAt(index);
        }

        private void ReplaceItem(int index, ViewModel vm)
        {
            existViews[index].SetVm(vm);
        }

        private void Clear(int itemCount)
        {
            while (itemCount > 0)
            {
                RemoveItem(--itemCount);
            }

            existViews.Clear();
        }

        public void ClearView()
        {
            foreach (var existView in existViews)
            {
                existView.Dispose();
            }

            existViews.Clear();
        }
    }
}