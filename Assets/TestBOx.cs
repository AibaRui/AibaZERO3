using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBOx : MonoBehaviour
{

    float _time;
    float time = 1;

    JustKaihiManager _justKaihiManager;
    PauseManager _pauseManager = default;
    Vector3 _angularVelocity;
    Vector3 _velocity;

    Animator _anim;
    Rigidbody _rb;
    void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time > time)
        {
            _rb.AddForce(transform.up * 3, ForceMode.Impulse);
            _time = 0;
        }

    }




    private void Awake()
    {
        _pauseManager = GameObject.FindObjectOfType<PauseManager>();
        _justKaihiManager = GameObject.FindObjectOfType<JustKaihiManager>();
    }

    void OnEnable()
    {
        // 呼んで欲しいメソッドを登録する。
        _pauseManager.OnPauseResume += PauseResume;
        _anim = gameObject.GetComponent<Animator>();


        // 呼んで欲しいメソッドを登録する。
        _justKaihiManager.OnJustKaihiResume += PauseResumeJustKaihi;
        // _anim = gameObject.GetComponent<Animator>();
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
        // 速度・回転を保存し、Rigidbody を停止する
        _angularVelocity = _rb.angularVelocity;
        _velocity = _rb.velocity;
        _rb.velocity = new Vector3(_rb.velocity.x / 10, _rb.velocity.y / 10, _rb.velocity.z / 10);
        _anim.speed = 0.3f;
        time = 10;
    }

    public void ResumeJustKaihi()
    {
        // Rigidbody の活動を再開し、保存しておいた速度・回転を戻す
        _rb.WakeUp();
        _rb.angularVelocity = _angularVelocity;
        _rb.velocity = _velocity;
        _anim.speed = 1;
        time = 1;
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
        // 速度・回転を保存し、Rigidbody を停止する
        _angularVelocity = _rb.angularVelocity;
        _velocity = _rb.velocity;
        _rb.Sleep();
        _rb.isKinematic = true;
        _anim.enabled = false;
    }

    public void Resume()
    {
        // Rigidbody の活動を再開し、保存しておいた速度・回転を戻す
        _rb.WakeUp();
        _rb.angularVelocity = _angularVelocity;
        _rb.velocity = _velocity;
        _rb.isKinematic = false;

        _anim.enabled = true;
    }
}