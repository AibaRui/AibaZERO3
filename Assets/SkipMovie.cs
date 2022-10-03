using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipMovie : MonoBehaviour
{
    [SerializeField] string _sceneName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(_sceneName);
            Destroy(gameObject);
        }
    }
}
