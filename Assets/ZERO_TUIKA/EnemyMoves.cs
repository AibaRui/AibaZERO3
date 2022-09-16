using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoves : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3;


    /// <summary>trueだったら思考</summary>
    bool _thinkNow = false;

    /// <summary>特定の行動をしているときは行動しない。攻撃等は多数呼ばれるから</summary>
    bool _isActionNow = false;

    JustKaihiManager _justKaihiManager;
    PauseManager _pauseManager = default;
    Vector3 _angularVelocity;
    Vector3 _velocity;
    float _time;
    float time = 1;

    [SerializeField] GameObject _weapon;

    Rigidbody _rb;
    Animator _anim;
    GameObject _player;
    public EnemyAction _enemyAction = EnemyAction.Wait;
    Animator _weaponAnim;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _pauseManager = GameObject.FindObjectOfType<PauseManager>();
        _justKaihiManager = GameObject.FindObjectOfType<JustKaihiManager>();
    }

    void Start()
    {
        _thinkNow = true;
        _rb = gameObject.GetComponent<Rigidbody>();
        _anim = gameObject.GetComponent<Animator>();
        _weaponAnim = _weapon.GetComponent<Animator>();
    }






    void Update()
    {
        SetAi();

        switch (_enemyAction)
        {
            case EnemyAction.Wait:
                Wait();
                break;
            case EnemyAction.Move:
                Move();
                break;

            case EnemyAction.Follow:
                Follow();
                break;

            case EnemyAction.Attack:
                Attack();
                break;

        }
    }

    void SetAi()
    {
        //タイマーを回してる間は思考させない
        if (!_thinkNow)
        {
            return;
        }
        MainRoutine();

        StartCoroutine(AiTimer());
    }

    void MainRoutine()
    {
        Debug.Log("Think");

        float dir = Vector2.Distance(_player.transform.position, transform.position);

        if (dir< 20)
        {
            _enemyAction = EnemyAction.Wait;
        }

        if (dir< 10)
        {
            _enemyAction = EnemyAction.Move;
        }

        if (dir< 6)
        {
            _enemyAction = EnemyAction.Follow;
        }

        if (dir< 3)
        {
            _enemyAction = EnemyAction.Attack;
        }
    }

    IEnumerator AiTimer()
    {
        _thinkNow = false;
        yield return new WaitForSeconds(0.1f);
        _thinkNow = true;
    }

    void Wait()
    {
        Debug.Log("wait");
    }

    void Move()
    {
        Debug.Log("move");
    }

    void Follow()
    {
        Debug.Log("Follow");
        Vector2 dir = _player.transform.position - transform.position;

        _rb.velocity = new Vector2(dir.normalized.x * 3, _rb.velocity.y);

    }

    void Attack()
    {
        if(_isActionNow)
        {
            return;
        }
        _isActionNow = true;
        _weaponAnim.Play("ArrmerEnemyWeaponAttackClose");

        Debug.Log("Attack");
        StartCoroutine(a());

    }

    IEnumerator a()
    {
        _enemyAction = EnemyAction.Next;
        yield return new WaitForSeconds(2f);
        _isActionNow = false;
    }



    public enum EnemyAction
    {
        Next,
        Wait,
        Move,
        Follow,
        Attack,
        Evation,
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="P_Attack")
        {
            Destroy(gameObject);
        }
    }


    void OnEnable()
    {
        // 呼んで欲しいメソッドを登録する。
        _pauseManager.OnPauseResume += PauseResume;
        _anim = gameObject.GetComponent<Animator>();


        // 呼んで欲しいメソッドを登録する。
        _justKaihiManager.OnJustKaihiResume += PauseResumeJustKaihi;
        // _anim = gameObject.GetComponent<Animator>();
    }

    void OnDisable()
    {
        // OnDisable ではメソッドの登録を解除すること。さもないとオブジェクトが無効にされたり破棄されたりした後にエラーになってしまう。
        _pauseManager.OnPauseResume -= PauseResume;


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
        // 速度・回転を保存し、Rigidbody を停止する
     //   _angularVelocity = _rb.angularVelocity;
        _velocity = _rb.velocity;
        _rb.velocity = new Vector3(_rb.velocity.x / 10, _rb.velocity.y / 10,0);
        _anim.speed = 0.3f;
        time = 10;
    }

    public void ResumeJustKaihi()
    {
        // Rigidbody の活動を再開し、保存しておいた速度・回転を戻す
        _rb.WakeUp();
       // _rb.angularVelocity = _angularVelocity;
        _rb.velocity = _velocity;
        _anim.speed = 1;
        time = 1;
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
     //   _angularVelocity = _rb.angularVelocity;
        _velocity = _rb.velocity;
        _rb.Sleep();
        _rb.isKinematic = true;
        _anim.enabled = false;
    }

    public void Resume()
    {
        // Rigidbody の活動を再開し、保存しておいた速度・回転を戻す
        _rb.WakeUp();
     //   _rb.angularVelocity = _angularVelocity;
        _rb.velocity = _velocity;
        _rb.isKinematic = false;

        _anim.enabled = true;
    }
}
