using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustKaihiManager : MonoBehaviour
{
    public bool _isJustKaihi = false;
    /// <summary>true の時は一時停止とする</summary>
    bool _justKaihiFlg = false;
    /// <summary>一時停止・再開を制御する関数の型（デリゲート）を定義する</summary>
    public delegate void JustKaihi(bool isPause);
    /// <summary>デリゲートを入れておく変数</summary>
    JustKaihi _onJustKaihiResume = default;


    bool _isCount;
     float _time=0;
    [SerializeField] float _justLimitTime = 3;


    /// <summary>一時停止・再開を入れるデリゲートプロパティ</summary>
    public JustKaihi OnJustKaihiResume
    {
        get { return _onJustKaihiResume; }
        set { _onJustKaihiResume = value; }
    }

    void Update()
    {
            if(_isCount)
        {
            _time += Time.deltaTime;

            if(_time>_justLimitTime)
            {
                _time = 0;
                JustKaihiResume();
            }
        }



    }

    /// <summary>一時停止・再開を切り替える</summary>
   public void JustKaihiResume()
    {
        _isCount = !_isCount;

        _justKaihiFlg = !_justKaihiFlg;
        _isJustKaihi = !_isJustKaihi;
        _onJustKaihiResume(_justKaihiFlg);  // これで変数に代入した関数を（全て）呼び出せる
    }



}
