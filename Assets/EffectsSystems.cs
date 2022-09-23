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
        // 呼んで欲しいメソッドを登録する。
        _pauseManager.OnPauseResume += PauseResume;
        _anim = gameObject.GetComponent<Animator>();
        _rb = gameObject.GetComponent<Rigidbody>();

        // 呼んで欲しいメソッドを登録する。
        _justKaihiManager.OnJustKaihiResume += PauseResumeJustKaihi;
    }

    void OnDisable()
    {
        // OnDisable ではメソッドの登録を解除すること。さもないとオブジェクトが無効にされたり破棄されたりした後にエラーになってしまう。
        _pauseManager.OnPauseResume -= PauseResume;


        // OnDisable ではメソッドの登録を解除すること。さもないとオブジェクトが無効にされたり破棄されたりした後にエラーになってしまう。
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
                // 速度・回転を保存し、Rigidbody を停止する
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
            // Rigidbody の活動を再開し、保存しておいた速度・回転を戻す
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
            // 速度・回転を保存し、Rigidbody を停止する
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
            // Rigidbody の活動を再開し、保存しておいた速度・回転を戻す
            _rb.WakeUp();
            //   _rb.angularVelocity = _angularVelocity;
            _rb.velocity = _velocity;
            _rb.isKinematic = false;
        }
    }
}
