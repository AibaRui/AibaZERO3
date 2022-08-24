using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    Rigidbody _rb;
    bool _isDamage = false;
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        

    }



}


