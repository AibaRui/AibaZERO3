using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamagedControler : MonoBehaviour
{
    /// <summary>�����G�t�F�N�g���\������鎞��</summary>
    [SerializeField] float m_lifeTime = 0.1f;

    void Start()
    {
        // m_lifeTime �b��ɏ���
        Destroy(this.gameObject, m_lifeTime);
    }
}
