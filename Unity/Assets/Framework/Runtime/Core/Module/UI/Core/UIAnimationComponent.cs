using UnityEngine;

namespace Framework
{
    public class UIAnimationComponent : Entity, IAwakeSystem<GameObject>
    {
        private IUIAnimation uiAnimation;
        public void Awake(GameObject go)
        {
            uiAnimation = go.GetComponent<IUIAnimation>();
        }

        public IAsyncResult PlayShowAnimation()
        {
            if (uiAnimation == null) return AsyncResult.Void();
            return uiAnimation.OnShowAnim();
        }

        public IAsyncResult PlayHideAnimation()
        {
            if (uiAnimation == null) return AsyncResult.Void();
            return uiAnimation.OnHideAnim();
        }
    }
}