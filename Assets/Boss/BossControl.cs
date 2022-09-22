using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    [Header("HP")]
    [Tooltip("HP")] [SerializeField] int _hp;

    [Header("�N�[���^�C���̍ŏ��^�C��")]
    [Tooltip("�N�[���^�C���̍ŏ��^�C��")] [SerializeField] float _minTime = 10;

    [Header("�N�[���^�C���̍ő�^�C��")]
    [Tooltip("�N�[���^�C���̍ő�^�C��")] [SerializeField] float _maxTime = 15;




    [Header("�Β��̍U���G�t�F�N�g")]
    [Tooltip("�Β��̍U���G�t�F�N�g")] [SerializeField] GameObject _fireWall;

    [Header("�����̍U���G�t�F�N�g")]
    [Tooltip("�����̍U���G�t�F�N�g")] [SerializeField] GameObject _exprosion;

    [Header("�΋��̃G�t�F�N�g")]
    [Tooltip("�΋��G�t�F�N�g")] [SerializeField] GameObject _fireBall;

    [Header("�΋��̏o���ꏊ")]
    [Tooltip("�΋��̏o���ꏊ")] [SerializeField] Transform[] _posFireBall = new Transform[3];


    [Header("�{�X�̈ړ��ꏊ")]
    [Tooltip("�{�X�̈ړ��ꏊ")] [SerializeField] Transform[] _pos = new Transform[2];

    /// <summary>�U�������ǂ���</summary>
    bool _isAttackNow = true;
    /// <summary>�U���\���ǂ���</summary>
    bool _isAttack = true;

    /// <summary>�U�����I���������ǂ����̔��f�B�A�j���[�V�����ŌĂ�</summary>
    bool _endAttack = false;

    /// <summary>�C�x���g�V�[�������ǂ���</summary>
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
            StartCoroutine(FireWall());
        }
        else if (num == 1)
        {
            StartCoroutine(Exprosion());
        }
        else if (num == 2)
        {
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

    /// <summary>�Β�</summary>
    IEnumerator FireWall()
    {
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
            var num = Random.Range(0, 3);
            var go = Instantiate(_fireWall);
            if (num == 0)//�v���C���[�̉E��
            {
                Vector3 ve = new Vector3(_player.transform.position.x + 2, _player.transform.position.y, -3);
                go.transform.position = ve;
            }
            else if (num == 1)//�v���C���[�̍���
            {
                Vector3 ve = new Vector3(_player.transform.position.x - 2, _player.transform.position.y, -3);
                go.transform.position = ve;
            }
            else if (num == 2)//�v���C���[�̏ꏊ
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
            var go = Instantiate(_fireWall);
            go.transform.position = _posFireBall[i].position;
            Rigidbody _rb = go.gameObject.GetComponent<Rigidbody>();
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
