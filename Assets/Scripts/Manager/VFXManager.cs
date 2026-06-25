using Cysharp.Threading.Tasks;
using UnityEngine;

public class VFXManager : BaseMonoManager<VFXManager>
{

    public async UniTaskVoid PlayVFX(string address, Vector3 position, Vector3 offset = default, float delay = 0f)
    {
        if (delay > 0f)
        {
            await UniTask.WaitForSeconds(delay);
        }

        GameObject prefab = await LoadUtil.Async.LoadPrefabAsync(address);
        if (prefab == null)
        {
            return;
        }

        Vector3 finalPosition = position + offset;

        GameObject vfxObject = Instantiate(prefab, finalPosition, Quaternion.identity);
        if (vfxObject == null)
        {
            ResourceManager.Instance.UnLoadAsset(address);
            return;
        }

        ParticleSystem particle = vfxObject.GetComponentInChildren<ParticleSystem>();

        if (particle != null)
        {
            while (particle != null && particle.IsAlive(true) == true)
            {
                await UniTask.Yield();
            }
        }
        Destroy(vfxObject);
        ResourceManager.Instance.UnLoadAsset(address);
    }
}
