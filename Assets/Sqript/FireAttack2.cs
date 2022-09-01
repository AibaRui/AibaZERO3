using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack2 : MonoBehaviour
{
    [SerializeField] GameObject _fire;　　///炎のプレハブ
    [SerializeField] GameObject _fireFollow;
    [SerializeField] float _speed = 3;　　///炎の速度

    int _attackCount = 0;

    float chageTime = 0;
    [SerializeField] float chageTimeLimit = 3;
    bool chagedAttack = false;

    [SerializeField] TargetSystem _targetSystem;

    [SerializeField] AttackCloseController _attackCloseController;


    ////////空中のそれぞれの移動制限//////////////////////////////////////////////

    bool _isAir = false;
    private float airTime = 0;        //攻撃時の空中滞在時間
    [SerializeField] float airTimeLimit = 2;        //攻撃時の空中滞在時間                                     
    [SerializeField] float _xSpeed = 0.5f;
    [SerializeField] float _ySpeed = 0f;
    [SerializeField] float _zSpeed = 0.5f;


    bool _isGround = false;
    private bool _downSpeed = false;    //空中攻撃の判定
    Rigidbody _playerRb;
    [SerializeField] GameObject _mousePos; //マウス
    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();

    }


    // Update is called once per frame
    void Update()
    {
        if (_downSpeed)　　　//空中攻撃時の移動制限
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

    void AirTime()　　　    ///空中滞在のの時間計算
    {

        airTime += Time.deltaTime;
        if (airTime > airTimeLimit)
        {
            _downSpeed = false;
            airTime = 0;
        }

    }



    void Charge()      //チャージ未完了だったら実施
    {

        if (Input.GetMouseButton(1))          //右クリック押してる間チャージする
        {
            chageTime += Time.deltaTime;

        }

        if (chageTime > chageTimeLimit)        //チャージ完了
        {
            chageTime = 0;
            chagedAttack = true;
            Debug.Log("Chaged");
            _attackCount = 0;
        }

        if (Input.GetMouseButtonUp(1) && chageTime < chageTimeLimit)  //チャージ不足
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

        /////////炎のオブジェクトを出す
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



            ////////////攻撃時のプレイヤーの向き
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
