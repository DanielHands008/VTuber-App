using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : Singleton<InputManager>
{
    public string settingsFile = "keybinds";
    string KeyToRebind = "";

    public SerializableDictionary<string, KeyCode> keyBindings = new SerializableDictionary<string, KeyCode>{
            new SerializableDictionary<string, KeyCode>.Pair("UI_Toggle", KeyCode.Space),
            new SerializableDictionary<string, KeyCode>.Pair("Move_Forward", KeyCode.W),
            new SerializableDictionary<string, KeyCode>.Pair("Move_Left", KeyCode.A),
            new SerializableDictionary<string, KeyCode>.Pair("Move_Back", KeyCode.S),
            new SerializableDictionary<string, KeyCode>.Pair("Move_Right", KeyCode.D),
            new SerializableDictionary<string, KeyCode>.Pair("Move_Fast", KeyCode.LeftShift),
            new SerializableDictionary<string, KeyCode>.Pair("Move_HeightLock", KeyCode.LeftControl),
            new SerializableDictionary<string, KeyCode>.Pair("Camera_RotateLeft", KeyCode.Q),
            new SerializableDictionary<string, KeyCode>.Pair("Camera_RotateRight", KeyCode.E),
            new SerializableDictionary<string, KeyCode>.Pair("Camera_RotateReset", KeyCode.R),
            new SerializableDictionary<string, KeyCode>.Pair("Camera_SelectDynamic", KeyCode.Tab),
            new SerializableDictionary<string, KeyCode>.Pair("Camera_Select1", KeyCode.Alpha1),
            new SerializableDictionary<string, KeyCode>.Pair("Camera_Select2", KeyCode.Alpha2),
            new SerializableDictionary<string, KeyCode>.Pair("Camera_Select3", KeyCode.Alpha3),
            new SerializableDictionary<string, KeyCode>.Pair("Camera_Select4", KeyCode.Alpha4),
            new SerializableDictionary<string, KeyCode>.Pair("Camera_Select5", KeyCode.Alpha5),
            new SerializableDictionary<string, KeyCode>.Pair("Camera_Select6", KeyCode.Alpha6),
            new SerializableDictionary<string, KeyCode>.Pair("Camera_SaveModifier", KeyCode.LeftControl),
            new SerializableDictionary<string, KeyCode>.Pair("World_Toggle", KeyCode.H),
            new SerializableDictionary<string, KeyCode>.Pair("VModel_RotateModifier", KeyCode.LeftAlt),
            new SerializableDictionary<string, KeyCode>.Pair("VModel_SnapZModifier", KeyCode.LeftControl),
            new SerializableDictionary<string, KeyCode>.Pair("VModel_SnapXModifier", KeyCode.LeftShift),
    };

    public KeyCode Get(string input)
    {
        if (keyBindings.ContainsKey(input))
            return keyBindings[input];
        return KeyCode.None;
    }

    public void Set(string input, KeyCode NewKey)
    {
        if (keyBindings.ContainsKey(input))
        {
            keyBindings[input] = NewKey;
        }
        else
            keyBindings.Add(new SerializableDictionary<string, KeyCode>.Pair(input, NewKey));
    }

    public void Remove(string input)
    {
        Set(input, KeyCode.None);
        SaveKeybinds();
    }

    public string[] GetInputNames()
    {
        // SerializableDictionary is unordered, return list in desired order.
        return new string[] {
            "UI_Toggle",
            "Move_Forward",
            "Move_Left",
            "Move_Back",
            "Move_Right",
            "Move_Fast",
            "Move_HeightLock",
            "Camera_RotateLeft",
            "Camera_RotateRight",
            "Camera_RotateReset",
            "Camera_SelectDynamic",
            "Camera_Select1",
            "Camera_Select2",
            "Camera_Select3",
            "Camera_Select4",
            "Camera_Select5",
            "Camera_Select6",
            "Camera_SaveModifier",
            "World_Toggle",
            "VModel_RotateModifier",
            "VModel_SnapZModifier",
            "VModel_SnapXModifier",
        };
        // return new List<string>(keyBindings.Keys).ToArray();
    }

    public int Length
    {
        get
        {
            return keyBindings.Count;
        }
    }

    void Start()
    {
        LoadKeybinds();
    }

    void Update()
    {
    }

    void OnGUI()
    {
        if (KeyToRebind != "" && Input.anyKeyDown && Event.current.isKey && Event.current.type == EventType.KeyDown)
        {
            Set(KeyToRebind, Event.current.keyCode);
            KeyToRebind = "";
            GlobalEvents.Instance.EventsUI.Invoke("RebindComplete");
            SaveKeybinds();
        }
    }

    public void RebindKey(string keyName)
    {
        KeyToRebind = keyName;
    }

    void SaveKeybinds()
    {
        string json = JsonUtility.ToJson(keyBindings);
        SettingsManager.Instance.saveFile(settingsFile, json);
    }

    void LoadKeybinds()
    {
        string json = SettingsManager.Instance.loadFile(settingsFile);
        if (json == "") return;
        JsonUtility.FromJsonOverwrite(json, keyBindings);
    }
}
