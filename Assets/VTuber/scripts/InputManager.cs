using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public KeyboardInputs keyboardInputs = new KeyboardInputs();


    // Ok so this class sucks there must be a better way. Change to use a dictonary
    public class KeyboardInputs
    {
        public KeyboardInputs()
        {
            // Default Keybinds.
            keyBindings.Add("UI_Toggle", KeyCode.Space);
            keyBindings.Add("Move_Forward", KeyCode.W);
            keyBindings.Add("Move_Left", KeyCode.A);
            keyBindings.Add("Move_Back", KeyCode.S);
            keyBindings.Add("Move_Right", KeyCode.D);
            keyBindings.Add("Move_Fast", KeyCode.LeftShift);
            keyBindings.Add("Move_HeightLock", KeyCode.LeftControl);
            keyBindings.Add("Camera_RotateLeft", KeyCode.Q);
            keyBindings.Add("Camera_RotateRight", KeyCode.E);
            keyBindings.Add("Camera_RotateReset", KeyCode.R);
            keyBindings.Add("Camera_ZoomModifier", KeyCode.LeftControl);
            keyBindings.Add("Camera_SelectDynamic", KeyCode.Tab);
            keyBindings.Add("Camera_Select1", KeyCode.Alpha1);
            keyBindings.Add("Camera_Select2", KeyCode.Alpha2);
            keyBindings.Add("Camera_Select3", KeyCode.Alpha3);
            keyBindings.Add("Camera_Select4", KeyCode.Alpha4);
            keyBindings.Add("Camera_Select5", KeyCode.Alpha5);
            keyBindings.Add("Camera_Select6", KeyCode.Alpha6);
            keyBindings.Add("Camera_SaveModifier", KeyCode.LeftControl);
            keyBindings.Add("World_Toggle", KeyCode.H);
            keyBindings.Add("VModel_RotateModifier", KeyCode.LeftAlt);
            keyBindings.Add("VModel_SnapZModifier", KeyCode.LeftControl);
            keyBindings.Add("VModel_SnapXModifier", KeyCode.LeftShift);
        }
        public string[] GetInputNames()
        {
            return new List<string>(keyBindings.Keys).ToArray();
        }
        public KeyCode GetKey(string inputName)
        {
            if (keyBindings.ContainsKey(inputName))
                return keyBindings[inputName];
            return KeyCode.None;
        }
        public bool SetKey(string inputName, KeyCode NewKey)
        {
            if (keyBindings.ContainsKey(inputName))
            {
                keyBindings[inputName] = NewKey;
            }
            return false;
        }
        public int Length
        {
            get
            {
                return keyBindings.Count;
            }
        }
        Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    }

    string KeyToRebind = "";
    void Start()
    {
    }
    void Update()
    {
    }

    void OnGUI()
    {
        if (KeyToRebind != "" && Input.anyKeyDown && Event.current.isKey && Event.current.type == EventType.KeyDown)
        {
            Debug.Log(Event.current.keyCode);
            keyboardInputs.SetKey(KeyToRebind, Event.current.keyCode);
            KeyToRebind = "";
            GlobalEvents.Instance.EventsUI.Invoke("RebindComplete");
        }
    }

    public void RebindKey(string keyName)
    {
        KeyToRebind = keyName;
    }

    // Add methods for saving, loading and setting controls.
}
