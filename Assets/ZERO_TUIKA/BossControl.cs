using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    [Header("�Β��̍U���G�t�F�N�g")]
    [Tooltip("�Β��̍U���G�t�F�N�g")] [SerializeField] GameObject _fireWall;

    [Header("�����̍U���G�t�F�N�g")]
    [Tooltip("�����̍U���G�t�F�N�g")] [SerializeField] GameObject _exprosion;

    [Header("�{�X�̈ړ��ꏊ")]
    [Tooltip("�{�X�̈ړ��ꏊ")] [SerializeField] Transform[] _pos = new Transform[2];


    /// <summary>�U�������ǂ���</summary>
    bool _isAttackNow = true;
    /// <summary>�U���\���ǂ���</summary>
    bool _isAttack = true;

    /// <summary>�U�����I���������ǂ����̔��f�B�A�j���[�V�����ŌĂ�</summary>
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

    /// <summary>�Β�</summary>
    void FireWall()
    {

    }

    void Exprosion()
    {

    }

}
