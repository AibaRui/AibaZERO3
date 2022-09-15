using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCloseAttack : MonoBehaviour
{

    [Header("�N���X�w�A�[���Ǘ�����I�u�W�F�N�g")]
    [SerializeField] GameObject _crosshairController;
    [Header("�^�[�Q�b�g�V�X�e�����Ǘ�����I�u�W�F�N�g")]
    [SerializeField] TargetSystem _targetSystem;
    [Header("�G���i�[�����̃I�u�W�F�N�g")]
    [SerializeField] GameObject _enemyBox;
    [Header("�v���C���[���g�̋ߐڍU���̃X�N���v�g")]
    [SerializeField] AttackCloseController _attackCloseController;

    [Header("�G�������񂹂����ꏊ")]
    [SerializeField] Transform _attractPos;


    [Header("���x�ݒ�")]
    [Tooltip("�G�������񂹂鑬�x")] [SerializeField] float _attractPower = 3;
    [Tooltip("�U�����Ɉړ����鑬�x")] [SerializeField] float _attackMovedPower = 10;

    [Header("���Ԑݒ�")]
    [Tooltip("�^�[�Q�b�g�U���𒆒f�������̃N�[���^�C��")] [SerializeField] float _targetEnemyBeforCoolTime = 2;
    [Tooltip("�^�[�Q�b�g�U�����Ō�܂ł����������̃N�[���^�C��")] [SerializeField] float _targetEnemyAfterCoolTime = 4;
    [Tooltip("�����񂹂��G�𗣂��܂ł̎���")] [SerializeField] float _releaseEnemyCountLimit = 2;


    [SerializeField] float _moveDistance = 2;

    

    /// <summary>�U�������񐔂��L��</summary>
    int _targetAttackCount = 0;
    /// <summary>�U���������̏ꏊ���L��</summary>
    Vector2 _nowPos;
    /// <summary>�G��b���܂ł̎��Ԃ��J�E���g����</summary>
    float _releaseEnemyCount = 0;


    /// <summary>�U���𒆒f�������ǂ������f</summary>
    bool _isTargetEnemyBefor;
    /// <summary>�U������true�B�G��b�����\�b�h���ĂԂ��ǂ����̔��f</summary>
    bool _isReleaceEnemy;
    /// <summary>�U���\�����ۂ̔��f</summary>
    bool _isOkTargetAttack = true;

    bool _endJudge = false;

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

            if (_isRevarseTargetAttack) //�G�𗣂��Ƃ�
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
        _endJudge = true;

        _nowPos = transform.position;
        Direction();
        _rb = gameObject.GetComponent<Rigidbody>();

        float dir = Vector2.Distance(_targetSystem._targetEnemy.transform.position, transform.position);
        _targetAttackCount++;

        Vector2 hani = _crosshairController.transform.position - transform.position;

        if (_isOkTargetAttack)
        {
            _attackCloseController.airTime = 0;
            if (dir <= 2)      //�^�[�Q�b�g���߂���
            {
                //���ꂼ��̌����Ă�p�x��-45~45�x�܂ł͈̔͂ɓ����Ă��邩�B
                if (transform.localScale.x == 1 && hani.normalized.x >= 0.3 && (hani.normalized.y <= 0.9 && hani.normalized.y >= -0.9f)
                || (transform.localScale.x == -1 && hani.normalized.x <= -0.3 && (hani.normalized.y <= 0.9 && hani.normalized.y >= -0.9f)))
                {
                    if (_targetAttackCount == 5)
                    {
                        _isOkTargetAttack = false;    //�^�[�Q�b�g�U����s��
                        _enemyBox.transform.DetachChildren();   //�G���q�I�u�W�F�N�g����O��

                        StartCoroutine(TargetAttackNear());
                        StartCoroutine(TargetEnemyCoolTime());  //�N�[���^�C���𐔂���

                        _isReleaceEnemy = false;

                        _attackCloseController._downSpeed = false;
                        _targetAttackCount = 0;
                        _releaseEnemyCount = 0;
                    }
                    else
                    {     
                        _attackCloseController._downSpeed = true;
                        _targetSystem._targetEnemy.transform.SetParent(_enemyBox.transform); //�G�����g�̎q�I�u�W�F�N�g�ɂ���
                        StartCoroutine(TargetAttackNear());

                        _rb.AddForce(hani.normalized * _attackMovedPower, ForceMode.Impulse);

                        _releaseEnemyCount = 0;
                        _isReleaceEnemy = true;
                    }
                }
                else                        ///////�^�[�Q�b�g�U���𒆎~�A���̕����̍U���Ɉڂ�////////
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

    /// <summary>���ʂɐ؂�^�[�Q�b�g�A�^�b�N(�߂��Ƃ�)</summary>
    public IEnumerator TargetAttackNear()
    {     
        Rigidbody _rbEnemy = _targetSystem._targetEnemy.GetComponent<Rigidbody>();
        if (_targetAttackCount == 5)//�G�������Ă�����ɒe����΂�
        {
            _rbEnemy.isKinematic = false;
            Vector2 ve = new Vector2(transform.localScale.x, 1);
            _rbEnemy.AddForce(ve * 3, ForceMode.Impulse);
            _targetSystem._targetEnemy = null;
        }
        else
        {  

            //_rbEnemy.velocity = Vector3.zero;
            _rbEnemy.isKinematic = true;

            yield return new WaitForSeconds(0.2f);

            //if (_attackCloseController._isGround == true)
            //{
                _attackCloseController._isAttackNow = false;
                //_attackCloseController._closeAttack = false;
            //}
        }
    }

    /// <summary>�G�������񂹂�^�[�Q�b�g�A�^�b�N(������)</summary>
    public IEnumerator TargetAttackFar()
    {
        Rigidbody _rbEnemy = _targetSystem._targetEnemy.GetComponent<Rigidbody>();
        Vector3 velo = _attractPos.position - _targetSystem._targetEnemy.transform.position;
        _rbEnemy.AddForce(velo.normalized * _attractPower, ForceMode.Impulse);

        yield return new WaitForSeconds(0.2f);

        _targetSystem._targetEnemy.transform.position = _attractPos.position;
        _rbEnemy.velocity = Vector3.zero;
        _rbEnemy.isKinematic = true;

        _attackCloseController._isAttackNow = false;

    }

    IEnumerator RevarseTargetAttack()
    {
        Vector2 hani = _crosshairController.transform.position - transform.position;

        _isRevarseTargetAttack = true;
        _targetAttackCount = 0;    //�^�[�Q�b�g�U���̒l�̃��Z�b�g
        _releaseEnemyCount = 0;
        _isReleaceEnemy = false;
        _isOkTargetAttack = false;    //�^�[�Q�b�g�U����s��
        _enemyBox.transform.DetachChildren();   //�G���q�I�u�W�F�N�g����O��


        Rigidbody _rbEnemy = _targetSystem._targetEnemy.GetComponent<Rigidbody>(); //�G�������Ă�����ɒe����΂�
        _rbEnemy.isKinematic = false;
        Vector2 ve = new Vector2(transform.localScale.x, 1);
        _rbEnemy.AddForce(ve * 3, ForceMode.Impulse);
        _targetSystem._targetEnemy = null;

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
        //_nowPos = transform.position;
        _rb.AddForce(hani.normalized * _attackMovedPower, ForceMode.Impulse);

 
        _attackCloseController._isAttackNow = false;
        
    }





    public IEnumerator TargetEnemyCoolTime()
    {
        if (_isTargetEnemyBefor)
        {
            yield return new WaitForSeconds(_targetEnemyBeforCoolTime);
        }
        else { yield return new WaitForSeconds(_targetEnemyAfterCoolTime); }

        Debug.Log("COOOl");
        _isOkTargetAttack = true;


        _targetAttackCount = 0;
    }

    public IEnumerator TestMoveEnd()
    {
        if (_attackCloseController._attackCount == 5)//_attackCloseController._isGround || 
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

            Rigidbody _rbEnemy = _targetSystem._targetEnemy.GetComponent<Rigidbody>();
            _rbEnemy.isKinematic = false;

            Vector2 ve = new Vector2(transform.localScale.x, 1);
            _rbEnemy.AddForce(ve * 3, ForceMode.Impulse);
            _targetSystem._targetEnemy = null;


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
