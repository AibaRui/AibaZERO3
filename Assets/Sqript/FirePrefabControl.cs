using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePrefabControl : MonoBehaviour
{
    [SerializeField] float _speed = 6;

    float _time = 0;
    [SerializeField] float _destroyTime = 3;

    void Update()
    {



        _time += Time.deltaTime;

        if (_time > _destroyTime)
        {
            Destroy(gameObject);
        }

    }

}