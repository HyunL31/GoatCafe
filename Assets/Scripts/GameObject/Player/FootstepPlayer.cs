using Cysharp.Threading.Tasks;
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public void PlayFootstep()
    {
        GameState state = GameManager.Instance.CurrentState;
        bool canPlayFootstep = (state == GameState.Playing || state == GameState.Home);

        if (canPlayFootstep == false)
        {
            return;
        }
        int randomIndex = UnityEngine.Random.Range(1, 6);
        SoundManager.Instance.PlaySFX($"Audio/SFX/Footstep/Concrete_{randomIndex}").Forget();
    }
}