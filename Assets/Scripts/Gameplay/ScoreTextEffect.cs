using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextEffect : MonoBehaviour
{
    public Action OnComplete;

    public Text scoreText;
    public RectTransform scoreParent;

    private Transform target;
    private Camera targetCamera;
    private float offsetY = 0f;
    
    public void Init(int score, Transform target, Camera camera, int index)
    {
        scoreText.text = $"+{score}";

        scoreParent.DOKill();
        scoreText.DOKill();
        scoreText.color = Color.white;

        this.target = target;
        this.targetCamera = camera;

        var pos = camera.WorldToScreenPoint(target.transform.position);
        transform.position = pos;
        transform.localScale = Vector3.zero;

        var localPos = scoreParent.localPosition;
        var seq = DOTween.Sequence();
        seq.Append(scoreParent.DOScale(Vector3.one * 1.3f, 0.25f));
        seq.Append(scoreParent.DOScale(Vector3.one, 0.25f));
        seq.Play();

        scoreText.DOFade(0f, 0.25f).OnComplete(() => {
            OnComplete?.Invoke();
            
            seq.Kill();
            Destroy(gameObject);
        }).SetDelay(index * 0.01f);;

        offsetY = 0f;
    }

    private void Update() 
    {
        if (target == null) return;
        if (targetCamera == null) return;

        var pos = targetCamera.WorldToScreenPoint(target.transform.position);
        pos.y += offsetY;
        transform.position = pos;

        offsetY += Time.deltaTime * 50f;
    }
}
