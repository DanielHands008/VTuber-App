using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using EVMC4U;


public class UI : Singleton<UI>
{
    public int MenuWidth = 100;
    public DynamicCameraControl MainCamera;
    public ExternalReceiver ExternalReceiver;
    GameObject modLoader;
    GameObject helpCanvas;
    bool enableVsync = true;
    string maxFPSString = "60";
    int maxFPS = -1;
    private string[] menusNames = { "Camera Settings", "VModel", "More" };
    private string[][] menus = new string[][]
    {
    new string[] { "Save Positions", "Reload Positions", "Reset Dynamic Camera" },
    new string[] { "Save Preset", "Load Preset", "Load Model", "Drag Type:", "Slider|0|Sensitivity", "Reset Position" },
    new string[] { "Graphics Settings", "Toggle Mod Loader", "Controls", "Help" }
    };
    public float[] sliderValues = { 0.1f };
    public float[] slidersMin = { 0.01f };
    public float[] slidersMax = { 1f };
    public static bool showUI = true;
    bool manualHideUI = false;
    bool wasModloaderActive = true;
    bool showSaveVmodalPresets = false;
    bool showLoadVmodalPresets = false;
    bool showLoadVmodels = false;
    bool showModLoader = true;
    bool cameraUnhidesModLoader = true;
    bool showHelp = false;
    bool showGraphicsSettings = false;
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
            if (buttonName == "Graphics Settings")
            {
                showGraphicsSettings = !showGraphicsSettings;
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
    Rect graphicsSettings;
    void Start()
    {
        menus[1][3] = "Drag Type: " + AlignVModel.Instance.dragTypeString();
        modLoader = GameObject.Find("ModLoader");
        helpCanvas = GameObject.Find("Help");
        helpCanvas.GetComponent<Canvas>().enabled = false;
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

        GlobalEvents.Instance.EventsGlobalHotkeys.AddListener(GlobalHotkeyEvent);
    }

    void Update()
    {
        if (Input.GetKeyDown(InputManager.Instance.UI_Toggle))
        {
            ToggleUI();
        }
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

}
