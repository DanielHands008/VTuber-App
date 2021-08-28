using System.Collections;
using UnityEngine;

public class DynamicCameraControl : MonoBehaviour
{
    public float normalSpeed = 2.0f; //Normal movement speed

    public float shiftSpeed = 5.0f; //multiplies movement speed by how long shift is held down.

    public float speedCap = 10.0f; //Max cap for speed when shift is held down

    public float cameraSensitivity = 1f; //How sensitive it with mouse

    public float mouseWheelHeightSensitivity = 10f;

    private Vector3 mouseLocation; //Mouse location on screen during play (Set to near the middle of the screen)

    private float totalSpeed = 1.0f; //Total speed variable for shift

    private bool camEnabled = false;

    private bool ctrlDown = false;

    public string mouseHorizontalAxisName = "Mouse X";
    public string mouseVerticalAxisName = "Mouse Y";

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private float originalFOV;
    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        originalFOV = Camera.main.fieldOfView;
    }

    void Update()
    {
        if (Camera.main != null)
        {

            if (camEnabled)
            {

                //Camera angles based on mouse position
                mouseLocation = Input.mousePosition - mouseLocation;

                // mouseLocation = new Vector3 (-mouseLocation.y * cameraSensitivity, mouseLocation.x * cameraSensitivity, 0);

                // mouseLocation = new Vector3 (transform.eulerAngles.x + mouseLocation.x, transform.eulerAngles.y + mouseLocation.y, 0);

                //mouseLocation = new Vector3 (-Input.GetAxis (mouseHorizontalAxisName) * cameraSensitivity, -Input.GetAxis (mouseVerticalAxisName) * cameraSensitivity, 0);
                if ((Input.GetAxis(mouseVerticalAxisName) > 0 && (transform.eulerAngles.x < 100 || transform.eulerAngles.x > 280)) || (Input.GetAxis(mouseVerticalAxisName) < 0 && (transform.eulerAngles.x < 80 || transform.eulerAngles.x > 260)))
                    mouseLocation = new Vector3(transform.eulerAngles.x - Input.GetAxis(mouseVerticalAxisName) * cameraSensitivity, transform.eulerAngles.y + Input.GetAxis(mouseHorizontalAxisName) * cameraSensitivity, transform.eulerAngles.z);
                else
                    mouseLocation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Input.GetAxis(mouseHorizontalAxisName) * cameraSensitivity, transform.eulerAngles.z);

                transform.eulerAngles = mouseLocation;

                mouseLocation = Input.mousePosition;
            }

            // FOV Controls / Height Control
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                if (ctrlDown)
                {
                    if (Camera.main.fieldOfView > 20)
                        Camera.main.fieldOfView -= 5;
                    else if (Camera.main.fieldOfView > 16)
                        Camera.main.fieldOfView -= 4;
                    else if (Camera.main.fieldOfView > 13)
                        Camera.main.fieldOfView -= 3;
                    else if (Camera.main.fieldOfView > 11)
                        Camera.main.fieldOfView -= 2;
                    else if (Camera.main.fieldOfView > 1)
                        Camera.main.fieldOfView -= 1;
                }

            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                if (ctrlDown)
                {
                    if (Camera.main.fieldOfView < 11)
                        Camera.main.fieldOfView += 1;
                    else if (Camera.main.fieldOfView < 13)
                        Camera.main.fieldOfView += 2;
                    else if (Camera.main.fieldOfView < 16)
                        Camera.main.fieldOfView += 3;
                    else if (Camera.main.fieldOfView < 20)
                        Camera.main.fieldOfView += 4;
                    else if (Camera.main.fieldOfView < 80)
                        Camera.main.fieldOfView += 5;
                }
            }

            //Keyboard controls

            Vector3 Cam = GetBaseInput();
            if (Input.GetKey(InputManager.Instance.Move_Fast))
            {
                totalSpeed += Time.deltaTime;

                Cam = Cam * totalSpeed * shiftSpeed;

                Cam.x = Mathf.Clamp(Cam.x, -speedCap, speedCap);

                Cam.y = Mathf.Clamp(Cam.y, -speedCap, speedCap);

                Cam.z = Mathf.Clamp(Cam.z, -speedCap, speedCap);

            }
            else
            {

                totalSpeed = Mathf.Clamp(totalSpeed * 0.5f, 1f, 1000f);

                Cam = Cam * normalSpeed;

            }

            Cam = Cam * Time.deltaTime;

            Vector3 newPosition = transform.position;

            if (Input.GetKey(InputManager.Instance.Camera_RotateLeft))
            {
                if (transform.eulerAngles.z < 30 || transform.eulerAngles.z > 329)
                    transform.Rotate(Vector3.forward, 1f);
            }
            if (Input.GetKey(InputManager.Instance.Camera_RotateRight))
            {
                if (transform.eulerAngles.z < 31 || transform.eulerAngles.z > 330)
                    transform.Rotate(Vector3.forward, -1f);
            }
            if (Input.GetKey(InputManager.Instance.Camera_RotateReset))
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
            }

            if (Input.GetKey(InputManager.Instance.Move_HeightLock))
            {
                ctrlDown = true;
                //If the player wants to move on X and Z axis only by pressing space (good for re-adjusting angle shots)
                transform.Translate(Cam);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;

            }
            else
            {
                ctrlDown = false;

                transform.Translate(Cam);

            }

            if (Input.GetMouseButtonDown(1))
            {
                camEnabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                mouseLocation = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(1))
            {
                camEnabled = false;
                Cursor.lockState = CursorLockMode.None;
                CameraControl.Instance.saveCamerasToFile();
            }
        }
    }

    private Vector3 GetBaseInput()
    {

        Vector3 cameraVelocity = new Vector3();

        float HorizontalInput = Input.GetAxis("Horizontal"); //Input for horizontal movement

        float VerticalInput = Input.GetAxis("Vertical"); //Input for Vertical movement

        float WheelInput = 0;
        if (!ctrlDown)
            WheelInput = Input.GetAxis("Mouse ScrollWheel");

        cameraVelocity += new Vector3(0, WheelInput * mouseWheelHeightSensitivity, 0);

        cameraVelocity += new Vector3(HorizontalInput, 0, 0);

        cameraVelocity += new Vector3(0, 0, VerticalInput);

        return cameraVelocity;

    }

    public void resetPosition()
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        Camera.main.fieldOfView = originalFOV;
    }

}