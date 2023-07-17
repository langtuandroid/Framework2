using System.Reflection;
using GraphProcessor;
using NPBehave;
using UnityEngine;

public class NP_DebugGraph : BaseGraph
{
    public void Init(GameObject gameObject)
    {
        if (gameObject == null) return;
        var test = gameObject.GetComponent("Test");
        var root = test.GetType().GetField("root", BindingFlags.Instance | BindingFlags.Public).GetValue(test) as Root;
        Debug.Log(root);
    }
}