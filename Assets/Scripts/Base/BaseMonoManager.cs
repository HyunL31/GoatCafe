using UnityEngine;

public class BaseMonoManager<T> : MonoBehaviour where T : BaseMonoManager<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        InitInstance();
    }

    private void InitInstance()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            this.LogWarning("이 클래스의 매니저가 이미 생성이 되어 있어, 게임 오브젝트를 파괴합니다!!");
            Destroy(this.gameObject);
        }
    }
}