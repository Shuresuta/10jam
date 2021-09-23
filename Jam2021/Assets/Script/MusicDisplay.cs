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
            //�摜�̕\��
            musicImage1.SetActive(true);
            musicImage2.SetActive(false);

            //�e�L�X�g��\��
            musicImageText1.SetActive(true);
            musicImageText2.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //�摜�̕\��
            musicImage2.SetActive(true);
            musicImage1.SetActive(false);

            //�e�L�X�g��\��
            musicImageText2.SetActive(true);
            musicImageText1.SetActive(false);
        }
        
    }
}
