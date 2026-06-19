using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    private int score;
    public bool isGame;
    public bool isUseItem;

    [SerializeField] GameObject NightMiniGamePanel;
    [SerializeField] GameObject PaperTrashPrefab;
    [SerializeField] GameObject PlasticTrashPrefab;
    [SerializeField] RectTransform SpawnZone;
    [SerializeField] private Transform trashParent;
    [SerializeField] private GameObject Button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        /*
        isGame = false;

        if (NightMiniGamePanel != null)
        {
            NightMiniGamePanel.SetActive(false);
        }
        */
        GameStart();
    }

    // Update is called once per frame
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

        for (int i = 0; i < PaperTrashCount; i++)
        {
            GameObject Papertrash = Instantiate(PaperTrashPrefab, trashParent);

            
            RectTransform trashRect = Papertrash.GetComponent<RectTransform>();


            float x = Random.Range(-SpawnZone.rect.width / 2f, SpawnZone.rect.width / 2f);
            float y = Random.Range(-SpawnZone.rect.height / 2f, SpawnZone.rect.height / 2f);

            trashRect.anchoredPosition = SpawnZone.anchoredPosition + new Vector2(x, y);

        }

        for (int j = 0; j < PlasticTrashCount; j++)
        {
            GameObject PlasticTrash = Instantiate(PlasticTrashPrefab, trashParent);


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
}
