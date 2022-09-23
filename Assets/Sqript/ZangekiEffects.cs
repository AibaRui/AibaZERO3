using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZangekiEffects : MonoBehaviour
{
    float _lifetimeCount;
    [SerializeField] float _lifetimeCountLimit;
    [SerializeField] AudioClip _au;
    AudioSource _aud;

    GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        transform.localScale = _player.transform.localScale;
        _aud = gameObject.GetComponent<AudioSource>();
        if (_aud)
        {
            _aud.PlayOneShot(_au);
        }

    }

    void Update()
    {
        _lifetimeCount += Time.deltaTime;
        if (_lifetimeCount > _lifetimeCountLimit)
        {
            Destroy(gameObject);
        }
    }
}
