using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour
{
    [SerializeField] public Button[] buttons;
    [SerializeField] private int firstSelectButton;
    public bool callSet;

    // Start is called before the first frame update
    void Start()
    {
        buttons[firstSelectButton].Select();

        callSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            Selects();
        }
    }

    public void Selects()
    {
        buttons[firstSelectButton].Select();
    }
}
