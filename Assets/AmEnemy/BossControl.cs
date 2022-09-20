using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    [Header("火柱の攻撃エフェクト")]
    [Tooltip("火柱の攻撃エフェクト")] [SerializeField] GameObject _fireWall;

    [Header("爆発の攻撃エフェクト")]
    [Tooltip("爆発の攻撃エフェクト")] [SerializeField] GameObject _exprosion;

    [Header("ボスの移動場所")]
    [Tooltip("ボスの移動場所")] [SerializeField] Transform[] _pos = new Transform[2];


    /// <summary>攻撃中かどうか</summary>
    bool _isAttackNow = true;
    /// <summary>攻撃可能かどうか</summary>
    bool _isAttack = true;

    /// <summary>攻撃が終了したかどうかの判断。アニメーションで呼ぶ</summary>
    bool _endAttack = false;

    GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {

    }





    void Update()
    {
        if (_endAttack)
        {
            StartCoroutine(AttackLate());
            _endAttack = true;
        }

        MainRoutine();
    }

    void MainRoutine()
    {

        if (_isAttackNow || !_isAttack)
        {
            return;
        }
        var num = Random.Range(0, 3);

        if (num == 0)
        {
            FireWall();
        }
        else if (num == 1)
        {
            Exprosion();
        }
        else if (num == 2)
        {

        }
        _isAttackNow = true;
    }

    IEnumerator AttackLate()
    {
        var num = Random.Range(6, 10);
        yield return new WaitForSeconds(num);
    }

    /// <summary>火柱</summary>
    void FireWall()
    {

    }

    void Exprosion()
    {

    }

}
