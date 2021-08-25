using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode UI_Toggle = KeyCode.Space;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(UI_Toggle))
        {
            GlobalEvents.Instance.EventsInput.Invoke("UI_Toggle");
        }
    }
}
