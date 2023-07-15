using System;
using System.Collections.Generic;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class EditorVisibleUnit : MonoBehaviour
{
#if UNITY_EDITOR
    public Unit unit { get; private set; }
    [ShowInInspector]
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout, IsReadOnly = true)]
    private Dictionary<Type,Entity> components => unit?.Components;

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }
    
    private void Update()
    {
        if(!transform.hasChanged) return;
        if(unit == null) return;
        unit.Position = transform.position;
        unit.EulerAngle = transform.eulerAngles;
        unit.Scale = transform.localScale;
    }
#endif
}