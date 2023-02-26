namespace Framework
{
    public abstract class ETObject
    {
        public override string ToString()
        {
            return JsonHelper.ToJson(this);
        }
    }
}