using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack2 : MonoBehaviour
{
    [SerializeField] GameObject _fire;�@�@///���̃v���n�u
    [SerializeField] GameObject _fireFollow;
    [SerializeField] float _speed = 3;�@�@///���̑��x

    int _attackCount = 0;

    float chageTime = 0;
    [SerializeField] float chageTimeLimit = 3;
    bool chagedAttack = false;

    [SerializeField] TargetSystem _targetSystem;

    [SerializeField] AttackCloseController _attackCloseController;


    ////////�󒆂̂��ꂼ��̈ړ�����//////////////////////////////////////////////

    bool _isAir = false;
    private float airTime = 0;        //�U�����̋󒆑؍ݎ���
    [SerializeField] float airTimeLimit = 2;        //�U�����̋󒆑؍ݎ���                                     
    [SerializeField] float _xSpeed = 0.5f;
    [SerializeField] float _ySpeed = 0f;
    [SerializeField] float _zSpeed = 0.5f;


    bool _isGround = false;
    private bool _downSpeed = false;    //�󒆍U���̔���
    Rigidbody _playerRb;
    [SerializeField] GameObject _mousePos; //�}�E�X
    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();

    }


    // Update is called once per frame
    void Update()
    {
        if (_downSpeed)�@�@�@//�󒆍U�����̈ړ�����
        {
            _playerRb.velocity = new Vector3(_xSpeed, _ySpeed, _zSpeed);
        }
        if (_isAir)
        {
            AirTime();
        }

        if (!_attackCloseController._isAttackNow)
        {
            Charge();
            Attack();
        }


    }

    void AirTime()�@�@�@    ///�󒆑؍݂̂̎��Ԍv�Z
    {

        airTime += Time.deltaTime;
        if (airTime > airTimeLimit)
        {
            _downSpeed = false;
            airTime = 0;
        }

    }



    void Charge()      //�`���[�W����������������{
    {

        if (Input.GetMouseButton(1))          //�E�N���b�N�����Ă�ԃ`���[�W����
        {
            chageTime += Time.deltaTime;

        }

        if (chageTime > chageTimeLimit)        //�`���[�W����
        {
            chageTime = 0;
            chagedAttack = true;
            Debug.Log("Chaged");
            _attackCount = 0;
        }

        if (Input.GetMouseButtonUp(1) && chageTime < chageTimeLimit)  //�`���[�W�s��
        {
            chageTime = 0;

        }
    }

    void Attack()
    {
        if (chagedAttack)
        {
            if (Input.GetMouseButtonDown(1))
            {
                GoAttack();
                _attackCount++;
            }
        }
    }
    void GoAttack()
    {
        if (_isGround == false)
        {
            _downSpeed = true;
            _isAir = true;
        }

        if (_attackCount == 0)
        {

        }
        if (_attackCount == 1)
        {

        }
        if (_attackCount == 2)
        {
            airTime = 0;
            _isAir = false;
            _downSpeed = false;
            chagedAttack = false;
            _attackCount++;

        }

        /////////���̃I�u�W�F�N�g���o��
        if (_targetSystem._targetEnemy != null && _targetSystem._targetEnemy.gameObject.tag == "Enemy")
        {
            var _followFires = Instantiate(_fireFollow);
            _followFires.transform.position = this.gameObject.transform.position;
        }
        else
        {
            Debug.Log("Nomal");
            var _fires = Instantiate(_fire);
            Rigidbody _rb = _fires.GetComponent<Rigidbody>();
            _fires.transform.position = this.gameObject.transform.position;

            Vector3 mouse = _mousePos.transform.position - _fires.transform.position;
            //mouse.z = 0;
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

    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = true;
            _downSpeed = false;
            airTime = 0;
            _isAir = false;
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
