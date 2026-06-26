using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : BaseMonoManager<MiniGameManager>
{
    private int score;
    public bool isGame;
    private bool isScoreDouble = false;  // 스코어 두배
    private bool isMiniGameEasier = false;  // 난이도 감소


    [SerializeField] GameObject NightMiniGamePanel;
    [SerializeField] GameObject[] PaperTrashPrefab;
    [SerializeField] GameObject[] PlasticTrashPrefab;
    [SerializeField] RectTransform SpawnZone;
    [SerializeField] private Transform trashParent;
    [SerializeField] private Sprite[] numberSprites;
    [SerializeField] private Image TimerImage;

    private float timer;
    private const float timeLimit = 5f;
    private bool isTimerRunning;

    private Trash _targetTrash;

    void Start()
    {
        score = 0;

        Trash.OnTrashEnter += TrashEnter;
        Trash.OnTrashExit += TrashExit;
    }

    void Update()
    {
        if(GameManager.Instance.CurrentDayPhase == DayPhase.Night && _targetTrash != null && Input.GetKeyDown(KeyCode.E))
        {
            NightMiniGamePanel.SetActive(true);

            GameStart();
        }

        if (isTimerRunning)
        {
            timer -= Time.deltaTime;
            UpdateTimerImage();

            if (timer <= 0f)
            {
                isTimerRunning = false;
                CheckResult();
            }
        }
    }

    private void TrashEnter(Trash trash)
    {
        _targetTrash = trash;
    }

    private void TrashExit(Trash trash)
    {
        if (_targetTrash == trash)
        {
            _targetTrash = null;
        }
    }

    private void AddScore(int amount)
    {
        score += amount;
    }

    public void GameStart()
    {
        CursorManager.Instance.UnlockCursor();
        GameManager.Instance.PauseGame();
        isGame = true;
        timer = timeLimit;
        isTimerRunning = true;
        UpdateTimerImage();
        score = 0;

        int PaperTrashCount = Random.Range(1, 6);
        int PlasticTrashCount = Random.Range(1, 6);

        if(isMiniGameEasier == true)
        {
            PaperTrashCount = Mathf.Max(1, PaperTrashCount / 2);
            PlasticTrashCount = Mathf.Max(1, PlasticTrashCount / 2);
        }

        for (int i = 0; i < PaperTrashCount; i++)
        {
            GameObject paperPrefab = PaperTrashPrefab[Random.Range(0, PaperTrashPrefab.Length)];
            GameObject paperTrash = Instantiate(paperPrefab, trashParent);

            RectTransform trashRect = paperTrash.GetComponent<RectTransform>();


            float x = Random.Range(-SpawnZone.rect.width / 2f, SpawnZone.rect.width / 2f);
            float y = Random.Range(-SpawnZone.rect.height / 2f, SpawnZone.rect.height / 2f);

            trashRect.anchoredPosition = SpawnZone.anchoredPosition + new Vector2(x, y);

        }

        for (int j = 0; j < PlasticTrashCount; j++)
        {
            GameObject PlasticPrefab = PlasticTrashPrefab[Random.Range(0, PlasticTrashPrefab.Length)];
            GameObject PlasticTrash = Instantiate(PlasticPrefab, trashParent);

            RectTransform trashRect = PlasticTrash.GetComponent<RectTransform>();


            float x = Random.Range(-SpawnZone.rect.width / 2f, SpawnZone.rect.width / 2f);
            float y = Random.Range(-SpawnZone.rect.height / 2f, SpawnZone.rect.height / 2f);

            trashRect.anchoredPosition = SpawnZone.anchoredPosition + new Vector2(x, y);
        }
    }

    public void CheckResult()
    {
        score = 0;

        for (int i = 0; i < trashParent.childCount; i++)
        {
            Transform child = trashParent.GetChild(i);
            TrashItem trashitem = child.GetComponent<TrashItem>();

            if (trashitem == null)
            {
                continue;
            }

            if (trashitem.TrashType == TrashType.Paper && trashitem.CurrentZone == ZoneType.Paper)
            {
                score += 10;
            }
            else if (trashitem.TrashType == TrashType.Plastic && trashitem.CurrentZone == ZoneType.Plastic)
            {
                score += 10;
            }
            else
            {
                score -= 5;
            }
        }

        if (isScoreDouble == true)
        {
            score = score * 2;
        }

        if (CursorManager.Instance != null)
        {
            CursorManager.Instance.LockCursor();
        }

        Debug.Log("최종 점수: " + score);
        CloseMiniGame();
        GameManager.Instance.ResumeGame();
    }

    public void CloseMiniGame()
    {
        if (NightMiniGamePanel != null)
        {
            NightMiniGamePanel.SetActive(false);
        }
    }

    public void SetMiniGameScoreDouble(bool isDouble)
    {
        isScoreDouble = isDouble;
    }

    public void SetMiniGameEasier(bool isEasier)
    {
        isMiniGameEasier = isEasier;
    }

    private void UpdateTimerImage()
    {
        if (TimerImage == null || numberSprites == null || numberSprites.Length < 5)
        {
            return;
        }

        int count = Mathf.Clamp(Mathf.CeilToInt(timer), 1, 5);

        TimerImage.sprite = numberSprites[count - 1];
    }
}
