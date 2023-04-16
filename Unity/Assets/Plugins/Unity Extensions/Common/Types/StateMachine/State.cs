using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityExtensions
{
    /// <summary>
    /// BaseState
    /// </summary>
    internal abstract class BaseState : IState
    {
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate(float deltaTime) { }
    }


    /// <summary>
    /// State. OnEnter & OnExit can be serialized.
    /// </summary>
    [Serializable]
    internal class State : BaseState
    {
        [SerializeField]
        UnityEvent _onEnter = default;

        [SerializeField]
        UnityEvent _onExit = default;

        internal event UnityAction onEnter
        {
            add
            {
                if (_onEnter == null) _onEnter = new UnityEvent();
                _onEnter.AddListener(value);
            }
            remove { _onEnter?.RemoveListener(value); }
        }

        internal event UnityAction onExit
        {
            add
            {
                if (_onExit == null) _onExit = new UnityEvent();
                _onExit.AddListener(value);
            }
            remove { _onExit?.RemoveListener(value); }
        }


        public override void OnEnter()
        {
            _onEnter?.Invoke();
        }


        public override void OnExit()
        {
            _onExit?.Invoke();
        }

    } // class State

} // namespace UnityExtensions
