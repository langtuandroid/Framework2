using System;

namespace Framework
{
    public class UIAttribute : BaseAttribute
    {
        public string Path { get; }

        public UIAttribute(string path)
        {
            Path = path;
        }
    }
}