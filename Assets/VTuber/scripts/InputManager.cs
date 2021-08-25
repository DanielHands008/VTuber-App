using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public KeyCode UI_Toggle = KeyCode.Space;
    public KeyCode Move_Forward = KeyCode.W;
    public KeyCode Move_Left = KeyCode.A;
    public KeyCode Move_Back = KeyCode.S;
    public KeyCode Move_Right = KeyCode.D;
    public KeyCode Move_Fast = KeyCode.LeftShift;
    public KeyCode Move_HeightLock = KeyCode.LeftControl;
    public KeyCode Camera_RotateLeft = KeyCode.Q;
    public KeyCode Camera_RotateRight = KeyCode.E;
    public KeyCode Camera_RotateReset = KeyCode.R;
    public KeyCode Camera_ZoomModifier = KeyCode.LeftControl;
    public KeyCode Camera_SelectDynamic = KeyCode.Tab;
    public KeyCode Camera_Select1 = KeyCode.Alpha1;
    public KeyCode Camera_Select2 = KeyCode.Alpha2;
    public KeyCode Camera_Select3 = KeyCode.Alpha3;
    public KeyCode Camera_Select4 = KeyCode.Alpha4;
    public KeyCode Camera_Select5 = KeyCode.Alpha5;
    public KeyCode Camera_Select6 = KeyCode.Alpha6;
    public KeyCode Camera_SaveModifier = KeyCode.LeftControl;
    public KeyCode World_Toggle = KeyCode.H;
    public KeyCode VModel_RotateModifier = KeyCode.LeftAlt;
    public KeyCode VModel_SnapZModifier = KeyCode.LeftControl;
    public KeyCode VModel_SnapXModifier = KeyCode.LeftShift;

    Dictionary<string, bool> inputStates = new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(UI_Toggle))
            InputChange("UI_Toggle", true);
        else if (Input.GetKeyUp(UI_Toggle))
            InputChange("UI_Toggle", false);

        if (Input.GetKeyDown(Move_Forward))
            InputChange("Move_Forward", true);
        else if (Input.GetKeyUp(Move_Forward))
            InputChange("Move_Forward", false);

        if (Input.GetKeyDown(Move_Left))
            InputChange("Move_Left", true);
        else if (Input.GetKeyUp(Move_Left))
            InputChange("Move_Left", false);

        if (Input.GetKeyDown(Move_Back))
            InputChange("Move_Back", true);
        else if (Input.GetKeyUp(Move_Back))
            InputChange("Move_Back", false);

        if (Input.GetKeyDown(Move_Right))
            InputChange("Move_Right", true);
        else if (Input.GetKeyUp(Move_Right))
            InputChange("Move_Right", false);

        if (Input.GetKeyDown(Move_Fast))
            InputChange("Move_Fast", true);
        else if (Input.GetKeyUp(Move_Fast))
            InputChange("Move_Fast", false);

        if (Input.GetKeyDown(Move_HeightLock))
            InputChange("Move_HeightLock", true);
        else if (Input.GetKeyUp(Move_HeightLock))
            InputChange("Move_HeightLock", false);

        if (Input.GetKeyDown(Camera_RotateLeft))
            InputChange("Camera_RotateLeft", true);
        else if (Input.GetKeyUp(Camera_RotateLeft))
            InputChange("Camera_RotateLeft", false);

        if (Input.GetKeyDown(Camera_RotateRight))
            InputChange("Camera_RotateRight", true);
        else if (Input.GetKeyUp(Camera_RotateRight))
            InputChange("Camera_RotateRight", false);

        if (Input.GetKeyDown(Camera_RotateReset))
            InputChange("Camera_RotateReset", true);
        else if (Input.GetKeyUp(Camera_RotateReset))
            InputChange("Camera_RotateReset", false);

        if (Input.GetKeyDown(Camera_ZoomModifier))
            InputChange("Camera_ZoomModifier", true);
        else if (Input.GetKeyUp(Camera_ZoomModifier))
            InputChange("Camera_ZoomModifier", false);

        if (Input.GetKeyDown(Camera_SelectDynamic))
            InputChange("Camera_SelectDynamic", true);
        else if (Input.GetKeyUp(Camera_SelectDynamic))
            InputChange("Camera_SelectDynamic", false);

        if (Input.GetKeyDown(Camera_Select1))
            InputChange("Camera_Select1", true);
        else if (Input.GetKeyUp(Camera_Select1))
            InputChange("Camera_Select1", false);

        if (Input.GetKeyDown(Camera_Select2))
            InputChange("Camera_Select2", true);
        else if (Input.GetKeyUp(Camera_Select2))
            InputChange("Camera_Select2", false);

        if (Input.GetKeyDown(Camera_Select3))
            InputChange("Camera_Select3", true);
        else if (Input.GetKeyUp(Camera_Select3))
            InputChange("Camera_Select3", false);

        if (Input.GetKeyDown(Camera_Select4))
            InputChange("Camera_Select4", true);
        else if (Input.GetKeyUp(Camera_Select4))
            InputChange("Camera_Select4", false);

        if (Input.GetKeyDown(Camera_Select5))
            InputChange("Camera_Select5", true);
        else if (Input.GetKeyUp(Camera_Select5))
            InputChange("Camera_Select5", false);

        if (Input.GetKeyDown(Camera_Select6))
            InputChange("Camera_Select6", true);
        else if (Input.GetKeyUp(Camera_Select6))
            InputChange("Camera_Select6", false);

        if (Input.GetKeyDown(Camera_SaveModifier))
            InputChange("Camera_SaveModifier", true);
        else if (Input.GetKeyUp(Camera_SaveModifier))
            InputChange("Camera_SaveModifier", false);

        if (Input.GetKeyDown(World_Toggle))
            InputChange("World_Toggle", true);
        else if (Input.GetKeyUp(World_Toggle))
            InputChange("World_Toggle", false);

        if (Input.GetKeyDown(VModel_RotateModifier))
            InputChange("VModel_RotateModifier", true);
        else if (Input.GetKeyUp(VModel_RotateModifier))
            InputChange("VModel_RotateModifier", false);

        if (Input.GetKeyDown(VModel_SnapZModifier))
            InputChange("VModel_SnapZModifier", true);
        else if (Input.GetKeyUp(VModel_SnapZModifier))
            InputChange("VModel_SnapZModifier", false);

        if (Input.GetKeyDown(VModel_SnapXModifier))
            InputChange("VModel_SnapXModifier", true);
        else if (Input.GetKeyUp(VModel_SnapXModifier))
            InputChange("VModel_SnapXModifier", false);
    }

    void InputChange(string input, bool state)
    {
        if (!inputStates.ContainsKey(input))
            inputStates.Add(input, state);
        else inputStates[input] = state;
        GlobalEvents.Instance.EventsInput.Invoke(input, state);
    }

    public bool InputState(string input)
    {
        if (inputStates.ContainsKey(input))
            return inputStates[input];
        return false;
    }

    // Add methods for saving, loading and setting controls.
}
