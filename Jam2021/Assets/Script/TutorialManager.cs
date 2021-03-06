using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject Text1;

    public GameManager gameManager;
    float GameTime;
    float GameTimeM;

    void OnEnable()
    {
        this.UpdateAsObservable()
           .First(x => GameTimeM > 13000f)
           .Subscribe(x => TutorialText());

        this.UpdateAsObservable()
           .First(x => GameTimeM > 30000f)
           .Subscribe(x => Next());

        //escape
        //this.UpdateAsObservable()
        // .Where(_ => !isPlaying)
        // .Where(_ => Input.GetKeyDown(KeyCode.Escape))
        // .Subscribe(_ => Next());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.PlayTime >= 1)
        {

            //曲が再生されてからの時間(秒数)
            GameTime = Time.time * 1 - (gameManager.PlayTime / 1000);
            //曲が再生されてからの時間(ミリ秒)
            GameTimeM = (GameTime * 1000) + 100;
            Debug.Log("GameTimeM"+GameTimeM);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("SZK");
        }
    }

    void TutorialText()
    {
        Text1.SetActive(false);
    }

    void Next()
    {
        SceneManager.LoadScene("SZK");
    }
}
