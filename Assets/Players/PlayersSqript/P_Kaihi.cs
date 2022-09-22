using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Kaihi : MonoBehaviour
{
    [Header("回避のエフェクト")]
    [Tooltip("回避のエフェクト")] [SerializeField] GameObject _kaihiEffect;

    [Header("回避する速度")]
    [Tooltip("回避する速度")] [SerializeField] float _dodgeSpeed;

    [Header("回避距離")]
    [Tooltip("回避距離")] [SerializeField] float _endDistance = 5;

    [Header("クールタイムの時間")]
    [Tooltip("クールタイムの時間")] [SerializeField] float _coolTime = 6;
    [Tooltip("クールタイムを数える")] float _cooltimeCount = 0;

    [Header("無敵時間")]
    [Tooltip("無敵時間")] [SerializeField] float _mutekiTime = 2;

    [SerializeField] float _srowTime = 0.2f;   //変速度化

    int _judgeCount = 0;    //回避した回数


    bool _isCount = false;　　　　//クールタイムの計算
    bool _isDedge = false;       //回避判定
    bool _justDodge = false;  //ジャスト回避判定
    public bool _isDodging = false; 　//ジャスト回避判定
    public bool _isDodges = false; //他スクリプト用


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
            if (Input.GetKeyDown(KeyCode.LeftShift))//１
            {
                _justKaihiManager.JustKaihiResume();

                Debug.Log("Now");
                _isDodges = true;//「Player_InQest」にて移動の制限


                if (dodgeNowing == DodgeNowing.FAZE)          //回避中でない
                {
                    Debug.Log("Go");
                    _isDodging = true;
                    _isDedge = true;     //回避処理の実装



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
                    _isCount = true;        //クールタイムを数えるメソッド
                    _justDodge = false;
                    /* JustDodge();  */          //ジャスト回避
                }
                else
                {
                    Dodge();                //回避
                }
            }
        }
    }



    //IEnumerator JustDodge()
    //{
    //    Time.timeScale = 0.5f;
    //    _mousePos = FindObjectOfType<CrosshairController>().transform.position;               //マウスのポジションを獲得
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

        _isCount = true;        //クールタイムを数えるメソッド
        _isDedge = false;     //回避処理の実装
        dodgeNowing = DodgeNowing.NOW;
    }


    /// /////////////////////////////////////////////////////////////////////////333333333333333333333//////////////////////////////////////////////

    void Count()         //クールタイムを数える
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
        if (other.gameObject.tag == "")             //ジャスト回避
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
        // 呼んで欲しいメソッドを登録する。
        _justKaihiManager.OnJustKaihiResume += PauseResumeJustKaihi;
        // _anim = gameObject.GetComponent<Animator>();
    }

    void OnDisable()
    {
        // OnDisable ではメソッドの登録を解除すること。さもないとオブジェクトが無効にされたり破棄されたりした後にエラーになってしまう。
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
