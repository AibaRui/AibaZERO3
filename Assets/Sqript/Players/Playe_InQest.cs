using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playe_InQest : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 2;
    [SerializeField] float _jumpPower = 4;

    bool _isGround = false;
    bool _isJump = false;
    bool _isRun = false;
    Rigidbody _rb;
    Animator _anim;
    // SpriteRenderer _sp;

    [SerializeField] P_Kaihi _kaihi;
    [SerializeField] AttackClose attackClose;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        // _sp = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        Anim();
        Jump();
       if(!_kaihi._isDodges||!_kaihi._isDodging)
        {
            if(!attackClose._closeAttack)
            {
                Move();
            }
            
        }
            
        
        
    }
    void FixedUpdate()
    {
        IsJump();
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
        float _v = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.E))
        {
            _isRun = !_isRun;
            if(_isRun)
            {
                _moveSpeed = 4;
            }
            else

            {
                _moveSpeed = 2;
            }

        }



        if(_h!=0||_v!=0)
        {
          Vector3 velo = new Vector3(_h * _moveSpeed, _rb.velocity.y, _v * _moveSpeed);
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



    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _anim.SetBool("Jump", false);


            _isGround = true;
        }
    }







}
