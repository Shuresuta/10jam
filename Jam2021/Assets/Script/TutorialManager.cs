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
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.PlayTime >= 1)
        {

            //‹È‚ªÄ¶‚³‚ê‚Ä‚©‚ç‚ÌŠÔ(•b”)
            GameTime = Time.time * 1 - (gameManager.PlayTime / 1000);
            //‹È‚ªÄ¶‚³‚ê‚Ä‚©‚ç‚ÌŠÔ(ƒ~ƒŠ•b)
            GameTimeM = (GameTime * 1000) + 100;
            Debug.Log("GameTimeM"+GameTimeM);
        }
    }

    void TutorialText()
    {
        Text1.SetActive(false);
    }

    void Next()
    {
        SceneManager.LoadScene("Select");
    }
}
