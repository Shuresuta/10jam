using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


public class MusicDisplay : MonoBehaviour
{
   
    //[SerializeField] Image image;

    [SerializeField, Header("曲１の画像")]
    GameObject musicImage1;

    [SerializeField, Header("曲2の画像")]
    GameObject musicImage2;

    [SerializeField, Header("曲１のテキスト")]
    GameObject musicImageText1;

    [SerializeField, Header("曲2のテキスト")]
    GameObject musicImageText2;

    int state;

    // Start is called before the first frame update
    void Start()
    {
        musicImage1.SetActive(true);
        musicImage2.SetActive(false);

        musicImageText1.SetActive(true);
        musicImageText2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //画像の表示
            musicImage1.SetActive(true);
            musicImage2.SetActive(false);

            //テキストを表示
            musicImageText1.SetActive(true);
            musicImageText2.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //画像の表示
            musicImage2.SetActive(true);
            musicImage1.SetActive(false);

            //テキストを表示
            musicImageText2.SetActive(true);
            musicImageText1.SetActive(false);
        }
        
    }
}
