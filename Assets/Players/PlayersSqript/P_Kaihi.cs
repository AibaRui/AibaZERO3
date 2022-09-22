using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Kaihi : MonoBehaviour
{
    [Header("����̃G�t�F�N�g")]
    [Tooltip("����̃G�t�F�N�g")] [SerializeField] GameObject _kaihiEffect;

    [Header("������鑬�x")]
    [Tooltip("������鑬�x")] [SerializeField] float _dodgeSpeed;

    [Header("�������")]
    [Tooltip("�������")] [SerializeField] float _endDistance = 5;

    [Header("�N�[���^�C���̎���")]
    [Tooltip("�N�[���^�C���̎���")] [SerializeField] float _coolTime = 6;
    [Tooltip("�N�[���^�C���𐔂���")] float _cooltimeCount = 0;

    [Header("���G����")]
    [Tooltip("���G����")] [SerializeField] float _mutekiTime = 2;

    [SerializeField] float _srowTime = 0.2f;   //�ϑ��x��

    int _judgeCount = 0;    //���������


    bool _isCount = false;�@�@�@�@//�N�[���^�C���̌v�Z
    bool _isDedge = false;       //��𔻒�
    bool _justDodge = false;  //�W���X�g��𔻒�
    public bool _isDodging = false; �@//�W���X�g��𔻒�
    public bool _isDodges = false; //���X�N���v�g�p


    Rigidbody _rb;
    Animator _anim;
    // SpriteRenderer _sp;

    PauseManager _pauseManager = default;
    JustKaihiManager _justKaihiManager;

    DodgeNowing dodgeNowing = DodgeNowing.FAZE;

    private void Awake()
    {
        _pauseManager = GameObject.FindObjectOfType<PauseManager>();
        _justKaihiManager = GameObject.FindObjectOfType<JustKaihiManager>();

    }
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        // _sp = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!_pauseManager._isPause)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))//�P
            {
                _justKaihiManager.JustKaihiResume();

                Debug.Log("Now");
                _isDodges = true;//�uPlayer_InQest�v�ɂĈړ��̐���


                if (dodgeNowing == DodgeNowing.FAZE)          //��𒆂łȂ�
                {
                    Debug.Log("Go");
                    _isDodging = true;
                    _isDedge = true;     //��������̎���



                    if (transform.localScale.x == 1)
                    {
                        _kaihiEffect.transform.localScale = new Vector3(-1, 1, 1);
                    }
                    else
                    {
                        _kaihiEffect.transform.localScale = new Vector3(1, 1, 1);
                    }
                    var _effectInstance = Instantiate(_kaihiEffect);
                    _effectInstance.transform.position = transform.position;
                    _effectInstance.transform.SetParent(transform);
                }
            }

            if (_isCount)
            {
                Count();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_pauseManager)
        {
            if (_isDedge)
            {
                if (_justDodge)
                {
                    _isCount = true;        //�N�[���^�C���𐔂��郁�\�b�h
                    _justDodge = false;
                    /* JustDodge();  */          //�W���X�g���
                }
                else
                {
                    Dodge();                //���
                }
            }
        }
    }



    //IEnumerator JustDodge()
    //{
    //    Time.timeScale = 0.5f;
    //    _mousePos = FindObjectOfType<CrosshairController>().transform.position;               //�}�E�X�̃|�W�V�������l��
    //    Debug.Log("JJJJJJUstKaihiZIkkou");

    //    _startPos = transform.position;
    //    Vector3 _pos = new Vector3(_mousePos.x, _mousePos.y, gameObject.transform.position.z);
    //    _rb.AddForce(_pos.normalized * _dodgeSpeed, ForceMode.Impulse);
    //    yield return new WaitForSeconds(_srowTime);
    //    Time.timeScale = 1;

    //    _isCount = true;
    //    _endJudg = true;
    //}


    ///////////////////////////////////////////////////////////////////////////////////22222222222/////////////
    void Dodge()
    {
        float _h = Input.GetAxisRaw("Horizontal");
        Vector2 ve;

        if (_judgeCount == 0 && _h == 0)
        {
            ve = new Vector3(transform.localScale.x * -1, 0);
            _rb.AddForce(ve * _dodgeSpeed, ForceMode.Impulse);
        }
        else if (_judgeCount == 0 && _h != 0)
        {
            ve = new Vector2(_h, 0);
            _rb.AddForce(ve * _dodgeSpeed, ForceMode.Impulse);
        }

        _isDodging = true;
        Debug.Log("KAIHI");
        _judgeCount++;

        _isCount = true;        //�N�[���^�C���𐔂��郁�\�b�h
        _isDedge = false;     //��������̎���
        dodgeNowing = DodgeNowing.NOW;
    }


    /// /////////////////////////////////////////////////////////////////////////333333333333333333333//////////////////////////////////////////////

    void Count()         //�N�[���^�C���𐔂���
    {
        _cooltimeCount += Time.deltaTime;
        if (_cooltimeCount > _mutekiTime)
        {
            _isDodges = false;
        }

        if (_cooltimeCount > _coolTime)
        {
            _isDodging = false;
            _cooltimeCount = 0;
            _isCount = false;
            _judgeCount = 0;
            dodgeNowing = DodgeNowing.FAZE;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "")             //�W���X�g���
        {

        }


    }




    enum DodgeNowing
    {
        FAZE,
        NOW,

    }

    void OnEnable()
    {
        // �Ă�ŗ~�������\�b�h��o�^����B
        _justKaihiManager.OnJustKaihiResume += PauseResumeJustKaihi;
        // _anim = gameObject.GetComponent<Animator>();
    }

    void OnDisable()
    {
        // OnDisable �ł̓��\�b�h�̓o�^���������邱�ƁB�����Ȃ��ƃI�u�W�F�N�g�������ɂ��ꂽ��j�����ꂽ�肵����ɃG���[�ɂȂ��Ă��܂��B
        _justKaihiManager.OnJustKaihiResume -= PauseResumeJustKaihi;
    }

    void PauseResumeJustKaihi(bool isPause)
    {
        if (isPause)
        {
            PauseJustKaihi();
        }
        else
        {
            ResumeJustKaihi();
        }
    }

    public void PauseJustKaihi()
    {

    }

    public void ResumeJustKaihi()
    {

    }
}
