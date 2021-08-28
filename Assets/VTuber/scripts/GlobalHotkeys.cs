using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRawInput;
using System;


public class GlobalHotkeys : Singleton<GlobalHotkeys>
{
    // Start is called before the first frame update
    // https://github.com/Elringus/UnityRawInput
    public string settingsFile = "hotkeys";

    public string rebindHotkey = "";
    Dictionary<RawKey, bool> keyStates = new Dictionary<RawKey, bool>();

    public Hotkeys HotkeyList = new Hotkeys();

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

    void Start()
    {
        var workInBackground = true;
        RawKeyInput.Start(workInBackground);
        RawKeyInput.OnKeyUp += HandleKeyUp;
        RawKeyInput.OnKeyDown += HandleKeyDown;
        // HotkeyList.Set("ToggleUI", RawKey.U, new RawKey[] { RawKey.LeftControl });
        loadHotkeys();
    }

    void Update()
    {
    }

    private void HandleKeyDown(RawKey key)
    {
        if (rebindHotkey != "")
        {
            RawKey[] modifiers = new RawKey[0];
            for (int i = 0; i < modifierList.Length; i++)
            {
                if (RawKeyInput.IsKeyDown(modifierList[i]))
                {
                    Array.Resize(ref modifiers, modifiers.Length + 1);
                    modifiers[modifiers.GetUpperBound(0)] = modifierList[i];

                }
            }
            HotkeyList.Set(rebindHotkey, key, modifiers);
            rebindHotkey = "";
            return;
        }

        string action = HandleHotkey(key);
        if (action != "") GlobalEvents.Instance.EventsGlobalHotkeys.Invoke(action);
    }

    private void HandleKeyUp(RawKey key)
    {
    }

    private string HandleHotkey(RawKey key)
    {

        for (int i = 0; i < HotkeyList.hotkeys.Length; i++)
        {
            if (HotkeyList.hotkeys[i].Key == key)
            {
                int modifierCount = 0;
                for (int j = 0; j < modifierList.Length; j++)
                {
                    if (RawKeyInput.IsKeyDown(modifierList[j]))
                    {
                        if (Array.Exists(HotkeyList.hotkeys[i].Modifiers, element => element == modifierList[j]))
                            modifierCount++;
                        else modifierCount--;
                    }
                }
                if (modifierCount == HotkeyList.hotkeys[i].Modifiers.Length)
                    return HotkeyList.hotkeys[i].Action;
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

    public class Hotkey
    {
        public Hotkey(string action, RawKey key, RawKey[] modifiers)
        {
            Action = action;
            Key = key;
            Modifiers = modifiers;
        }
        public RawKey Key;
        public RawKey[] Modifiers;
        public string Action;
    }

    public class Hotkeys
    {
        public bool Add(Hotkey hotkey)
        {
            print(hotkey);
            // Check if action is already assigned.
            Array.Resize(ref hotkeys, hotkeys.Length + 1);
            hotkeys[hotkeys.GetUpperBound(0)] = hotkey;
            return true;
        }
        public bool Remove(string action)
        {
            for (int i = 0; i < hotkeys.Length; i++)
            {
                if (hotkeys[i].Action == action)
                {
                    RemoveAt<Hotkey>(ref hotkeys, i);
                    return true;
                }
            }
            return false;
        }
        public bool Replace(Hotkey hotkey)
        {
            for (int i = 0; i < hotkeys.Length; i++)
            {
                if (hotkeys[i].Action == hotkey.Action)
                {
                    hotkeys[i] = hotkey;
                    return true;
                }
            }
            return false;
        }
        public bool Set(string action, RawKey key, RawKey[] modifiers)
        {
            for (int i = 0; i < hotkeys.Length; i++)
            {
                if (hotkeys[i].Action == action)
                {
                    return Replace(new Hotkey(action, key, modifiers));
                }
            }
            return Add(new Hotkey(action, key, modifiers));
        }
        public bool Load(SaveData input)
        {
            // Enum.Parse(typeof(RawKey), "A")
            Array.Resize(ref hotkeys, input.GlobalHotkeys.Length);
            for (int i = 0; i < input.GlobalHotkeys.Length; i++)
            {
                RawKey[] newModifiers = new RawKey[input.GlobalHotkeys[i].Modifiers.Length];
                for (int j = 0; j < input.GlobalHotkeys[i].Modifiers.Length; j++)
                {
                    newModifiers[j] = (RawKey)Enum.Parse(typeof(RawKey), input.GlobalHotkeys[i].Modifiers[j]);
                }
                hotkeys[i] = new Hotkey(input.GlobalHotkeys[i].Action, (RawKey)Enum.Parse(typeof(RawKey), input.GlobalHotkeys[i].Key), newModifiers);
            }
            return true;
        }
        public Hotkey[] hotkeys = new Hotkey[0];
    }

    public static void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref arr, arr.Length - 1);
    }

    [System.Serializable]
    public class HotkeyForSave
    {
        public HotkeyForSave(Hotkey hotkey)
        {
            Action = hotkey.Action;
            Key = hotkey.Key.ToString();
            Array.Resize(ref Modifiers, hotkey.Modifiers.Length);
            for (int i = 0; i < hotkey.Modifiers.Length; i++)
            {
                Modifiers[i] = hotkey.Modifiers[i].ToString();
            }
        }
        public string Action;
        public string Key;
        public string[] Modifiers;
    }

    [System.Serializable]
    public class SaveData
    {
        public SaveData()
        {

        }
        public SaveData(Hotkeys input)
        {
            Array.Resize(ref GlobalHotkeys, input.hotkeys.Length);
            for (int i = 0; i < input.hotkeys.Length; i++)
            {
                GlobalHotkeys[i] = new HotkeyForSave(input.hotkeys[i]);
            }
        }

        public HotkeyForSave[] GlobalHotkeys = new HotkeyForSave[0];
    }

    public void saveHotkeys()
    {
        string[] RawKeyNames = Enum.GetNames(typeof(RawKey));
        SaveData saveData = new SaveData(HotkeyList);
        HotkeyList.Remove("ToggleUI");

        string json = JsonUtility.ToJson(saveData);
        print(json);
        SettingsManager.Instance.saveFile(settingsFile, json);
    }
    public void loadHotkeys()
    {
        string json = SettingsManager.Instance.loadFile(settingsFile);
        if (json == "") return;
        SaveData saveData = new SaveData();
        JsonUtility.FromJsonOverwrite(json, saveData);
        HotkeyList.Load(saveData);
    }
}

