using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using EVMC4U;
using uOSC;


public class UI : Singleton<UI>
{
    public int MenuWidth = 100;
    public DynamicCameraControl MainCamera;
    public ExternalReceiver ExternalReceiver;
    GameObject modLoader;
    GameObject helpCanvas;
    uOscServer uOscServer;
    bool enableVsync = true;
    string maxFPSString = "60";
    int maxFPS = -1;
    private string[] menusNames = { "Camera Settings", "VModel", "More" };
    private string[][] menus = new string[][]
    {
    new string[] { "Save Positions", "Reload Positions", "Slider|1|Sensitivity", "Reset Dynamic Camera" },
    new string[] { "Save Preset", "Load Preset", "Load Model", "Drag Type:", "Slider|0|Sensitivity", "Reset Position" },
    new string[] { "Settings", "Graphics Settings", "Controls", "Key Bindings", "Hotkeys", "Toggle Mod Loader", "Help" }
    };

    // Look sensitivity, VModel move sensitivity.
    public float[] sliderValues = { 0.1f, 1f };
    public float[] slidersMin = { 0.01f, 0.5f };
    public float[] slidersMax = { 1f, 3f };
    public static bool showUI = true;
    [HideInInspector] public string VMCPort = "";
    bool manualHideUI = false;
    bool wasModloaderActive = true;
    bool showSaveVmodalPresets = false;
    bool showLoadVmodalPresets = false;
    bool showLoadVmodels = false;
    bool showModLoader = true;
    bool cameraUnhidesModLoader = true;
    bool showHelp = false;
    bool showSettings = false;
    bool showGraphicsSettings = false;
    bool showHotkeys = false;
    bool showKeyBindings = false;
    string[] modelPaths;
    int saveAndLoadTopOffset = 85;
    void MenuItemClicked(int menu, int item)
    {
        string menuName = menusNames[menu];
        string buttonName = menus[menu][item];
        print("Item " + item + " of menu " + menu + " clicked.");
        if (menu == 0) // Camera Settings
        {
            if (item == 0) // Save Positions
                CameraControl.Instance.saveCamerasToFile();
            if (item == 1) // Reload Positions
                CameraControl.Instance.loadCamerasFromFile();
            if (item == 2) // Reset Dynamic Camera
                MainCamera.resetPosition();
        }
        if (menuName == "VModel")
        {
            if (buttonName == "Save Preset") // Save Preset
                showSaveVmodalPresets = !showSaveVmodalPresets;
            if (buttonName == "Load Preset") // Load Preset
                showLoadVmodalPresets = !showLoadVmodalPresets;
            if (buttonName == "Load Model") // Load Model
            {
                if (!showLoadVmodels)
                {
                    modelPaths = Directory.GetFiles(@"models");
                    loadVmodel = new Rect(50 + MenuWidth, 160 + saveAndLoadTopOffset, 300, 30 + (20 * modelPaths.Length));
                }
                showLoadVmodels = !showLoadVmodels;
            }
            if (buttonName.StartsWith("Drag Type:"))
            {
                AlignVModel.Instance.nextDragType();
                menus[1][3] = "Drag Type: " + AlignVModel.Instance.dragTypeString();
            }
            if (buttonName == "Reset Position") // Reset Position
                AlignVModel.Instance.resetPosition();
        }

        if (menuName == "More")
        {
            if (buttonName == "Settings")
            {
                showSettings = !showSettings;
            }
            if (buttonName == "Graphics Settings")
            {
                showGraphicsSettings = !showGraphicsSettings;
            }
            if (buttonName == "Key Bindings")
            {
                showKeyBindings = !showKeyBindings;
            }
            if (buttonName == "Hotkeys")
            {
                showHotkeys = !showHotkeys;
            }
            if (buttonName == "Toggle Mod Loader")
            {
                cameraUnhidesModLoader = !cameraUnhidesModLoader;
                toggleModloader(!showModLoader);
            }
            if (buttonName == "Controls")
            {
                toggleHelp();
            }
        }
    }

    Rect[] menuWindows;
    Rect saveVmodelPresets;
    Rect loadVmodelPresets;
    Rect loadVmodel;
    Rect settings;
    Rect graphicsSettings;
    Rect hotkeysEditor;
    Rect keyBindingEditor;
    void Start()
    {
        modLoader = GameObject.Find("ModLoader");
        helpCanvas = GameObject.Find("Help");
        uOscServer = GameObject.Find("ExternalReceiver").GetComponent<uOscServer>();
        helpCanvas.GetComponent<Canvas>().enabled = false;
        VMCPort = uOscServer.port.ToString();

        LoadSettings();
        menus[1][3] = "Drag Type: " + AlignVModel.Instance.dragTypeString();

        menuWindows = new Rect[menus.Length];
        int lastHeight = 0;
        for (int i = 0; i < menus.Length; i++)
        {
            menuWindows[i] = new Rect(20, 40 + (20 * lastHeight) + (40 * i), MenuWidth + 20, 30 + (20 * menus[i].Length));
            lastHeight += menus[i].Length;
        }
        saveVmodelPresets = new Rect(50 + MenuWidth, 40 + saveAndLoadTopOffset, 150, 50);
        loadVmodelPresets = new Rect(50 + MenuWidth, 100 + saveAndLoadTopOffset, 150, 50);

        graphicsSettings = new Rect(50 + MenuWidth, 160 + saveAndLoadTopOffset, MenuWidth + 20, 80);
        settings = new Rect(50 + MenuWidth, 250 + saveAndLoadTopOffset, MenuWidth + 20, 80);

        hotkeysEditor = new Rect(50 + MenuWidth, 200 + saveAndLoadTopOffset, 460, 30 + (20 * GlobalHotkeys.Instance.hotkeyActions.Length));
        keyBindingEditor = new Rect(50 + MenuWidth, 200 + saveAndLoadTopOffset, 300, 30 + (20 * InputManager.Instance.keyboardInputs.Length));

        GlobalEvents.Instance.EventsGlobalHotkeys.AddListener(GlobalHotkeyEvent);
        GlobalEvents.Instance.EventsUI.AddListener(EventsUI);
    }

    void Update()
    {
        if (!Int32.TryParse(VMCPort, out uOscServer.port))
            uOscServer.port = 39539;
        if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("UI_Toggle")))
            ToggleUI();
    }

    void GlobalHotkeyEvent(string eventName)
    {
        if (eventName == "ToggleUI")
            ToggleUI();
    }
    void ToggleUI()
    {
        SetUI(manualHideUI);
    }
    void SetUI(bool value)
    {
        // Fix: modloader not staying hidden bug.
        if (!value)
        {
            wasModloaderActive = modLoader.activeSelf;
            if (wasModloaderActive)
                modLoader.SetActive(false);
        }
        else if (wasModloaderActive)
            modLoader.SetActive(true);
        manualHideUI = !value;
    }

    void OnGUI()
    {
        // Set Vsync options.
        if (enableVsync)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;

        // Set Max FPS
        int newFPS = 0;
        bool result = int.TryParse(maxFPSString, out newFPS);
        if (newFPS != 0 && newFPS < 30) newFPS = 30;
        if (newFPS != maxFPS)
        {
            maxFPS = newFPS;
            maxFPSString = maxFPS.ToString();
            Application.targetFrameRate = maxFPS;
        }

        if (showUI && !manualHideUI)
            for (int i = 0; i < menus.Length; i++)
            {
                menuWindows[i] = GUI.Window(i, menuWindows[i], MenuButtons, menusNames[i]);

            }

        if (showUI && showSaveVmodalPresets && !manualHideUI)
            saveVmodelPresets = GUI.Window(menus.Length + 1, saveVmodelPresets, saveVmodelPresetsButtons, "Save Vmodel Preset");
        if (showUI && showLoadVmodalPresets && !manualHideUI)
            loadVmodelPresets = GUI.Window(menus.Length + 2, loadVmodelPresets, loadVmodelPresetsButtons, "Load Vmodel Preset");

        if (showUI && showLoadVmodels && !manualHideUI)
            loadVmodel = GUI.Window(menus.Length + 3, loadVmodel, loadVmodelButtons, "Load Model");

        if (showUI && showGraphicsSettings && !manualHideUI)
            graphicsSettings = GUI.Window(menus.Length + 4, graphicsSettings, graphicsSettingsButtons, "Graphics Settings");

        if (showUI && showHotkeys && !manualHideUI)
            hotkeysEditor = GUI.Window(menus.Length + 6, hotkeysEditor, hotkeysEditorButtons, "Hotkeys");

        if (showUI && showKeyBindings && !manualHideUI)
            keyBindingEditor = GUI.Window(menus.Length + 7, keyBindingEditor, keyBindingEditorButtons, "Hotkeys");


        if (showUI && showSettings && !manualHideUI)
            settings = GUI.Window(menus.Length + 5, settings, settingsButtons, "Settings");
    }


    void MenuButtons(int windowID)
    {
        for (int i = 0; i < menus[windowID].Length; i++)
        {
            if (menus[windowID][i].StartsWith("Slider|"))
            {
                string[] parts = menus[windowID][i].Split('|');
                GUI.Label(new Rect(10, 20 + (20 * i), MenuWidth / 2, 20), parts[2]);
                sliderValues[int.Parse(parts[1])] = GUI.HorizontalSlider(new Rect(10 + MenuWidth / 2, 25 + (20 * i), MenuWidth / 2, 20), sliderValues[int.Parse(parts[1])], slidersMin[int.Parse(parts[1])], slidersMax[int.Parse(parts[1])]);
            }
            else
            if (GUI.Button(new Rect(10, 20 + (20 * i), MenuWidth, 20), menus[windowID][i]))
            {
                MenuItemClicked(windowID, i);
            }
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    void saveVmodelPresetsButtons(int windowID)
    {
        if (GUI.Button(new Rect(10, 20, 20, 20), "1"))
            AlignVModel.Instance.SaveVmodelPreset(1);
        if (GUI.Button(new Rect(32, 20, 20, 20), "2"))
            AlignVModel.Instance.SaveVmodelPreset(2);
        if (GUI.Button(new Rect(54, 20, 20, 20), "3"))
            AlignVModel.Instance.SaveVmodelPreset(3);
        if (GUI.Button(new Rect(76, 20, 20, 20), "4"))
            AlignVModel.Instance.SaveVmodelPreset(4);
        if (GUI.Button(new Rect(98, 20, 20, 20), "5"))
            AlignVModel.Instance.SaveVmodelPreset(5);
        if (GUI.Button(new Rect(120, 20, 20, 20), "6"))
            AlignVModel.Instance.SaveVmodelPreset(6);
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    void loadVmodelPresetsButtons(int windowID)
    {
        if (GUI.Button(new Rect(10, 20, 20, 20), "1"))
            AlignVModel.Instance.LoadVmodelPreset(1);
        if (GUI.Button(new Rect(32, 20, 20, 20), "2"))
            AlignVModel.Instance.LoadVmodelPreset(2);
        if (GUI.Button(new Rect(54, 20, 20, 20), "3"))
            AlignVModel.Instance.LoadVmodelPreset(3);
        if (GUI.Button(new Rect(76, 20, 20, 20), "4"))
            AlignVModel.Instance.LoadVmodelPreset(4);
        if (GUI.Button(new Rect(98, 20, 20, 20), "5"))
            AlignVModel.Instance.LoadVmodelPreset(5);
        if (GUI.Button(new Rect(120, 20, 20, 20), "6"))
            AlignVModel.Instance.LoadVmodelPreset(6);
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    void loadVmodelButtons(int windowID)
    {
        for (int i = 0; i < modelPaths.Length; i++)
        {
            string[] modelName = modelPaths[i].Split('\\');
            if (GUI.Button(new Rect(10, 20 + (20 * i), 280, 20), modelName[modelName.Length - 1]))
            {
                showLoadVmodels = false;
                ExternalReceiver.LoadVRM(modelPaths[i]);
            }
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    void graphicsSettingsButtons(int windowID)
    {
        enableVsync = GUI.Toggle(new Rect(10, 20, MenuWidth, 20), enableVsync, " Enable VSync");

        GUI.Label(new Rect(10, 40, 100, 20), "Max FPS");
        if (!enableVsync) maxFPSString = GUI.TextField(new Rect(70, 40, MenuWidth - 60, 20), maxFPSString, 3);

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    void settingsButtons(int windowID)
    {
        if (GUI.Button(new Rect(10, 40, MenuWidth, 20), "Apply"))
        {
            uOscServer.Restart();
            SaveSettings();
        }
        GUI.Label(new Rect(10, 20, 100, 20), "VMC Port");
        VMCPort = GUI.TextField(new Rect(70, 20, MenuWidth - 60, 20), VMCPort, 8);

        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
    bool hotkeyRebindInProgress = false;
    int hotkeyToRebind = 0;
    void hotkeysEditorButtons(int windowID)
    {
        for (int i = 0; i < GlobalHotkeys.Instance.hotkeyActions.Length; i++)
        {
            GUI.Label(new Rect(10, 20 + (20 * i), 130, 20), GlobalHotkeys.Instance.hotkeyActions[i]);
            string buttonText = "";
            if (hotkeyRebindInProgress && hotkeyToRebind == i)
                buttonText = "???";
            else
                buttonText = GlobalHotkeys.Instance.HotkeyList.GetKeysFromActionAsString(GlobalHotkeys.Instance.hotkeyActions[i]);
            if (GUI.Button(new Rect(140, 20 + (20 * i), 280, 20), buttonText))
            {
                if (!keyRebindInProgress)
                    if (!hotkeyRebindInProgress)
                    {
                        hotkeyRebindInProgress = true;
                        hotkeyToRebind = i;
                        GlobalHotkeys.Instance.RebindKey(GlobalHotkeys.Instance.hotkeyActions[i]);
                    }
                    else if (hotkeyToRebind == i)
                    {
                        hotkeyRebindInProgress = false;
                        GlobalHotkeys.Instance.RebindKey("");
                    }

            }
            if (GUI.Button(new Rect(425, 20 + (20 * i), 25, 20), "X"))
            {
                if (!hotkeyRebindInProgress)
                    GlobalHotkeys.Instance.RemoveHotkey(GlobalHotkeys.Instance.hotkeyActions[i]);
            }
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
    bool keyRebindInProgress = false;
    int keyToRebind = 0;
    void keyBindingEditorButtons(int windowID)
    {
        string[] inputNames = InputManager.Instance.keyboardInputs.GetInputNames();
        for (int i = 0; i < inputNames.Length; i++)
        {
            GUI.Label(new Rect(10, 20 + (20 * i), 150, 20), inputNames[i]);
            string buttonText = "";
            if (keyRebindInProgress && keyToRebind == i)
                buttonText = "???";
            else
                buttonText = InputManager.Instance.keyboardInputs.GetKey(inputNames[i]).ToString();
            if (GUI.Button(new Rect(160, 20 + (20 * i), 100, 20), buttonText))
            {
                if (!hotkeyRebindInProgress)
                    if (!keyRebindInProgress)
                    {
                        keyRebindInProgress = true;
                        keyToRebind = i;
                        InputManager.Instance.RebindKey(inputNames[i]);
                    }
                    else if (keyToRebind == i)
                    {
                        keyRebindInProgress = false;
                        InputManager.Instance.RebindKey("");
                    }

            }
            if (GUI.Button(new Rect(265, 20 + (20 * i), 25, 20), "X"))
            {
                if (!keyRebindInProgress)
                    InputManager.Instance.keyboardInputs.SetKey(inputNames[i], KeyCode.None);
            }
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    void EventsUI(string eventType)
    {
        if (eventType == "RebindComplete")
        {
            hotkeyRebindInProgress = false;
            keyRebindInProgress = false;
        }
    }

    public void ShowUI(bool value)
    {
        showUI = value;
        toggleModloader(value);
    }

    public void ShowUI(bool value, bool fromCamera)
    {
        showUI = value;
        if (!fromCamera || cameraUnhidesModLoader)
            toggleModloader(value);

    }

    public void toggleModloader()
    {
        toggleModloader(!showModLoader);
    }
    public void toggleModloader(bool value)
    {
        showModLoader = value;
        modLoader.SetActive(showModLoader);
    }

    public void toggleHelp()
    {
        toggleHelp(!showHelp);
    }
    public void toggleHelp(bool value)
    {
        showHelp = value;
        helpCanvas.GetComponent<Canvas>().enabled = showHelp;
    }

    void LoadSettings()
    {

    }
    void SaveSettings()
    {

    }

}
