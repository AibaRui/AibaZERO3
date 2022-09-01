using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCloseAttack : MonoBehaviour
{

    [Header("�N���X�w�A�[���Ǘ�����I�u�W�F�N�g")]
    [SerializeField] GameObject _crosshairController;
    [Header("�^�[�Q�b�g�V�X�e�����Ǘ�����I�u�W�F�N�g")]
    [SerializeField] TargetSystem targetSystem;
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


    /// <summary>�U�������񐔂��L��</summary>
    int _targetAttackCount = 0;
    /// <summary>�U���������̏ꏊ���L��</summary>
    Vector3 _nowPos;
    /// <summary>�G��b���܂ł̎��Ԃ��J�E���g����</summary>
    float _releaseEnemyCount = 0;


    /// <summary>�U���𒆒f�������ǂ������f</summary>
    bool _isTargetEnemyBefor;
    /// <summary>�U������true�B�G��b�����\�b�h���ĂԂ��ǂ����̔��f</summary>
    bool _isReleaceEnemy;
    /// <summary>�U���\�����ۂ̔��f</summary>
    bool _isOkTargetAttack = true;

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
    }


    public void Attack()
    {
        _rb = gameObject.GetComponent<Rigidbody>();

        float dir = Vector2.Distance(targetSystem._targetEnemy.transform.position, transform.position);
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
                        targetSystem._targetEnemy.transform.SetParent(_enemyBox.transform); //�G�����g�̎q�I�u�W�F�N�g�ɂ���
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
        Rigidbody _rbEnemy = targetSystem._targetEnemy.GetComponent<Rigidbody>();
        if (_targetAttackCount == 5)//�G�������Ă�����ɒe����΂�
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

    /// <summary>�G�������񂹂�^�[�Q�b�g�A�^�b�N(������)</summary>
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

        _attackCloseController._isRevarseTargetAttack = true;
        _targetAttackCount = 0;    //�^�[�Q�b�g�U���̒l�̃��Z�b�g
        _releaseEnemyCount = 0;
        _isReleaceEnemy = false;
        _isOkTargetAttack = false;    //�^�[�Q�b�g�U����s��
        _enemyBox.transform.DetachChildren();   //�G���q�I�u�W�F�N�g����O��


        Rigidbody _rbEnemy = targetSystem._targetEnemy.GetComponent<Rigidbody>(); //�G�������Ă�����ɒe����΂�
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


}
