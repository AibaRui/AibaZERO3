using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class StopGameText : MonoBehaviour
{
    [SerializeField] GameObject _stopPanel;
    [SerializeField] List<AudioSource> _audio = new List<AudioSource>();
    PauseManager _pauseManager = default;


    private void Awake()
    {
        _pauseManager = FindObjectOfType<PauseManager>();
    }

    void OnEnable()
    {
        // 呼んで欲しいメソッドを登録する。
        _pauseManager.OnPauseResume += PauseResume;

    }

    void OnDisable()
    {
        // OnDisable ではメソッドの登録を解除すること。さもないとオブジェクトが無効にされたり破棄されたりした後にエラーになってしまう。
        _pauseManager.OnPauseResume -= PauseResume;

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
        _stopPanel.SetActive(true);
        _audio.ForEach(i => i.GetComponent<AudioSource>());
        _audio.ForEach(i => i.Stop());
    }

    public void Resume()
    {
        _stopPanel.SetActive(false);
        _audio.ForEach(i => i.GetComponent<AudioSource>());
        _audio.ForEach(i => i.Play());
    }
}

