using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    public bool _isPause = false;
    /// <summary>true の時は一時停止とする</summary>
    bool _pauseFlg = false;
    /// <summary>一時停止・再開を制御する関数の型（デリゲート）を定義する</summary>
    public delegate void Pause(bool isPause);

    /// <summary>デリゲートを入れておく変数</summary>
    Pause _onPauseResume = default;

    /// <summary>一時停止・再開を入れるデリゲートプロパティ</summary>
    public Pause OnPauseResume
    {
        get { return _onPauseResume; }
        set { _onPauseResume = value; }
    }

    void Update()
    {
        // ESC キーが押されたら一時停止・再開を切り替える
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResume();
        }
    }

    /// <summary>一時停止・再開を切り替える</summary>
    void PauseResume()
    {
        _pauseFlg = !_pauseFlg;
        _isPause = !_isPause;

        if (_onPauseResume != null)
        {
            _onPauseResume(_pauseFlg);  // これで変数に代入した関数を（全て）呼び出せる
        }

    }
}
