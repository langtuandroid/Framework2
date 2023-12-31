﻿using System;

using Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using Action = System.Action;

[BoxGroup("用于包含Action的数据类"), GUIColor(0.961f, 0.902f, 0.788f, 1f)]
[HideReferenceObjectPicker]
public class NP_ClassForStoreAction
{
    /// <summary>
    /// 归属的UnitID
    /// </summary>
    [HideInInspector] public Unit BelongToUnit;

    /// <summary>
    /// 归属的运行时行为树实例
    /// </summary>
    [HideInInspector] public NP_RuntimeTree BelongtoRuntimeTree;

    [HideInInspector] private Action Action;

    [HideInInspector] private Func<bool> Func1;

    [HideInInspector] private Func<bool, NPBehave.Action.Result> Func2;

    /// <summary>
    /// 获取将要执行的委托函数，也可以在这里面做一些初始化操作
    /// </summary>
    /// <returns></returns>
    public virtual Action GetActionToBeDone()
    {
        return null;
    }

    public virtual Func<bool> GetFunc1ToBeDone()
    {
        return null;
    }

    public virtual Func<bool, NPBehave.Action.Result> GetFunc2ToBeDone()
    {
        return null;
    }

    public NPBehave.Action _CreateNPBehaveAction()
    {
        Action = GetActionToBeDone();
        if (this.Action != null)
        {
            return new NPBehave.Action(Action, GetType().Name);
        }

        Func1 = GetFunc1ToBeDone();
        if (this.Func1 != null)
        {
            return new NPBehave.Action(Func1, GetType().Name);
        }

        Func2 = GetFunc2ToBeDone();
        if (this.Func2 != null)
        {
            return new NPBehave.Action(Func2, GetType().Name);
        }

        Log.Msg($"{this.GetType()} _CreateNPBehaveAction失败，因为没有找到可以绑定的委托");
        return null;
    }
}