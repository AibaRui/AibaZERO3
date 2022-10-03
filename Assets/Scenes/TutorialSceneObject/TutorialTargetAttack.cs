using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialTargetAttack : MonoBehaviour
{
    [SerializeField] GameObject _enemy;
    [SerializeField] GameObject _fadePanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_enemy==null)
        {
            StartCoroutine(Next());
        }
    }

    IEnumerator Next()
    {
        _fadePanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Home");
    }
}
