using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsLife : MonoBehaviour
{
    [SerializeField] float _lifeTime;
    float _time;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if(_lifeTime<=_time)
        {
            Destroy(gameObject);
        }
    }
}
