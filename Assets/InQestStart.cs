using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
using UnityEngine.Playables; //Timeline�̐���ɕK�v


public class InQestStart : MonoBehaviour
{
    [Header("�X�^�[�g���ɏo�������I�u�W�F�N�g")]
    [Tooltip("�X�^�[�g���ɏo�������I�u�W�F�N�g")] [SerializeField] List<GameObject> _instantiateObjects = new List<GameObject>();

    [Header("�X�^�[�g���ɃA�N�e�B�u�ɂ������I�u�W�F�N�g")]
    [Tooltip("�X�^�[�g���ɃA�N�e�B�u�ɂ������I�u�W�F�N�g")]
    [SerializeField] List<GameObject> _activeObjects = new List<GameObject>();

    [Header("�X�^�[�g���ɔ�A�N�e�B�u�ɂ������I�u�W�F�N�g")]
    [Tooltip("�X�^�[�g���ɔ�A�N�e�B�u�ɂ������I�u�W�F�N�g")]
    [SerializeField] List<GameObject> _noActiveObjects = new List<GameObject>();


    [Header("�X�^�[�g���ɏ����I�u�W�F�N�g")]
    [Tooltip("�X�^�[�g���ɏ����I�u�W�F�N�g")]
    [SerializeField] List<GameObject> _destroyObjects = new List<GameObject>();

    [SerializeField] CinemachineVirtualCamera _main;



    [SerializeField] GameObject _player;
    [SerializeField] Vector3 _pos;


    [Header("�Đ�����^�C�����C��")]
    [Tooltip("�Đ�����^�C�����C��")]
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
