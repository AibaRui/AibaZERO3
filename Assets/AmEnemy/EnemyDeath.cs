using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    float _lifetimeCount;
    [SerializeField] float _lifetimeCountLimit;

    GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        transform.localScale = _player.transform.localScale;

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
