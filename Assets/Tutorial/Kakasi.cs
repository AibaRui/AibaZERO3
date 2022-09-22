using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kakasi : MonoBehaviour
{
    int _hp = 5;
    Animator _anim;
    void Start()
    {
        _anim = gameObject.GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "P_Attack")
        {
            _hp--;
            _anim.Play("kakasiDamage");
        }
    }
}
