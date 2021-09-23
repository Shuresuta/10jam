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

    int state = 1;

    [SerializeField, Header("シャイニングスターPreのパス")]
    string ClipPath1;
    [SerializeField, Header("バーニングハートPreのパス")]
    string ClipPath2;
    AudioSource Music;

    // Start is called before the first frame update
    void Start()
    {
        Music = this.GetComponent<AudioSource>();

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

            state = 1;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //画像の表示
            musicImage2.SetActive(true);
            musicImage1.SetActive(false);

            //テキストを表示
            musicImageText2.SetActive(true);
            musicImageText1.SetActive(false);

            state = 2;
        }

        switch (state)
        {
            case 1:
                Music.clip = (AudioClip)Resources.Load(ClipPath1);
                Music.Play();
                state = 0;
                break;
            case 2:
                Music.clip = (AudioClip)Resources.Load(ClipPath2);
                Music.Play();
                state = 0;
                break;
        }

        
    }
}
