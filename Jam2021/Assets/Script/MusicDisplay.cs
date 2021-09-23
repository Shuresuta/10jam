using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;


public class MusicDisplay : MonoBehaviour
{
   
    //[SerializeField] Image image;

    [SerializeField, Header("�ȂP�̉摜")]
    GameObject musicImage1;

    [SerializeField, Header("��2�̉摜")]
    GameObject musicImage2;

    [SerializeField, Header("�ȂP�̃e�L�X�g")]
    GameObject musicImageText1;

    [SerializeField, Header("��2�̃e�L�X�g")]
    GameObject musicImageText2;

    int state = 1;

    [SerializeField, Header("�V���C�j���O�X�^�[Pre�̃p�X")]
    string ClipPath1;
    [SerializeField, Header("�o�[�j���O�n�[�gPre�̃p�X")]
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
            //�摜�̕\��
            musicImage1.SetActive(true);
            musicImage2.SetActive(false);

            //�e�L�X�g��\��
            musicImageText1.SetActive(true);
            musicImageText2.SetActive(false);

            state = 1;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //�摜�̕\��
            musicImage2.SetActive(true);
            musicImage1.SetActive(false);

            //�e�L�X�g��\��
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
