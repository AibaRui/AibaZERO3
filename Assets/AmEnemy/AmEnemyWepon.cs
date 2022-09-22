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
        // Rigidbody の活動を再開し、保存しておいた速度・回転を戻す

        _anim.enabled = true;
    }
}
