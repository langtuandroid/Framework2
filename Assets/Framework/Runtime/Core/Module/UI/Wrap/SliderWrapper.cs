﻿using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework
{
    public class SliderWrapper : BaseWrapper<Slider>, IFieldChangeCb<float>, IComponentEvent<float>
    {
        Action<float> IFieldChangeCb<float>.GetFieldChangeCb()
        {
            return (value) => Component.value = value;
        }

        UnityEvent<float> IComponentEvent<float>.GetComponentEvent()
        {
            return Component.onValueChanged;
        }

        public SliderWrapper(Slider component, View view) : base(component, view)
        {
        }
    }
}