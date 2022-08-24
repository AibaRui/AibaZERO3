using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHome : MonoBehaviour
{
    [SerializeField] GameObject _level0;
    [SerializeField] GameObject _level1;
    [SerializeField] GameObject _level2;
    [SerializeField] GameObject _level3;



   public void CheckSkillHome()
    {

        Debug.Log("aaa");
        if (GameManager._gM._skillHomeLevel==0)
        {
            _level0.SetActive(true);     Å@//0Çtrue
            _level1.SetActive(false);
            _level2.SetActive(false);
            _level3.SetActive(false);
            Debug.Log("bbb");
        }
       else if ( GameManager._gM._skillHomeLevel==1)
        {  
            _level0.SetActive(false);
            _level1.SetActive(true);        //1Çtru
            _level2.SetActive(false);
            _level3.SetActive(false);
            Debug.Log("ccc");
        }
        else if (GameManager._gM._skillHomeLevel == 2)
        {
            _level0.SetActive(false);
            _level1.SetActive(false);      
            _level2.SetActive(true);        //2Çtru
            _level3.SetActive(false);
            Debug.Log("ddd");
        }
        else if (GameManager._gM._skillHomeLevel == 3)
        {
            _level0.SetActive(false);
            _level1.SetActive(false);       
            _level2.SetActive(false);
            _level3.SetActive(true);    //3Çtru
            Debug.Log("eee");
        }
    }

}
