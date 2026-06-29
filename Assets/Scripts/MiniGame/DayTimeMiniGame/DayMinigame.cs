using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DayMinigame : BaseMonoManager<DayMinigame>
{
    [SerializeField] private GameObject Popup_GoatDayMiniGame;
    [SerializeField] private RectTransform Transform_BarArea;
    [SerializeField] private RectTransform Transform_Mover;
    [SerializeField] private RectTransform Transform_TargetArea;
    [SerializeField] private Button Button_DayMiniGame;
    [SerializeField] private Button Button_Close;
    [SerializeField] private Text Text_Result;
    [SerializeField] private float _speed = 500f;
    [SerializeField] private float _targetHeight = 120f;
    [SerializeField] private float _closeDelay = 0.4f;

    public bool IsPlaying { get; private set; }

    public Action OnStartDayMiniGame;
    public Action OnSuccessDayMiniGame;
    public Action OnFailDayMiniGame;
    public Action OnEndDayMiniGame;

    private int _moveDirection = 1;
    private float _minY;
    private float _maxY;
    private Coroutine _closeCoroutine;

    private CustomerSensor _currentSensor;

    private void Start()
    {
        if (Button_DayMiniGame != null)
            Button_DayMiniGame.onClick.AddListener(OnClickDayMiniGame);

        if (Button_Close != null)
            Button_Close.onClick.AddListener(OnClickClose);
    }

    private void OnEnable()
    {
        CustomerSensor.OnStealableInteract += OpenDayMiniGame;
    }

    private void OnDisable()
    {
        CustomerSensor.OnStealableInteract -= OpenDayMiniGame;
    }

    private void OnDestroy()
    {
        if (Button_DayMiniGame != null)
            Button_DayMiniGame.onClick.RemoveListener(OnClickDayMiniGame);

        if (Button_Close != null)
            Button_Close.onClick.RemoveListener(OnClickClose);
    }

    private void Update()
    {
        if (IsPlaying == false)
            return;

        MoveDayMiniGame();
    }

    public void OpenDayMiniGame(CustomerSensor customerSensor)
    {
        CursorManager.Instance.UnlockCursor();
        _currentSensor = customerSensor;
        GameManager.Instance.PauseGame();

        if (_closeCoroutine != null)
        {
            StopCoroutine(_closeCoroutine);
            _closeCoroutine = null;
        }

        if (Popup_GoatDayMiniGame != null)
            Popup_GoatDayMiniGame.SetActive(true);

        if (Text_Result != null)
            Text_Result.text = string.Empty;

        SetMoveRange();
        SetRandomTargetArea();
        ResetMover();

        IsPlaying = true;
        _moveDirection = 1;

        OnStartDayMiniGame?.Invoke();
    }

    public void CloseDayMiniGame()
    {
        IsPlaying = false;
        _currentSensor = null;

        if (_closeCoroutine != null)
        {
            StopCoroutine(_closeCoroutine);
            _closeCoroutine = null;
        }

        if (Popup_GoatDayMiniGame != null)
            Popup_GoatDayMiniGame.SetActive(false);
    }

    private void OnClickDayMiniGame()
    {
        if (IsPlaying == false)
            return;

        CheckDayMiniGame();
    }

    private void OnClickClose()
    {
        CloseDayMiniGame();
    }

    private void MoveDayMiniGame()
    {
        if (Transform_Mover == null)
            return;

        Vector2 position = Transform_Mover.anchoredPosition;
        position.y += _moveDirection * _speed * Time.unscaledDeltaTime;

        if (position.y >= _maxY)
        {
            position.y = _maxY;
            _moveDirection = -1;
        }
        else if (position.y <= _minY)
        {
            position.y = _minY;
            _moveDirection = 1;
        }

        Transform_Mover.anchoredPosition = position;
    }

    private void CheckDayMiniGame()
    {
        IsPlaying = false;

        bool isSuccess = IsSuccessDayMiniGame();

        if (Text_Result != null)
            Text_Result.text = isSuccess ? "성공" : "실패";

        if (isSuccess)
        {
            SaveManager.Instance.CurrentPlayerModel.StolenItemCount++;

            if (_currentSensor != null)
            {
                _currentSensor.SetStolen(true);
            }

            OnSuccessDayMiniGame?.Invoke();
        }
        else
        {
            OnFailDayMiniGame?.Invoke();
        }

        OnEndDayMiniGame?.Invoke();

        _closeCoroutine = StartCoroutine(CloseAfterDelay());
    }

    private bool IsSuccessDayMiniGame()
    {
        if (Transform_Mover == null || Transform_TargetArea == null)
            return false;

        float moverY = Transform_Mover.anchoredPosition.y;
        float targetY = Transform_TargetArea.anchoredPosition.y;
        float halfTarget = Transform_TargetArea.rect.height * 0.5f;
        float targetMinY = targetY - halfTarget;
        float targetMaxY = targetY + halfTarget;

        return moverY >= targetMinY && moverY <= targetMaxY;
    }

    private void SetMoveRange()
    {
        if (Transform_BarArea == null || Transform_Mover == null)
            return;

        float barHeight = Transform_BarArea.rect.height;
        float moverHeight = Transform_Mover.rect.height;

        _minY = -(barHeight * 0.5f) + (moverHeight * 0.5f);
        _maxY = (barHeight * 0.5f) - (moverHeight * 0.5f);
    }

    private void SetRandomTargetArea()
    {
        if (Transform_TargetArea == null)
            return;

        Transform_TargetArea.sizeDelta = new Vector2(Transform_TargetArea.sizeDelta.x, _targetHeight);

        float halfTarget = _targetHeight * 0.5f;
        float minTargetY = _minY + halfTarget;
        float maxTargetY = _maxY - halfTarget;

        if (minTargetY > maxTargetY)
        {
            Transform_TargetArea.anchoredPosition = new Vector2(Transform_TargetArea.anchoredPosition.x, 0f);
            return;
        }

        float randomY = UnityEngine.Random.Range(minTargetY, maxTargetY);
        Transform_TargetArea.anchoredPosition = new Vector2(Transform_TargetArea.anchoredPosition.x, randomY);
    }

    private void ResetMover()
    {
        if (Transform_Mover == null)
            return;

        Transform_Mover.anchoredPosition = new Vector2(Transform_Mover.anchoredPosition.x, _minY);
    }

    private IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSecondsRealtime(_closeDelay);
        GameManager.Instance.ResumeGame();
        CursorManager.Instance.LockCursor();
        CloseDayMiniGame();
    }
}