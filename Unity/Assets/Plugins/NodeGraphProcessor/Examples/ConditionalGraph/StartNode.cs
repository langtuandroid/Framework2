using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphProcessor;

namespace NodeGraphProcessor.Examples
{
    [Serializable]
    [NodeMenuItem("Conditional/Start")]
    public class StartNode : BaseNode, IConditionalNode
    {
        [Output(name = "Executes")] public ConditionalLink executes;

        public override string name => "Start";

        public IEnumerable<ConditionalNode> GetExecutedNodes()
        {
            // Return all the nodes connected to the executes port
            return GetOutputNodes().OfType<ConditionalNode>();
        }

        public override FieldInfo[] GetNodeFields()
        {
            return base.GetNodeFields();
        }
    }
}