using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSG : MonoBehaviour
{
    public bool changeFlag1;
    public bool changeFlag2;
    // Start is called before the first frame update
    void Start()
    {
        changeFlag1 = false;
        changeFlag2 = false;
    }

    public void OnClickStartStage1Button()
    {
       
        if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Return))
        {
            changeFlag1 = true;
        }
    }

    public void OnClickStartStage2Button()
    {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            changeFlag2 = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(changeFlag1)
        {
            SceneManager.LoadScene("Select");
        }
        if (changeFlag2)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
