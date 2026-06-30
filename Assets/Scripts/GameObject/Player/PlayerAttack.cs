using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAttack : MonoBehaviour
{
    private bool _isJerk = false;
    private bool _isNormal = false;

    private CustomerBase _targetCustomer;

    public void Attack()
    {
        if (_targetCustomer == null) return;

        _targetCustomer.OnHit();
        PlayHitEffect();

        if (_isJerk)
        {
            SaveManager.Instance.CurrentPlayerModel.Coin += 100;
            Debug.Log("진상 스코어");
        }
        else if (_isNormal)
        {
            SaveManager.Instance.CurrentPlayerModel.Coin -= 100;
            Debug.Log("노멀 패널티");
        }
    }

    private void PlayHitEffect()
    {
        int randomIndex = Random.Range(1, 6);
        SoundManager.Instance.PlaySFX($"Audio/SFX/Punch/Punch_{randomIndex}").Forget();
        VFXManager.Instance.PlayVFX(AddressUtil.Prefab.VFX.HitC3D, _targetCustomer.transform.position, new Vector3(0, 1f, 0), 0f).Forget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jerk"))
        {
            _isJerk = true;
            _targetCustomer = other.GetComponent<CustomerBase>();
        }
        else if (other.CompareTag("Normal"))
        {
            _isNormal = true;
            _targetCustomer = other.GetComponent<CustomerBase>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Jerk"))
        {
            _isJerk = false;
        }
        else if (other.CompareTag("Normal"))
        {
            _isNormal = false;
        }

        _targetCustomer = null;
    }
}
