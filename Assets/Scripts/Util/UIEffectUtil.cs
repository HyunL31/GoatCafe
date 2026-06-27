using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class UIEffectUtil
{
    // 스케일을 0에서 1로 키우기
    public static void SetScaleOne(Transform target, float duration)
    {
        target.DOKill();
        target.localScale = Vector3.zero;
        target.DOScale(Vector3.one, duration).SetEase(Ease.OutBack).SetUpdate(true);
    }

    // 스케일을 1에서 0으로 줄이기
    public static void SetScaleZero(Transform target, float duration)
    {
        target.DOKill();
        target.DOScale(Vector3.zero, duration).SetEase(Ease.InBack).OnComplete(() =>
        {
            target.gameObject.SetActive(false);
        }).SetUpdate(true);
    }

    // 알파값을 0에서 1로 만들기
    public static void FadeIn(CanvasGroup cg, float duration)
    {
        cg.alpha = 0f;
        cg.DOFade(1f, duration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    // 알파값을 1에서 0로 만들기
    public static void FadeOut(CanvasGroup cg, float duration, Action onCompleteCallback)
    {
        cg.DOFade(0f, duration).SetEase(Ease.InOutSine).OnComplete(() => {
            onCompleteCallback?.Invoke();
        }).SetUpdate(true);
    }

    // 화면밖에서 UI가 솟아오르는 연출
    public static void SetUISlideUp(RectTransform rect, float duration)
    {
        rect.DOKill();
        float StartY = -Screen.height;
        Vector2 endPos = rect.anchoredPosition;

        rect.anchoredPosition = new Vector2(endPos.x, StartY);
        rect.gameObject.SetActive(true);

        rect.DOAnchorPos(endPos, duration).SetEase(Ease.OutBack).SetUpdate(true);
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
        }).SetUpdate(true);
    }

    public static void AnimateAndDestroy(RectTransform target, float duration = 0.3f)  // SaveSlotUI 삭제 전용 (삭제 애니메이션, Destroy, Layout 강제 재계산
    {
        if (target == null) return;

        RectTransform parentRect = target.parent as RectTransform;

        target.DOScale(Vector3.zero, duration)
            .SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                GameObject.Destroy(target.gameObject);

                if (parentRect != null)
                {
                    LayoutRebuilder.MarkLayoutForRebuild(parentRect);
                }
            }).SetUpdate(true);
    }
}