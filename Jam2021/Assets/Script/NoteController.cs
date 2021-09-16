using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class NoteController : MonoBehaviour
{
    //ノーツの種類
    string Type;
    //ノーツの発射タイミング
    float Timing;

    //初期位置から判定ラインまでの距離
    float Distance;
    //初期位置から判定ラインまでの時間
    float During;

    //ノーツの初期位置
    Vector3 firstPos;
    //ノーツが動いているか
    bool isGo;
    float GoTime;

    void OnEnable()
    {
        isGo = false;
        firstPos = this.transform.position;

        //ノーツの位置を計算して動かす
        this.UpdateAsObservable()
          .Where(_ => isGo)
          .Subscribe(_ => {
              this.gameObject.transform.position = new Vector3(firstPos.x - Distance * (Time.time * 1000 - GoTime) / During, firstPos.y, firstPos.z);
          });
    }

    //ノーツを出現させるとき
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

    //ノーツの発射タイミング
    public void go(float distance, float during)
    {
        Distance = distance;
        During = during;
        GoTime = Time.time * 1000;

        isGo = true;
    }
}