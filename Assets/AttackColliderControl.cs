using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderControl : MonoBehaviour
{
    public bool _hitEnemy=false;

    [SerializeField] GameObject _enemyBox;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy")
        {
            _hitEnemy = true;
            other.transform.SetParent(_enemyBox.transform);
        }
    }
}
