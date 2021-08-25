using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GlobalEvents : Singleton<GlobalEvents>
{
    public UnityEvent<string, bool> EventsInput = new UnityEvent<string, bool>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}