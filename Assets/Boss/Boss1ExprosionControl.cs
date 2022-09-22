using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1ExprosionControl : MonoBehaviour
{
    [SerializeField] GameObject _exprosion;

   public void Exe()
    {
        var go = Instantiate(_exprosion);
        go.transform.position = transform.position;
    }
}
