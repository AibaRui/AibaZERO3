using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
using UnityEngine.Playables; //Timeline�̐���ɕK�v


public class Tutoris : MonoBehaviour
{
    [Header("����������I�u�W�F�N�g")]
    [Tooltip("����������I�u�W�F�N�g")] [SerializeField] List<GameObject> _destroyObjects = new List<GameObject>();


    [Header("�Đ�����^�C�����C��")]
    [Tooltip("�Đ�����^�C�����C��")]
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
