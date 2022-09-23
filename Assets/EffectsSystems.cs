using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsSystems : MonoBehaviour
{
    JustKaihiManager _justKaihiManager;
    PauseManager _pauseManager = default;

    Vector3 _angularVelocity;
    Vector3 _velocity;

    [SerializeField] bool _isPlayer = false;


    Animator _anim;
    Rigidbody _rb;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        _justKaihiManager = FindObjectOfType<JustKaihiManager>();
        _pauseManager = FindObjectOfType<PauseManager>();
    }

    void OnEnable()
    {
        // �Ă�ŗ~�������\�b�h��o�^����B
        _pauseManager.OnPauseResume += PauseResume;
        _anim = gameObject.GetComponent<Animator>();
        _rb = gameObject.GetComponent<Rigidbody>();

        // �Ă�ŗ~�������\�b�h��o�^����B
        _justKaihiManager.OnJustKaihiResume += PauseResumeJustKaihi;
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
        if (!_isPlayer)
        {
            Debug.Log("Slow");
            _anim.speed = 0.3f;

            if (_rb)
            {
                // ���x�E��]��ۑ����ARigidbody ���~����
                //   _angularVelocity = _rb.angularVelocity;
                _velocity = _rb.velocity;
                _rb.velocity = new Vector3(_rb.velocity.x / 10, _rb.velocity.y / 10, 0);
            }
        }
    }

    public void ResumeJustKaihi()
    {
        _anim.speed = 1;

        if (_rb)
        {
            // Rigidbody �̊������ĊJ���A�ۑ����Ă��������x�E��]��߂�
            _rb.WakeUp();
            // _rb.angularVelocity = _angularVelocity;
            _rb.velocity = _velocity;
        }
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

        if (_rb)
        {
            // ���x�E��]��ۑ����ARigidbody ���~����
            //   _angularVelocity = _rb.angularVelocity;
            _velocity = _rb.velocity;
            _rb.Sleep();
            _rb.isKinematic = true;
        }
    }

    public void Resume()
    {
        _anim.enabled = true;

        if (_rb)
        {
            // Rigidbody �̊������ĊJ���A�ۑ����Ă��������x�E��]��߂�
            _rb.WakeUp();
            //   _rb.angularVelocity = _angularVelocity;
            _rb.velocity = _velocity;
            _rb.isKinematic = false;
        }
    }
}
