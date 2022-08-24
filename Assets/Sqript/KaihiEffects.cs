using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaihiEffects : MonoBehaviour
{
    P_Kaihi _pKaihi;

    /// <summary>�G�t�F�N�g�̊Ǘ�</summary>
    private void Start()
    {
        _pKaihi = FindObjectOfType<P_Kaihi>();
    }
    void EndDestroy()
    {
        Debug.Log("EnDD");
        _pKaihi._isDodging = false;
        Destroy(this.gameObject);
    }
}
