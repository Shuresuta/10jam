using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public GameObject target;
    [SerializeField] float speed;

    void Update()
    {
        //�����̈ʒu�A�^�[�Q�b�g�A���x
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
    }
}
