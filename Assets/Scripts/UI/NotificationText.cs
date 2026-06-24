using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class NotificationText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    private Sequence seq;

    public void Setup(string message, Color textColor, float moveY, float duration, Action onCompleteCallback)
    {

        if(seq != null && seq.IsActive())
        {
            seq.Kill();
        }
        messageText.text = message;
        messageText.color = textColor;
        canvasGroup.alpha = 1f;

        seq = DOTween.Sequence();

        seq.Join(rectTransform.DOAnchorPos(new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + moveY), duration)
            .SetEase(Ease.OutCubic))
            .SetUpdate(true);
        seq.Join(canvasGroup.DOFade(0f, duration))
            .SetUpdate(true);

        seq.SetUpdate(true);

        seq.OnComplete(() => {
            onCompleteCallback?.Invoke();
        });
    }
}