using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_efekutNorts : MonoBehaviour
{
    [SerializeField]
    int count;
    [SerializeField]
    GameObject pict;
    [SerializeField]
    GameObject pict2;
    [SerializeField]
    GameObject pict3;
    [SerializeField]
    GameObject pict4;
    [SerializeField]
    GameObject pict5;

    bool st = false;
    bool st2 = false;
    bool st3 = false;
    bool st4 = false;
    bool st5 = false;

    float wh;//width
    float hh;//hight
    float wh2;
    float hh2;
    float wh3;//width
    float hh3;//hight
    float wh4;
    float hh4;
    float wh5;//width
    float hh5;//hight

    RectTransform rectt;//recttransform
    RectTransform rectt2;//recttransform
    RectTransform rectt3;//recttransform
    RectTransform rectt4;//recttransform
    RectTransform rectt5;//recttransform

    void Start()
    {
        count = 0;
        wh = 30; hh = 30;
        wh2 = 40; hh2 = 40;
        wh3= 30; hh3 = 30;
        wh4 = 40; hh4 = 40;
        wh5 = 30; hh5 = 30;

    }
    void Update()
    {
        count += 1;
        RectTransform rectt = pict.GetComponent<RectTransform>(); rectt.sizeDelta = new Vector2(wh, hh);
        RectTransform rectt2 = pict2.GetComponent<RectTransform>(); rectt2.sizeDelta = new Vector2(wh2, hh2);
        RectTransform rectt3 = pict3.GetComponent<RectTransform>(); rectt3.sizeDelta = new Vector2(wh3, hh3);
        RectTransform rectt4 = pict4.GetComponent<RectTransform>(); rectt4.sizeDelta = new Vector2(wh4, hh4);
        RectTransform rectt5 = pict5.GetComponent<RectTransform>(); rectt5.sizeDelta = new Vector2(wh5, hh5);

        if (count > 100) { wh += 0.5f; hh += 0.5f; }//Žn‚Ü‚è
        if (count > 450) { st = true; }//I‚í‚è
        if (st == true) { wh = 30; hh = 30; st = false; }//I‚í‚è‚ÌŽž‚Í‚±‚¤

        if (count > 200) { wh2 += 0.5f; hh2 += 0.5f; }
        if (count > 750) { st2 = true; }
        if (st2 == true) { wh2 = 40; hh2 = 40; st2 = false; }

        if (count > 400) { wh3 += 0.5f; hh3 += 0.5f; }
        if (count > 800) { st3 = true; }
        if (st3 == true) { wh3 = 30; hh3 = 30; st3 = false; }

        if (count > 500) { wh4 += 0.5f; hh4 += 0.5f; }
        if (count > 950) { st4 = true; }
        if (st4 == true) { wh4 = 40; hh4 = 40; st4 = false; }

        if (count > 700) { wh5 += 0.5f; hh5 += 0.5f; }
        if (count > 1100) { st5 = true; }
        if (st5 == true) { wh5 = 30; hh5 = 30; count = 0; st5 = false; }
    }
}
