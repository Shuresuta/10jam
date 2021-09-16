using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public GameObject target;
    [SerializeField] float speed;

    void Update()
    {
        //自分の位置、ターゲット、速度
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
    }
}
