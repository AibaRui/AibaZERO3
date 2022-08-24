using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AttackClose : MonoBehaviour
{

    [SerializeField] int _attackZanryou = 10;

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
    PushdKey _pushdKey = PushdKey.NoMove;

    /// <summary>攻撃開始時の場所</summary>
    Vector3 _nowPos;

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
    ///
    /// <summary>下攻撃の降下中であるかどうか </summary>
    public bool _isDownNow = false;
    /// <summary>攻撃中であるかどうか </summary>
    public bool _isAttackNow = false;

    public bool _closeAttack;
    bool _judgeMoved = false;


    /// <summary>上攻撃入力の判定 </summary>
    bool _isUpAttack = false;
    /// <summary>下攻撃入力の判定 </summary>
    bool _isDownAttack = false;



    /// <summary>攻撃可能かどうかの判定</summary>
    bool okAttack = false;
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

            //攻撃硬直解除の判定
            judgeMovedEnd();

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
        else if (_pushdKey == PushdKey.RisingAttack)
        {
            yield return new WaitForSeconds(_attackLate);
        }
        else if (_pushdKey == PushdKey.MoveX)
        {
            yield return new WaitForSeconds(_attackLate);
            _closeAttack = false;
        }
        else
        {
            yield return new WaitForSeconds(_attackLate);
        }

        _isAttackNow = false;
        if (_enemyBox.transform.childCount != 0)
        {
            //_enemyBox.transform.DetachChildren();
        }

        if (_pushdKey == PushdKey.MoveX)
        {
            _closeAttack = false;
        }
    }

    /// <summary>硬直解除の関数</summary>
    void judgeMovedEnd()
    {
        if (_closeAttack)
        {

            float distance = Vector3.Distance(_nowPos, transform.position);

            if (_pushdKey == PushdKey.Target)
            {
                if (_isRevarseTargetAttack)
                {

                    if (distance > _movedDistanceX)
                    {
                        _isRevarseTargetAttack = false;
                        _rb.velocity = Vector3.zero;
                        _closeAttack = false;
                    }
                }
                else
                {
                    if (distance > _movedDistanceX)
                    {
                        _rb.velocity = Vector3.zero;
                        _closeAttack = false;
                    }
                }
            }

            if (_isDownNow && _isGround)
            {
                _isAttackContnue = false;
                okAttack = false;
                var effect = Instantiate(_downAttackEffect); //エフェクトを出す
                effect.transform.position = transform.position;
                _isDownNow = false;
                _weaponAnim.Play("DownAttack");
            }

            if (_pushdKey == PushdKey.RisingAttack)
            {
                if (distance > _movedDistanceUp && _risingAttackCount < 3)
                {
                    _closeAttack = false;
                    _rb.velocity = Vector3.zero;
                    _downSpeed = true;
                }
                else if (_risingAttackCount > 3)
                {
                    //_rb.velocity = Vector3.zero;

                }

            }

            if (_pushdKey == PushdKey.UpAttack)
            {
                _closeAttack = false;
                _rb.velocity = Vector3.zero;

            }


            if (_pushdKey == PushdKey.UpMoveAttack)
            {
                if (distance > _movedDistanceUpX)
                {
                    _closeAttack = false;
                    _rb.velocity = Vector3.zero;
                }

            }

            if (_pushdKey == PushdKey.MoveX)
            {
                if (distance > 4)
                {
                    _closeAttack = false;
                    Debug.Log("ffx");
                    _rb.velocity = Vector3.zero;
                }
            }

            if (_pushdKey == PushdKey.NoMove)
            {
                if (distance > _movedDistanceX)
                {

                    _rb.velocity = Vector3.zero;
                    _closeAttack = false;
                }
            }


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
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        if (v > 0)
        {
            _isUpAttack = true;
        }
        else if (v < 0)
        {
            _isDownAttack = true;
        }
        else
        {
            _isUpAttack = false;
            _isDownAttack = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("キー:" + _isUpAttack);



            StartCoroutine(ReleaseAttackStiffenss());
            _nowPos = this.transform.position;

            _isAttackNow = true;
            //  _isAttackContnue = true;

            _attackContenueTimeCount = 0;

            if (targetSystem._targetEnemy != null)
            {
                _pushdKey = PushdKey.Target;
                AttackdMove();
                return;
            }

            _attackCount++;


            if (_isDownAttack)
            {

                _pushdKey = PushdKey.DownAttack;
                _isDownNow = true;
                airTime = 0;
                _downSpeed = false;
                AttackdMove();
                Effects();

                return;
            }


            if (_isUpAttack && _risingAttackCount < 3)            //上昇攻撃
            {

                _pushdKey = PushdKey.RisingAttack;
                _risingAttackCount++;
                AttackdMove();
                Effects();
                return;
            }

            if (h != 0)            //横攻撃
            {
                _pushdKey = PushdKey.MoveX;
                _downSpeed = false;
                airTime = 0;

                AttackdMove();
                Effects();
                return;
            }

            if (!_isGround && (h == 0))       //空中攻撃
            {
                Debug.Log("回数:" + _upMoveAttackCount);
                _upMoveAttackCount++;
                _pushdKey = PushdKey.UpMoveAttack;
                AttackdMove();
                Effects();
                return;
            }

            //if (!_isGround)                          //空中での単発範囲攻撃
            //{
            //    _pushdKey = PushdKey.UpAttack;
            //    AttackdMove();
            //    Effects();
            //    return;
            //}


            if (h == 0)  //その場攻撃
            {
                _pushdKey = PushdKey.NoMove;
                AttackdMove();
                Effects();
            }

        }
    }

    /// <summary>攻撃時の向きを判定する関数</summary>
    void Direction()
    {
        if (_pushdKey == PushdKey.Target)
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

        if (_pushdKey == PushdKey.MoveX)
        {
            Vector3 muki;
            if (_crosshairController.transform.position.x - transform.position.x > 0)
            {
                muki = new Vector3(1, transform.localScale.y, transform.localScale.z);
                transform.localScale = muki;
            }
            else if (_crosshairController.transform.position.x - transform.position.x < 0)
            {
                muki = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                transform.localScale = muki;
            }
        }
        if (_pushdKey == PushdKey.NoMove)
        {
            Vector3 muki;
            if (_crosshairController.transform.position.x - transform.position.x > 0)
            {
                muki = new Vector3(1, transform.localScale.y, transform.localScale.z);
                transform.localScale = muki;
            }
            else if (_crosshairController.transform.position.x - transform.position.x < 0)
            {
                muki = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                transform.localScale = muki;
            }
        }

    }





    void AttackdMove()
    {

        _closeAttack = true;
        airTime = 0;
        Direction();
        if (_isGround == false && _pushdKey == PushdKey.UpMoveAttack)
        {
            _downSpeed = true;
        }

        if (_pushdKey == PushdKey.Target)
        {
            // _downSpeed = true;
            _targetCloseAttack.Attack();
            return;
        }



        if (_pushdKey == PushdKey.MoveX)
        {
            Vector3 velo = _crosshairController.transform.position - transform.position;
            _rb.AddForce(velo.normalized * _attackMovedPower, ForceMode.Impulse);
        }
        if (_pushdKey == PushdKey.NoMove)
        {
            float ve = _crosshairController.transform.position.x - transform.position.x;
            _rb.AddForce(ve * transform.right * _attackMovedPower, ForceMode.Impulse);

        }
        if (_pushdKey == PushdKey.RisingAttack)
        {
            if (_risingAttackCount < 3)
            {
                Vector3 velo = _crosshairController.transform.position - transform.position;
                Vector3 mousPos = new Vector3(velo.normalized.x, Mathf.Abs(velo.normalized.y), velo.normalized.z);
                _rb.AddForce(mousPos * _attackMovedPower, ForceMode.Impulse);
            }
            else
            {
                return;
            }

        }
        if (_pushdKey == PushdKey.UpMoveAttack)
        {
            _rb.AddForce(_crosshairController.gameObject.transform.position.x * transform.right * _attackMovedPower, ForceMode.Impulse);
        }
        //if (_pushdKey == PushdKey.UpAttack)
        //{
        //    _rb.velocity = Vector3.zero;
        //}
        if (_pushdKey == PushdKey.DownAttack)
        {
            // Vector3 mousPos = new Vector3(_crosshairController.transform.position.normalized.x, -1 * Mathf.Abs(_crosshairController.transform.position.normalized.y));
            _rb.AddForce(-1 * transform.up * _dawnAttackMovePower, ForceMode.Impulse);
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

        if (_pushdKey == PushdKey.DownAttack)        //降下アタック
        {
            Debug.Log("Down");
            //_isAttackContnue = false;
            //okAttack = false;
            return;
        }

        if (_pushdKey == PushdKey.RisingAttack)
        {
            //上昇攻撃
            if (_risingAttackCount == 1)
            {
                Debug.Log("raizing1");
            }
            else if (_risingAttackCount == 2)
            {
                Debug.Log("raizing2");
            }
            else if (_risingAttackCount == 3)
            {
                Debug.Log("raizing");
                airTime = 0;
                _downSpeed = false;
                return;
            }
            else
            {
                Debug.Log("NoneRaizing");
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


        //////////////////////////////地上での攻撃
        if (_attackXCount == 0)
        {
            _attackXCount++;
            _weaponAnim.Play("Zangeki1");                                       //鎌のアニメーション
            var effect = Instantiate(_zangekiEffects[0]);                       //エッフェクトを出す
            effect.transform.position = _zangekiEffectsPosition[0].position;
        }
        else if (_attackXCount == 1)
        {
            _attackXCount++;
            _weaponAnim.Play("Zangeki2");
            var effect = Instantiate(_zangekiEffects[1]);
            effect.transform.position = _zangekiEffectsPosition[1].position;
        }
        else if (_attackXCount == 2)
        {
            _attackXCount++;
            _weaponAnim.Play("Zangeki3");
            var effect = Instantiate(_zangekiEffects[2]);
            effect.transform.position = _zangekiEffectsPosition[2].position;

        }
        else if (_attackXCount == 3)
        {
            _attackXCount++;
            _weaponAnim.Play("Zangeki4");
            var effect = Instantiate(_zangekiEffects[3]);
            effect.transform.position = _zangekiEffectsPosition[3].position;
        }
        else if (_attackXCount == 4)
        {
            _weaponAnim.Play("Zangeki5");
            var effect = Instantiate(_zangekiEffects[4]);
            effect.transform.position = _zangekiEffectsPosition[4].position;
            _attackXCount = 0;
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
            _closeAttack = false;
            _downSpeed = false;
            airTime = 0;
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


            //if (_upMoveAttackCount > 0)        //空中攻撃をやめて地面についたら、攻撃コンボを中止
            //{
            //    Debug.Log("dalseup");
            //    _closeAttack = false;
            //    _isAttackContnue = false;
            //   // okAttack = false;     //攻撃を不可。クールタイムを数える
            //    airTime = 0;
            //}

            if (_isDownNow)         //降下攻撃のエフェクト
            {
                _weaponAnim.Play("DownAttack");
                _isAttackContnue = false;
                // okAttack = false;
                var effect = Instantiate(_downAttackEffect); //エフェクトを出す
                effect.transform.position = transform.position;
                _isDownNow = false;
            }
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
