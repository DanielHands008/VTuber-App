using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignVModel : MonoBehaviour
{
    public SettingsManager SettingsManager;
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
    }
    void Update()
    {
        if (MainCamera.enabled)
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

            if (Input.GetKeyDown(KeyCode.LeftShift))
                moveY = 0;
            if (Input.GetKeyUp(KeyCode.LeftShift))
                moveY = 1;

            if (Input.GetKeyDown(KeyCode.LeftControl))
                moveX = 0;
            if (Input.GetKeyUp(KeyCode.LeftControl))
                moveX = 1;

            if (Input.GetKeyDown(KeyCode.LeftAlt))
                rotateVmodel = true;
            if (Input.GetKeyUp(KeyCode.LeftAlt))
                rotateVmodel = false;

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
        string json = SettingsManager.loadFile(settingsFile);
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
        SettingsManager.saveFile(settingsFile, json);
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
}
