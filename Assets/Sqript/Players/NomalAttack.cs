using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAttack : MonoBehaviour
{
    [Header("攻撃時の移動スピード")]
    [Tooltip("攻撃時の移動スピード")] [SerializeField] float _attackSpeed = 3;

    [Header("空中無移動攻撃時の移動スピード")]
    [Tooltip("空中無移動攻撃時の移動スピード")] [SerializeField] float _attackUpSpeedNoMoe = 3;

    [Header("空中移動攻撃時の移動スピード")]
    [Tooltip("空中移動攻撃時の移動スピード")] [SerializeField] float _attackUpSpeedMove = 10;

    [Header("横移動の距離")]
    [Tooltip("横移動の距離")] [SerializeField] float _moveDis = 3;

    [Header("空中横移動の距離")]
    [Tooltip("空中横移動の距離")] [SerializeField] float _upMoveDis = 5;


    [SerializeField] float _timeMove = 0.3f;
    [SerializeField] float _timeNoMove = 0.5f;
    [SerializeField] float _timeUpNoMove = 0.5f;
    [SerializeField] float _timeUpMove = 2f;

    [Header("エフェクトとポジション")]
    [SerializeField] GameObject[] _zangekiEffects = new GameObject[4];
    [SerializeField] Transform[] _zangekiEffectsPosition = new Transform[4];
    [SerializeField] GameObject _downAttackEffect;

    [Header("武器のアニメーション")]
    [SerializeField] Transform _zangekiEffectPosition;
    [SerializeField] Animator _weaponAnim;

    [Header("必要なオブジェクト")]
    ///<summary>クロスヘアーのスクリプト</summary>
    [SerializeField] GameObject _crosshairController;


    [SerializeField] AttackCloseController _attackCloseController;

    PushdKey _pushdKey = PushdKey.NoMove;

    int _noMoveAttackCount = 0;
    Rigidbody _rb;
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _weaponAnim = _weaponAnim.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovedEnd();
    }
    /// <summary>攻撃の硬直解除</summary>
    IEnumerator ReleaseAttackStiffenss()
    {
        Debug.Log("a333a");

        if (_pushdKey == PushdKey.MoveX)
        {
            yield return new WaitForSeconds(_timeMove);

        }
        else if (_pushdKey == PushdKey.NoMove)
        {
            yield return new WaitForSeconds(_timeNoMove);
        }
        else if (_pushdKey == PushdKey.UpAttack)
        {
            yield return new WaitForSeconds(_timeUpNoMove);
        }
        else if (_pushdKey == PushdKey.UpMoveAttack)
        {
            yield return new WaitForSeconds(_timeUpMove);
        }

        _attackCloseController._closeAttack = false;
        _attackCloseController._isAttackNow = false;
        //if (_enemyBox.transform.childCount != 0)
        //{
        //    //_enemyBox.transform.DetachChildren();
        //}
    }
    void MovedEnd()
    {
        if (_attackCloseController._closeAttack)
        {
            float distance = Vector3.Distance(_attackCloseController._nowPos, transform.position);

            if (_pushdKey == PushdKey.UpAttack)
            {
                _attackCloseController._closeAttack = false;
                _rb.velocity = Vector3.zero;

            }


            if (_pushdKey == PushdKey.UpMoveAttack)
            {
                if (distance > _upMoveDis)
                {
                    _attackCloseController._closeAttack = false;
                    _rb.velocity = Vector3.zero;
                }
            }

            if (_pushdKey == PushdKey.MoveX)
            {
                if (distance > 4)
                {
                    _attackCloseController._closeAttack = false;
                    Debug.Log("ffx");
                    _rb.velocity = Vector3.zero;
                }
            }

            if (_pushdKey == PushdKey.NoMove)
            {
                if (distance > _moveDis)
                {
                    _rb.velocity = Vector3.zero;
                    _attackCloseController._closeAttack = false;
                }
            }
        }
    }


    public void MoveAttack()
    {
        _pushdKey = PushdKey.MoveX;
        StartCoroutine(ReleaseAttackStiffenss());
        _attackCloseController._downSpeed = false;
        _attackCloseController.airTime = 0;


    }

    public void MoveAttackEffeck()
    {
        var effect = Instantiate(_zangekiEffects[0]);                       //エッフェクトを出す
        effect.transform.position = _zangekiEffectsPosition[0].position;
    }

    public void UpAttack()
    {

        _pushdKey = PushdKey.UpAttack;
        StartCoroutine(ReleaseAttackStiffenss());
        Vector3 velo = _crosshairController.transform.position - transform.position;
        _rb.AddForce(velo.normalized * _attackUpSpeedNoMoe, ForceMode.Impulse);
    }
    public void UpAttackEffect()
    {


    }
    public void UpMoveAttack(float h)
    {
        _pushdKey = PushdKey.UpMoveAttack;
        StartCoroutine(ReleaseAttackStiffenss());
        Debug.Log("XXX");
        if (h > 0)
        {
            _rb.AddForce(1 * transform.right * _attackUpSpeedMove, ForceMode.Impulse);
        }
        else
        {
            _rb.AddForce(-1 * transform.right * _attackUpSpeedMove, ForceMode.Impulse);
        }



    }

    public void UpMoveAttackEffect()
    {


    }

    public void NoMoveAttack()
    {

        _pushdKey = PushdKey.NoMove;
        StartCoroutine(ReleaseAttackStiffenss());
    }


    public void NoMoveAttackEffeck()
    {
        //////////////////////////////地上での攻撃
        if (_noMoveAttackCount == 0)
        {
            _noMoveAttackCount++;
            _weaponAnim.Play("Zangeki1");                                       //鎌のアニメーション
            var effect = Instantiate(_zangekiEffects[0]);                       //エッフェクトを出す
            effect.transform.position = _zangekiEffectsPosition[0].position;
        }
        else if (_noMoveAttackCount == 1)
        {
            _noMoveAttackCount++;
            _weaponAnim.Play("Zangeki2");
            var effect = Instantiate(_zangekiEffects[1]);
            effect.transform.position = _zangekiEffectsPosition[1].position;
        }
        else if (_noMoveAttackCount == 2)
        {
            _noMoveAttackCount++;
            _weaponAnim.Play("Zangeki3");
            var effect = Instantiate(_zangekiEffects[2]);
            effect.transform.position = _zangekiEffectsPosition[2].position;

        }
        else if (_noMoveAttackCount == 3)
        {
            _noMoveAttackCount++;
            _weaponAnim.Play("Zangeki4");
            var effect = Instantiate(_zangekiEffects[3]);
            effect.transform.position = _zangekiEffectsPosition[3].position;
        }
        else if (_noMoveAttackCount == 4)
        {
            _weaponAnim.Play("Zangeki5");
            var effect = Instantiate(_zangekiEffects[4]);
            effect.transform.position = _zangekiEffectsPosition[4].position;
            _noMoveAttackCount = 0;
        }
    }

    void Dir()
    {
        Vector3 muki;
        if (_crosshairController.transform.position.x - transform.position.x > 0)
        {
            muki = new Vector3(1, transform.localScale.y, transform.localScale.z);
            transform.localScale = muki;
        }
        else if (_crosshairController.transform.position.x - transform.position.x < 0)
        {
            muki = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            transform.localScale = muki;
        }
    }

    public enum PushdKey
    {
        NoMove,
        MoveX,
        /// <summary>空中移動攻撃</summary>
        UpMoveAttack,
        /// <summary>空中攻撃</summary>
        UpAttack,


    }

}
