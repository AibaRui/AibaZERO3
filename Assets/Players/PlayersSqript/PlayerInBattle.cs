using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInBattle : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 2;
    [SerializeField] float _jumpPower = 4;

    [SerializeField] int _hp;
    bool _isGround = false;
    bool _isJump = false;
    bool _isRun = false;
    Rigidbody _rb;


    public bool _damaged;
    Animator _anim;
    // SpriteRenderer _sp;

    [SerializeField] P_Kaihi _kaihi;
    [SerializeField] AttackCloseController _attackCloseController;

    PauseManager _pauseManager = default;
    Vector3 _angularVelocity;
    Vector3 _velocity;



    public PlayerAction _playerAction = PlayerAction.Nomal;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        // _sp = GetComponent<SpriteRenderer>();
        FindObjectOfType<PlayerHpControl>().ChangeHpText(_hp);
    }


    void Update()
    {
        if (!_pauseManager._isPause)
        {
            Anim();

            if (_playerAction == PlayerAction.Nomal)
            {
                if (!_kaihi._isDodges || !_kaihi._isDodging)
                {
                        Jump();
                        Move();
                    
                }
            }
        }
    }
    void FixedUpdate()
    {
        if (!_pauseManager._isPause)
        {
            IsJump();
        }
    }


    void Jump()
    {
        if (_isGround)
        {
            if (Input.GetButton("Jump"))
            {
                _anim.SetBool("Jump", true);
                _isJump = true;
            }
        }
    }

    void IsJump()
    {
        if (_isJump)
        {
            _rb.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
            _isJump = false;
            _isGround = false;
        }

    }


    void Move()
    {
        float _h = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.E))
        {
            _isRun = !_isRun;
            if (_isRun)
            {
                _moveSpeed = 4;
            }
            else

            {
                _moveSpeed = 2;
            }

        }

        if (_h != 0)
        {
            Vector2 velo = new Vector2(_h * _moveSpeed, _rb.velocity.y);
            _rb.velocity = velo;
        }

        if (_h > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            // _sp.flipX = false;
        }
        else if (_h < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            // _sp.flipX = true;
        }

    }

    void Anim()
    {
        if (_rb.velocity.x != 0 || _rb.velocity.z != 0)
        {
            _anim.SetBool("WalkNomal", true);
        }
        else
        {
            _anim.SetBool("WalkNomal", false);
        }
    }

    IEnumerator Damaged()
    {
        Debug.Log("Damage");
        gameObject.layer = LayerMask.NameToLayer("PlayerMuteki");
        yield return new WaitForSeconds(1);
        gameObject.layer = LayerMask.NameToLayer("PlayerBase");
        _damaged = false;
    }



    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _anim.SetBool("Jump", false);

            _isGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyAttack")
        {
            StartCoroutine(Damaged());
            _damaged = true;
            _hp--;
            FindObjectOfType<PlayerHpControl>().ChangeHpText(_hp);
        }
    }

    ///////Parse処理/////

    private void Awake()
    {
        _pauseManager = GameObject.FindObjectOfType<PauseManager>();
    }

    void OnEnable()
    {
        // 呼んで欲しいメソッドを登録する。
        _pauseManager.OnPauseResume += PauseResume;
        _anim = gameObject.GetComponent<Animator>();
    }

    void OnDisable()
    {
        // OnDisable ではメソッドの登録を解除すること。さもないとオブジェクトが無効にされたり破棄されたりした後にエラーになってしまう。
        _pauseManager.OnPauseResume -= PauseResume;
    }

    void PauseResume(bool isPause)
    {
        if (isPause)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        // 速度・回転を保存し、Rigidbody を停止する
        _angularVelocity = _rb.angularVelocity;
        _velocity = _rb.velocity;
        _rb.Sleep();
        _rb.isKinematic = true;
        _anim.enabled = false;
    }

    public void Resume()
    {
        // Rigidbody の活動を再開し、保存しておいた速度・回転を戻す
        _rb.WakeUp();
        _rb.angularVelocity = _angularVelocity;
        _rb.velocity = _velocity;
        _rb.isKinematic = false;

        _anim.enabled = true;
    }




    public enum PlayerAction
    {
        /// <summary>通常状態</summary>
        Nomal,
        /// <summary>攻撃中、移動不可</summary>
        Attack,


    }

}
