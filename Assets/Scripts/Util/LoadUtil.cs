using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public static class LoadUtil
{
    public static class Async
    {
        public async static UniTask<Sprite> LoadSpriteAsync(string address)
        {
            Sprite sprite = await LoadGenericAsync<Sprite>(address);
            return sprite;
        }

        public async static UniTask<GameObject> LoadPrefabAsync(string address)
        {
            GameObject prefab = await LoadGenericAsync<GameObject>(address);
            return prefab;
        }

        public async static UniTask<TMP_FontAsset> LoadFontAssetAsync(string address)
        {
            TMP_FontAsset fontAsset = await LoadGenericAsync<TMP_FontAsset>(address);
            return fontAsset;
        }

        public async static UniTask<VideoClip> LoadVideoClipAsync(string address)
        {
            VideoClip videoClip = await LoadGenericAsync<VideoClip>(address);
            return videoClip;
        }

        public async static UniTask<T> LoadGenericAsync<T>(string address) where T : Object
        {
            T asset = await ResourceManager.Instance.LoadAssetAsync<T>(address);

            if(asset == null)
            {
                LogError(address);
                return null;
            }

            return asset;
        }
    }

    public static class Sync
    {
        public static GameObject LoadPrefab(string address)
        {
            GameObject prefab = LoadGeneric<GameObject>(address);
            return prefab;
        }

        public static TextAsset LoadTextAsset(string address)
        {
            TextAsset textAsset = LoadGeneric<TextAsset>(address);
            return textAsset;
        }

        public static T LoadGeneric<T>(string address) where T : Object
        {
            T asset = ResourceManager.Instance.LoadAssetSync<T>(address);

            if(asset == null)
            {
                LogError(address);
                return null;
            }

            return asset;
        }
    }

    private static void LogError(string address)
    {
        Debug.LogError($"ResourceManager : {address} 주소에 리소스가 없습니다!! 확인해주세요!!");
    }
}