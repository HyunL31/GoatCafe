using Cysharp.Threading.Tasks;
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public void PlayFootstep()
    {
        if (GameManager.Instance.IsPlaying == false)
        {
            return;
        }
        int randomIndex = UnityEngine.Random.Range(1, 6);
        SoundManager.Instance.PlaySFX($"Audio/SFX/Footstep/Concrete_{randomIndex}").Forget();
    }
}