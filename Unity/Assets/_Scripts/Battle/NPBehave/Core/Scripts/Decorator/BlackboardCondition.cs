using Framework;

namespace NPBehave
{
    public class BlackboardCondition : ObservingDecorator
    {
        public string Key { get; }

        public ANP_BBValue Value { get; }

        public Operator Operator { get; }

        public BlackboardCondition(string Key, Operator Operator, ANP_BBValue Value, Stops stopsOnChange,
            Node decoratee) : base("BlackboardCondition", stopsOnChange, decoratee)
        {
            this.Operator = Operator;
            this.Key = Key;
            this.Value = Value;
            this.stopsOnChange = stopsOnChange;
        }

        public BlackboardCondition(string Key, Operator Operator, Stops stopsOnChange, Node decoratee) : base(
            "BlackboardCondition", stopsOnChange, decoratee)
        {
            this.Operator = Operator;
            this.Key = Key;
            this.stopsOnChange = stopsOnChange;
        }


        override protected void StartObserving()
        {
            this.RootNode.Blackboard.AddObserver(Key, onValueChanged);
        }

        override protected void StopObserving()
        {
            this.RootNode.Blackboard.RemoveObserver(Key, onValueChanged);
        }

        private void onValueChanged(Blackboard.Type type, object newValue)
        {
            Evaluate();
        }

        override protected bool IsConditionMet()
        {
            if (Operator == Operator.ALWAYS_TRUE)
            {
                return true;
            }

            if (!this.RootNode.Blackboard.IsSet(Key))
            {
                return Operator == Operator.IS_NOT_SET;
            }

            ANP_BBValue o = this.RootNode.Blackboard.Get(Key);

            switch (this.Operator)
            {
                case Operator.IS_SET: return true;
                default:
                    return NP_BBValueHelper.Compare(Value, o, Operator);
            }
        }

        override public string ToString()
        {
            return "(" + this.Operator + ") " + this.Key + " ? " + this.Value;
        }
    }
}