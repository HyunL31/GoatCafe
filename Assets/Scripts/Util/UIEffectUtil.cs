using UnityEngine;
using DG.Tweening;
using System;

public static class UIEffectUtil
{
    // 스케일을 0에서 1로 키우기
    public static void SetScaleOne(Transform target, float duration)
    {
        target.localScale = Vector3.zero;
        target.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
    }

    // 스케일을 1에서 0으로 줄이기
    public static void SetScaleZero(Transform target, float duration)
    {
        target.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);
    }

    // 알파값을 0에서 1로 만들기
    public static void FadeIn(CanvasGroup cg, float duration)
    {
        cg.alpha = 0f;
        cg.DOFade(1f, duration).SetEase(Ease.InOutSine);
    }

    // 알파값을 1에서 0로 만들기
    public static void FadeOut(CanvasGroup cg, float duration, Action onCompleteCallback)
    {
        cg.DOFade(0f, duration).SetEase(Ease.InOutSine).OnComplete(() => {
            onCompleteCallback?.Invoke();
        });
    }

    // 화면밖에서 UI가 솟아오르는 연출
    public static void SetUISlideUp(RectTransform rect, float duration)
    {
        rect.DOKill();
        float StartY = -Screen.height;
        Vector2 endPos = rect.anchoredPosition;

        rect.anchoredPosition = new Vector2(endPos.x, StartY);
        rect.gameObject.SetActive(true);

        rect.DOAnchorPos(endPos, duration).SetEase(Ease.OutBack);
    }

    // UI가 아래로 이동해 화면 밖으로 이동하는 연출
    public static void SetUISlideDown(RectTransform rect, Vector2 originalPos, float duration)
    {
        rect.DOKill();
        Vector2 firstPos = rect.anchoredPosition;
        Vector2 endPos = new Vector2(firstPos.x, -Screen.height);


        rect.DOAnchorPos(endPos, duration).SetEase(Ease.OutBack).OnComplete(() => {
            rect.gameObject.SetActive(false);
            rect.anchoredPosition = originalPos;
        });

    }
}