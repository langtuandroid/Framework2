using UnityEngine.UIElements;

namespace GraphProcessor
{
    public class ExposedParameterPropertyView : VisualElement
    {
        protected BaseGraphView baseGraphView;

        public ExposedParameterPropertyView(BaseGraphView graphView, ExposedParameter param)
        {
            baseGraphView = graphView;
            parameter = param;

            var settingField = graphView.exposedParameterFactory.GetParameterSettingsField(param,
                newValue => { param.settings = newValue as ExposedParameter.Settings; });

            var valueField = graphView.exposedParameterFactory.GetParameterValueField(param, newValue =>
            {
                param.value = newValue;
                //serializedObject.ApplyModifiedProperties();
                baseGraphView.graph.NotifyExposedParameterValueChanged(param);
            });

            Add(valueField);

            Add(settingField);
        }

        public ExposedParameter parameter { get; }

        public Toggle hideInInspector { get; private set; }
    }
}