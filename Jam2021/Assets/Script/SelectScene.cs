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
            Invoke("ChangeScene", 1.0f);//呼び出すメソッド、何秒後に移動するかの時間
            Debug.Log("ゲームオーバーのシーンへ");
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("GameOver");//次のシーンの名前
    }
}
