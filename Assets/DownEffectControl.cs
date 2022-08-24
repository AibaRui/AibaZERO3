using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownEffectControl : MonoBehaviour
{
    [SerializeField] float _lifetimeCount;
    [SerializeField] float _lifetimeCountLimit;
    AttackClose _attackClose;

    void Start()
    {
        _attackClose = FindObjectOfType<AttackClose>();
    }

    // Update is called once per frame
    void Update()
    {
        _lifetimeCount += Time.deltaTime;
        if (_lifetimeCount > _lifetimeCountLimit)
        {
            _attackClose._closeAttack = false;
            _attackClose._isDownAttackEffect = false;

            Destroy(gameObject);
        }
    }

}
