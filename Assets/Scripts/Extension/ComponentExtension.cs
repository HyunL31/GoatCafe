using UnityEngine;

public static class ComponentExtension
{
    public static void Log(this Component component, string text)
    {
        Debug.Log($"{component.name} : " + text);
    }

    public static void LogWarning(this Component component, string text)
    {
        Debug.LogWarning($"{component.name} : " + text);
    }

    public static void LogError(this Component component, string text)
    {
        Debug.LogError($"{component.name} : " + text);
    }
}