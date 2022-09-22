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

    [Header("�K�v�ȃI�u�W�F�N�g")]
    ///<summary>�N���X�w�A�[�̃X�N���v�g</summary>
    [SerializeField] GameObject _crosshairController;
    /// <summary>�^�[�Q�b�g�@�\�̃X�N���v�g</summary>
    [SerializeField] TargetSystem targetSystem;
    /// <summary>�U�����ɓG�����Ă�����̃I�u�W�F�N�g</summary>
    [SerializeField] GameObject _enemyBox;
    /// <summary>�^�[�Q�b�g�A�^�b�N�̃X�N���v�g</summary>
    [SerializeField] TargetCloseAttack _targetCloseAttack;
    /// <summary>�U������p�̃I�u�W�F�N�g</summary>
    [SerializeField] GameObject _attackCollider;




    float _countResetTime = 0;
    float _countResetLimit = 0;

    [Header("�G�t�F�N�g�ƃ|�W�V����")]
    [SerializeField] GameObject[] _zangekiEffects = new GameObject[4];
    [SerializeField] Transform[] _zangekiEffectsPosition = new Transform[4];
    [SerializeField] GameObject _downAttackEffect;

    [Header("����̃A�j���[�V����")]
    [SerializeField] Transform _zangekiEffectPosition;
    [SerializeField] Animator _weaponAnim;


    [Header("�U�����̈ړ��̑���")]
    [Tooltip("�U�����̈ړ��X�s�[�h")] [SerializeField] float _attackMovedPower = 5;
    [Tooltip("��A�U�����̈ړ��X�s�[�h")] [SerializeField] float _dawnAttackMovePower = 20;

    [Header("�U�����̈ړ��̋���")]
    [Tooltip("���A�U�����̈ړ�����")] [SerializeField] float _movedDistanceX = 2;
    [Tooltip("���A�U�����̈ړ�����")] [SerializeField] float _movedTargetDistanceX = 5;
    [Tooltip("��A�U�����̈ړ�����")] [SerializeField] float _movedDistanceUp = 0.5f;
    [Tooltip("�󒆉��A�U�����̈ړ�����")] [SerializeField] float _movedDistanceUpX = 0.5f;


    [Header("�U�����[�g�̐ݒ�")]
    [Tooltip("�U���̃��[�g")] [SerializeField] float _attackLate = 0.3f;
    [Tooltip("�N�[���^�C��")] [SerializeField] float _attackCoolTimeLimit = 3;
    [Tooltip("�R���{�̎�������")] [SerializeField] float _attackContenueTimeLimit = 2f;
    [Tooltip("���U���̍d������")] [SerializeField] float _attackDownLate = 1f;
    /// <summary>�N�[���^�C���𐔂���</summary>
    private float _attackCoolTimeCount = 0;
    /// <summary>�R���{�������Ԃ𐔂���</summary>
    private float _attackContenueTimeCount = 0f;


    /// <summary>�U���̎�ނ𔻒�</summary>
    public PushdKey _pushdKey = PushdKey.NoMove;

    /// <summary>�U���J�n���̏ꏊ</summary>
    public Vector3 _nowPos;

    ///////////////////////////////////////////////�U����////////////////

    public int _attackCount = 0;
    /// <summary>���A�U���̃J�E���g</summary>
    int _attackXCount = 0;
    /// <summary>�㏸�A�U���̃J�E���g</summary>
    int _risingAttackCount = 0;
    /// <summary>�󒆉��A�U���̃J�E���g</summary>
    int _upMoveAttackCount = 0;
    /// <summary>�󒆁A�U���̃J�E���g</summary>
    int _upAttackCount = 0;

    ///////////////////////////////////////////////�U���̔��f////////////////

    /// <summary>�U�����ł��邩�ǂ��� </summary>
    public bool _isAttackNow = false;

    public bool _closeAttack;
    bool _judgeMoved = false;


    /// <summary>��U�����͂̔��� </summary>
    bool _isUpAttack = false;
    /// <summary>���U�����͂̔��� </summary>
    bool _isDownAttack = false;


    bool _isRisingAttack = false;

    /// <summary>�U���\���ǂ����̔���</summary>
    public bool okAttack = false;
    /// <summary>�U���R���{���������ǂ����̔���</summary>
    bool _isAttackContnue = false;


    public bool _isRevarseTargetAttack;

    bool _isMove = false;

    /// <summary>�uDownEffectControl�v�Ŏg�p</summary>
    public bool _isDownAttackEffect = false;
    // [SerializeField] GameObject _attackCollider;

    ////////�󒆂̂��ꂼ��̈ړ�����//////////////////////////////////////////////
    public float airTime = 0;                      //�U�����̋󒆑؍ݎ���
    [SerializeField] float airTimeLimit = 2;        //�U�����̋󒆑؍ݎ���                                     
    [SerializeField] float _xSpeed = 0.5f;
    [SerializeField] float _ySpeed = 0f;
    [SerializeField] float _zSpeed = 0.5f;

    public bool _isGround = false;

    /// <summary>�U���̍ہA�󒆂ŗ��܂邩�ǂ����̔���Btrue�ŗ��܂邽�߂̊֐��yAirtime���Ăԁz</summary>
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
            ///////////////////////�U������ꏊ�̈ړ�/////////
            float _h = Input.GetAxisRaw("Horizontal");



            if (_attackCount == _attackZanryou)
            {
                okAttack = false;
            }

            if (okAttack)   // �U���\��������U��
            {
                if (_isAttackNow == false)
                {
                    Attack();
                }
            }
            else   //�U���s�\��������N�[���^�C���𐔂���
            {
                CountCoolTime();
            }

            ResetCount();



            ////�R���{�������Ԃ𐔂���
            //if (_isAttackContnue)
            //{
            //   // CountContenueTime();
            //}

            //�󒆍U�����̈ړ�����
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


    /// <summary>�U���̍d������</summary>
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

    /// <summary>�N�[���^�C���̌v��</summary>
    void CountCoolTime()
    {
        _attackCoolTimeCount += Time.deltaTime;
        if (_attackCoolTimeCount > _attackCoolTimeLimit)
        {
            okAttack = true;    //�U���ł���悤�Ȃ�

            _attackCoolTimeCount = 0;     //�N�[���^�C���̃��Z�b�g

            _attackCount = 0;           //�U���񐔂�0��
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

            Debug.Log("�L�[:" + _isUpAttack);

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


            if ((v > 0) && !_isRisingAttack)            //�㏸�U��
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


            if (v < 0)                                //���U��
            {
                _closeAttack = true;
                _pushdKey = PushdKey.DownAttack;
                _downAttack.Attack();
                return;
            }



            if (!_isGround && h != 0)       //�󒆈ړ��U��
            {
                _closeAttack = true;
                _pushdKey = PushdKey.UpMoveAttack;
                _nomalAttack.UpMoveAttack(h);
                _nomalAttack.UpAttackEffect();
                return;
            }
            if (h != 0)            //���ړ��U��
            {
                airTime = 0;
                _closeAttack = true;
                _pushdKey = PushdKey.MoveX;
                _nomalAttack.MoveAttack();
                _nomalAttack.MoveAttackEffeck();
                return;
            }

            if (!_isGround)�@//�󒆍U��
            {
                Debug.Log("aaa");
                _closeAttack = true;
                _pushdKey = PushdKey.UpAttack;
                _nomalAttack.UpAttack();
                _nomalAttack.UpMoveAttackEffect();
            }



            if (h == 0)  //���̏�U��
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
            okAttack = false;     //�U����s�B�N�[���^�C���𐔂���     
            airTime = 0;
            _downSpeed = false;
            return;
        }


        //�P���A�󒆔͈͍U�� 
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
            //////�󒆉��A�U���i�A���j
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


    void AirTime()       ///�󒆑؍݂̂̎��Ԍv�Z
    {
        //�󒆍U�����̈ړ�����
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
            //if (_upMoveAttackCount > 0)        //�󒆍U������߂Ēn�ʂɂ�����A�U���R���{�𒆎~
            //{
            //    Debug.Log("dalseup");
            //    _closeAttack = false;
            //    _isAttackContnue = false;
            //   // okAttack = false;     //�U����s�B�N�[���^�C���𐔂���
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
        /// <summary>�㏸�U��</summary>
        RisingAttack,
        /// <summary>�󒆈ړ��U��</summary>
        UpMoveAttack,
        /// <summary>�󒆍U��</summary>
        UpAttack,
        /// <summary>���U��</summary>
        DownAttackOnGround,
        /// <summary>�󒆂���̉��U��</summary>
        DownAttack,
        /// <summary>�^�[�Q�b�g�U��</summary>
        Target,

    }



}
