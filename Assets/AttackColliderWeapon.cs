using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderWeapon : MonoBehaviour
{
    [SerializeField] GameObject _collider;
    [SerializeField] GameObject _colliderUpAttack;
    [SerializeField] GameObject _colliderDownAttack;


    IEnumerator NomalAttack()
    {
        _collider.SetActive(true);
        yield return new WaitForSeconds(1f);
        _collider.SetActive(false);
    }

    IEnumerator UpAttack()
    {
        _colliderUpAttack.SetActive(true);
        yield return new WaitForSeconds(0.12f);
        _colliderUpAttack.SetActive(false);
    }

    IEnumerator DownAttack()
    {
        _colliderDownAttack.SetActive(true);
        yield return new WaitForSeconds(0.12f);
        _colliderDownAttack.SetActive(false);
    }
}
