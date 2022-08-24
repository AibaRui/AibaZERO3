using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaihiEffects : MonoBehaviour
{
    P_Kaihi _pKaihi;

    /// <summary>エフェクトの管理</summary>
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
