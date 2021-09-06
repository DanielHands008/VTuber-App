using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignVModel : Singleton<AlignVModel>
{
    public string settingsFile = "vmodel";
    public Camera MainCamera;
    public float dragSensitivity = 1f;
    public string mouseHorizontalAxisName = "Mouse X";
    public string mouseVerticalAxisName = "Mouse Y";
    string xMoveAxis;
    string yMoveAxis;
    int invertY = 1;
    int invertX = 1;
    int moveX = 1;
    int moveY = 1;
    bool rotateVmodel = false;
    private bool dragEnabled = false;
    public int dragType = 0; // "Screen Space" "Cardinal" "Height Locked"
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Vector3 mouseLocation;

    vModelData[] vModelPresets = new vModelData[7];
    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        vModelPresets[0] = new vModelData(transform);
        vModelPresets[1] = new vModelData(transform);
        vModelPresets[2] = new vModelData(transform);
        vModelPresets[3] = new vModelData(transform);
        vModelPresets[4] = new vModelData(transform);
        vModelPresets[5] = new vModelData(transform);
        vModelPresets[6] = new vModelData(transform);
        loadPosition();
        GlobalEvents.Instance.EventsGlobalHotkeys.AddListener(GlobalHotkeyEvent);
    }
    void GlobalHotkeyEvent(string eventName)
    {
        if (eventName == "LoadVModelPreset1")
            LoadVmodelPreset(1);
        else if (eventName == "LoadVModelPreset2")
            LoadVmodelPreset(2);
        else if (eventName == "LoadVModelPreset3")
            LoadVmodelPreset(3);
        else if (eventName == "LoadVModelPreset4")
            LoadVmodelPreset(4);
        else if (eventName == "LoadVModelPreset5")
            LoadVmodelPreset(5);
        else if (eventName == "LoadVModelPreset6")
            LoadVmodelPreset(6);
    }
    void Update()
    {
        dragSensitivity = UI.Instance.sliderValues[0];
        if (MainCamera.enabled)
        {
            if (dragType == 0)
            {
                if (dragEnabled)
                {
                    mouseLocation = Input.mousePosition - mouseLocation;
                    if (rotateVmodel)
                    {
                        mouseLocation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Input.GetAxis(mouseHorizontalAxisName), transform.eulerAngles.z);
                        transform.eulerAngles = mouseLocation;
                    }
                    else
                        transform.Translate(Input.GetAxis(mouseHorizontalAxisName) * dragSensitivity, Input.GetAxis(mouseVerticalAxisName) * dragSensitivity, 0, Camera.main.transform);
                }
                if (Input.GetMouseButtonDown(2))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    mouseLocation = Input.mousePosition;
                    dragEnabled = true;
                }
            }
            else if (dragType == 1)
            {
                if (dragEnabled)
                {
                    mouseLocation = Input.mousePosition - mouseLocation;
                    if (rotateVmodel)
                    {
                        mouseLocation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Input.GetAxis(mouseHorizontalAxisName), transform.eulerAngles.z);
                        transform.eulerAngles = mouseLocation;
                    }
                    else
                    {
                        GameObject emptyGO = new GameObject();
                        emptyGO.transform.position = Camera.main.transform.position;
                        emptyGO.transform.rotation = new Quaternion(0, Camera.main.transform.rotation.y, 0, Camera.main.transform.rotation.w);
                        transform.Translate(Input.GetAxis(mouseHorizontalAxisName) * dragSensitivity, 0, Input.GetAxis(mouseVerticalAxisName) * dragSensitivity, emptyGO.transform);
                        Destroy(emptyGO);
                    }
                }
                if (Input.GetMouseButtonDown(2))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    mouseLocation = Input.mousePosition;
                    dragEnabled = true;
                }
            }
            else if (dragType == 2)
            {
                if (dragEnabled)
                {
                    mouseLocation = Input.mousePosition - mouseLocation;
                    if (rotateVmodel)
                    {
                        mouseLocation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Input.GetAxis(mouseHorizontalAxisName), transform.eulerAngles.z);
                        transform.eulerAngles = mouseLocation;
                    }
                    else if (moveX == 0 && moveY == 0) // Move Y axis (up/down) if both ctrl and shift are press.
                    {
                        mouseLocation = new Vector3(transform.position.x, transform.position.y + (Input.GetAxis(mouseVerticalAxisName) * dragSensitivity), transform.position.z);
                        transform.position = mouseLocation;
                    }
                    else
                    {
                        mouseLocation = new Vector3(transform.position.x + (((invertX * Input.GetAxis(xMoveAxis)) * dragSensitivity) * moveX), transform.position.y, transform.position.z + (((invertY * Input.GetAxis(yMoveAxis)) * dragSensitivity) * moveY));
                        transform.position = mouseLocation;
                    }
                    mouseLocation = Input.mousePosition;
                }

                if (Input.GetMouseButtonDown(2))
                {
                    int quad = (int)Mathf.Round((MainCamera.transform.eulerAngles.y) / 90);
                    if (quad == 4)
                        quad = 0;

                    if (quad == 0)
                    {
                        xMoveAxis = mouseHorizontalAxisName;
                        yMoveAxis = mouseVerticalAxisName;
                        invertX = 1;
                        invertY = 1;
                    }
                    if (quad == 1)
                    {
                        xMoveAxis = mouseVerticalAxisName;
                        yMoveAxis = mouseHorizontalAxisName;
                        invertX = 1;
                        invertY = -1;
                    }
                    if (quad == 2)
                    {
                        xMoveAxis = mouseHorizontalAxisName;
                        yMoveAxis = mouseVerticalAxisName;
                        invertX = -1;
                        invertY = -1;
                    }
                    if (quad == 3)
                    {
                        xMoveAxis = mouseVerticalAxisName;
                        yMoveAxis = mouseHorizontalAxisName;
                        invertX = -1;
                        invertY = 1;
                    }

                    Cursor.lockState = CursorLockMode.Locked;
                    mouseLocation = Input.mousePosition;
                    dragEnabled = true;
                }
            }
            if (Input.GetKeyDown(InputManager.Instance.Get("VModel_SnapXModifier")))
                moveY = 0;
            if (Input.GetKeyUp(InputManager.Instance.Get("VModel_SnapXModifier")))
                moveY = 1;

            if (Input.GetKeyDown(InputManager.Instance.Get("VModel_SnapZModifier")))
                moveX = 0;
            if (Input.GetKeyUp(InputManager.Instance.Get("VModel_SnapZModifier")))
                moveX = 1;

            if (Input.GetKeyDown(InputManager.Instance.Get("VModel_RotateModifier")))
                rotateVmodel = true;
            if (Input.GetKeyUp(InputManager.Instance.Get("VModel_RotateModifier")))
                rotateVmodel = false;
        }
        if (Input.GetMouseButtonUp(2))
        {
            Cursor.lockState = CursorLockMode.None;
            dragEnabled = false;
            savePosition();
        }

    }

    class vModelData
    {
        public vModelData()
        {
            Position = new Vector3(0, 0, 0);
            Rotation = new Quaternion(0, 0, 0, 0);
        }
        public vModelData(Transform transform)
        {
            Position = transform.position;
            Rotation = transform.rotation;
        }
        public vModelData(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;

        }
        public Vector3 Position = new Vector3(0, 0, 0);
        public Quaternion Rotation = new Quaternion(0, 0, 0, 0);
    }

    vModelData[] presetsFromSaveData(SaveData saveData)
    {
        vModelData[] outputArray = new vModelData[7];
        for (int i = 0; i < outputArray.Length; i++)
        {
            outputArray[i] = new vModelData(saveData.Positions[i], saveData.Rotations[i]);
        }
        return outputArray;
    }
    class SaveData
    {
        public SaveData()
        {

        }
        public SaveData(vModelData[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                Positions[i] = input[i].Position;
                Rotations[i] = input[i].Rotation;
            }
        }
        public Vector3[] Positions = new Vector3[7];
        public Quaternion[] Rotations = new Quaternion[7];
    }

    public void loadPosition()
    {
        string json = SettingsManager.Instance.loadFile(settingsFile);
        if (json == "") return;
        SaveData saveData = new SaveData();
        JsonUtility.FromJsonOverwrite(json, saveData);
        vModelPresets = presetsFromSaveData(saveData);
        setVmodalPosition(vModelPresets[0]);
    }

    public void savePosition()
    {
        vModelPresets[0] = new vModelData(transform);
        SaveData saveData = new SaveData(vModelPresets);
        string json = JsonUtility.ToJson(saveData);
        SettingsManager.Instance.saveFile(settingsFile, json);
    }
    void setVmodalPosition(vModelData vmodelData)
    {
        transform.position = vmodelData.Position;
        transform.rotation = vmodelData.Rotation;
    }

    public void SaveVmodelPreset(int preset)
    {
        vModelPresets[preset] = new vModelData(transform);
        savePosition();
    }
    public void LoadVmodelPreset(int preset)
    {
        transform.position = vModelPresets[preset].Position;
        transform.rotation = vModelPresets[preset].Rotation;
    }
    public void resetPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
    public void setDragType(int type)
    {
        dragType = type;
    }
    public void nextDragType()
    {
        dragType++;
        if (dragType == 3)
            dragType = 0;
    }
    public string dragTypeString()
    {
        if (dragType == 0)
            return "Screen Space";
        else if (dragType == 1)
            return "Height Locked";
        else if (dragType == 2)
            return "Cardinal";
        else
            return dragType.ToString();
    }
}
