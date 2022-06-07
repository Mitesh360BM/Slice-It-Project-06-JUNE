using System;
using DG.Tweening;
using UnityEngine;

namespace Tiny
{
    public class ZoomAnimation : SceneAnimation
    {
        public CanvasGroup Background;
        public CanvasGroup Panel;

        private float _animationTime = 0.25f;

        private float _originBGOpacity = 0.0f;
        private float _originPanelOpacity = 0.0f;

        private void Awake()
        {
            _originBGOpacity = Background.alpha;
            _originPanelOpacity = Panel.alpha;
        }

        public override void Show(Action callback)
        {
            Background.alpha = 0.0f;
            Background.DOFade(_originBGOpacity, _animationTime).SetUpdate(true);

            Panel.transform.localScale = Vector3.zero;
            Panel.transform.DOScale(Vector3.one, _animationTime * 0.8f).SetEase(Ease.OutBack).SetUpdate(true);
            Panel.alpha = 0.0f;
            Panel.DOFade(_originPanelOpacity, _animationTime).OnComplete(() =>
            {
                if (callback != null) callback();
            }).SetUpdate(true);
        }
        public override void Hide(Action callback)
        {
            Background.alpha = _originBGOpacity;
            Background.DOFade(0, _animationTime).SetUpdate(true);

            Panel.transform.localScale = Vector3.one;
            Panel.transform.DOScale(Vector3.zero, _animationTime * 0.8f).SetEase(Ease.InBack).SetUpdate(true);
            Panel.alpha = _originPanelOpacity;
            Panel.DOFade(0, _animationTime).OnComplete(() =>
            {
                if (callback != null) callback();
            }).SetUpdate(true);
        }
    }
}
