using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BossControl : MonoBehaviour
{

    [SerializeField] string _sceneName = "";
    [SerializeField] GameObject _fadePanel;
    [SerializeField] GameObject _box;

    [SerializeField] AudioClip[] _auClip = new AudioClip[2];
    AudioSource _aud;

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
    bool _isAttackNow = false;
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
        _aud = gameObject.GetComponent<AudioSource>();
    }





    void Update()
    {
        if (_player)
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

        if (!_player)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void MainRoutine()
    {

        if (_isAttackNow || !_isAttack)
        {
            return;
        }
        _box.transform.DetachChildren();
        var num = Random.Range(0, 3);

        if (num == 0)
        {
            Debug.Log("FFFF");
            StartCoroutine(FireWall());
        }
        else if (num == 1)
        {
            Debug.Log("EEEXXX");
            StartCoroutine(Exprosion());
        }
        else if (num == 2)
        {
            Debug.Log("VVVV");
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
        Debug.Log("FFFF");
        for (int i = 0; i < 5; i++)
        {
            var go = Instantiate(_fireWall);
            go.transform.position = _player.transform.position;
            go.transform.SetParent(_box.transform);
            yield return new WaitForSeconds(3);
        }



        _endAttack = true;
        _isAttackNow = false;
    }

    IEnumerator Exprosion()
    {

        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i);
            var num = Random.Range(0, 3);
            var go = Instantiate(_exprosion);
            go.transform.SetParent(_box.transform);
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
            var go = Instantiate(_fireBall);
            go.transform.position = _posFireBall[i].position;
            go.transform.SetParent(_box.transform);
            Rigidbody _rb = go.GetComponent<Rigidbody>();
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
            _aud.PlayOneShot(_auClip[1]);
            _hp--;
            if (_hp <= 0)
            {
                StartCoroutine(Lode());
            }

        }
    }

    IEnumerator Lode()
    {
        _fadePanel.SetActive(true);
        _box.transform.DetachChildren();
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(_sceneName);
    }

}