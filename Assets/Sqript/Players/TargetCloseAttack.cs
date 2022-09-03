using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCloseAttack : MonoBehaviour
{

    [Header("クロスヘアーを管理するオブジェクト")]
    [SerializeField] GameObject _crosshairController;
    [Header("ターゲットシステムを管理するオブジェクト")]
    [SerializeField] TargetSystem targetSystem;
    [Header("敵を格納する空のオブジェクト")]
    [SerializeField] GameObject _enemyBox;
    [Header("プレイヤー自身の近接攻撃のスクリプト")]
    [SerializeField] AttackCloseController _attackCloseController;

    [Header("敵を引き寄せたい場所")]
    [SerializeField] Transform _attractPos;


    [Header("速度設定")]
    [Tooltip("敵を引き寄せる速度")] [SerializeField] float _attractPower = 3;
    [Tooltip("攻撃時に移動する速度")] [SerializeField] float _attackMovedPower = 10;

    [Header("時間設定")]
    [Tooltip("ターゲット攻撃を中断した時のクールタイム")] [SerializeField] float _targetEnemyBeforCoolTime = 2;
    [Tooltip("ターゲット攻撃を最後までしたした時のクールタイム")] [SerializeField] float _targetEnemyAfterCoolTime = 4;
    [Tooltip("引き寄せた敵を離すまでの時間")] [SerializeField] float _releaseEnemyCountLimit = 2;


    [SerializeField] float _moveDistance = 2;

    /// <summary>攻撃した回数を記す</summary>
    int _targetAttackCount = 0;
    /// <summary>攻撃した時の場所を記す</summary>
    Vector3 _nowPos;
    /// <summary>敵を話すまでの時間をカウントする</summary>
    float _releaseEnemyCount = 0;


    /// <summary>攻撃を中断したかどうか判断</summary>
    bool _isTargetEnemyBefor;
    /// <summary>攻撃時にtrue。敵を話すメソッドを呼ぶかどうかの判断</summary>
    bool _isReleaceEnemy;
    /// <summary>攻撃可能か同課の判断</summary>
    bool _isOkTargetAttack = true;


    bool _isRevarseTargetAttack=false;

    PauseManager _pauseManager = default;
    Rigidbody _rb;
    private void Awake()
    {
        _pauseManager = GameObject.FindObjectOfType<PauseManager>();
    }

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (!_pauseManager._isPause)
        {
            if (_isReleaceEnemy && targetSystem._targetEnemy != null)
            {
                ReleaseEnemy();
            }
        }

        MoveEnd();

    }

    void MoveEnd()
    {
        if (_attackCloseController._closeAttack)
        {
            float distance = Vector3.Distance(_nowPos, transform.position);

            if (_isRevarseTargetAttack) //敵を離すとき
            {

                if (distance > _moveDistance)
                {
                    _isRevarseTargetAttack = false;
                    _rb.velocity = Vector3.zero;
                    _attackCloseController._closeAttack = false;
                }
            }
            else
            {
                if (distance > _moveDistance)
                {
                    _rb.velocity = Vector3.zero;
                    _attackCloseController._closeAttack = false;
                }
            }
        }
    }


    public void Attack()
    {
        Direction();
        _rb = gameObject.GetComponent<Rigidbody>();

        float dir = Vector2.Distance(targetSystem._targetEnemy.transform.position, transform.position);
        _targetAttackCount++;

        Vector2 hani = _crosshairController.transform.position - transform.position;

        if (_isOkTargetAttack)
        {
            _attackCloseController.airTime = 0;
            if (dir <= 2)      //ターゲットが近い時
            {
                //それぞれの向いてる角度で-45~45度までの範囲に入っているか。
                if (transform.localScale.x == 1 && hani.normalized.x >= 0.3 && (hani.normalized.y <= 0.9 && hani.normalized.y >= -0.9f)
                || (transform.localScale.x == -1 && hani.normalized.x <= -0.3 && (hani.normalized.y <= 0.9 && hani.normalized.y >= -0.9f)))
                {
                    if (_targetAttackCount == 5)
                    {
                        _isOkTargetAttack = false;    //ターゲット攻撃を不可
                        _enemyBox.transform.DetachChildren();   //敵を子オブジェクトから外す

                        StartCoroutine(TargetAttackNear());
                        StartCoroutine(TargetEnemyCoolTime());  //クールタイムを数える

                        _isReleaceEnemy = false;

                        _attackCloseController._downSpeed = false;
                        _targetAttackCount = 0;
                        _releaseEnemyCount = 0;
                    }
                    else
                    {
                        _attackCloseController._downSpeed = true;
                        targetSystem._targetEnemy.transform.SetParent(_enemyBox.transform); //敵を自身の子オブジェクトにする
                        StartCoroutine(TargetAttackNear());

                        _rb.AddForce(hani.normalized * _attackMovedPower, ForceMode.Impulse);

                        _releaseEnemyCount = 0;
                        _isReleaceEnemy = true;
                    }
                }
                else                        ///////ターゲット攻撃を中止、他の方向の攻撃に移る////////
                {

                    StartCoroutine(RevarseTargetAttack());

                }
            }
            else if (dir > 2)
            {

                if (hani.x >= 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                _attackCloseController._downSpeed = true;
                StartCoroutine(TargetAttackFar());

                _releaseEnemyCount = 0;
                _isReleaceEnemy = true;
                return;
            }
            else
            {
                Debug.Log("NOOOOO");
                StartCoroutine(TestMoveEnd());
            }
        }

    }

    /// <summary>普通に切るターゲットアタック(近いとき)</summary>
    public IEnumerator TargetAttackNear()
    {
        Rigidbody _rbEnemy = targetSystem._targetEnemy.GetComponent<Rigidbody>();
        if (_targetAttackCount == 5)//敵を向いてる方向に弾き飛ばす
        {
            _rbEnemy.isKinematic = false;
            Vector2 ve = new Vector2(transform.localScale.x, 1);
            _rbEnemy.AddForce(ve * 3, ForceMode.Impulse);
        }
        else
        {
            _rbEnemy.velocity = Vector3.zero;
            _rbEnemy.isKinematic = true;

            yield return new WaitForSeconds(0.2f);

            if (_attackCloseController._isGround == true)
            {
                _attackCloseController._isAttackNow = false;
                _attackCloseController._closeAttack = false;
            }
        }
    }

    /// <summary>敵を引き寄せるターゲットアタック(遠い時)</summary>
    public IEnumerator TargetAttackFar()
    {
        Rigidbody _rbEnemy = targetSystem._targetEnemy.GetComponent<Rigidbody>();
        Vector3 velo = _attractPos.position - targetSystem._targetEnemy.transform.position;
        _rbEnemy.AddForce(velo.normalized * _attractPower, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        targetSystem._targetEnemy.transform.position = _attractPos.position;
        _rbEnemy.velocity = Vector3.zero;
        _rbEnemy.isKinematic = true;
    }

    IEnumerator RevarseTargetAttack()
    {
        Vector2 hani = _crosshairController.transform.position - transform.position;

        _isRevarseTargetAttack = true;
        _targetAttackCount = 0;    //ターゲット攻撃の値のリセット
        _releaseEnemyCount = 0;
        _isReleaceEnemy = false;
        _isOkTargetAttack = false;    //ターゲット攻撃を不可
        _enemyBox.transform.DetachChildren();   //敵を子オブジェクトから外す


        Rigidbody _rbEnemy = targetSystem._targetEnemy.GetComponent<Rigidbody>(); //敵を向いてる方向に弾き飛ばす
        _rbEnemy.isKinematic = false;
        Vector2 ve = new Vector2(transform.localScale.x, 1);
        _rbEnemy.AddForce(ve * 3, ForceMode.Impulse);


        yield return new WaitForSeconds(0.5f);

        _attackCloseController._downSpeed = false;

        StartCoroutine(TargetEnemyCoolTime());
        if (hani.x >= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        _nowPos = transform.position;
        _rb.AddForce(hani.normalized * _attackMovedPower, ForceMode.Impulse);


    }





    public IEnumerator TargetEnemyCoolTime()
    {
        if (_isTargetEnemyBefor)
        {
            yield return new WaitForSeconds(_targetEnemyBeforCoolTime);
        }
        else { yield return new WaitForSeconds(_targetEnemyAfterCoolTime); }

        _isOkTargetAttack = true;
        _targetAttackCount = 0;
    }

    public IEnumerator TestMoveEnd()
    {
        if (_attackCloseController._isGround || _attackCloseController._attackCount == 5)
        {
            yield return new WaitForSeconds(0.3f);
            _attackCloseController._closeAttack = false;
            _attackCloseController._isAttackNow = false;
        }
    }

    public void ReleaseEnemy()
    {
        _releaseEnemyCount += Time.deltaTime;
        if (_releaseEnemyCount > _releaseEnemyCountLimit)
        {
            _targetAttackCount = 0;

            Rigidbody _rbEnemy = targetSystem._targetEnemy.GetComponent<Rigidbody>();
            _rbEnemy.isKinematic = false;

            Vector2 ve = new Vector2(transform.localScale.x, 1);
            _rbEnemy.AddForce(ve * 3, ForceMode.Impulse);

            _enemyBox.transform.DetachChildren();
            _releaseEnemyCount = 0;
            _isReleaceEnemy = false;


            _isOkTargetAttack = false;
            StartCoroutine(TargetEnemyCoolTime());
            StartCoroutine(TestMoveEnd());


            _attackCloseController._closeAttack = false;
            _attackCloseController._isAttackNow = false;
            _attackCloseController.airTime = 0;
            _attackCloseController._downSpeed = false;

        }

    }

    void Direction()
    {
        Vector3 muki;
        if (targetSystem._targetEnemy.transform.position.x - transform.position.x > 0)
        {
            muki = new Vector3(1, transform.localScale.y, transform.localScale.z);
            transform.localScale = muki;
        }
        else
        {
            muki = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            transform.localScale = muki;
        }
    }

}
