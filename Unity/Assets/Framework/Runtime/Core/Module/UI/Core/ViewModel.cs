using System;

namespace Framework
{
    public abstract class ViewModel : Entity
    {
        public virtual void OnViewHide()
        {
        }
        
        public virtual void OnViewDestroy()
        {
        }
    }
}