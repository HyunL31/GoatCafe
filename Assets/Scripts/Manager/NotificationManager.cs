using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationManager : BaseMonoManager<NotificationManager>
{
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private float stayDuration = 2.0f; // 텍스트 유지 시간
    [SerializeField] private float moveDistance = 100f; // Y방향으로 움직일 거리
    [SerializeField] private int maxCount = 5; // 미리 만들 개수

    private Queue<GameObject> notificationQueue = new Queue<GameObject>();

    private void Start()
    {
        InitializeQueue();
    }
    private void InitializeQueue()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject obj = Instantiate(notificationPrefab, canvasTransform);
            obj.SetActive(false); 
            notificationQueue.Enqueue(obj);
        }
    }

    public void ShowNotification(string message)
    {
        ShowNotification(message, Color.white);
    }
    public void ShowNotification(string message, Color color)
    {
        GameObject obj = notificationQueue.Dequeue();

        notificationQueue.Enqueue(obj);

        obj.SetActive(true);
        obj.transform.localPosition = Vector3.zero;

        NotificationText item = obj.GetComponent<NotificationText>();
        item.Setup(message, color, moveDistance, stayDuration, () => {
            obj.SetActive(false); 
        });
    }

}
