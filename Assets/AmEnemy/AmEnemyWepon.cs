using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmEnemyWepon : MonoBehaviour
{
    [SerializeField] LayerMask _layerBase;
    [SerializeField] LayerMask _layerNoHit;


    JustKaihiManager _justKaihiManager;
    PauseManager _pauseManager = default;
    Animator _anim;
    private void Awake()
    {
        _pauseManager = GameObject.FindObjectOfType<PauseManager>();
        _justKaihiManager = GameObject.FindObjectOfType<JustKaihiManager>();

    }

    void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_justKaihiManager._justKaihiFlg)
        {
            _anim.speed = 0.3f;
        }
        else
        {
            _anim.speed = 1;

        }
    }

    void OnEnable()
    {
        // �Ă�ŗ~�������\�b�h��o�^����B
        _pauseManager.OnPauseResume += PauseResume;
        _anim = gameObject.GetComponent<Animator>();


        // �Ă�ŗ~�������\�b�h��o�^����B
        _justKaihiManager.OnJustKaihiResume += PauseResumeJustKaihi;
        // _anim = gameObject.GetComponent<Animator>();
    }

    void OnDisable()
    {
        // OnDisable �ł̓��\�b�h�̓o�^���������邱�ƁB�����Ȃ��ƃI�u�W�F�N�g�������ɂ��ꂽ��j�����ꂽ�肵����ɃG���[�ɂȂ��Ă��܂��B
        _pauseManager.OnPauseResume -= PauseResume;


        // OnDisable �ł̓��\�b�h�̓o�^���������邱�ƁB�����Ȃ��ƃI�u�W�F�N�g�������ɂ��ꂽ��j�����ꂽ�肵����ɃG���[�ɂȂ��Ă��܂��B
        _justKaihiManager.OnJustKaihiResume -= PauseResumeJustKaihi;
    }

    void PauseResumeJustKaihi(bool isPause)
    {
        if (isPause)
        {
            PauseJustKaihi();
        }
        else
        {
            ResumeJustKaihi();
        }
    }

    public void PauseJustKaihi()
    {
        _anim.speed = 0.3f;
        gameObject.layer = _layerNoHit;
    }

    public void ResumeJustKaihi()
    {
        _anim.speed = 1;
        gameObject.layer = _layerBase;
    }


    void PauseResume(bool isPause)
    {
        if (isPause)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {

        _anim.enabled = false;
    }

    public void Resume()
    {
        // Rigidbody �̊������ĊJ���A�ۑ����Ă��������x�E��]��߂�

        _anim.enabled = true;
    }
}
