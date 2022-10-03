using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsLife : MonoBehaviour
{
    [SerializeField] float _lifeTime;
    float _time;
    [SerializeField] AudioClip _au;
    AudioSource _aud;
    void Start()
    {
        _aud = gameObject.GetComponent<AudioSource>();
        if (_aud)
        {
            _aud.PlayOneShot(_au);
        }
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
