using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : Singleton<InputManager>
{
    public string settingsFile = "keybinds";
    public KeyboardInputs keyboardInputs = new KeyboardInputs();

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
        // public void Load(SaveData saveData)
        // {
        //     for (int i = 0; i < saveData.keybindsNames.Length; i++)
        //     {
        //         SetKey(saveData.keybindsNames[i], (KeyCode)Enum.Parse(typeof(KeyCode), saveData.keybinds[i]));
        //     }
        // }
        public void Load(SaveData saveData)
        {
            SetKey("UI_Toggle", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.UI_Toggle));
            SetKey("Move_Forward", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Move_Forward));
            SetKey("Move_Left", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Move_Left));
            SetKey("Move_Back", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Move_Back));
            SetKey("Move_Right", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Move_Right));
            SetKey("Move_Fast", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Move_Fast));
            SetKey("Move_HeightLock", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Move_HeightLock));
            SetKey("Camera_RotateLeft", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Camera_RotateLeft));
            SetKey("Camera_RotateRight", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Camera_RotateRight));
            SetKey("Camera_RotateReset", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Camera_RotateReset));
            SetKey("Camera_SelectDynamic", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Camera_SelectDynamic));
            SetKey("Camera_Select1", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Camera_Select1));
            SetKey("Camera_Select2", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Camera_Select2));
            SetKey("Camera_Select3", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Camera_Select3));
            SetKey("Camera_Select4", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Camera_Select4));
            SetKey("Camera_Select5", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Camera_Select5));
            SetKey("Camera_Select6", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Camera_Select6));
            SetKey("Camera_SaveModifier", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.Camera_SaveModifier));
            SetKey("World_Toggle", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.World_Toggle));
            SetKey("VModel_RotateModifier", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.VModel_RotateModifier));
            SetKey("VModel_SnapZModifier", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.VModel_SnapZModifier));
            SetKey("VModel_SnapXModifier", (KeyCode)Enum.Parse(typeof(KeyCode), saveData.VModel_SnapXModifier));
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
        LoadKeybinds();
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
            SaveKeybinds();
        }
    }

    public void RebindKey(string keyName)
    {
        KeyToRebind = keyName;
    }

    // [System.Serializable]
    // public class SaveData
    // {
    //     public SaveData()
    //     {

    //     }
    //     public SaveData(KeyboardInputs inputData)
    //     {
    //         string[] bindingNames = inputData.GetInputNames();
    //         Array.Resize(ref keybindsNames, bindingNames.Length);
    //         Array.Resize(ref keybinds, bindingNames.Length);
    //         for (int i = 0; i < bindingNames.Length; i++)
    //         {
    //             keybindsNames[i] = bindingNames[i];
    //             keybinds[i] = inputData.GetKey(bindingNames[i]).ToString();
    //         }
    //     }
    //     public string[] keybindsNames = new string[] { };
    //     public string[] keybinds = new string[] { };
    // }

    [System.Serializable]
    public class SaveData
    {
        public SaveData()
        {

        }
        public SaveData(KeyboardInputs inputData)
        {
            UI_Toggle = inputData.GetKey("UI_Toggle").ToString();
            Move_Forward = inputData.GetKey("Move_Forward").ToString();
            Move_Left = inputData.GetKey("Move_Left").ToString();
            Move_Back = inputData.GetKey("Move_Back").ToString();
            Move_Right = inputData.GetKey("Move_Right").ToString();
            Move_Fast = inputData.GetKey("Move_Fast").ToString();
            Move_HeightLock = inputData.GetKey("Move_HeightLock").ToString();
            Camera_RotateLeft = inputData.GetKey("Camera_RotateLeft").ToString();
            Camera_RotateRight = inputData.GetKey("Camera_RotateRight").ToString();
            Camera_RotateReset = inputData.GetKey("Camera_RotateReset").ToString();
            Camera_SelectDynamic = inputData.GetKey("Camera_SelectDynamic").ToString();
            Camera_Select1 = inputData.GetKey("Camera_Select1").ToString();
            Camera_Select2 = inputData.GetKey("Camera_Select2").ToString();
            Camera_Select3 = inputData.GetKey("Camera_Select3").ToString();
            Camera_Select4 = inputData.GetKey("Camera_Select4").ToString();
            Camera_Select5 = inputData.GetKey("Camera_Select5").ToString();
            Camera_Select6 = inputData.GetKey("Camera_Select6").ToString();
            Camera_SaveModifier = inputData.GetKey("Camera_SaveModifier").ToString();
            World_Toggle = inputData.GetKey("World_Toggle").ToString();
            VModel_RotateModifier = inputData.GetKey("VModel_RotateModifier").ToString();
            VModel_SnapZModifier = inputData.GetKey("VModel_SnapZModifier").ToString();
            VModel_SnapXModifier = inputData.GetKey("VModel_SnapXModifier").ToString();
        }
        public string UI_Toggle;
        public string Move_Forward;
        public string Move_Left;
        public string Move_Back;
        public string Move_Right;
        public string Move_Fast;
        public string Move_HeightLock;
        public string Camera_RotateLeft;
        public string Camera_RotateRight;
        public string Camera_RotateReset;
        public string Camera_SelectDynamic;
        public string Camera_Select1;
        public string Camera_Select2;
        public string Camera_Select3;
        public string Camera_Select4;
        public string Camera_Select5;
        public string Camera_Select6;
        public string Camera_SaveModifier;
        public string World_Toggle;
        public string VModel_RotateModifier;
        public string VModel_SnapZModifier;
        public string VModel_SnapXModifier;
    }

    void SaveKeybinds()
    {
        SaveData saveData = new SaveData(keyboardInputs);
        string json = JsonUtility.ToJson(saveData);
        SettingsManager.Instance.saveFile(settingsFile, json);
    }

    void LoadKeybinds()
    {
        string json = SettingsManager.Instance.loadFile(settingsFile);
        if (json == "") return;
        SaveData saveData = new SaveData();
        JsonUtility.FromJsonOverwrite(json, saveData);
        keyboardInputs.Load(saveData);
    }

    // Add methods for saving, loading and setting controls.
}
