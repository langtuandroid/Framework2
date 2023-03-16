using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphProcessor
{
    public class MiniMapView : MiniMap
    {
        public MiniMapView(BaseGraphView baseGraphView)
        {
            graphView = baseGraphView;
            SetPosition(new Rect(2, 20, 100, 100));
        }

        public override void UpdatePresenterPosition()
        {
            var pos = GetPosition();
            SetPosition(new Rect(2, Math.Max(20, pos.y), pos.width, pos.height));
        }
    }
}