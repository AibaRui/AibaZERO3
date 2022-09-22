using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : MonoBehaviour
{
    
     [SerializeField] GameObject _fire;�@�@///���̃v���n�u
    [SerializeField] float _speed = 3;�@�@///���̑��x

    private float _attackCoolTime = 2;    //
    [SerializeField] float _attackCoolTimeLimit = 3;    //�U�����̂̃N�[���^�C��
    private  float _attackContenueTime  = 0f;          
    [SerializeField] float _attackContenueTimeLimit = 2f;    //�U�����f����ĊJ�܂ł͈̔͂̐���
    int _attackCount=0;

    

    bool _isAttackContnue=false;
    bool _isAttack=true;

   ////////�󒆂̂��ꂼ��̈ړ�����//////////////////////////////////////////////
                           
    private float airTime=0;        //�U�����̋󒆑؍ݎ���
    [SerializeField] float airTimeLimit=2;        //�U�����̋󒆑؍ݎ���                                     //
    [SerializeField] float _xSpeed=0.5f;�@
    [SerializeField] float _ySpeed = 0f;
    [SerializeField] float _zSpeed = 0.5f;


    bool _isGround = false;
    private bool _downSpeed=false;    //�󒆍U���̔���
    Rigidbody _playerRb;
    [SerializeField] GameObject _mousePos;�@//�}�E�X
    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if(_isAttack)           //�U��
        {
            Attack();
        }
        
        if(_downSpeed)�@�@�@//�󒆍U�����̈ړ�����
        {
            
            _playerRb.velocity = new Vector3(_xSpeed, _ySpeed, _zSpeed);
        }

        if(_attackCount>2)�@�@                             //�Ō�܂őł��I������A�N�[���^�C���̌v�Z
        {
            _attackContenueTime = 0;
            _isAttackContnue = false;
            _isAttack = false;   //�U���ł��Ȃ�����
            _downSpeed = false;�@//�󒆐��������
            CountCoolTime();

        }

        if(_isAttackContnue)�@         ///�U���ĊJ�̎��Ԍv�Z
        {
            CountContenueTime();
        }
    }

    void CountContenueTime()�@�@�@    ///�U���ĊJ�̎��Ԍv�Z
    {
        
        _attackContenueTime += Time.deltaTime;
        airTime += Time.deltaTime;
        if(airTime>airTimeLimit)
        {
            _isAttack = false;
            _downSpeed = false;
            airTime = 0;
        }
        
        if (_attackContenueTime>_attackContenueTimeLimit)
        {
            _isAttack = true;
            _attackContenueTime = 0;
            _isAttackContnue = false;
             _attackCount = 0;
            _downSpeed = false;
        }

    }

    void CountCoolTime()�@�@�@�@�@�@�@�@�@�@�@�@            //�Ō�܂őł��I������A�N�[���^�C���̌v�Z
    {
        _isAttack = false;
        _attackCoolTime += Time.deltaTime;

            Debug.Log(_attackCoolTime);
        if(_attackCoolTime>_attackCoolTimeLimit)
        {
            _attackCount = 0;                         //�U���񐔂�0��
            _isAttack = true;�@�@�@�@�@�@�@�@�@�@�@ //�U���ł���悤�Ȃ�
            _attackCoolTime = 0;                   //�N�[���^�C���̃��Z�b�g
        }
    }


    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_attackCount == 0)
            {
                _attackContenueTime = 0;
                _isAttackContnue = false;
                FirstAttack();
            }
            else if (_attackCount == 1)
            {
                _attackContenueTime = 0;
                _isAttackContnue = false;
                SecondAttack();
            }
            else if (_attackCount == 2)
            {
                _attackContenueTime = 0;
                _isAttackContnue = false;
                ThirdAttack();
            }

        }
    }


   // IEnumerator
      void  FirstAttack()
    {

        _attackCount=1;
        _isAttackContnue = true;   
        //yield return new WaitForSeconds(_attackLate);     //���[�g
        

        if (_isGround == false)
        {
            Debug.Log("www");
            _downSpeed = true;
        }

        /////////���̃I�u�W�F�N�g���o��
        var _fires = Instantiate(_fire);
        Rigidbody _rb = _fires.GetComponent<Rigidbody>();
        _fires.transform.position = this.gameObject.transform.position;


        Vector3 mouse = _mousePos.transform.position - _fires.transform.position;
        mouse.z = 0;
        Vector3 _velo = mouse.normalized * _speed;
        _rb.velocity = _velo;



        ////////////�U�����̃v���C���[�̌���
        if (mouse.x > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }

    }

     /// //////////////////////////////////////////////////////////////�Q��ڂ̍U��             ///////////////////////////////////////

    void SecondAttack()
    {
        airTime=0;
        _attackCount=2;
        _isAttackContnue = true;
        //yield return new WaitForSeconds(_attackLate);     //���[�g


        /////////���̃I�u�W�F�N�g���o��
        var _fires = Instantiate(_fire);
        Rigidbody _rb = _fires.GetComponent<Rigidbody>();
        _fires.transform.position = this.gameObject.transform.position;


        Vector3 mouse = _mousePos.transform.position - _fires.transform.position;
        mouse.z = 0;
        Vector3 _velo = mouse.normalized * _speed;
        _rb.velocity = _velo;



        ////////////�U�����̃v���C���[�̌���
        if (mouse.x > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }



    }
    void ThirdAttack()
    {
        airTime = 0;
        _attackCount++;

        /////////���̃I�u�W�F�N�g���o��
        var _fires = Instantiate(_fire);
        Rigidbody _rb = _fires.GetComponent<Rigidbody>();
        _fires.transform.position = this.gameObject.transform.position;


        Vector3 mouse = _mousePos.transform.position - _fires.transform.position;
        mouse.z = 0;
        Vector3 _velo = mouse.normalized * _speed;
        _rb.velocity = _velo;



        ////////////�U�����̃v���C���[�̌���
        if (mouse.x > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }



    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
             _isGround = true;
            _downSpeed = false;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = false;

            
        }
    }
 
}
