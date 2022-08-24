using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Qest : MonoBehaviour
{

    /// <summary>
    /// クエスト一覧を出す画像
    /// </summary>
    [SerializeField] GameObject _qestPanel;

    /// <summary> クエストの詳細を出す画像</summary>
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

  public  void _showQestPanel()             ///クエストパネルを出す。
    {
        _qestPanel.SetActive(true);
        _panel1.SetActive(false);
        _panel2.SetActive(false);
        _panel3.SetActive(false);
    }



   public void BackQest()        　　　　　/// <summary> クエスト選択画面に戻る。</summary>
    {
        _panel1.SetActive(false);
        _panel2.SetActive(false);
        _panel3.SetActive(false);
    }

  public  void BackGame()  　　　       　/// <summary> ゲーム画面に戻る。</summary>
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



  public  void SelectQest1()　　        　/// <summary> クエスト１の詳細を出す</summary>
    {
        _qestNumber = 1;
        _panel1.SetActive(true);　　
    }

    void SelectQest2()              /// <summary> クエスト２の詳細を出す</summary>
    {
        _qestNumber = 2;
        _panel2.SetActive(true);　　
    }

    void SelectQest3()　　　　　　　/// <summary> クエスト３の詳細を出す</summary>
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
