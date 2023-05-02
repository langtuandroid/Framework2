#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class B2D_MenuTree
{
    [ListDrawerSettings(Expanded = true, IsReadOnly = true)] [ShowInInspector]
    private List<NameId> nameAndId = new List<NameId>();

    private Action<string> onEdit;

    private ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter;

    public B2D_MenuTree(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter,
        Action<string> onEdit)
    {
        this.colliderNameAndIdInflectSupporter = colliderNameAndIdInflectSupporter;
        this.onEdit = onEdit;
        RefreshData();
    }

    public void RefreshData()
    {
        nameAndId.Clear();
        foreach (var item in colliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic)
        {
            nameAndId.Add(new NameId(item.Key, item.Value, onEdit));
        }

        nameAndId.Sort((s1, s2) => s1.Id.CompareTo(s2.Id));
    }

    public bool ContainsName(string name)
    {
        foreach (var nameId in nameAndId)
        {
            if (nameId.Name == name) return true;
        }

        return false;
    }

    public void RemoveId(long id)
    {
        for (int i = 0; i < nameAndId.Count; i++)
        {
            if (nameAndId[i].Id == id)
            {
                nameAndId.RemoveAt(i);
                break;
            }
        }
    }

    private void DeleteItem(NameId name, ColliderNameAndIdInflectSupporter supporter)
    {
        supporter.colliderNameAndIdInflectDic.Remove(name.Name);
        nameAndId.Remove(name);
    }

    [HideReferenceObjectPicker]
    private class NameId
    {
        [HorizontalGroup(PaddingLeft = 30, LabelWidth = 40, PaddingRight = 30)] [ReadOnly]
        public string Name;

        [HorizontalGroup(PaddingLeft = 30, LabelWidth = 20, PaddingRight = 30)] [ReadOnly]
        public long Id;

        [HideInInspector] private Action<string> Action;


        public NameId(string name, long id, Action<string> action)
        {
            Name = name;
            Id = id;
            Action = action;
        }

        [Button(name: "编辑")]
        [HorizontalGroup(PaddingLeft = 30, PaddingRight = 10)]
        private void OnClick()
        {
            Action(Name);
        }
    }
}
#endif