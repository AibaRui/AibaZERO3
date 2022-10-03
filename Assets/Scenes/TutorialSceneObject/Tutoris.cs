using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
using UnityEngine.Playables; //Timelineの制御に必要


public class Tutoris : MonoBehaviour
{
    [Header("ついたら消すオブジェクト")]
    [Tooltip("ついたら消すオブジェクト")] [SerializeField] List<GameObject> _destroyObjects = new List<GameObject>();


    [Header("再生するタイムライン")]
    [Tooltip("再生するタイムライン")]
    [SerializeField] PlayableDirector playableDirector;

    [SerializeField] GameObject _fadePanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(NextTutorial());
        }
    }


    IEnumerator NextTutorial()
    {
        _fadePanel.SetActive(true);
        yield return new WaitForSeconds(1);
        playableDirector.Play();
        _fadePanel.SetActive(false);
        _destroyObjects.ForEach(i => Destroy(i));
        Destroy(gameObject);
    }

}
