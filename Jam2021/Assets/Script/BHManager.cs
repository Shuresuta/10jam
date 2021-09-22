using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class BHManager : MonoBehaviour
{
    public GameManager gameManager;
    float GameTime;
    float GameTimeM;

    void OnEnable()
    {

        this.UpdateAsObservable()
           .First(x => GameTimeM > 118600f)
           .Subscribe(x => Next());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.PlayTime >= 1)
        {

            //�Ȃ��Đ�����Ă���̎���(�b��)
            GameTime = Time.time * 1 - (gameManager.PlayTime / 1000);
            //�Ȃ��Đ�����Ă���̎���(�~���b)
            GameTimeM = (GameTime * 1000) + 100;
            Debug.Log("GameTimeM BH" + GameTimeM);
        }
    }

    void Next()
    {
        SceneManager.LoadScene("GameOver");
    }
}