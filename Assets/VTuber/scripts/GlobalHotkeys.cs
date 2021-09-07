using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRawInput;
using System;
using System.Linq;

public class GlobalHotkeys : Singleton<GlobalHotkeys>
{
    public string settingsFile = "hotkeys";

    public SerializableDictionary<string, Hotkey> Hotkeys = new SerializableDictionary<string, Hotkey> { };

    public bool hotkeysEnabled = true;

    public string[] hotkeyActions = {
        "ToggleUI",
        "ToggleWorld",
        "SetCameraDynamic",
        "SetCamera1",
        "SetCamera2",
        "SetCamera3",
        "SetCamera4",
        "SetCamera5",
        "SetCamera6",
        "LoadVModelPreset1",
        "LoadVModelPreset2",
        "LoadVModelPreset3",
        "LoadVModelPreset4",
        "LoadVModelPreset5",
        "LoadVModelPreset6",
    };

    RawKey[] modifierList = {
        RawKey.LeftControl,
        RawKey.LeftMenu,
        RawKey.LeftShift,
        RawKey.LeftWindows,
        RawKey.RightControl,
        RawKey.RightMenu,
        RawKey.RightShift,
        RawKey.RightWindows
    };

    string rebindHotkey = "";

    [System.Serializable]
    public class Hotkey
    {
        public Hotkey()
        { }

        public Hotkey(RawKey key, RawKey[] modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public RawKey Key;
        public RawKey[] Modifiers;
    }

    void Start()
    {
        if (!hotkeysEnabled)
            return;

        var workInBackground = true;
        RawKeyInput.Start(workInBackground);
        RawKeyInput.OnKeyUp += HandleKeyUp;
        RawKeyInput.OnKeyDown += HandleKeyDown;
        loadHotkeys();
    }

    void Update()
    {
    }
    public void Set(string action, Hotkey newHotkey)
    {
        if (Hotkeys.ContainsKey(action))
            Hotkeys[action] = newHotkey;
        else
            Hotkeys.Add(new SerializableDictionary<string, Hotkey>.Pair(action, newHotkey));
        saveHotkeys();
    }
    public void Remove(string action)
    {
        if (Hotkeys.ContainsKey(action))
            Hotkeys.Remove(action);
        saveHotkeys();
    }

    public string GetHotkeyAsString(string action)
    {
        if (!Hotkeys.ContainsKey(action))
            return "Not Set";
        string hotkeyAsString = "";
        for (int i = 0; i < Hotkeys[action].Modifiers.Length; i++)
        {
            hotkeyAsString += Hotkeys[action].Modifiers[i].ToString() + "+";
        }
        hotkeyAsString += Hotkeys[action].Key.ToString();
        return hotkeyAsString;
    }

    public void RebindKey(string action)
    {
        rebindHotkey = action;
    }

    private void HandleKeyDown(RawKey key)
    {
        if (!hotkeysEnabled)
            return;

        if (rebindHotkey != "")
        {
            RawKey[] modifiers = new RawKey[0];
            if (!modifierList.Contains<RawKey>(key))
            {
                for (int i = 0; i < modifierList.Length; i++)
                {
                    if (RawKeyInput.IsKeyDown(modifierList[i]))
                    {
                        Array.Resize(ref modifiers, modifiers.Length + 1);
                        modifiers[modifiers.GetUpperBound(0)] = modifierList[i];
                    }
                }
                Set(rebindHotkey, new Hotkey(key, modifiers));
                rebindHotkey = "";
                GlobalEvents.Instance.EventsUI.Invoke("RebindComplete");
                return;
            }
        }
        else
        {
            string action = HandleHotkey(key);
            if (action != "") GlobalEvents.Instance.EventsGlobalHotkeys.Invoke(action);
        }
    }



    private void HandleKeyUp(RawKey key)
    {
    }

    private string HandleHotkey(RawKey key)
    {
        string[] actions = new List<string>(Hotkeys.Keys).ToArray();
        for (int i = 0; i < actions.Length; i++)
        {
            if (Hotkeys[actions[i]].Key == key)
            {
                int modifiersDown = 0;
                for (int j = 0; j < modifierList.Length; j++)
                {
                    if (RawKeyInput.IsKeyDown(modifierList[j]))
                        if (Array.Exists(Hotkeys[actions[i]].Modifiers, element => element == modifierList[j]))
                            modifiersDown++;
                        else
                            modifiersDown--;
                }
                if (modifiersDown == Hotkeys[actions[i]].Modifiers.Length)
                    return actions[i];
            }
        }
        return "";
    }

    void OnApplicationQuit()
    {
        RawKeyInput.OnKeyUp -= HandleKeyUp;
        RawKeyInput.OnKeyDown -= HandleKeyDown;
        RawKeyInput.Stop();
    }

    public void saveHotkeys()
    {
        string json = JsonUtility.ToJson(Hotkeys);
        SettingsManager.Instance.saveFile(settingsFile, json);
    }
    public void loadHotkeys()
    {
        string json = SettingsManager.Instance.loadFile(settingsFile);
        if (json == "") return;
        JsonUtility.FromJsonOverwrite(json, Hotkeys);
    }
}


