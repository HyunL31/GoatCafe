using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : BaseMonoManager<ResourceManager>
{
    private Dictionary<string, AsyncOperationHandle> m_assetHandle = new();
    private Dictionary<string, int> m_assetLoadCount = new();

    public T LoadAssetSync<T>(string address) where T : Object
    {
        if (m_assetHandle.TryGetValue(address, out AsyncOperationHandle operationHandle) && operationHandle.IsValid())
        {
            m_assetLoadCount[address]++;
            T asset = operationHandle.WaitForCompletion() as T;

            return asset;
        }

        AsyncOperationHandle newOperationHandle = Addressables.LoadAssetAsync<T>(address);
        m_assetHandle.Add(address, newOperationHandle);
        m_assetLoadCount.Add(address, 1);
        try
        {
            T newAsset = newOperationHandle.WaitForCompletion() as T;
            return newAsset;
        }
        catch (System.Exception e)
        {
            this.LogError("에셋 로드 중 예외가 발생하였습니다!!");
            UnLoadAsset(address);
            Debug.LogException(e);
            return null;
        }
    }

    public async UniTask<T> LoadAssetAsync<T>(string address) where T : Object
    {
        if (m_assetHandle.TryGetValue(address, out AsyncOperationHandle operationHandle) && operationHandle.IsValid())
        {
            await operationHandle.ToUniTask();

            if (operationHandle.Status == AsyncOperationStatus.Failed)
            {
                this.LogError("에셋 로드에 실패한 에셋을 불러오기 하였습니다!!");
                UnLoadAsset(address);
                return null;
            }

            m_assetLoadCount[address]++;
            return operationHandle.Result as T;
        }

        AsyncOperationHandle newOperationHandle = Addressables.LoadAssetAsync<T>(address);
        m_assetHandle.Add(address, newOperationHandle);
        m_assetLoadCount.Add(address, 1);

        try
        {
            await newOperationHandle.ToUniTask();
            return newOperationHandle.Result as T;
        }
        catch (System.OperationCanceledException)
        {
            this.LogWarning("에셋 로드를 취소하였습니다!!");
            UnLoadAsset(address);
            return null;
        }
        catch (System.Exception)
        {
            this.LogError("에셋 로드 중 예외가 발생하였습니다!!");
            UnLoadAsset(address);
            return null;
        }
    }

    public void UnLoadAsset(string address)
    {
        if (m_assetLoadCount.ContainsKey(address))
        {
            m_assetLoadCount[address]--;

            if (m_assetLoadCount[address] <= 0)
            {
                ReleaseAsset(address);
            }
        }
    }

    private void ReleaseAsset(string address)
    {
        if (m_assetHandle.TryGetValue(address, out AsyncOperationHandle operationHandle))
        {
            m_assetHandle.Remove(address);
            m_assetLoadCount.Remove(address);
            Addressables.Release(operationHandle);
        }
    }
}
