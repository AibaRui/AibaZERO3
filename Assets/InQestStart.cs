using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
using UnityEngine.Playables; //Timelineの制御に必要


public class InQestStart : MonoBehaviour
{
    [Header("スタート時に出したいオブジェクト")]
    [Tooltip("スタート時に出したいオブジェクト")] [SerializeField] List<GameObject> _instantiateObjects = new List<GameObject>();

    [Header("スタート時にアクティブにしたいオブジェクト")]
    [Tooltip("スタート時にアクティブにしたいオブジェクト")]
    [SerializeField] List<GameObject> _activeObjects = new List<GameObject>();

    [Header("スタート時に非アクティブにしたいオブジェクト")]
    [Tooltip("スタート時に非アクティブにしたいオブジェクト")]
    [SerializeField] List<GameObject> _noActiveObjects = new List<GameObject>();


    [Header("スタート時に消すオブジェクト")]
    [Tooltip("スタート時に消すオブジェクト")]
    [SerializeField] List<GameObject> _destroyObjects = new List<GameObject>();

    [SerializeField] CinemachineVirtualCamera _main;



    [SerializeField] GameObject _player;
    [SerializeField] Vector3 _pos;


    [Header("再生するタイムライン")]
    [Tooltip("再生するタイムライン")]
    [SerializeField] PlayableDirector playableDirector;


    public void StartGame()
    {
        _activeObjects.ForEach(i => i.SetActive(true));
        _noActiveObjects.ForEach(i => i.SetActive(false));
        _destroyObjects.ForEach(i => Destroy(i));
        _main.Priority = 30;

        if (_player)
        {
            _player.transform.position = _pos;
        }

        Debug.Log("fef");
        if (playableDirector)
        {

            playableDirector.Play();
        }
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
