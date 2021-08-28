using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GlobalEvents : Singleton<GlobalEvents>
{
    public UnityEvent<string, bool> EventsInput = new UnityEvent<string, bool>();
    public UnityEvent<int> EventsChangeCamera = new UnityEvent<int>();
    public UnityEvent EventsToggleWorld = new UnityEvent();
    public UnityEvent EventsToggleUI = new UnityEvent();
    public UnityEvent<bool> EventsSetUI = new UnityEvent<bool>();
    public UnityEvent<int> EventsLoadVModalPreset = new UnityEvent<int>();
    public UnityEvent<int> EventsGlobal = new UnityEvent<int>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}