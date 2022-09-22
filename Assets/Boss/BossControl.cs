using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    [Header("HP")]
    [Tooltip("HP")] [SerializeField] int _hp;

    [Header("クールタイムの最小タイム")]
    [Tooltip("クールタイムの最小タイム")] [SerializeField] float _minTime = 10;

    [Header("クールタイムの最大タイム")]
    [Tooltip("クールタイムの最大タイム")] [SerializeField] float _maxTime = 15;




    [Header("火柱の攻撃エフェクト")]
    [Tooltip("火柱の攻撃エフェクト")] [SerializeField] GameObject _fireWall;

    [Header("爆発の攻撃エフェクト")]
    [Tooltip("爆発の攻撃エフェクト")] [SerializeField] GameObject _exprosion;

    [Header("火球のエフェクト")]
    [Tooltip("火球エフェクト")] [SerializeField] GameObject _fireBall;

    [Header("火球の出現場所")]
    [Tooltip("火球の出現場所")] [SerializeField] Transform[] _posFireBall = new Transform[3];


    [Header("ボスの移動場所")]
    [Tooltip("ボスの移動場所")] [SerializeField] Transform[] _pos = new Transform[2];

    /// <summary>攻撃中かどうか</summary>
    bool _isAttackNow = false;
    /// <summary>攻撃可能かどうか</summary>
    bool _isAttack = true;

    /// <summary>攻撃が終了したかどうかの判断。アニメーションで呼ぶ</summary>
    bool _endAttack = false;

    /// <summary>イベントシーン中かどうか</summary>
    bool _isEvent = false;

    GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {

    }





    void Update()
    {
        if (!_isEvent)
        {
            if (_endAttack)
            {
                StartCoroutine(AttackLate());
                _endAttack = false;
            }
            MainRoutine();
        }
    }

    void MainRoutine()
    {

        if (_isAttackNow || !_isAttack)
        {
            return;
        }
        var num = Random.Range(0, 3);

        if (num == 0)
        {
            Debug.Log("FFFF");
            StartCoroutine(FireWall());
        }
        else if (num == 1)
        {
            Debug.Log("EEEXXX");
            StartCoroutine(Exprosion());
        }
        else if (num == 2)
        {
            Debug.Log("VVVV");
            StartCoroutine(FireBall());
        }
        _isAttackNow = true;
        _isAttack = false;
    }

    IEnumerator AttackLate()
    {
        var num = Random.Range(6, 10);
        yield return new WaitForSeconds(num);
        _isAttack = true;
    }

    /// <summary>火柱</summary>
    IEnumerator FireWall()
    {
        Debug.Log("FFFF");
        for (int i = 0; i < 5; i++)
        {
            var go = Instantiate(_fireWall);
            go.transform.position = _player.transform.position;
            yield return new WaitForSeconds(3);
        }



        _endAttack = true;
        _isAttackNow = false;
    }

    IEnumerator Exprosion()
    {

        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i);
            var num = Random.Range(0, 3);
            var go = Instantiate(_exprosion);
            if (num == 0)//プレイヤーの右側
            {
                Vector3 ve = new Vector3(_player.transform.position.x + 2, _player.transform.position.y, -3);
                go.transform.position = ve;
            }
            else if (num == 1)//プレイヤーの左側
            {
                Vector3 ve = new Vector3(_player.transform.position.x - 2, _player.transform.position.y, -3);
                go.transform.position = ve;
            }
            else if (num == 2)//プレイヤーの場所
            {
                Vector3 ve = new Vector3(_player.transform.position.x, _player.transform.position.y, -3);
                go.transform.position = ve;
            }
            yield return new WaitForSeconds(3);
        }

        _endAttack = true;
        _isAttackNow = false;

    }

    IEnumerator FireBall()
    {

        for (int i = 0; i < 3; i++)
        {
            var go = Instantiate(_fireBall);
            go.transform.position = _posFireBall[i].position;
            Rigidbody _rb = go.GetComponent<Rigidbody>();
            _rb.velocity = new Vector3(-3, 0, 0);
            yield return new WaitForSeconds(3);
        }

        _endAttack = true;
        _isAttackNow = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "P_Attack")
        {
            _hp--;
        }
    }

}
