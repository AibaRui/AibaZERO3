using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    [SerializeField] int _hp;


    [Tooltip("敵の移動速度")]
    [SerializeField] float _moveSpeed = 3;
    [SerializeField] float _dashSpeed = 3;

    [Tooltip("プレイヤーの探知範囲")]
    [SerializeField] float _lookPlayerPos = 20;
    [SerializeField] float _cautiousPlayerPos = 10;
    [SerializeField] float _attackPlayerPos = 3;

    [Tooltip("攻撃クールタイム")]
    [SerializeField] float _attackCoolTimeCount = 0;
    [SerializeField] float _attackCoolTimeCountLimit = 5;

    bool _okAttack = true;



    [SerializeField] float _stopAttackd = 3;

    [SerializeField] GameObject _attackCollider;

    //[Tooltip("プレイヤーの探知範囲")]
    [SerializeField] float _processCoolTimeTimeLimit = 2;
    float _processCoolTimeCount = 0;
    bool _isprocessCoolTime = false;


    [SerializeField] float _lookTimeCountLimit = 3;
    float _lookTimeCount = 0;


    [SerializeField] float _chaiseTimeLimit = 5;
    float _chaiseTime = 0;

    [SerializeField] float _gardTimeCountLimit = 3;
    float _gardTimeCount = 0;


    [SerializeField] GameObject _player;
    [SerializeField] GameObject _damaged;
    int _attackKindNomber = 0;

    NextMove _nextMove = NextMove.Wait;


    bool _isGard = false;


    float _dir = 30;
    Vector3 velo;
    [SerializeField] GameObject _panel;
    Animator _panelAnim;
    Animator _anim;
    Rigidbody _rb;
    /// <summary>攻撃のクールタイムを数える</summary>
    void AttackCoolTimeCount()
    {
        _attackCoolTimeCount += Time.deltaTime;
        if (_attackCoolTimeCount > _attackCoolTimeCountLimit)
        {
            _attackCoolTimeCount = 0;
            _okAttack = true;
        }
    }
    void Move()
    {
        if (_isprocessCoolTime)
        {
            return;
        }

        if (_nextMove == NextMove.Wait)
        {
            Debug.Log("Chack");
            Chack();
            _isprocessCoolTime = true;
        }

    }

    void Chack()
    {
        _dir = Vector3.Distance(_player.transform.position, gameObject.transform.position);

        if (_dir < _attackPlayerPos)     //攻撃範囲
        {
            var r = Random.Range(0, 2);
            _attackKindNomber = Random.Range(0, 3);
            if (r == 0 && _okAttack)
            {
                _nextMove = NextMove.Attack;    //攻撃

            }
            else if (r == 1)
            {
                _nextMove = NextMove.Looking;   //警戒
            }
            else if (r == 2)
            {
                _nextMove = NextMove.Gard;      //ガード
            }


        }
        else if (_dir < _cautiousPlayerPos)     //警戒範囲
        {
            var r = Random.Range(0, 3);
            if (r == 0)
            {
                _nextMove = NextMove.Chaise;            //追う
            }
            else if (r == 1)
            {
                _nextMove = NextMove.ChaseAndAttack;    //追いかけて攻撃
            }
            else if (r == 2)
            {
                _nextMove = NextMove.Gard;              //ガード
            }

        }
        else if (_dir < _lookPlayerPos)   //プレイヤー検知範囲
        {
            //追いかける
            _nextMove = NextMove.Chaise;
        }

        //アニメ
        else if (_dir < _lookPlayerPos)  
        {
            _panelAnim.SetBool("Look", true);

        }
        else if (_dir > _lookPlayerPos)
        {
            _panelAnim.SetBool("Look", false);
        }

    }

    //void Count()
    //{
    //    _processCoolTimeCount += Time.deltaTime;
    //    if(_processCoolTimeCount>_processCoolTimeTimeLimit)
    //    {
    //        _isprocessCoolTime = false;
    //        _processCoolTimeCount = 0;
    //    }

    //}

    /// <summary>警戒範囲まで近づく</summary>
    void Chaise()
    {
        _dir = Vector3.Distance(_player.transform.position, gameObject.transform.position);
        velo = _player.transform.position - transform.position;
        if (_dir > _cautiousPlayerPos)  //警戒範囲に近づく
        {
            _rb.velocity = velo.normalized * _dashSpeed;
            _chaiseTime += Time.deltaTime;
            if (_chaiseTime > _chaiseTimeLimit)  //時間内に攻撃範囲に行かなかったら次の行動に移る
            {
                _nextMove = NextMove.Wait;
                _chaiseTime = 0;
            }
        }
        else { _nextMove = NextMove.Wait; }

    }

    void Attack()
    {
        if (_attackKindNomber == 0)
        {
            _okAttack = false;
            Debug.Log("Attackpatarn1");
            _nextMove = NextMove.Wait;
            _anim.SetTrigger("Attack");
            _panelAnim.SetTrigger("Attack");
        }
        else
        {
            _okAttack = false;
            Debug.Log("Attackpatarn2");
            _nextMove = NextMove.Wait;
            _anim.SetTrigger("Attack");
            _panelAnim.SetTrigger("Attack");
        }

        //_attackCollider.SetActive(true);
        //_rb.velocity = Vector3.zero;                
    }

    /// <summary>近づいて攻撃</summary>
    void ChaseAndAttack()
    {
        velo = _player.transform.position - transform.position;
        _dir = Vector3.Distance(_player.transform.position, gameObject.transform.position);
        if (_dir > _attackPlayerPos)
        {
            _rb.velocity = velo.normalized * _dashSpeed;
            _chaiseTime += Time.deltaTime;
            if (_chaiseTime > _chaiseTimeLimit)  //時間内に攻撃範囲に行かなかったら次の行動に移る
            {
                _nextMove = NextMove.Wait;
                _chaiseTime = 0;
            }
        }

        if (_dir < _attackPlayerPos && _okAttack)
        {
            _okAttack = false;
            _nextMove = NextMove.Attack;　　　　//攻撃メソッドを呼ぶ
        }
    }

    /// <summary>警戒行動</summary>
    void Looking()
    {
        float _x = _player.transform.position.x - transform.position.x;
        if (_x > 0)
        {
            _rb.AddForce(transform.right * 1);
        }
        else if (_x < 0)
        {
            _rb.AddForce(transform.right * -1);
        }

        _lookTimeCount += Time.deltaTime;
        if (_lookTimeCount > _lookTimeCountLimit)  //次の行動に移る
        {
            _nextMove = NextMove.Wait;
            _chaiseTime = 0;
        }

        _dir = Vector3.Distance(_player.transform.position, gameObject.transform.position);
        if (_dir < _attackPlayerPos)
        {
            _nextMove = NextMove.Attack;　　　　//攻撃メソッドを呼ぶ
        }
    }

    void Gard()
    {
        _isGard = true;
        _rb.velocity = Vector3.zero;
        _gardTimeCount += Time.deltaTime;
        if (_gardTimeCount > _gardTimeCountLimit)
        {
            _isGard = false;
            _gardTimeCount = 0;
            _nextMove = NextMove.Wait;
        }
    }

    enum NextMove
    {
        Wait,
        Think,
        Chaise,
        Attack,
        ChaseAndAttack,
        Looking,
        Gard,

    }


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = gameObject.GetComponent<Animator>();
        _panelAnim = _panel.GetComponent<Animator>();
    }


    void Update()
    {
        //Move();

        //Count();


        if (!_okAttack)
        {
            AttackCoolTimeCount();
        }


        if (_nextMove == NextMove.Wait)
        {
            Chack();
        }




        if (_nextMove == NextMove.Chaise)
        {
            Chaise();
        }
        if (_nextMove == NextMove.Attack)
        {
            if (_okAttack)
            {
                Attack();
            }
            else
            {
                Gard();
            }

        }
        if (_nextMove == NextMove.ChaseAndAttack)
        {
            ChaseAndAttack();
        }
        if (_nextMove == NextMove.Looking)
        {
            Looking();
        }
        if (_nextMove == NextMove.Gard)
        {
            Gard();
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "P_Attack")
        {
            var go = Instantiate(_damaged);
            go.transform.position = transform.position;
            Debug.Log("dAmamge");
            _hp--;

            if (_hp == 0)
            {
                Destroy(gameObject, 0.1f);
            }
        }

    }

}
