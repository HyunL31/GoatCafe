using UnityEngine;

public static class MonoBehaviourExtension
{
    public static void ActiveTrue(this MonoBehaviour monoBehaviour)
    {
        monoBehaviour.gameObject.SetActive(true);
    }

    public static void ActiveFalse(this MonoBehaviour monoBehaviour)
    {
        monoBehaviour.gameObject.SetActive(false);
    }

    public static T InstantiateAndGetComponent<T>(this MonoBehaviour monoBehaviour, GameObject prefab, Transform parentsTransform) where T : MonoBehaviour
    {
        if (prefab == null)
        {
            monoBehaviour.LogError("InstantiateAndGetComponent 실행 도중 오류가 발생하였습니다!! prefab이 null입니다!!");
            return null;
        }

        if (parentsTransform == null)
        {
            monoBehaviour.LogError("InstantiateAndGetComponent 실행 도중 오류가 발생하였습니다!! parentsTransform이 null입니다!!");
            return null;
        }

        GameObject gameObject = Object.Instantiate(prefab, parentsTransform);
        T component = gameObject.GetComponent<T>();

        return component;
    }
}