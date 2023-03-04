using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class UIBindFactory : BindFactory
    {

        public void BindViewList<TItemVm, TItemView>
            (ObservableList<TItemVm> list,Transform root) where TItemVm : ViewModel
        where TItemView : View
        {
            BindViewList<TItemVm, TItemView> bind;
            if (CacheBinds.Count > 0)
            {
                bind = (BindViewList<TItemVm, TItemView>) CacheBinds.Dequeue();
            }
            else
            {
                bind = ReferencePool.Allocate<BindViewList<TItemVm, TItemView>>();
                bind.Init(Container);
            }
            bind.Reset(list, root);
            AddClearable(bind);
        }
        
        public void BindViewList<TItemVm, TItemView>
            (ObservableList<TItemVm> list,LoopScrollRect root) where TItemVm : ViewModel
            where TItemView : View , new()
        {
            BindLoopViewList<TItemVm, TItemView> bind;
            if (CacheBinds.Count > 0)
            {
                bind = (BindLoopViewList<TItemVm, TItemView>) CacheBinds.Dequeue();
            }
            else
            {
                bind = ReferencePool.Allocate<BindLoopViewList<TItemVm, TItemView>>();
                bind.Init(Container);
            }
            bind.Reset(list, root);
            AddClearable(bind);
        }

        public void BindIpairs<TItemVm, TItemView>
            (ObservableList<TItemVm> list, Transform root, string pattern) where TItemVm : ViewModel
        where TItemView : View
        {
            BindIpairsViewList<TItemVm, TItemView> bind;
            if (CacheBinds.Count > 0)
            {
                bind = (BindIpairsViewList<TItemVm, TItemView>) CacheBinds.Dequeue();
            }
            else
            {
                bind = ReferencePool.Allocate<BindIpairsViewList<TItemVm, TItemView>>();
                bind.Init(Container);
            }
            bind.Reset(list, pattern, root);
            AddClearable(bind);
        }

        public void BindDropDown(Dropdown dropdown, ObservableProperty<int> property,
            ObservableList<Dropdown.OptionData> listProperty = null)
        {
            TwoWayBind(dropdown, property);
            BindList(dropdown, listProperty);
        }
    }
}