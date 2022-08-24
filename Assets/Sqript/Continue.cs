using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continue : MonoBehaviour
{
    [SerializeField] GameObject _player;
    private void Awake()
    {
        _player.transform.position = this.gameObject.transform.position;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
