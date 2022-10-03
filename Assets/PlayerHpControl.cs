using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerHpControl : MonoBehaviour
{
    [SerializeField] Text _hpText;

    [SerializeField] GameObject _hpTexts;

    [SerializeField] GameObject _gameOverPanel;

    [SerializeField] List<AudioSource> _audio = new List<AudioSource>();


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ChangeHpText(int i)
    {
        _hpText.text = i.ToString();

        if (i == 0)
        {
            _audio.ForEach(i => i.GetComponent<AudioSource>());
            _audio.ForEach(i => i.Stop());
            var go = FindObjectsOfType<EnemyMoves>();
            if (go.Length != 0)
            {
                go.ToList().ForEach(i => Destroy(i));
            }


            var go2 = FindObjectsOfType<BossControl>();
            if (go2.Length != 0)
            {
                go.ToList().ForEach(i => Destroy(i));
            }


            _hpTexts.SetActive(false);
            _gameOverPanel.SetActive(true);
        }
    }
}
