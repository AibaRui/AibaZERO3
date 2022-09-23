using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Q1Control : MonoBehaviour
{

    [SerializeField] string _name;
    [SerializeField] GameObject _fadePanel;

    [SerializeField] GameObject _enemy;

    [SerializeField] Transform[] _spownPointWave1 = new Transform[1];

    [SerializeField] Transform[] _spownPointWave2 = new Transform[1];

    [SerializeField] Transform[] _spownPointWave3 = new Transform[1];


    int _waveCount = 0;




    private void Update()
    {
        if (FindObjectsOfType<EnemyMoves>().Length==0)
        {
            if (_waveCount == 0)
            {
                Debug.Log("wave1");
                _waveCount++;
                for (int i = 0; i < _spownPointWave1.Length; i++)
                {
                    var go = Instantiate(_enemy);
                    go.transform.position = _spownPointWave1[i].position;
                }
            }
            else if (_waveCount == 1)
            {
                Debug.Log("wave2");
                _waveCount++;
                for (int i = 0; i < _spownPointWave2.Length; i++)
                {
                    var go = Instantiate(_enemy);
                    go.transform.position = _spownPointWave2[i].position;
                }
            }
            else if (_waveCount == 2)
            {
                Debug.Log("wave3");
                _waveCount++;
                for (int i = 0; i < _spownPointWave3.Length; i++)
                {
                    var go = Instantiate(_enemy);
                    go.transform.position = _spownPointWave3[i].position;
                }
            }
            else if(_waveCount==3)
            {
                _waveCount++;
                StartCoroutine(Lode());
            }
        }
    }


    IEnumerator Lode()
    {
        _fadePanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(_name);
    }
}



