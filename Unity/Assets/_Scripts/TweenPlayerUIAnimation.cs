using System;
using System.Collections.Generic;
using Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityExtensions.Tween;
using IAsyncResult = Framework.IAsyncResult;

[RequireComponent(typeof(TweenPlayer))]
public class TweenPlayerUIAnimation : MonoBehaviour, IUIAnimation
{
    private TweenPlayer showTweenPlayer;
    private TweenPlayer hideTweenPlayer;
    [SerializeField]
    [LabelText("出现动画的名字")]
    private string showAnimName;
    [SerializeField]
    [LabelText("隐藏动画的名字")]
    private string hideAnimName;

    private void Awake()
    {
        foreach (var tweenPlayer in GetComponents<TweenPlayer>())
        {
            if (tweenPlayer.AnimName == showAnimName)
            {
                showTweenPlayer = tweenPlayer;
            }else if (tweenPlayer.AnimName == hideAnimName)
            {
                hideTweenPlayer = tweenPlayer;
            }
        }
    }

    public IAsyncResult OnShowAnim()
    {
        AsyncResult result = AsyncResult.Create();
        showTweenPlayer.Play();
        void OnArrive()
        {
            result.SetResult();
            showTweenPlayer.onForwardArrived -= OnArrive;
        }
        showTweenPlayer.onForwardArrived += OnArrive;
        return result;
    }

    public IAsyncResult OnHideAnim()
    {
         AsyncResult result = AsyncResult.Create();
         hideTweenPlayer.Play();
         void OnArrive()
         {
             result.SetResult();
             hideTweenPlayer.onForwardArrived -= OnArrive;
         }
         hideTweenPlayer.onForwardArrived += OnArrive;
         return result;   
    }
}