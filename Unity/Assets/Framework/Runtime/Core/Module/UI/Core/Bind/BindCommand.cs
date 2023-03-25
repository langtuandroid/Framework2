﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    public class BindCommand<TComponent> : BaseBind where TComponent : class
    {
        private TComponent _component;
        private Action _command;
        private UnityEvent _componentEvent;
        private object _defaultWrapper;
        private Func<Action, Action> _wrapFunc;

        public void Reset(TComponent component, Action command, UnityEvent componentEvent,
            Func<Action, Action> wrapFunc)
        {
            SetValue(component, command, componentEvent, wrapFunc);
            InitEvent();
        }

        private void SetValue(TComponent component, Action command, UnityEvent componentEvent,
            Func<Action, Action> wrapFunc)
        {
            this._component = component;
            this._command = ()=> command?.Invoke();
            this._componentEvent = componentEvent;
            this._wrapFunc = wrapFunc;
        }

        private void InitEvent()
        {
            _defaultWrapper = BindTool.GetDefaultWrapper(Container, _component);
            _componentEvent = _componentEvent ?? (_component as IComponentEvent)?.GetComponentEvent() ??
                (_defaultWrapper as IComponentEvent)?.GetComponentEvent();
            Debug.Assert(_componentEvent != null, "componentEvent can not be null");
            // 清除一下上次的绑定
            _componentEvent.RemoveListener(Listener);
            _componentEvent.AddListener(Listener);
        }
        
        private void Listener()
        {
            if (_wrapFunc != null)
                _wrapFunc(_command)();
            else
                _command();
        }

        protected override void OnReset()
        {
            _componentEvent.RemoveListener(Listener);
        }

        protected override void OnClear()
        {
            _component = default;
            _command = default;
            _componentEvent = default;
            _defaultWrapper = default;
            _wrapFunc = default;
        }
    }

    public class BindCommandWithPara<TComponent, TData> : BaseBind
    {
        private TComponent _component;
        private Action<TData> _command;
        private Func<Action<TData>, Action<TData>> _wrapFunc;
        private UnityEvent<TData> _componentEvent;
        private object _defaultWrapper;

        public void Reset(TComponent component, Action<TData> command, UnityEvent<TData> componentEvent,
            Func<Action<TData>, Action<TData>> wrapFunc)
        {
            SetValue(component, command, componentEvent, wrapFunc);
            InitEvent();
        }
        
        private void SetValue(TComponent component, Action<TData> command, UnityEvent<TData> componentEvent,
            Func<Action<TData>, Action<TData>> wrapFunc)
        {
            this._component = component;
            this._command = data=> command?.Invoke(data);
            this._componentEvent = componentEvent;
            this._wrapFunc = wrapFunc;
        }

        private void InitEvent()
        {
            _defaultWrapper = BindTool.GetDefaultWrapper(Container, _component);
            if (_componentEvent == null)
            {
                IComponentEvent<TData> changeCb = _defaultWrapper as IComponentEvent<TData>;
                if (_component is IComponentEvent<TData> cb)
                {
                    changeCb = cb;
                }
                _componentEvent = changeCb?.GetComponentEvent();
            }
            Debug.Assert(_componentEvent != null,
                $" can not found wrapper , check if the folder(Runtime/UI/Wrap) has {typeof(TComponent).Name} wrapper or {typeof(TComponent).Name} implements IComponentEvent<{typeof(TData).Name}> interface");
            // 清除一下上次的绑定
            _componentEvent.RemoveListener(Listener);
            _componentEvent.AddListener(Listener);
        }

        private void Listener(TData data)
        {
            if (_wrapFunc != null)
                _wrapFunc(_command)(data);
            else
                _command(data);
        }

        protected override void OnReset()
        {
            _componentEvent.RemoveListener(Listener);
        }

        protected override void OnClear()
        {
            _component = default;
            _command = null;
            _wrapFunc = null;
            _componentEvent = null;
            _defaultWrapper = null;
        }
    }
}