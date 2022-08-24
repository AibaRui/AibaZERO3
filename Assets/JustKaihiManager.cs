using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustKaihiManager : MonoBehaviour
{
    public bool _isJustKaihi = false;
    /// <summary>true �̎��͈ꎞ��~�Ƃ���</summary>
    bool _justKaihiFlg = false;
    /// <summary>�ꎞ��~�E�ĊJ�𐧌䂷��֐��̌^�i�f���Q�[�g�j���`����</summary>
    public delegate void JustKaihi(bool isPause);
    /// <summary>�f���Q�[�g�����Ă����ϐ�</summary>
    JustKaihi _onJustKaihiResume = default;


    bool _isCount;
     float _time=0;
    [SerializeField] float _justLimitTime = 3;


    /// <summary>�ꎞ��~�E�ĊJ������f���Q�[�g�v���p�e�B</summary>
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

    /// <summary>�ꎞ��~�E�ĊJ��؂�ւ���</summary>
   public void JustKaihiResume()
    {
        _isCount = !_isCount;

        _justKaihiFlg = !_justKaihiFlg;
        _isJustKaihi = !_isJustKaihi;
        _onJustKaihiResume(_justKaihiFlg);  // ����ŕϐ��ɑ�������֐����i�S�āj�Ăяo����
    }



}
