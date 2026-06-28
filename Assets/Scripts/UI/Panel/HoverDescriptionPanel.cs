using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class HoverDescriptionPanel : MonoBehaviour
{
    [SerializeField] private RectTransform Root;
    [SerializeField] private Canvas Canvas;
    [SerializeField] private VideoPlayer VideoPlayer;
    [SerializeField] private Vector2 Offset = new(20f, -20f);

    private RectTransform _canvasRect;
    private bool _isShowing;

    private void Awake()
    {
        Canvas  = GetComponentInParent<Canvas>();
        _canvasRect = Canvas.transform as RectTransform;
    }

    public void Open(VideoClip clip)
    {
        VideoPlayer.clip = clip;
        VideoPlayer.Play();
        Root.gameObject.SetActive(true);
        _isShowing = true;
    }

    public void Close()
    {
        VideoPlayer.Stop();
        Root.gameObject.SetActive(false);
        _isShowing = false;
    }

    private void Update()
    {
        if (!_isShowing) return;

        Camera cam = Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Canvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect, Input.mousePosition, cam, out Vector2 local);

        Root.localPosition = local + Offset;
    }
}
