using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBox : MonoBehaviour
{
    List<GameObject> e = new List<GameObject>();
    private void Update()
    {
        if(transform.childCount!=0)
        {
            for(int i=0;i<transform.childCount;i++)
            {
              var a = transform.GetChild(i);
                a.transform.position = Vector3.zero;
            }

        }

    }

}
