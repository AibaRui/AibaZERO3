using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoves : MonoBehaviour
{

    [SerializeField] int _hp;
    [SerializeField] float _moveSpeed = 3;
    [SerializeField] float _attackCoolTime = 3;
    [SerializeField] GameObject _longAttackEffect;
    [SerializeField] GameObject _weapon;
    [SerializeField] GameObject _deathBody;



    /// <summary>true��������v�l</summary>
    bool _thinkNow = false;

    /// <summary>����̍s�������Ă���Ƃ��͍s�����Ȃ��B�U�����͑����Ă΂�邩��</summary>
    bool _isActionNow = false;

    /// <summary>�^�[�Q�b�g�U���������Ă邩�ǂ���</summary>
    public bool _isDamagedTargetAttack = false;

    int _countDamagedTargetAttack = 0;

    JustKaihiManager _justKaihiManager;
    PauseManager _pauseManager = default;
    Vector3 _angularVelocity;
    Vector3 _velocity;
    float _time;
    float time = 1;

    public bool _isJustKaihi = false;



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


        if (_isJustKaihi)
        {
            _isActionNow = true;
        }
        if (FindObjectOfType<TargetSystem>()._targetEnemy == this.gameObject)
        {
            _enemyAction = EnemyAction.TargetAttackDamaged;
        }



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

            case EnemyAction.Stop:
                Stop();
                break;

            case EnemyAction.LongAttack:
                LongAttck();
                break;

            case EnemyAction.Back:
                Back();
                break;

            case EnemyAction.TargetAttackDamaged:
                TargetAttackDamaged();
                break;

        }
    }

    void SetAi()
    {
        //�^�C�}�[���񂵂Ă�Ԃ͎v�l�����Ȃ�
        if (!_thinkNow || _isActionNow)
        {
            return;
        }
        MainRoutine();

        StartCoroutine(AiTimer());
    }

    void MainRoutine()
    {
        //Debug.Log("Think");

        float dir = Vector2.Distance(_player.transform.position, transform.position);



        if (dir < 5)
        {
            var r = Random.Range(0, 4);
            if (r == 0)
            {
                _enemyAction = EnemyAction.Attack;
            }
            else if (r == 1)
            {
                _enemyAction = EnemyAction.Stop;
            }
            else if (r == 2)
            {
                _enemyAction = EnemyAction.LongAttack;
            }
            else
            {
                _enemyAction = EnemyAction.Back;
            }
            return;
        }

        if (dir < 10)
        {
            _enemyAction = EnemyAction.Follow;
            return;
        }
        if (dir < 15)
        {
            _enemyAction = EnemyAction.Move;
            return;
        }


        if (dir < 20)
        {
            _enemyAction = EnemyAction.Wait;
            return;
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
        //  Debug.Log("wait");
    }

    void Damaged()
    {
        _isActionNow = true;
        float h = GameObject.FindGameObjectWithTag("Player").transform.position.x - transform.position.x;
        _rb.velocity = Vector3.zero;
        if (h >= 0)
        {
            _rb.AddForce(transform.right * -2, ForceMode.Impulse);
        }
        else
        {
            _rb.AddForce(1 * transform.right * 2, ForceMode.Impulse);
        }

        StartCoroutine(Damagedd());
    }
    IEnumerator Damagedd()
    {
        _hp--;
        Debug.Log(_hp);
        if (_hp <= 0)
        {
            var go = Instantiate(_deathBody);
            go.transform.position = transform.position;
            Destroy(gameObject);
            yield break;
        }
        yield return new WaitForSeconds(1);
        _isActionNow = false;
    }


    void Stop()
    {
        if (_isActionNow)
        {
            return;
        }
        _isActionNow = true;
        Dirction();
        StartCoroutine(Stopa());
    }
    IEnumerator Stopa()
    {
        yield return new WaitForSeconds(2);
        _isActionNow = false;
    }

    void Back()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        Dirction();
        if (player)
        {
            Vector2 v = player.transform.position - transform.position;

            _rb.velocity = new Vector3(-2 * v.normalized.x, _rb.velocity.y, 0);
        }

        if (_isActionNow)
        {
            return;
        }
        _isActionNow = true;
        StartCoroutine(Backs());
    }
    IEnumerator Backs()
    {
        yield return new WaitForSeconds(2);

        if (_enemyAction == EnemyAction.Damaged)
        {
            yield break;
        }
        _isActionNow = false;
    }


    void Move()
    {
        // Debug.Log("move");
    }

    void Follow()
    {
        // Debug.Log("Follow");
        Vector2 dir = _player.transform.position - transform.position;

        _rb.velocity = new Vector2(dir.normalized.x * 3, _rb.velocity.y);
        Dirction();

    }

    void Attack()
    {
        if (_isActionNow)
        {
            return;
        }
        _isActionNow = true;
        Dirction();

        // Debug.Log("Attack");
        StartCoroutine(a());

    }

    IEnumerator a()
    {
        //_anim.Play("Attack");
        yield return new WaitForSeconds(0.5f);
        if (_enemyAction == EnemyAction.Damaged)
        {
            yield break;
        }
        _weaponAnim.Play("ArrmerEnemyWeaponAttackClose");
        //_enemyAction = EnemyAction.Next;
        yield return new WaitForSeconds(1.1f + _attackCoolTime);
        if (_enemyAction == EnemyAction.Damaged)
        {
            yield break;
        }
        _isActionNow = false;
    }

    void LongAttck()
    {
        if (_isActionNow)
        {
            return;
        }
        _isActionNow = true;
        Dirction();
        StartCoroutine(b());

    }

    IEnumerator b()
    {
        _weaponAnim.Play("ArmerEnemyWeaponFar1");
        yield return new WaitForSeconds(0.5f);
        if (_enemyAction == EnemyAction.Damaged)
        {
            yield break;
        }
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            var go = Instantiate(_longAttackEffect);
            go.transform.position = player.transform.position + new Vector3(0, 1, -1.5f);
        }

        yield return new WaitForSeconds(1.1f);
        if (_enemyAction == EnemyAction.Damaged)
        {
            yield break;
        }
        yield return new WaitForSeconds(_attackCoolTime);
        _isActionNow = false;
    }

    void TargetAttackDamaged()
    {
        _isActionNow = true;
        Dirction();

        if (_countDamagedTargetAttack > 0 && !_isDamagedTargetAttack)
        {
            StartCoroutine(TargetAttackDamagedC());

        }
    }

    IEnumerator TargetAttackDamagedC()
    {
        Debug.Log("AAAD");
        _rb.velocity = Vector3.zero;
        _countDamagedTargetAttack = 0;
        yield return new WaitForSeconds(2);

        _isActionNow = false;
        _isDamagedTargetAttack = false;
        _enemyAction = EnemyAction.Damaged;
        Damaged();
    }


    void Dirction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 v = player.transform.position - transform.position;
        if (v.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (v.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public enum EnemyAction
    {
        Next,
        Wait,
        Move,
        Follow,
        Attack,
        Evation,
        Stop,
        LongAttack,
        Back,
        Damaged,
        TargetAttackDamaged,
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "P_Attack" && _enemyAction != EnemyAction.TargetAttackDamaged)
        {
            _enemyAction = EnemyAction.Damaged;
            Damaged();
        }

        if (other.gameObject.tag == "RaisingAttack")
        {

        }

        if (other.gameObject.tag == "TargetAttack")
        {
            _isDamagedTargetAttack = true;
            _countDamagedTargetAttack++;
            _hp--;
        }
    }


    void OnEnable()
    {
        // �Ă�ŗ~�������\�b�h��o�^����B
        _pauseManager.OnPauseResume += PauseResume;
        _anim = gameObject.GetComponent<Animator>();


        // �Ă�ŗ~�������\�b�h��o�^����B
        _justKaihiManager.OnJustKaihiResume += PauseResumeJustKaihi;
        // _anim = gameObject.GetComponent<Animator>();
    }

    void OnDisable()
    {
        // OnDisable �ł̓��\�b�h�̓o�^���������邱�ƁB�����Ȃ��ƃI�u�W�F�N�g�������ɂ��ꂽ��j�����ꂽ�肵����ɃG���[�ɂȂ��Ă��܂��B
        _pauseManager.OnPauseResume -= PauseResume;


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
        // ���x�E��]��ۑ����ARigidbody ���~����
        //   _angularVelocity = _rb.angularVelocity;
        _velocity = _rb.velocity;
        _rb.velocity = new Vector3(_rb.velocity.x / 10, _rb.velocity.y / 10, 0);
        Debug.Log("Slow");
        _anim.speed = 0.3f;

    }

    public void ResumeJustKaihi()
    {
        // Rigidbody �̊������ĊJ���A�ۑ����Ă��������x�E��]��߂�
        _rb.WakeUp();
        // _rb.angularVelocity = _angularVelocity;
        _rb.velocity = _velocity;
        _anim.speed = 1;
        _isActionNow = false;
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
        // ���x�E��]��ۑ����ARigidbody ���~����
        //   _angularVelocity = _rb.angularVelocity;
        _velocity = _rb.velocity;
        _rb.Sleep();
        _rb.isKinematic = true;
        _anim.enabled = false;
    }

    public void Resume()
    {
        // Rigidbody �̊������ĊJ���A�ۑ����Ă��������x�E��]��߂�
        _rb.WakeUp();
        //   _rb.angularVelocity = _angularVelocity;
        _rb.velocity = _velocity;
        _rb.isKinematic = false;

        _anim.enabled = true;
    }
}
