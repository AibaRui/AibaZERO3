using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AttackCloseController : MonoBehaviour
{
    [SerializeField] int _attackZanryou = 1000;

    [SerializeField] RisingAttack _risingAttack;
    [SerializeField] DownAttack _downAttack;
    [SerializeField] NomalAttack _nomalAttack;

    [Header("必要なオブジェクト")]
    ///<summary>クロスヘアーのスクリプト</summary>
    [SerializeField] GameObject _crosshairController;
    /// <summary>ターゲット機能のスクリプト</summary>
    [SerializeField] TargetSystem targetSystem;
    /// <summary>攻撃時に敵を入れておく空のオブジェクト</summary>
    [SerializeField] GameObject _enemyBox;
    /// <summary>ターゲットアタックのスクリプト</summary>
    [SerializeField] TargetCloseAttack _targetCloseAttack;
    /// <summary>攻撃判定用のオブジェクト</summary>
    [SerializeField] GameObject _attackCollider;




    float _countResetTime = 0;
    float _countResetLimit = 0;

    [Header("エフェクトとポジション")]
    [SerializeField] GameObject[] _zangekiEffects = new GameObject[4];
    [SerializeField] Transform[] _zangekiEffectsPosition = new Transform[4];
    [SerializeField] GameObject _downAttackEffect;

    [Header("武器のアニメーション")]
    [SerializeField] Transform _zangekiEffectPosition;
    [SerializeField] Animator _weaponAnim;


    [Header("攻撃時の移動の速さ")]
    [Tooltip("攻撃時の移動スピード")] [SerializeField] float _attackMovedPower = 5;
    [Tooltip("上、攻撃時の移動スピード")] [SerializeField] float _dawnAttackMovePower = 20;

    [Header("攻撃時の移動の距離")]
    [Tooltip("横、攻撃時の移動距離")] [SerializeField] float _movedDistanceX = 2;
    [Tooltip("横、攻撃時の移動距離")] [SerializeField] float _movedTargetDistanceX = 5;
    [Tooltip("上、攻撃時の移動距離")] [SerializeField] float _movedDistanceUp = 0.5f;
    [Tooltip("空中横、攻撃時の移動距離")] [SerializeField] float _movedDistanceUpX = 0.5f;


    [Header("攻撃レートの設定")]
    [Tooltip("攻撃のレート")] [SerializeField] float _attackLate = 0.3f;
    [Tooltip("クールタイム")] [SerializeField] float _attackCoolTimeLimit = 3;
    [Tooltip("コンボの持続時間")] [SerializeField] float _attackContenueTimeLimit = 2f;
    [Tooltip("下攻撃の硬直時間")] [SerializeField] float _attackDownLate = 1f;
    /// <summary>クールタイムを数える</summary>
    private float _attackCoolTimeCount = 0;
    /// <summary>コンボ持続時間を数える</summary>
    private float _attackContenueTimeCount = 0f;


    /// <summary>攻撃の種類を判定</summary>
    public PushdKey _pushdKey = PushdKey.NoMove;

    /// <summary>攻撃開始時の場所</summary>
    public Vector3 _nowPos;

    ///////////////////////////////////////////////攻撃回数////////////////

    public int _attackCount = 0;
    /// <summary>横、攻撃のカウント</summary>
    int _attackXCount = 0;
    /// <summary>上昇、攻撃のカウント</summary>
    int _risingAttackCount = 0;
    /// <summary>空中横、攻撃のカウント</summary>
    int _upMoveAttackCount = 0;
    /// <summary>空中、攻撃のカウント</summary>
    int _upAttackCount = 0;

    ///////////////////////////////////////////////攻撃の判断////////////////

    /// <summary>攻撃中であるかどうか </summary>
    public bool _isAttackNow = false;

    public bool _closeAttack;
    bool _judgeMoved = false;


    /// <summary>上攻撃入力の判定 </summary>
    bool _isUpAttack = false;
    /// <summary>下攻撃入力の判定 </summary>
    bool _isDownAttack = false;


    bool _isRisingAttack = false;

    /// <summary>攻撃可能かどうかの判定</summary>
    public bool okAttack = false;
    /// <summary>攻撃コンボ持続中かどうかの判定</summary>
    bool _isAttackContnue = false;


    public bool _isRevarseTargetAttack;

    bool _isMove = false;

    /// <summary>「DownEffectControl」で使用</summary>
    public bool _isDownAttackEffect = false;
    // [SerializeField] GameObject _attackCollider;

    ////////空中のそれぞれの移動制限//////////////////////////////////////////////
    public float airTime = 0;                      //攻撃時の空中滞在時間
    [SerializeField] float airTimeLimit = 2;        //攻撃時の空中滞在時間                                     
    [SerializeField] float _xSpeed = 0.5f;
    [SerializeField] float _ySpeed = 0f;
    [SerializeField] float _zSpeed = 0.5f;

    public bool _isGround = false;

    /// <summary>攻撃の際、空中で留まるかどうかの判定。trueで留まるための関数【Airtimeを呼ぶ】</summary>
    public bool _downSpeed = false;

    PauseManager _pauseManager = default;

    Animator _anim;
    Rigidbody _rb;

    [SerializeField] PlayerInBattle _playerInBattle;
    private void Awake()
    {
        _pauseManager = GameObject.FindObjectOfType<PauseManager>();
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _weaponAnim = _weaponAnim.GetComponent<Animator>();

    }


    void Update()
    {
        if (!_pauseManager._isPause)
        {
            ///////////////////////攻撃判定場所の移動/////////
            float _h = Input.GetAxisRaw("Horizontal");



            if (_attackCount == _attackZanryou)
            {
                okAttack = false;
            }

            if (okAttack)   // 攻撃可能だったら攻撃
            {
                if (_isAttackNow == false)
                {
                    Attack();
                }
            }
            else   //攻撃不可能だったらクールタイムを数える
            {
                CountCoolTime();
            }

            ResetCount();



            ////コンボ持続時間を数える
            //if (_isAttackContnue)
            //{
            //   // CountContenueTime();
            //}

            //空中攻撃時の移動制限
            if (_downSpeed)
            {
                AirTime();
            }
            else
            {
                _rb.useGravity = true;
            }

        }
    }

    private void FixedUpdate()
    {
        if (!_pauseManager._isPause)
        {
            //if (_isMove)
            //{
            //    AttackdMove();
            //    _isMove = false;
            //}


            if (_pushdKey == PushdKey.RisingAttack || _pushdKey == PushdKey.MoveX && _closeAttack == true)
            {
                if (_rb.velocity.x > 10)
                {
                    Vector3 varlo = new Vector3(10, _rb.velocity.y, _rb.velocity.z);
                    _rb.velocity = varlo;
                }
            }
        }
    }
    IEnumerator TestMoveEnd()
    {
        if (_isGround || _attackCount == 5)
        {
            yield return new WaitForSeconds(0.3f);
            _closeAttack = false;
            _isAttackNow = false;
        }

    }


    /// <summary>攻撃の硬直解除</summary>
    IEnumerator ReleaseAttackStiffenss()
    {
        if (_pushdKey == PushdKey.DownAttack || _pushdKey == PushdKey.DownAttackOnGround)
        {
            yield return new WaitForSeconds(_attackDownLate);
        }
        //  _isAttackNow = false;
        if (_enemyBox.transform.childCount != 0)
        {
            //_enemyBox.transform.DetachChildren();
        }
    }

    /// <summary>クールタイムの計測</summary>
    void CountCoolTime()
    {
        _attackCoolTimeCount += Time.deltaTime;
        if (_attackCoolTimeCount > _attackCoolTimeLimit)
        {
            okAttack = true;    //攻撃できるようなる

            _attackCoolTimeCount = 0;     //クールタイムのリセット

            _attackCount = 0;           //攻撃回数を0に
            _attackXCount = 0;
            _risingAttackCount = 0;
            _upAttackCount = 0;
            _upMoveAttackCount = 0;
        }
    }


    void ResetCount()
    {
        if (Input.GetMouseButton(0))
        {
            _countResetTime += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _countResetTime = 0;
        }

        if (_countResetTime > _countResetLimit)
        {
            _attackCount = 0;
        }
    }



    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _downSpeed = false;
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Debug.Log("キー:" + _isUpAttack);

            StartCoroutine(ReleaseAttackStiffenss());
            _nowPos = this.transform.position;

            _isAttackNow = true;
            //  _isAttackContnue = true;

            _attackContenueTimeCount = 0;

            if (targetSystem._targetEnemy != null)
            {
                if (_targetCloseAttack._isOkTargetAttack)
                {
                    _pushdKey = PushdKey.Target;
                    _targetCloseAttack.Attack();
                    return;
                }
                else
                {
                    _isAttackNow = false;
                    return;
                }

            }


            if ((v > 0) && !_isRisingAttack)            //上昇攻撃
            {
                airTime = 0;
                _attackCount++;
                _isRisingAttack = true;
                _closeAttack = true;
                _pushdKey = PushdKey.RisingAttack;
                _risingAttack.Attack();
                _risingAttack.Effect();
                return;
            }
            else if ((v > 0) && _isRisingAttack)
            {
                _closeAttack = true;
                _pushdKey = PushdKey.RisingAttack;
                StartCoroutine(_risingAttack.NoRisingEffect());
                return;
            }
            _attackCount++;


            if (v < 0)                                //下攻撃
            {
                _closeAttack = true;
                _pushdKey = PushdKey.DownAttack;
                _downAttack.Attack();
                return;
            }



            if (!_isGround && h != 0)       //空中移動攻撃
            {
                _closeAttack = true;
                _pushdKey = PushdKey.UpMoveAttack;
                _nomalAttack.UpMoveAttack(h);
                _nomalAttack.UpAttackEffect();
                return;
            }
            if (h != 0)            //横移動攻撃
            {
                airTime = 0;
                _closeAttack = true;
                _pushdKey = PushdKey.MoveX;
                _nomalAttack.MoveAttack();
                _nomalAttack.MoveAttackEffeck();
                return;
            }

            if (!_isGround)　//空中攻撃
            {
                Debug.Log("aaa");
                _closeAttack = true;
                _pushdKey = PushdKey.UpAttack;
                _nomalAttack.UpAttack();
                _nomalAttack.UpMoveAttackEffect();
            }



            if (h == 0)  //その場攻撃
            {
                _closeAttack = true;
                _pushdKey = PushdKey.NoMove;
                _nomalAttack.NoMoveAttackEffeck();
                _nomalAttack.NoMoveAttack();
            }

        }
    }

    void Effects()
    {
        if (_attackCount == 10)
        {
            _isAttackContnue = false;
            okAttack = false;     //攻撃を不可。クールタイムを数える     
            airTime = 0;
            _downSpeed = false;
            return;
        }


        //単発、空中範囲攻撃 
        if (_pushdKey == PushdKey.UpAttack)
        {
            if (_upAttackCount == 0)
            {
                Debug.Log("up1");
                return;
            }
            else if (_upAttackCount == 1)
            {
                Debug.Log("up2");
                return;
            }
            else if (_upAttackCount == 2)
            {
                Debug.Log("up3");
                _upAttackCount = 0;
                return;
            }
        }



        if (_pushdKey == PushdKey.UpMoveAttack)
        {
            //////空中横、攻撃（連撃）
            if (_upMoveAttackCount == 1)
            {
                Debug.Log("upMove1");
                return;
            }
            else if (_upMoveAttackCount == 2)
            {
                Debug.Log("upMove2");
                return;
            }
            else if (_upMoveAttackCount == 3)
            {
                Debug.Log("upMove3");
                return;
            }
            else if (_upMoveAttackCount == 4)
            {
                Debug.Log("upMove4");
                _upMoveAttackCount = 0;
                return;
            }

        }




    }


    void AirTime()       ///空中滞在のの時間計算
    {
        //空中攻撃時の移動制限
        // _rb.velocity = new Vector3(_xSpeed, _ySpeed, _zSpeed);
        _rb.useGravity = false;

        airTime += Time.deltaTime;
        if (airTime > airTimeLimit)
        {
            _rb.useGravity = true;
            // _closeAttack = false;
            _downSpeed = false;
            airTime = 0;
            _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;
        }

    }

    void AttackNow()
    {
        _isAttackNow = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = true;
            _downSpeed = false;
            airTime = 0;
            _rb.isKinematic = false;
            _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;

            _isRisingAttack = false;
            //if (_upMoveAttackCount > 0)        //空中攻撃をやめて地面についたら、攻撃コンボを中止
            //{
            //    Debug.Log("dalseup");
            //    _closeAttack = false;
            //    _isAttackContnue = false;
            //   // okAttack = false;     //攻撃を不可。クールタイムを数える
            //    airTime = 0;
            //}   
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = false;
        }
    }

    public enum PushdKey
    {
        NoMove,
        MoveX,
        /// <summary>上昇攻撃</summary>
        RisingAttack,
        /// <summary>空中移動攻撃</summary>
        UpMoveAttack,
        /// <summary>空中攻撃</summary>
        UpAttack,
        /// <summary>下攻撃</summary>
        DownAttackOnGround,
        /// <summary>空中からの下攻撃</summary>
        DownAttack,
        /// <summary>ターゲット攻撃</summary>
        Target,

    }



}
