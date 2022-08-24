using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeLevelCheck : MonoBehaviour
{
    [SerializeField] SkillHome _skillHome;



    private void Awake()
    {
        StartCoroutine(Check());
    }

    
    IEnumerator Check()
    {
        yield return new WaitForSeconds(0.2f);
        _skillHome.CheckSkillHome();

    }
}
