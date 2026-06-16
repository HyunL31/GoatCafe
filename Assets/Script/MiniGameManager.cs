using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    private int score;
    public bool isGame;
    [SerializeField] GameObject NightMiniGamePanel;
    [SerializeField] GameObject PaperTrashPrefab;
    [SerializeField] GameObject PlasticTrashPrefab;
    [SerializeField] RectTransform SpawnZone;
    [SerializeField] private Transform trashParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        isGame = false;

        if (NightMiniGamePanel != null)
        {
            NightMiniGamePanel.SetActive(false);
        }
        */

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
        /*
         * 정리 버튼에서 호출
         * 모든 TrashItem을 검사
         * 올바른 구역이면 +점수
         * 틀린 구역이면 -점수
         * StartZone에 남아있으면 -점수
         */

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
