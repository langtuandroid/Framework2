using System;
using System.Collections.Generic;
using GraphProcessor;
using UnityEngine;

namespace NodeGraphProcessor.Examples
{
    [Serializable]
    [NodeMenuItem("Debug/Console Log")]
    public class ConsoleNode : LinearConditionalNode
    {
        [Input("Log")] [SerializeField] [Tooltip("If Object is null, this will be the log.")]
        public string logText = "Log";

        [Setting("Log Type")] public LogType logType = LogType.Log;

        [Input("Object")] public object obj;

        [ShowInInspector(true)] public Dictionary<string, string> TestDic = new();

        public override string name => "Console Log";

        protected override void Process()
        {
            TryGetInputValue(nameof(obj), ref obj);

            switch (logType)
            {
                case LogType.Error:
                case LogType.Exception:
                    Debug.LogError(obj != null ? obj.ToString() : logText);
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(obj != null ? obj.ToString() : logText);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(obj != null ? obj.ToString() : logText);
                    break;
                case LogType.Log:
                    Debug.Log(obj != null ? obj.ToString() : logText);
                    break;
            }
        }
    }
}