using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingAttack : MonoBehaviour
{
    [Header("必要なオブジェクト")]
    ///<summary>クロスヘアーのスクリプト</summary>
    [SerializeField] GameObject _crosshairController;
    [SerializeField] AttackCloseController _attackCloseController;


    [Header("攻撃時の移動スピード")]
    [Tooltip("攻撃時の移動スピード")] [SerializeField] float _attackSpeed = 3;


    [SerializeField] float _movedDistance = 3;


    [Header("上昇攻撃の範囲の右上")]
    [SerializeField] Transform _rightPos;
    [Header("上昇攻撃の範囲の左上")]
    [SerializeField] Transform _leftPos;

    [SerializeField] PlayerInBattle _playerInBattle;

    Vector3 _nowPos;

    Rigidbody _rb;
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (_attackCloseController._closeAttack && _attackCloseController._pushdKey == AttackCloseController.PushdKey.RisingAttack)
        {
            MoveEnd();
        }
    }
    IEnumerator ReleaseAttackStiffenss()
    {
        yield return new WaitForSeconds(1);

        _attackCloseController._closeAttack = false;
        _attackCloseController._isAttackNow = false;
    }
    public void Attack()
    {
        _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Attack;
        _nowPos = transform.position;
        StartCoroutine(ReleaseAttackStiffenss());

        Vector3 velo = _crosshairController.transform.position - transform.position;

        if (velo.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (velo.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        Debug.Log(velo.x);
        if (velo.y < 0)
        {
            _rb.AddForce(transform.up * _attackSpeed, ForceMode.Impulse);
        }
        else if (velo.x > 2)
        {
            Vector3 pos = _rightPos.position - transform.position;
            _rb.AddForce(pos.normalized * _attackSpeed, ForceMode.Impulse);
        }
        else if (velo.x < -2)
        {
            Vector3 pos = _rightPos.position - transform.position;
            _rb.AddForce(pos.normalized * _attackSpeed, ForceMode.Impulse);
        }
        else
        {
            Vector3 mousPos = new Vector3(velo.normalized.x, Mathf.Abs(velo.normalized.y), velo.normalized.z);
            _rb.AddForce(mousPos * _attackSpeed, ForceMode.Impulse);
        }

    }

    public void Effect()
    {
        Debug.Log("raizing1");
    }
    public IEnumerator NoRisingEffect()
    {
        Debug.Log("PANO");
        yield return new WaitForSeconds(0.5f);
        _attackCloseController._isAttackNow = false;
        _attackCloseController._closeAttack = false;
    }


    public void MoveEnd()
    {
        float distance = Vector2.Distance(_nowPos, this.transform.position);


        if (distance > _movedDistance)
        {
            _attackCloseController._closeAttack = false;
            _attackCloseController._isAttackNow = false;
            _rb.velocity = Vector3.zero;
            _attackCloseController._downSpeed = true;
        }

    }

}
