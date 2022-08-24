using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] GameObject _cameraBefor;
    [SerializeField] GameObject _cameraNew;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            _cameraBefor.SetActive(false);
            _cameraNew.SetActive(true);
        }

    }

}
