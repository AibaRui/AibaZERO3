using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Qest1Goal : MonoBehaviour
{
    
    void Animations()
    {

    }
   
    void GoHome()
    {
        if(GameManager._gM._skillHomeLevel==0)     //スキルホームが 0　だったら　１　　にする
        {
            GameManager._gM._skillHomeLevel = 1;
        }

        SceneManager.LoadScene("Home");
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
             Animations();    　//クエスト終了時のアニメーションを流す
            GoHome();
        }

    }

}
