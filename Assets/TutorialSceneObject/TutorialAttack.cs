using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; //Timelineの制御に必要

public class TutorialAttack : MonoBehaviour
{
    [Header("敵オブジェクト")]
    [Tooltip("敵オブジェクト")] [SerializeField] List<GameObject> _enemyObjects = new List<GameObject>();

    [Header("再生するタイムライン")]
    [Tooltip("再生するタイムライン")]
    [SerializeField] PlayableDirector playableDirector;

    [SerializeField] GameObject _fadePanel;

    [SerializeField] GameObject _player;


    bool _is = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyObjects[0] == null)
        {
            if (_enemyObjects[1] == null && !_is)
            {
                _is = true;
                StartCoroutine(Next());
            }

        }
    }

    IEnumerator Next()
    {
        _fadePanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        _fadePanel.SetActive(false);
        _player.SetActive(false);
        playableDirector.Play();
        Destroy(gameObject);
    }

}
