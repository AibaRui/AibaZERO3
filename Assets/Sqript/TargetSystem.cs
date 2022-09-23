using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetSystem : MonoBehaviour
{
    public RaycastHit hit; //���C�L���X�g�������������̂��擾������ꕨ
    public GameObject _targetEnemy = null;

    /// <summary>�U�����ɓG�̑O�Ƀe���|�[�g������Ƃ��Ɏg��</summary>
    public RaycastHit hitTeleportEnemy;
    /// <summary>�U�����ɓG�̑O�Ƀe���|�[�g������Ƃ��Ɏg���G�I�u�W�F�N�g</summary>
    public GameObject _teleportTargetEnemy = null;
    [SerializeField] GameObject player;

    [SerializeField] GameObject _targetUI;

    bool _targetting = false;

    private RectTransform myRectTfm;
    private Vector3 offset = new Vector3(0, 1.5f, 0);

    TargetCloseAttack _targetCloseAttack;

    void Start()
    {
        myRectTfm = _targetUI.GetComponent<RectTransform>();
        _targetCloseAttack = FindObjectOfType<TargetCloseAttack>();
        //_orizinQuaternion = _virtualCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_targetCloseAttack)
        {
            _targetCloseAttack = FindObjectOfType<TargetCloseAttack>();
        }

        if (_targetCloseAttack)
        {
            if (!_targetCloseAttack._isTargetAttackNow)
            {
                if (Input.GetMouseButtonDown(2))
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))  //�}�E�X�̃|�W�V��������Ray�𓊂��ĉ����ɓ���������hit�ɓ����
                    {

                        if (hit.collider.gameObject.tag == "Enemy")
                        {
                            _targetEnemy = hit.collider.gameObject;
                            _targetUI.SetActive(true);
                            _targetting = true;
                            Debug.Log("Target"); //�I�u�W�F�N�g�����R���\�[���ɕ\��            
                        }
                        else
                        {
                            _targetEnemy = null;
                            _targetting = false;
                            _targetUI.SetActive(false);
                        }

                    }
                    //  }
                }
            }

        }
        if (_targetEnemy == null)
        {
            _targetUI.SetActive(false);
        }
        if (_targetting && _targetEnemy != null)
        {

            myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _targetEnemy.transform.position + offset);

        }

    }



}
