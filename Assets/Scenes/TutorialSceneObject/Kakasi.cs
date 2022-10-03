using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kakasi : MonoBehaviour
{
    [SerializeField] int _hp = 10;
    [SerializeField] GameObject _deathEffect;

    [SerializeField] bool _isTargetAttack = false;

    Animator _anim;
    void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "P_Attack")
        {
            if (_isTargetAttack == false)
            {
                _hp--;
                _anim.Play("kakasiDamage");

                if (_hp <= 0)
                {
                    var go = Instantiate(_deathEffect);
                    go.transform.position = transform.position;
                    Destroy(gameObject);
                }
            }
        }
        if (other.gameObject.tag == "TargetAttack")
        {
            if (_isTargetAttack == true)
            {
                _hp--;
                _anim.Play("kakasiDamage");

                if (_hp <= 0)
                {
                    var go = Instantiate(_deathEffect);
                    go.transform.position = transform.position;
                    Destroy(gameObject);
                }
            }
        }
    }
}

