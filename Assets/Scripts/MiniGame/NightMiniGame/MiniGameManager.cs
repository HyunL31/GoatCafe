using UnityEngine;

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

    void Start()
    {
        score = 0;
        /*
        
        타 코드에서 아이템 사용 확인하는 로직 필요함

        스코어 두배 아이템 사용 -> isScoreDouble;
        난이도 감소 아이템 사용 -> isMiniGameEasier;

        변수 이름 마음대로 바꿔도 가능





        여기부터 Update()의 각주내용은 필드와 미니게임 연결 관련

        isGame = false;

        if (NightMiniGamePanel != null)
        {
            NightMiniGamePanel.SetActive(false);
        }
        */
        GameStart();
    }

    void Update()
    {
        /*
        if(Input.KeyDown(KeyCode.E) && )
        {
            NightMiniGamePanel.SetActive(true);

            GameStart();
        }
        */
    }

    private void AddScore(int amount)
    {
        score += amount;
    }

    public void GameStart()
    {

        isGame = true;

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
        Debug.Log("최종 점수: " + score);
        CloseMiniGame();
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
}
