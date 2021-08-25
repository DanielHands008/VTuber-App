using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GlobalEvents : Singleton<GlobalEvents>
{
    public UnityEvent<string> EventsInput = new UnityEvent<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}