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

    public static bool InstantiateAndGetComponent<T>(this MonoBehaviour monoBehaviour, GameObject prefab, Transform parentsTransform, out T component) where T : MonoBehaviour
    {
        if (prefab == null)
        {
            monoBehaviour.LogError("InstantiateAndGetComponent 실행 도중 오류가 발생하였습니다!! prefab이 null입니다!!");
            component = null;
            return false;
        }

        if (parentsTransform == null)
        {
            monoBehaviour.LogError("InstantiateAndGetComponent 실행 도중 오류가 발생하였습니다!! parentsTransform이 null입니다!!");
            component = null;
            return false;
        }

        GameObject gameObject = Object.Instantiate(prefab, parentsTransform);
        component = gameObject.GetComponent<T>();

        if(component == null)
        {
            monoBehaviour.LogError($"InstantiateAndGetComponent 실행 도중 오류가 발생하였습니다!! {typeof(T)} 컴포넌트가 없습니다!!");
            Object.Destroy(gameObject);
            component = null;
            return false;
        }

        return true;
    }
}