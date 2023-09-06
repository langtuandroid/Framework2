using System.Collections.Generic;
using Framework;
using GraphProcessor;
using UnityEngine;
using UnityEngine.UIElements;

public class NP_BehaveTreesView : PinnedElementView
{
    private ScrollView scrollView;
    private NP_DebugGraphView view;

    protected override void Initialize(BaseGraphView graphView)
    {
        scrollView = new ScrollView(ScrollViewMode.Vertical);
        content.Add(scrollView);
    }

    public void Refresh(GameObject gameObject, NP_DebugGraphView view)
    {
        if (gameObject == null) return;
        if (gameObject.GetComponent<GoConnectedUnitId>() == null)
        {
            return;
        }

        this.view = view;
        long unitId = gameObject.GetComponent<GoConnectedUnitId>().UnitId;
        Unit unit = Root.Instance.Scene.GetComponent<CurrentScenesComponent>().Scene.GetComponent<UnitComponent>()
            .Get(unitId);
        Dictionary<long, NP_RuntimeTree> trees = unit.GetComponent<NP_RuntimeTreeManager>().runtimeId2Tree;
        scrollView.Clear();
        foreach (NP_RuntimeTree runtimeTree in trees.Values)
        {
            var tree = runtimeTree;
            var b = new Button(() => OnTreeClick(tree));
            b.text = tree.DebugName;
            scrollView.Add(b);
        }
    }

    private void OnTreeClick(NP_RuntimeTree tree)
    {
        this.view.Refresh(tree);
    }
}