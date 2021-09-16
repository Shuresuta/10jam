using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class NoteController : MonoBehaviour
{
    //�m�[�c�̎��
    string Type;
    //�m�[�c�̔��˃^�C�~���O
    float Timing;

    //�����ʒu���画�胉�C���܂ł̋���
    float Distance;
    //�����ʒu���画�胉�C���܂ł̎���
    float During;

    //�m�[�c�̏����ʒu
    Vector3 firstPos;
    //�m�[�c�������Ă��邩
    bool isGo;
    float GoTime;

    void OnEnable()
    {
        isGo = false;
        firstPos = this.transform.position;

        //�m�[�c�̈ʒu���v�Z���ē�����
        this.UpdateAsObservable()
          .Where(_ => isGo)
          .Subscribe(_ => {
              this.gameObject.transform.position = new Vector3(firstPos.x - Distance * (Time.time * 1000 - GoTime) / During, firstPos.y, firstPos.z);
          });
    }

    //�m�[�c���o��������Ƃ�
    public void setParameter(string type, float timing)
    {
        Type = type;
        Timing = timing;
    }

    public string getType()
    {
        return Type;
    }

    public float getTiming()
    {
        return Timing;
    }

    //�m�[�c�̔��˃^�C�~���O
    public void go(float distance, float during)
    {
        Distance = distance;
        During = during;
        GoTime = Time.time * 1000;

        isGo = true;
    }
}