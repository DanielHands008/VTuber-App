using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : Singleton<CameraControl>
{
    public string settingsFile = "cameras";

    public Camera MainCamera;
    public Camera FixedCamear1;
    public Camera FixedCamear2;
    public Camera FixedCamear3;
    public Camera FixedCamear4;
    public Camera FixedCamear5;
    public Camera FixedCamear6;

    GameObject UIContainer;

    bool hideWorld = false;

    public class CameraData
    {
        public CameraData()
        {

        }
        public CameraData(Camera mainCamera, Camera camera1, Camera camera2, Camera camera3, Camera camera4, Camera camera5, Camera camera6)
        {

            cameraPosition[0] = mainCamera.transform.position;
            cameraRotation[0] = mainCamera.transform.rotation;
            cameraFOV[0] = mainCamera.fieldOfView;

            cameraPosition[1] = camera1.transform.position;
            cameraRotation[1] = camera1.transform.rotation;
            cameraFOV[1] = camera1.fieldOfView;

            cameraPosition[2] = camera2.transform.position;
            cameraRotation[2] = camera2.transform.rotation;
            cameraFOV[2] = camera2.fieldOfView;

            cameraPosition[3] = camera3.transform.position;
            cameraRotation[3] = camera3.transform.rotation;
            cameraFOV[3] = camera3.fieldOfView;

            cameraPosition[4] = camera4.transform.position;
            cameraRotation[4] = camera4.transform.rotation;
            cameraFOV[4] = camera4.fieldOfView;

            cameraPosition[5] = camera5.transform.position;
            cameraRotation[5] = camera5.transform.rotation;
            cameraFOV[5] = camera5.fieldOfView;

            cameraPosition[6] = camera6.transform.position;
            cameraRotation[6] = camera6.transform.rotation;
            cameraFOV[6] = camera6.fieldOfView;

        }

        public Vector3[] cameraPosition = new Vector3[7];
        public Quaternion[] cameraRotation = new Quaternion[7];
        public float[] cameraFOV = new float[7];
    }

    // Start is called before the first frame update
    void Start()
    {
        UIContainer = GameObject.Find("UIContainer");
        CameraExtensions.LayerCullingHide(FixedCamear1, "Hidden To Fixed Cameras");
        CameraExtensions.LayerCullingHide(FixedCamear2, "Hidden To Fixed Cameras");
        CameraExtensions.LayerCullingHide(FixedCamear3, "Hidden To Fixed Cameras");
        CameraExtensions.LayerCullingHide(FixedCamear4, "Hidden To Fixed Cameras");
        CameraExtensions.LayerCullingHide(FixedCamear5, "Hidden To Fixed Cameras");
        CameraExtensions.LayerCullingHide(FixedCamear6, "Hidden To Fixed Cameras");
        loadCamerasFromFile();

        GlobalEvents.Instance.EventsGlobalHotkeys.AddListener(GlobalHotkeyEvent);
    }

    void GlobalHotkeyEvent(string eventName)
    {
        if (eventName == "SetCameraDynamic")
            setActiveCamera(0);
        else if (eventName == "SetCamera1")
            setActiveCamera(1);
        else if (eventName == "SetCamera2")
            setActiveCamera(2);
        else if (eventName == "SetCamera3")
            setActiveCamera(3);
        else if (eventName == "SetCamera4")
            setActiveCamera(4);
        else if (eventName == "SetCamera5")
            setActiveCamera(5);
        else if (eventName == "SetCamera6")
            setActiveCamera(6);
        else if (eventName == "ToggleWorld")
            ToggleWorld();
    }

    // Update is called once per frame
    void Update()
    {

        // https://docs.unity3d.com/ScriptReference/KeyCode.html
        if (Input.GetKey(InputManager.Instance.keyboardInputs.GetKey("Camera_SaveModifier")))
        {
            if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select1")))
                setCamera(1);
            if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select2")))
                setCamera(2);
            if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select3")))
                setCamera(3);
            if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select4")))
                setCamera(4);
            if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select5")))
                setCamera(5);
            if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select6")))
                setCamera(6);
        }
        if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select1")))
            setActiveCamera(1);
        if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select2")))
            setActiveCamera(2);
        if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select3")))
            setActiveCamera(3);
        if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select4")))
            setActiveCamera(4);
        if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select5")))
            setActiveCamera(5);
        if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_Select6")))
            setActiveCamera(6);
        if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("Camera_SelectDynamic")))
            setActiveCamera(0);

        if (Input.GetKeyDown(InputManager.Instance.keyboardInputs.GetKey("World_Toggle")))
            ToggleWorld();

    }

    void ToggleWorld()
    {
        if (!hideWorld)
        {
            CameraExtensions.LayerCullingHide(MainCamera, "World");
            CameraExtensions.LayerCullingHide(FixedCamear1, "World");
            CameraExtensions.LayerCullingHide(FixedCamear2, "World");
            CameraExtensions.LayerCullingHide(FixedCamear3, "World");
            CameraExtensions.LayerCullingHide(FixedCamear4, "World");
            CameraExtensions.LayerCullingHide(FixedCamear5, "World");
            CameraExtensions.LayerCullingHide(FixedCamear6, "World");
        }
        else
        {
            CameraExtensions.LayerCullingShow(MainCamera, "World");
            CameraExtensions.LayerCullingShow(FixedCamear1, "World");
            CameraExtensions.LayerCullingShow(FixedCamear2, "World");
            CameraExtensions.LayerCullingShow(FixedCamear3, "World");
            CameraExtensions.LayerCullingShow(FixedCamear4, "World");
            CameraExtensions.LayerCullingShow(FixedCamear5, "World");
            CameraExtensions.LayerCullingShow(FixedCamear6, "World");
        }
        hideWorld = !hideWorld;
    }


    void setActiveCamera(int cameraNumber)
    {
        if (cameraNumber != 0)
            MainCamera.enabled = false;
        if (cameraNumber != 1)
            FixedCamear1.enabled = false;
        if (cameraNumber != 2)
            FixedCamear2.enabled = false;
        if (cameraNumber != 3)
            FixedCamear3.enabled = false;
        if (cameraNumber != 4)
            FixedCamear4.enabled = false;
        if (cameraNumber != 5)
            FixedCamear5.enabled = false;
        if (cameraNumber != 6)
            FixedCamear6.enabled = false;

        if (cameraNumber == 0)
            MainCamera.enabled = true;
        else if (cameraNumber == 1)
            FixedCamear1.enabled = true;
        else if (cameraNumber == 2)
            FixedCamear2.enabled = true;
        else if (cameraNumber == 3)
            FixedCamear3.enabled = true;
        else if (cameraNumber == 4)
            FixedCamear4.enabled = true;
        else if (cameraNumber == 5)
            FixedCamear5.enabled = true;
        else if (cameraNumber == 6)
            FixedCamear6.enabled = true;
        // Change to switch case

        if (cameraNumber != 0)
        {
            UI.Instance.ShowUI(false, true);
            UIContainer.SetActive(false);
        }
        else
        {
            UI.Instance.ShowUI(true, true);
            UIContainer.SetActive(true);

        }
    }

    void setCamera(int cameraNumber)
    {

        Vector3 MainCameraPosition = MainCamera.transform.position;
        Quaternion MainCameraRotation = MainCamera.transform.rotation;
        if (cameraNumber == 1)
        {
            FixedCamear1.transform.position = MainCamera.transform.position;
            FixedCamear1.transform.rotation = MainCamera.transform.rotation;
            FixedCamear1.fieldOfView = MainCamera.fieldOfView;
        }
        if (cameraNumber == 2)
        {
            FixedCamear2.transform.position = MainCamera.transform.position;
            FixedCamear2.transform.rotation = MainCamera.transform.rotation;
            FixedCamear2.fieldOfView = MainCamera.fieldOfView;
        }
        if (cameraNumber == 3)
        {
            FixedCamear3.transform.position = MainCamera.transform.position;
            FixedCamear3.transform.rotation = MainCamera.transform.rotation;
            FixedCamear3.fieldOfView = MainCamera.fieldOfView;
        }
        if (cameraNumber == 4)
        {
            FixedCamear4.transform.position = MainCamera.transform.position;
            FixedCamear4.transform.rotation = MainCamera.transform.rotation;
            FixedCamear4.fieldOfView = MainCamera.fieldOfView;
        }
        if (cameraNumber == 5)
        {
            FixedCamear5.transform.position = MainCamera.transform.position;
            FixedCamear5.transform.rotation = MainCamera.transform.rotation;
            FixedCamear5.fieldOfView = MainCamera.fieldOfView;
        }
        if (cameraNumber == 6)
        {
            FixedCamear6.transform.position = MainCamera.transform.position;
            FixedCamear6.transform.rotation = MainCamera.transform.rotation;
            FixedCamear6.fieldOfView = MainCamera.fieldOfView;
        }
        saveCamerasToFile();
    }

    public void saveCamerasToFile()
    {
        // http://docs.unity3d.com/Manual/JSONSerialization.html
        // http://docs.unity3d.com/ScriptReference/JsonUtility.html
        CameraData myCamData = new CameraData(MainCamera, FixedCamear1, FixedCamear2, FixedCamear3, FixedCamear4, FixedCamear5, FixedCamear6);
        string json = JsonUtility.ToJson(myCamData);
        SettingsManager.Instance.saveFile(settingsFile, json);
    }
    public void loadCamerasFromFile()
    {
        string json = SettingsManager.Instance.loadFile(settingsFile);
        if (json == "") return;
        CameraData myCamData = new CameraData();
        JsonUtility.FromJsonOverwrite(json, myCamData);
        setCameraData(myCamData);

    }
    void setCameraData(CameraData myCamData)
    {
        MainCamera.transform.position = myCamData.cameraPosition[0];
        MainCamera.transform.rotation = myCamData.cameraRotation[0];
        MainCamera.fieldOfView = myCamData.cameraFOV[0];

        FixedCamear1.transform.position = myCamData.cameraPosition[1];
        FixedCamear1.transform.rotation = myCamData.cameraRotation[1];
        FixedCamear1.fieldOfView = myCamData.cameraFOV[1];

        FixedCamear2.transform.position = myCamData.cameraPosition[2];
        FixedCamear2.transform.rotation = myCamData.cameraRotation[2];
        FixedCamear2.fieldOfView = myCamData.cameraFOV[2];

        FixedCamear3.transform.position = myCamData.cameraPosition[3];
        FixedCamear3.transform.rotation = myCamData.cameraRotation[3];
        FixedCamear3.fieldOfView = myCamData.cameraFOV[3];

        FixedCamear4.transform.position = myCamData.cameraPosition[4];
        FixedCamear4.transform.rotation = myCamData.cameraRotation[4];
        FixedCamear4.fieldOfView = myCamData.cameraFOV[4];

        FixedCamear5.transform.position = myCamData.cameraPosition[5];
        FixedCamear5.transform.rotation = myCamData.cameraRotation[5];
        FixedCamear5.fieldOfView = myCamData.cameraFOV[5];

        FixedCamear6.transform.position = myCamData.cameraPosition[6];
        FixedCamear6.transform.rotation = myCamData.cameraRotation[6];
        FixedCamear6.fieldOfView = myCamData.cameraFOV[6];
    }

}