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
        if(GameManager._gM._skillHomeLevel==0)     //�X�L���z�[���� 0�@��������@�P�@�@�ɂ���
        {
            GameManager._gM._skillHomeLevel = 1;
        }

        SceneManager.LoadScene("Home");
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
             Animations();    �@//�N�G�X�g�I�����̃A�j���[�V�����𗬂�
            GoHome();
        }

    }

}
