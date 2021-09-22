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

    int state;

    // Start is called before the first frame update
    void Start()
    {
        musicImage1.SetActive(true);
        musicImage2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            musicImage1.SetActive(true);
            musicImage2.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            musicImage2.SetActive(true);
            musicImage1.SetActive(false);
        }
        
    }
}
