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
        VFXManager.Instance.PlayVFX(AddressUtil.Prefab.VFX.HitC3D, _targetCustomer.transform.position, new Vector3(0, 1f, 0), 0f).Forget();

        if (_isJerk)
        {
            // 스코어
            Debug.Log("진상 스코어");
        }
        else if (_isNormal)
        {
            // 패널티
            Debug.Log("노멀 패널티");
        }
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
