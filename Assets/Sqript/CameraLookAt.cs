using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLookAt : MonoBehaviour
{
    [SerializeField] TargetSystem targetSystem;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    Transform _Camera;
    void Start()
    {
        _Camera.rotation = this.gameObject.transform.rotation;
    }

    
    void Update()
    {
        if(targetSystem.hit.collider.gameObject.tag=="Enemy"&& targetSystem._targetEnemy!=null)
        {
            _virtualCamera.LookAt = targetSystem._targetEnemy.transform;
        }
        else
        {
     
            _virtualCamera.LookAt = null;
            gameObject.transform.rotation = _Camera.rotation;
        }


    }
}
