using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Invoke("ChangeScene", 1.0f);//�Ăяo�����\�b�h�A���b��Ɉړ����邩�̎���
            Debug.Log("�Q�[���I�[�o�[�̃V�[����");
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("GameOver");//���̃V�[���̖��O
    }
}