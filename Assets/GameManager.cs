using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _gM;


    public int _skillHomeLevel=0;

    void Awake()
    {
        if (_gM == null)
        {
            _gM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }






    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
