using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAttack : MonoBehaviour
{
    [Header("�U�����̈ړ��X�s�[�h")]
    [Tooltip("�U�����̈ړ��X�s�[�h")] [SerializeField] float _attackSpeed = 3;

    [Header("�G�t�F�N�g�ƃ|�W�V����")]
    [SerializeField] GameObject[] _zangekiEffects = new GameObject[4];
    [SerializeField] Transform[] _zangekiEffectsPosition = new Transform[4];
    [SerializeField] GameObject _downAttackEffect;

    [Header("����̃A�j���[�V����")]
    [SerializeField] Transform _zangekiEffectPosition;
    [SerializeField] Animator _weaponAnim;


    [SerializeField] AttackCloseController _attackCloseController;

    int _noMoveAttackCount = 0;

    Rigidbody _rb;
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveAttack()
    {
       _attackCloseController._downSpeed = false;
       _attackCloseController.airTime = 0;

    }

    public void NoMoveAttackEffeck()
    {
        //////////////////////////////�n��ł̍U��
        if (_noMoveAttackCount == 0)
        {
            _noMoveAttackCount++;
            _weaponAnim.Play("Zangeki1");                                       //���̃A�j���[�V����
            var effect = Instantiate(_zangekiEffects[0]);                       //�G�b�t�F�N�g���o��
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
}
 