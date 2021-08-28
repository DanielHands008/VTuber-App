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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    // Add methods for saving, loading and setting controls.
}
