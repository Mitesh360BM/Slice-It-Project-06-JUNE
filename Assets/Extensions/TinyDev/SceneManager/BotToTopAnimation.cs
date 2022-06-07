using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Tiny
{
    public class BotToTopAnimation : SceneAnimation
    {
        public CanvasGroup Background;
        public CanvasGroup Panel;

        private float _animationTime = 0.5f;
        private float _originBGOpacity = 0.0f;
        private Vector3 _originPanelPosition;

        private void Awake()
        {
            _originPanelPosition = Panel.transform.localPosition;
            _originBGOpacity = Background.alpha;
        }

        public override void Show(Action callback)
        {
            Background.alpha = 0.0f;
            Background.DOFade(_originBGOpacity, _animationTime).SetUpdate(true);

            var startPos = _originPanelPosition;
            startPos.y -= 1000;
            Panel.transform.localPosition = startPos;

            Panel.transform.DOLocalMoveY(_originPanelPosition.y, _animationTime).OnComplete(() =>
            {
                if (callback != null) callback();
            }).SetEase(Ease.OutBack).SetUpdate(true);

        }

        public override void Hide(Action callback)
        {
            Background.alpha = _originBGOpacity;
            Background.DOFade(0, _animationTime).SetUpdate(true);

            Panel.transform.localPosition = _originPanelPosition;
            Panel.transform.DOLocalMoveY(_originPanelPosition.y - 1000, _animationTime).OnComplete(() =>
            {
                if (callback != null) callback();
            }).SetEase(Ease.InBack).SetUpdate(true);
        }
    }

}
