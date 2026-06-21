using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public enum DanceType
{
    None,
    Hiphop,
    Samba,
    Shuffle
}

public class PlayerEmote : MonoBehaviour
{
    [SerializeField] private GameObject Goat_Basic;
    [SerializeField] private GameObject Goat_Humanoid;
    [SerializeField] private Animator Animator_Humanoid;
    [SerializeField] private PlayerMoving PlayerMoving;

    private CancellationTokenSource _danceCancel;
    private Camera _main;

    private void Awake()
    {
        _main = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            StartDance(DanceType.Hiphop);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            StartDance(DanceType.Samba);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            StartDance(DanceType.Shuffle);
        }
    }

    private void StartDance(DanceType dance)
    {
        PlayDanceAnimation(dance).Forget();
    }

    private void SetDance(DanceType dance)
    {
        // 가게 매니저 관련 조건(구매했는지 안했는지) 추가 예정
        if (!Goat_Humanoid.activeSelf)
        {
            return;
        }

        if (dance == DanceType.Hiphop)
        {
            Animator_Humanoid.SetTrigger("Hiphop");   
        }
        else if (dance == DanceType.Samba)
        {
            Animator_Humanoid.SetTrigger("Samba");
        }
        else if (dance == DanceType.Shuffle)
        {
            Animator_Humanoid.SetTrigger("Shuffle");
        }
    }

    private async UniTask PlayDanceAnimation(DanceType dance)
    {
        if (!PlayerMoving.IsAlive())
        {
            return;
        }

        Goat_Basic.SetActive(false);
        Goat_Humanoid.SetActive(true);

        Goat_Humanoid.transform.position = transform.position;
        Goat_Humanoid.transform.LookAt(_main.transform);

        Vector3 currentRotation = Goat_Humanoid.transform.rotation.eulerAngles;
        Goat_Humanoid.transform.rotation = Quaternion.Euler(0f, currentRotation.y, currentRotation.z);

        _danceCancel?.Cancel();
        _danceCancel = new CancellationTokenSource();

        SetDance(dance);

        await UniTask.Yield(PlayerLoopTiming.Update);

        float delayTime = Animator_Humanoid.GetCurrentAnimatorStateInfo(0).length;

        await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: _danceCancel.Token);

        Goat_Basic.SetActive(true);
        Goat_Humanoid.SetActive(false);
    }
}