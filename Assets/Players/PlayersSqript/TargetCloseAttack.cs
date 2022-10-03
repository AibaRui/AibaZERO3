using System.Collections;
using UnityEngine;

public class TargetCloseAttack : MonoBehaviour
{

    [Header("クロスヘアーを管理するオブジェクト")]
    [SerializeField] GameObject _crosshairController;
    [Header("ターゲットシステムを管理するオブジェクト")]
    [SerializeField] TargetSystem _targetSystem;
    [Header("敵を格納する空のオブジェクト")]
    [SerializeField] GameObject _enemyBox;
    [Header("プレイヤー自身の近接攻撃のスクリプト")]
    [SerializeField] AttackCloseController _attackCloseController;

    [Header("プレイヤー移動スクリプト")]
    [SerializeField] PlayerInBattle _playerInBattle;

    [SerializeField] AudioClip[] _auClip = new AudioClip[5];
    AudioSource _aud;




    [Header("敵を引き寄せたい場所")]
    [SerializeField] Transform _attractPos;


    [Header("速度設定")]
    [Tooltip("敵を引き寄せる速度")] [SerializeField] float _attractPower = 3;
    [Tooltip("攻撃時に移動する速度")] [SerializeField] float _attackMovedPower = 10;

    [Header("時間設定")]
    [Tooltip("ターゲット攻撃を中断した時のクールタイム")] [SerializeField] float _targetEnemyBeforCoolTime = 2;
    [Tooltip("ターゲット攻撃を最後までしたした時のクールタイム")] [SerializeField] float _targetEnemyAfterCoolTime = 4;
    [Tooltip("引き寄せた敵を離すまでの時間")] [SerializeField] float _releaseEnemyCountLimit = 2;


    [Header("敵を引き寄せたい場所")]
    [SerializeField] float _barsePower = 5;

    [SerializeField] float _moveDistance = 2;

    [SerializeField] Animator _weaponAnim;
    [SerializeField] GameObject _farEffect;
    [SerializeField] GameObject _effect1;
    [SerializeField] GameObject _effect2;
    [SerializeField] GameObject _effect3;
    [SerializeField] GameObject _effectEnd;
    [SerializeField] Transform _effectPos;


    /// <summary>攻撃した回数を記す</summary>
    int _targetAttackCount = 0;
    /// <summary>攻撃した時の場所を記す</summary>
    Vector2 _nowPos;
    /// <summary>敵を話すまでの時間をカウントする</summary>
    float _releaseEnemyCount = 0;


    /// <summary>攻撃を中断したかどうか判断</summary>
    bool _isTargetEnemyBefor;
    /// <summary>攻撃時にtrue。敵を話すメソッドを呼ぶかどうかの判断</summary>
    bool _isReleaceEnemy;
    /// <summary>攻撃可能か同課の判断</summary>
    public bool _isOkTargetAttack = true;

    /// <summary>攻撃時の移動制限をするかどうか</summary>
    bool _endJudge = false;

    /// <summary>ターゲット攻撃中かどうかの判断（ターゲットシステムで使う）</summary>
    public bool _isTargetAttackNow = false;

    bool _isRevarseTargetAttack = false;

    PauseManager _pauseManager = default;
    Rigidbody _rb;
    Animator _anim;
    private void Awake()
    {
        _pauseManager = GameObject.FindObjectOfType<PauseManager>();
    }

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _anim = gameObject.GetComponent<Animator>();
        _weaponAnim = _weaponAnim.gameObject.GetComponent<Animator>();
        _aud = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {

        if (!_pauseManager._isPause)
        {
            if (_isReleaceEnemy && _targetSystem._targetEnemy != null)
            {
                ReleaseEnemy();
            }
        }
        MoveEnd();
    }

    void MoveEnd()
    {
        float distance = Vector2.Distance(_nowPos, transform.position);

        if (_endJudge)
        {

            if (_isRevarseTargetAttack) //敵を離すとき
            {

                if (distance > _moveDistance)
                {
                    _endJudge = false;
                    _isRevarseTargetAttack = false;

                    _attackCloseController._closeAttack = false;
                    _attackCloseController._isAttackNow = false;
                }
            }
            else
            {
                if (distance > _moveDistance)
                {
                    _endJudge = false;

                    _rb.velocity = Vector3.zero;
                    _attackCloseController._closeAttack = false;
                    _attackCloseController._isAttackNow = false;
                }
            }
        }
    }


    public void Attack()
    {
        _anim.SetBool("isTargetAttack", true);

        _isTargetAttackNow = true;
        _endJudge = true;
        _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Attack;

        _rb.velocity = Vector3.zero;
        _nowPos = transform.position;
        Vector2 hani = _crosshairController.transform.position - transform.position;

        Direction();　//向き調整の関数

        _targetAttackCount++;


        if (_isOkTargetAttack)
        {
            _attackCloseController.airTime = 0;
            if (_targetAttackCount != 1)      //2回目以降の攻撃
            {
                //それぞれの向いてる角度で-45~45度までの範囲に入っているか。
                if (transform.localScale.x == 1 && hani.normalized.x >= 0.3 && (hani.normalized.y <= 0.9 && hani.normalized.y >= -0.9f)
                || (transform.localScale.x == -1 && hani.normalized.x <= -0.3 && (hani.normalized.y <= 0.9 && hani.normalized.y >= -0.9f)))
                {
                    if (_targetAttackCount == 5)
                    {
                        _weaponAnim.Play("Zangeki4");
                        var go = Instantiate(_effectEnd);
                        go.transform.position = _effectPos.position;

                        _enemyBox.transform.DetachChildren();   //敵を子オブジェクトから外す

                        StartCoroutine(TargetAttackNear());　//敵との距離が近いときの判定
                        StartCoroutine(TargetEnemyCoolTime());  //クールタイムを数える

                        _isOkTargetAttack = false;    //ターゲット攻撃を不可
                        _isReleaceEnemy = false;

                        _attackCloseController._isAttackNow = false;
                        _attackCloseController._downSpeed = false;
                        _targetAttackCount = 0;
                        _releaseEnemyCount = 0;
                    }
                    else
                    {
                        _targetSystem._targetEnemy.transform.SetParent(_enemyBox.transform); //敵を自身の子オブジェクトにする

                        StartCoroutine(TargetAttackNear());

                        _rb.AddForce(hani.normalized * _attackMovedPower, ForceMode.Impulse);

                        _attackCloseController._downSpeed = true;
                        _releaseEnemyCount = 0;
                        _isReleaceEnemy = true;

                        _anim.Play("P_TargetAttack2");
                        if (_targetAttackCount == 2)
                        {
                            _weaponAnim.Play("Zangeki1");
                            var go = Instantiate(_effect1);
                            go.transform.position = _effectPos.position;
                        }
                        else if (_targetAttackCount == 3)
                        {
                            _weaponAnim.Play("Zangeki2");
                            var go = Instantiate(_effect2);
                            go.transform.position = _effectPos.position;
                        }
                        else if (_targetAttackCount == 4)
                        {
                            _weaponAnim.Play("Zangeki5");
                            var go = Instantiate(_effect3);
                            go.transform.position = _effectPos.position;
                        }
                    }
                }
                else                        ///////ターゲット攻撃を中止、他の方向の攻撃に移る////////
                {
                    var go = Instantiate(_effectEnd);
                    go.transform.position = _effectPos.position;

                    StartCoroutine(RevarseTargetAttack()); //敵を離す
                    _weaponAnim.Play("Zangeki4");
                }
            }
            else if (_targetAttackCount > 0) //1回目の攻撃
            {
                if (hani.x >= 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                StartCoroutine(TargetAttackFirst());

                _releaseEnemyCount = 0;
                _isReleaceEnemy = true;
                _attackCloseController._downSpeed = true;
                return;
            }
        }
    }

    /// <summary>普通に切るターゲットアタック(2回目以降の攻撃)</summary>
    public IEnumerator TargetAttackNear()
    {
        Rigidbody _rbEnemy = _targetSystem._targetEnemy.GetComponent<Rigidbody>();
        if (_targetAttackCount == 5)//敵を向いてる方向に弾き飛ばす
        {
            if (FindObjectOfType<EnemyMoves>())
            {
                FindObjectOfType<EnemyMoves>()._isDamagedTargetAttack = false;
            }

            //敵の動きの制御を解除、前方に吹き飛ばす
            _rbEnemy.isKinematic = false;
            Vector2 ve = new Vector2(transform.localScale.x, 1);
            _rbEnemy.AddForce(ve * _barsePower, ForceMode.Impulse);
            _targetSystem._targetEnemy = null;

            _isTargetAttackNow = false;

            _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;
            _anim.SetBool("isTargetAttack", false);
            _aud.PlayOneShot(_auClip[2]);
            _anim.Play("P_TargetAttackEnd");
        }
        else
        {
            _rbEnemy.isKinematic = true;
            _aud.PlayOneShot(_auClip[1]);

            _anim.Play("P_TargetAttack2");
            yield return new WaitForSeconds(0.2f);
            _attackCloseController._isAttackNow = false;
        }
    }

    /// <summary>敵を引き寄せる。最初の攻撃</summary>
    public IEnumerator TargetAttackFirst()
    {
        Rigidbody _rbEnemy = _targetSystem._targetEnemy.GetComponent<Rigidbody>();
        Vector3 velo = _attractPos.position - _targetSystem._targetEnemy.transform.position;
        _rbEnemy.AddForce(velo.normalized * _attractPower, ForceMode.Impulse);

        var go = Instantiate(_farEffect);
        go.transform.position = _targetSystem._targetEnemy.transform.position;
        go.transform.SetParent(_targetSystem._targetEnemy.transform);

        _aud.PlayOneShot(_auClip[0]);
        _anim.Play("P_TargetAttackS");

        yield return new WaitForSeconds(0.2f);

        _targetSystem._targetEnemy.transform.position = _attractPos.position;
        _rbEnemy.velocity = Vector3.zero;
        _rbEnemy.isKinematic = true;

        _attackCloseController._isAttackNow = false;

    }

    /// <summary>敵を吹き飛ばす（攻撃中断したときに使う）</summary>
    IEnumerator RevarseTargetAttack()
    {
        Vector2 hani = _crosshairController.transform.position - transform.position;
        if (FindObjectOfType<EnemyMoves>())
        {
            FindObjectOfType<EnemyMoves>()._isDamagedTargetAttack = false;
        }


        _isRevarseTargetAttack = true;
        _targetAttackCount = 0;    //ターゲット攻撃の値のリセット
        _releaseEnemyCount = 0;
        _isReleaceEnemy = false;
        _isOkTargetAttack = false;    //ターゲット攻撃を不可
        _enemyBox.transform.DetachChildren();   //敵を子オブジェクトから外す

        //敵を向いてる方向に弾き飛ばす
        Rigidbody _rbEnemy = _targetSystem._targetEnemy.GetComponent<Rigidbody>();
        _rbEnemy.isKinematic = false;
        Vector2 ve = new Vector2(transform.localScale.x, 1);
        _rbEnemy.AddForce(ve * _barsePower, ForceMode.Impulse);
        _targetSystem._targetEnemy = null;

        _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;
        _anim.Play("P_TargetAttackEnd");
        _aud.PlayOneShot(_auClip[2]);

        yield return new WaitForSeconds(0.5f);

        _isTargetAttackNow = false;
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
        _rb.AddForce(hani.normalized * _attackMovedPower, ForceMode.Impulse);


        _attackCloseController._isAttackNow = false;
        _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;
        _anim.SetBool("isTargetAttack", false);
    }




    /// <summary>攻撃のクールタイム </summary>
    /// <returns></returns>
    public IEnumerator TargetEnemyCoolTime()
    {
        //中断したか最後まで攻撃したかで、クールタイムが変わる
        if (_isTargetEnemyBefor)
        {
            yield return new WaitForSeconds(_targetEnemyBeforCoolTime);
        }
        else { yield return new WaitForSeconds(_targetEnemyAfterCoolTime); }

        _isOkTargetAttack = true;
        _targetAttackCount = 0;
    }


    //public IEnumerator TestMoveEnd()
    //{
    //    if (_attackCloseController._attackCount == 5)//_attackCloseController._isGround || 
    //    {
    //        FindObjectOfType<EnemyMoves>()._isDamagedTargetAttack = false;
    //        yield return new WaitForSeconds(0.3f);
    //        _attackCloseController._closeAttack = false;
    //        _attackCloseController._isAttackNow = false;
    //        _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;
    //    }
    //}

    public void ReleaseEnemy()
    {
        _releaseEnemyCount += Time.deltaTime;
        if (_releaseEnemyCount > _releaseEnemyCountLimit)
        {
            _aud.PlayOneShot(_auClip[2]);
            _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;
            _anim.SetBool("isTargetAttack", false);
            _anim.Play("P_TargetAttackEnd");
            _weaponAnim.Play("Zangeki4");
            var go = Instantiate(_effectEnd);
            go.transform.position = _effectPos.position;

            if (FindObjectOfType<EnemyMoves>())
            {
                FindObjectOfType<EnemyMoves>()._isDamagedTargetAttack = false;
            }



            _targetAttackCount = 0;

            Rigidbody _rbEnemy = _targetSystem._targetEnemy.GetComponent<Rigidbody>();
            _rbEnemy.isKinematic = false;

            Vector2 ve = new Vector2(transform.localScale.x, 1);
            _rbEnemy.AddForce(ve * _barsePower, ForceMode.Impulse);
            _targetSystem._targetEnemy = null;

            _enemyBox.transform.DetachChildren();
            _releaseEnemyCount = 0;
            _isReleaceEnemy = false;


            _isOkTargetAttack = false;
            StartCoroutine(TargetEnemyCoolTime());
       //    StartCoroutine(TestMoveEnd());


            _attackCloseController._closeAttack = false;
            _attackCloseController._isAttackNow = false;
            _attackCloseController.airTime = 0;
            _attackCloseController._downSpeed = false;
            _isTargetAttackNow = false;
        }

    }

    void Direction()
    {
        Vector3 muki;
        if (_targetSystem._targetEnemy.transform.position.x - transform.position.x > 0)
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
