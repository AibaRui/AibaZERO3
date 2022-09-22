using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : MonoBehaviour
{
    
     [SerializeField] GameObject _fire;　　///炎のプレハブ
    [SerializeField] float _speed = 3;　　///炎の速度

    private float _attackCoolTime = 2;    //
    [SerializeField] float _attackCoolTimeLimit = 3;    //攻撃自体のクールタイム
    private  float _attackContenueTime  = 0f;          
    [SerializeField] float _attackContenueTimeLimit = 2f;    //攻撃中断から再開までの範囲の制限
    int _attackCount=0;

    

    bool _isAttackContnue=false;
    bool _isAttack=true;

   ////////空中のそれぞれの移動制限//////////////////////////////////////////////
                           
    private float airTime=0;        //攻撃時の空中滞在時間
    [SerializeField] float airTimeLimit=2;        //攻撃時の空中滞在時間                                     //
    [SerializeField] float _xSpeed=0.5f;　
    [SerializeField] float _ySpeed = 0f;
    [SerializeField] float _zSpeed = 0.5f;


    bool _isGround = false;
    private bool _downSpeed=false;    //空中攻撃の判定
    Rigidbody _playerRb;
    [SerializeField] GameObject _mousePos;　//マウス
    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if(_isAttack)           //攻撃
        {
            Attack();
        }
        
        if(_downSpeed)　　　//空中攻撃時の移動制限
        {
            
            _playerRb.velocity = new Vector3(_xSpeed, _ySpeed, _zSpeed);
        }

        if(_attackCount>2)　　                             //最後まで打ち終えたら、クールタイムの計算
        {
            _attackContenueTime = 0;
            _isAttackContnue = false;
            _isAttack = false;   //攻撃できなくする
            _downSpeed = false;　//空中制御を解除
            CountCoolTime();

        }

        if(_isAttackContnue)　         ///攻撃再開の時間計算
        {
            CountContenueTime();
        }
    }

    void CountContenueTime()　　　    ///攻撃再開の時間計算
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

    void CountCoolTime()　　　　　　　　　　　　            //最後まで打ち終えたら、クールタイムの計算
    {
        _isAttack = false;
        _attackCoolTime += Time.deltaTime;

            Debug.Log(_attackCoolTime);
        if(_attackCoolTime>_attackCoolTimeLimit)
        {
            _attackCount = 0;                         //攻撃回数を0に
            _isAttack = true;　　　　　　　　　　　 //攻撃できるようなる
            _attackCoolTime = 0;                   //クールタイムのリセット
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
        //yield return new WaitForSeconds(_attackLate);     //レート
        

        if (_isGround == false)
        {
            Debug.Log("www");
            _downSpeed = true;
        }

        /////////炎のオブジェクトを出す
        var _fires = Instantiate(_fire);
        Rigidbody _rb = _fires.GetComponent<Rigidbody>();
        _fires.transform.position = this.gameObject.transform.position;


        Vector3 mouse = _mousePos.transform.position - _fires.transform.position;
        mouse.z = 0;
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

     /// //////////////////////////////////////////////////////////////２回目の攻撃             ///////////////////////////////////////

    void SecondAttack()
    {
        airTime=0;
        _attackCount=2;
        _isAttackContnue = true;
        //yield return new WaitForSeconds(_attackLate);     //レート


        /////////炎のオブジェクトを出す
        var _fires = Instantiate(_fire);
        Rigidbody _rb = _fires.GetComponent<Rigidbody>();
        _fires.transform.position = this.gameObject.transform.position;


        Vector3 mouse = _mousePos.transform.position - _fires.transform.position;
        mouse.z = 0;
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
    void ThirdAttack()
    {
        airTime = 0;
        _attackCount++;

        /////////炎のオブジェクトを出す
        var _fires = Instantiate(_fire);
        Rigidbody _rb = _fires.GetComponent<Rigidbody>();
        _fires.transform.position = this.gameObject.transform.position;


        Vector3 mouse = _mousePos.transform.position - _fires.transform.position;
        mouse.z = 0;
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
