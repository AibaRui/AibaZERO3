using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAttack : MonoBehaviour
{
    [Header("�U�����̈ړ��X�s�[�h")]
    [Tooltip("�U�����̈ړ��X�s�[�h")] [SerializeField] float _attackSpeed = 3;

    [Header("�󒆖��ړ��U�����̈ړ��X�s�[�h")]
    [Tooltip("�󒆖��ړ��U�����̈ړ��X�s�[�h")] [SerializeField] float _attackUpSpeedNoMoe = 3;

    [Header("�󒆈ړ��U�����̈ړ��X�s�[�h")]
    [Tooltip("�󒆈ړ��U�����̈ړ��X�s�[�h")] [SerializeField] float _attackUpSpeedMove = 10;

    [Header("���ړ��̋���")]
    [Tooltip("���ړ��̋���")] [SerializeField] float _moveDis = 3;

    [Header("�󒆉��ړ��̋���")]
    [Tooltip("�󒆉��ړ��̋���")] [SerializeField] float _upMoveDis = 4;



    [SerializeField] float _timeMove = 0.3f;
    [SerializeField] float _timeNoMove = 0.5f;
    [SerializeField] float _timeUpNoMove = 0.5f;
    [SerializeField] float _timeUpMove = 2f;

    [Header("�G�t�F�N�g�ƃ|�W�V����")]
    [SerializeField] GameObject[] _zangekiEffects = new GameObject[4];
    [SerializeField] Transform[] _zangekiEffectsPosition = new Transform[4];
    [SerializeField] GameObject _downAttackEffect;

    [Header("����̃A�j���[�V����")]
    [SerializeField] Transform _zangekiEffectPosition;
    [SerializeField] Animator _weaponAnim;

    [Header("�K�v�ȃI�u�W�F�N�g")]
    ///<summary>�N���X�w�A�[�̃X�N���v�g</summary>
    [SerializeField] GameObject _crosshairController;
    [SerializeField] AttackCloseController _attackCloseController;
    [SerializeField] PlayerInBattle _playerInBattle;



    PushdKey _pushdKey = PushdKey.NoMove;

    int _upAttackCount = 0;
    int _noMoveAttackCount = 0;

    /// <summary>�󒆍U���̃R���{�p�����m�F����</summary>
    bool _countUpAttackChain = false;
    /// <summary>�󒆍U���̃R���{�p�����Ԃ��J�E���g����</summary>
    float _countUpAttackChainCount = 0;
    /// <summary>�󒆍U���̃R���{�p���\���Ԃ��J�E���g����</summary>
    [SerializeField] float _countUpAttackChainCountLimit = 2;

    Animator _anim;
    Rigidbody _rb;
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _anim = gameObject.GetComponent<Animator>();
        _weaponAnim = _weaponAnim.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovedEnd();

        CountChain();
    }
    /// <summary>�U���̍d������</summary>
    IEnumerator ReleaseAttackStiffenss()
    {
        Debug.Log("a333a");

        if (_pushdKey == PushdKey.MoveX)
        {
            yield return new WaitForSeconds(_timeMove);

        }
        else if (_pushdKey == PushdKey.NoMove)
        {
            yield return new WaitForSeconds(_timeNoMove);
        }
        else if (_pushdKey == PushdKey.UpAttack)
        {
            yield return new WaitForSeconds(_timeUpNoMove);
        }
        else if (_pushdKey == PushdKey.UpMoveAttack)
        {
            yield return new WaitForSeconds(_timeUpMove);
        }

        _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;
        _attackCloseController._closeAttack = false;
        _attackCloseController._isAttackNow = false;
        _attackCloseController._downSpeed = false;

        _anim.SetBool("isAttack", false);

        //if (_enemyBox.transform.childCount != 0)
        //{
        //    //_enemyBox.transform.DetachChildren();
        //}
    }



    void MovedEnd()
    {
        if (_attackCloseController._closeAttack)
        {
            float distance = Vector3.Distance(_attackCloseController._nowPos, transform.position);

            if (_pushdKey == PushdKey.UpAttack && _attackCloseController._pushdKey == AttackCloseController.PushdKey.UpAttack)
            {
                _attackCloseController._closeAttack = false;
                _rb.velocity = Vector3.zero;
                _countUpAttackChain = true;
                _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;
            }


            if (_pushdKey == PushdKey.UpMoveAttack && _attackCloseController._pushdKey == AttackCloseController.PushdKey.UpMoveAttack)
            {
                if (distance > _upMoveDis)
                {
                    _attackCloseController._closeAttack = false;
                    _rb.velocity = Vector3.zero;
                    _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;
                }
            }

            if (_pushdKey == PushdKey.MoveX && _attackCloseController._pushdKey == AttackCloseController.PushdKey.MoveX)
            {
                if (distance > 4)
                {
                    _attackCloseController._closeAttack = false;
                    Debug.Log("ffx");
                    _rb.velocity = Vector3.zero;
                    _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;
                }
            }

            if (_pushdKey == PushdKey.NoMove && _attackCloseController._pushdKey == AttackCloseController.PushdKey.NoMove)
            {
                if (distance > _moveDis)
                {
                    _rb.velocity = Vector3.zero;
                    _attackCloseController._closeAttack = false;
                    _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Nomal;
                }
            }
        }
    }


    public void MoveAttack()
    {
        _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Attack;
        _pushdKey = PushdKey.MoveX;
        StartCoroutine(ReleaseAttackStiffenss());
        _attackCloseController._downSpeed = false;
        _attackCloseController.airTime = 0;


    }

    public void MoveAttackEffeck()
    {
        var effect = Instantiate(_zangekiEffects[0]);                       //�G�b�t�F�N�g���o��
        effect.transform.position = _zangekiEffectsPosition[0].position;
    }

    public void UpAttack()
    {
        _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Attack;
        _rb.velocity = Vector3.zero;
        _attackCloseController._downSpeed = true;
        _pushdKey = PushdKey.UpAttack;
        StartCoroutine(ReleaseAttackStiffenss());

    }
    public void UpAttackEffect()
    {
        _upAttackCount++;
        _countUpAttackChainCount = 0;
        _countUpAttackChain = false;

    }
    public void UpMoveAttack(float h)
    {
        _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Attack;
        _pushdKey = PushdKey.UpMoveAttack;
        StartCoroutine(ReleaseAttackStiffenss());
        Debug.Log("XXX");
        if (h > 0)
        {
            _rb.AddForce(1 * transform.right * _attackUpSpeedMove, ForceMode.Impulse);
        }
        else
        {
            _rb.AddForce(-1 * transform.right * _attackUpSpeedMove, ForceMode.Impulse);
        }



    }

    public void UpMoveAttackEffect()
    {


    }





    public void NoMoveAttack()
    {
        _playerInBattle._playerAction = PlayerInBattle.PlayerAction.Attack;
        _pushdKey = PushdKey.NoMove;
        StartCoroutine(ReleaseAttackStiffenss());
    }


    public void NoMoveAttackEffeck()
    {
        _anim.SetBool("isAttack", true);
        //////////////////////////////�n��ł̍U��
        if (_noMoveAttackCount == 0)
        {
            _anim.Play("P_Attack1");
            _noMoveAttackCount++;
            _weaponAnim.Play("Zangeki1");                                       //���̃A�j���[�V����
            var effect = Instantiate(_zangekiEffects[0]);                       //�G�b�t�F�N�g���o��
            effect.transform.position = _zangekiEffectsPosition[0].position;
        }
        else if (_noMoveAttackCount == 1)
        {
            _anim.Play("P_Attack2");
            _noMoveAttackCount++;
            _weaponAnim.Play("Zangeki2");
            var effect = Instantiate(_zangekiEffects[1]);
            effect.transform.position = _zangekiEffectsPosition[1].position;
        }
        else if (_noMoveAttackCount == 2)
        {
            _anim.Play("P_Attack3");
            _noMoveAttackCount++;
            _weaponAnim.Play("Zangeki3");
            var effect = Instantiate(_zangekiEffects[2]);
            effect.transform.position = _zangekiEffectsPosition[2].position;

        }
        else if (_noMoveAttackCount == 3)
        {
            _anim.Play("P_Attack4");
            _noMoveAttackCount++;
            _weaponAnim.Play("Zangeki4");
            var effect = Instantiate(_zangekiEffects[3]);
            effect.transform.position = _zangekiEffectsPosition[3].position;
        }
        else if (_noMoveAttackCount == 4)
        {
            _anim.Play("P_Attack5");
            _weaponAnim.Play("Zangeki5");
            var effect = Instantiate(_zangekiEffects[4]);
            effect.transform.position = _zangekiEffectsPosition[4].position;
            _noMoveAttackCount = 0;
        }
    }

    void Dir()
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

    public enum PushdKey
    {
        NoMove,
        MoveX,
        /// <summary>�󒆈ړ��U��</summary>
        UpMoveAttack,
        /// <summary>�󒆍U��</summary>
        UpAttack,


    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _upAttackCount = 0;
        }
    }

    void CountChain()
    {
        if (_countUpAttackChain)
        {
            _countUpAttackChainCount += Time.deltaTime;
            if (_countUpAttackChainCount >= _countUpAttackChainCountLimit)
            {
                _upAttackCount = 0;
                _attackCloseController._downSpeed = false;
            }
        }
    }


}
