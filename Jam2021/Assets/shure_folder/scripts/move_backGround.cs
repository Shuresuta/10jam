using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class move_backGround : MonoBehaviour
{
    [SerializeField] GameObject back_image;
    
    private int counter = 0;
    private float move = -0.15f;
    private int pos_X;
    private int pos_y;
    private int pos_z;
    

    void Update()
    {
        this.transform.position += new Vector3(move, 0, 0);
        counter++;

        if (counter == 530)
        {
            counter = 0;
            this.transform.position = new Vector3(1300,318,0);
        }
    }
}
