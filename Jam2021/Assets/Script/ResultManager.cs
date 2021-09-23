using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    //テキスト表示
    [SerializeField] Text ScoreText;
    [SerializeField] Text GreatText;
    [SerializeField] Text GoodText;
    [SerializeField] Text MissText;
    [SerializeField] Text TitleText;

    
    [SerializeField] Text RANKText;

    [SerializeField] GameObject CLEAR;
    [SerializeField] GameObject FAILURE;

    //スコア関連代入
    float Score;
    float Gage;
    float GreatNum;
    float GoodNum;
    float MissNum;
    string Title;
    int SelectNum;
    string Rank;

    // Start is called before the first frame update
    void Start()
    {
        Score = GameManager.getScore();
        Gage = GameManager.getGage();
        GreatNum = GameManager.getGreat();
        GoodNum = GameManager.getGood();
        MissNum = GameManager.getMiss();
        Title = GameManager.getTitle();

        ScoreText.text = Score.ToString();
        GreatText.text = GreatNum.ToString();
        GoodText.text = GoodNum.ToString();
        MissText.text = MissNum.ToString();
        TitleText.text = Title.ToString();

        if (Gage > 0.7f)
        {
            CLEAR.SetActive(true);
        }
        else
        {
            FAILURE.SetActive(true);
        }

        if (Score >= 100000) { Rank = "S"; }
        else if (Score >= 90000) { Rank = "AAA"; }
        else if (Score >= 80000) { Rank = "AA"; }
        else if (Score >= 70000) { Rank = "A"; }
        else if (Score >= 60000) { Rank = "B"; }
        else if (Score >= 50000) { Rank = "C"; }
        else if (Score <= 49999) { Rank = "D"; }
        RANKText.text = Rank;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Return))
        {
            CLEAR.SetActive(false);
            FAILURE.SetActive(false);
            SceneManager.LoadScene("Select");
        }

    }
   
}
