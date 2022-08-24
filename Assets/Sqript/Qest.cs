using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Qest : MonoBehaviour
{

    /// <summary>
    /// �N�G�X�g�ꗗ���o���摜
    /// </summary>
    [SerializeField] GameObject _qestPanel;

    /// <summary> �N�G�X�g�̏ڍׂ��o���摜</summary>
    [SerializeField] GameObject _panel1;
    [SerializeField] GameObject _panel2;
    [SerializeField] GameObject _panel3;

    [SerializeField] GameObject _textGoQest;

    bool _inQestZone=false;

    int _qestNumber = 0;

    private void Update()
    {
        if(_inQestZone&&Input.GetKeyDown(KeyCode.Space))
        {
            _showQestPanel();
        }
    }

  public  void _showQestPanel()             ///�N�G�X�g�p�l�����o���B
    {
        _qestPanel.SetActive(true);
        _panel1.SetActive(false);
        _panel2.SetActive(false);
        _panel3.SetActive(false);
    }



   public void BackQest()        �@�@�@�@�@/// <summary> �N�G�X�g�I����ʂɖ߂�B</summary>
    {
        _panel1.SetActive(false);
        _panel2.SetActive(false);
        _panel3.SetActive(false);
    }

  public  void BackGame()  �@�@�@       �@/// <summary> �Q�[����ʂɖ߂�B</summary>
    {
        _qestPanel.SetActive(false);
    }

   public void GoQest()
    {
        if(_qestNumber==1)
        {
            SceneManager.LoadScene("Qest1");
        }
        //else if (_qestNumber == 2)
        //{
        //    // SceneManager.LoadScene();
        //}
        //else if (_qestNumber == 3)
        //{
        //    // SceneManager.LoadScene();
        //}
    }



  public  void SelectQest1()�@�@        �@/// <summary> �N�G�X�g�P�̏ڍׂ��o��</summary>
    {
        _qestNumber = 1;
        _panel1.SetActive(true);�@�@
    }

    void SelectQest2()              /// <summary> �N�G�X�g�Q�̏ڍׂ��o��</summary>
    {
        _qestNumber = 2;
        _panel2.SetActive(true);�@�@
    }

    void SelectQest3()�@�@�@�@�@�@�@/// <summary> �N�G�X�g�R�̏ڍׂ��o��</summary>
    {
        _qestNumber = 3;
        _panel3.SetActive(true);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            _textGoQest.SetActive(true);
            _inQestZone = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _textGoQest.SetActive(false);
            _inQestZone = false;
        }
    }

}
