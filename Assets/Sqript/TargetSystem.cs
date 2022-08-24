using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetSystem : MonoBehaviour
{
    public RaycastHit hit; //レイキャストが当たったものを取得する入れ物
    public GameObject _targetEnemy = null;

    /// <summary>攻撃時に敵の前にテレポートさせるときに使う</summary>
    public RaycastHit hitTeleportEnemy;
    /// <summary>攻撃時に敵の前にテレポートさせるときに使う敵オブジェクト</summary>
    public GameObject _teleportTargetEnemy = null;
    [SerializeField] GameObject player;

    [SerializeField] AttackClose _attackClose;

    [SerializeField] GameObject _targetUI;

    bool _targetting = false;

    private RectTransform myRectTfm;
    private Vector3 offset = new Vector3(0, 1.5f, 0);

    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    Quaternion _orizinQuaternion;


    [SerializeField] GameObject _cameraNomal;
    [SerializeField] GameObject _cameraFollow;

    void Start()
    {
        myRectTfm = _targetUI.GetComponent<RectTransform>();
        //_orizinQuaternion = _virtualCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //var rays = Camera.main.ScreenPointToRay(Input.mousePosition);


        //if (Input.GetMouseButtonDown(1))
        //{
        //    if (Physics.Raycast(rays, out hitTeleportEnemy))
        //    {
        //        if (hitTeleportEnemy.collider.gameObject.tag == "Enemy")
        //        {
        //            _teleportTargetEnemy = hitTeleportEnemy.collider.gameObject;
        //        }
        //    }
        //    }


        if (!_attackClose._isAttackNow)
        {

            if (Input.GetMouseButtonDown(2))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))  //マウスのポジションからRayを投げて何かに当たったらhitに入れる
                {

                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        _cameraNomal.SetActive(false);     //ターゲット用カメラをOFF
                        _cameraFollow.SetActive(true);    //ターゲット用カメラをON


                        _virtualCamera.LookAt = hit.collider.gameObject.transform;
                        _targetEnemy = hit.collider.gameObject;
                        _targetUI.SetActive(true);
                        _targetting = true;
                        Debug.Log("Target"); //オブジェクト名をコンソールに表示            
                    }
                    else
                    {
                        _cameraNomal.SetActive(true);
                        _cameraFollow.SetActive(false);

                        _virtualCamera.LookAt = null;
                        _virtualCamera.transform.rotation = _orizinQuaternion;

                        _targetEnemy = null;
                        _targetting = false;
                        _targetUI.SetActive(false);
                    }


                }
            }

        }
        if (_targetEnemy == null)
        {
            _cameraNomal.SetActive(true);        //ターゲット用カメラをON
            _cameraFollow.SetActive(false);     //ターゲット用カメラをOFF
            _targetUI.SetActive(false);
        }
        if (_targetting && _targetEnemy != null)
        {

            myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _targetEnemy.transform.position + offset);

        }

    }



}
