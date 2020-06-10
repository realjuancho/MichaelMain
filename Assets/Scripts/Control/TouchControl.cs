using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    public float distanceFromCamera = 1.0f;
    public bool DebugTouchControl;
    public Camera controllerCamera;
    public LayerMask TouchControllerLayer;

    [Range(0.33f, 0.5f)]
    public float leftStickAreaProportion = 0.33f;
    [Range(0.33f, 0.5f)]
    public float rightStickAreaProportion = 0.33f;
    [Range(0.33f, 0.5f)]
    public float buttonsAreaProportion = 0.33f;

    const string Left_Stick_VerticalAxisName = "LeftAnalog_Vertical";
    const string Left_StickHorizontalAxisName = "LeftAnalog_Horizontal";
    const string Right_VerticalAxisName = "RightAnalog_Vertical";
    const string Right_HorizontalAxisName = "RightAnalog_Horizontal";
    const float grados = 25f;

    AnalogStick leftStick;
    AnalogStick rightStick;


    ButtonController[] buttons;


    Transform referencePlane;

    Transform leftStickAreaPlane;
    Transform rightStickAreaPlane;
    Transform buttonsAreaPlane;

    Transform mainCameraTransform;

    private void Awake()
    {
        InitializeObjects();
    }

    private void Update()
    {
        AdjustReferencePlane();
        HandleTouch();
    }


    const string verticalAxisName = "Vertical";
    const string horizontalAxisName = "Horizontal";

    public enum AxisName { LeftStick_Horizontal, LeftStick_Vertical, RightStick_Horizontal, RightStick_Vertical };

    public float GetAxis(AxisName axisName)
    {
        switch (axisName)
        {
            case AxisName.LeftStick_Vertical:
                return leftStick.GetInput(verticalAxisName);

            case AxisName.LeftStick_Horizontal:
                return leftStick.GetInput(horizontalAxisName);

            case AxisName.RightStick_Vertical:
                return rightStick.GetInput(verticalAxisName);

            case AxisName.RightStick_Horizontal:
                return rightStick.GetInput(horizontalAxisName);
            default:
                return 0f;
        }
    }

    public bool GetButton(string buttonName)
    {
        foreach(ButtonController button in buttons)
        {
            if (button.buttonName == buttonName)
            {
                return button.GetButton();
            }
        }
        return false;
    }

    public bool GetButtonDown(string buttonName)
    {
        foreach (ButtonController button in buttons)
        {
            if (button.buttonName == buttonName)
            {
                return button.GetButtonDown();
            }
        }
        return false;
    }

    public bool GetButtonUp(string buttonName)
    {
        foreach (ButtonController button in buttons)
        {
            if (button.buttonName == buttonName)
            {
                return button.GetButtonUp();
            }
        }
        return false;
    }

    enum AnalogStickOperation { FadeIn, FadeOut, Move, Center, Leave }

    void OperateAnalogStick(AnalogStick stick, AnalogStickOperation operation, Vector3 point)
    {
        //Debug.Log(stick.ToString() + "" + operation.ToString());

        switch (operation)
        {
            case AnalogStickOperation.FadeIn:
            {
                if (stick == leftStick)
                {
                    leftStickPosition = point;
                }
                else if (stick == rightStick)
                {
                    rightStickPosition = point;
                }
            }
            break;

            case AnalogStickOperation.Move:
                {
                    stick.moveDirection = point;
                    stick.Move();
                }
                break;
            case AnalogStickOperation.Leave:
                {

                    stick.ReturnToZero();
                }
                break;
        }      
    }


    void HandleTouch()
    {
        if (Input.touchCount > 0)
        {

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                Vector3 touchPosition = touch.position;
                touchPosition.z = distanceFromCamera * 1.5f;

                Vector3 destination = controllerCamera.ScreenToWorldPoint(touchPosition);
                Vector3 direction = destination - controllerCamera.transform.position;

                Debug.DrawRay(controllerCamera.transform.position, direction, Color.white);

                Ray ray = new Ray(controllerCamera.transform.position, direction);
                RaycastHit rhit = new RaycastHit();
                Physics.Raycast(ray, out rhit, TouchControllerLayer);
                if (rhit.collider)
                {
                    AnalogStick stick = null;
                    Vector3 point = rhit.point;
                    GameObject colliderGameObject = rhit.collider.gameObject;
                    if (colliderGameObject.name == leftStickAreaPlane.name)
                    {
                        Debug.DrawRay(controllerCamera.transform.position, direction, Color.Lerp(Color.red, Color.yellow, 0.5f));
                        stick = leftStick;
                    }
                    else if (colliderGameObject.name == rightStickAreaPlane.name)
                    {
                        Debug.DrawRay(controllerCamera.transform.position, direction, Color.blue);
                        stick = rightStick;
                    }

                    if (stick)
                    {
                        switch (touch.phase)
                        {
                            case TouchPhase.Began:
                                OperateAnalogStick(stick, AnalogStickOperation.FadeIn, point);
                                break;
                            case TouchPhase.Moved:
                                OperateAnalogStick(stick, AnalogStickOperation.Move, point);
                                break;
                            case TouchPhase.Stationary:
                                OperateAnalogStick(stick, AnalogStickOperation.Move, point);
                                OperateAnalogStick(stick, AnalogStickOperation.Center, point);
                                break;
                            case TouchPhase.Ended:
                                OperateAnalogStick(stick, AnalogStickOperation.Leave, point);
                                break;
                        }
                    }
                    //Array.Exists(planets, element => element.StartsWith("M")));

                    ButtonController button = Array.Find(buttons, element => element.gameObject == colliderGameObject.transform.parent.gameObject);
                    if (button)
                    {
                        switch (touch.phase)
                        {
                            case TouchPhase.Began:
                                button.PushButton();
                                break;

                            case TouchPhase.Stationary:
                                button.HoldButton();
                               break;

                            case TouchPhase.Moved:
                                button.HoldButton();
                                break;

                            case TouchPhase.Canceled:
                                button.ReleaseButton();
                                break;
                            case TouchPhase.Ended:
                                button.ReleaseButton();
                                break;
                        }
                    }
                } // end check for collider
            } //end for loop
        }
        else
        {
            OperateAnalogStick(leftStick, AnalogStickOperation.Leave, Vector3.zero);
            OperateAnalogStick(rightStick, AnalogStickOperation.Leave, Vector3.zero);

            foreach (ButtonController b in buttons)
            {
                b.ReleaseButton();
            }
        }
    }

    void InitializeObjects()
    {
        if(!leftStick) leftStick = transform.Find("left_analogStick").GetComponent<AnalogStick>();
        if (!rightStick) rightStick = transform.Find("right_analogStick").GetComponent<AnalogStick>();
        if (!referencePlane) referencePlane = transform.Find("referencePlane").transform;

        if (!leftStickAreaPlane) leftStickAreaPlane = transform.Find("leftStickArea").transform;
        if (!rightStickAreaPlane) rightStickAreaPlane = transform.Find("rightStickArea").transform;
        if (!buttonsAreaPlane) buttonsAreaPlane = transform.Find("buttonsArea").transform;

        if (buttons == null) 
            buttons = transform.GetComponentsInChildren<ButtonController>();

       

        if (!controllerCamera) { 
            controllerCamera = Camera.main; 
        }

        mainCameraTransform = controllerCamera.transform;
    }






    Vector3 leftStickPosition = Vector3.zero;
    Vector3 rightStickPosition = Vector3.zero;

    void AdjustReferencePlane()
    {
        //Adjust the reference plane and puts it a certain distance From Camera (distanceFromCamera)
        Vector3 controlsHeight = mainCameraTransform.position + mainCameraTransform.forward * distanceFromCamera;
        referencePlane.position = controlsHeight;

        float frustumHeight = distanceFromCamera * Mathf.Tan(controllerCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * controllerCamera.aspect;

        Vector3 referencePlaneScale = Vector3.one;
        referencePlaneScale.z = frustumHeight;
        referencePlaneScale.x = frustumWidth;
        referencePlaneScale /= 5;
        referencePlane.localScale = referencePlaneScale;


        controlsHeight -= mainCameraTransform.forward * 0.01f;
        //AdjustLeftStickArea a little bit above the reference plane

        Vector3 leftStickAreaScale = referencePlaneScale * leftStickAreaProportion;
        leftStickAreaScale.z = frustumHeight;

        leftStickAreaPlane.localScale = leftStickAreaScale;
        leftStickAreaPlane.position = controlsHeight
            - mainCameraTransform.right * frustumWidth * leftStickAreaProportion
            - mainCameraTransform.up * frustumHeight * leftStickAreaProportion;

        rightStickAreaPlane.localScale = referencePlaneScale * rightStickAreaProportion;
        rightStickAreaPlane.position = controlsHeight
            + mainCameraTransform.right * frustumWidth * rightStickAreaProportion
            - mainCameraTransform.up * frustumHeight * rightStickAreaProportion;




        buttonsAreaPlane.localScale = referencePlaneScale * buttonsAreaProportion;
        buttonsAreaPlane.position = controlsHeight
            + mainCameraTransform.right * frustumWidth * buttonsAreaProportion
            + mainCameraTransform.up * frustumHeight * buttonsAreaProportion;



        
        //Scale the analog sticks proportional to 1 fourth of the distance from the camera 
        Vector3 leftStickScale = Vector3.one * distanceFromCamera / (4 / leftStickAreaProportion) ;
        leftStick.transform.localScale = leftStickScale;

        Vector3 rightstickScale = Vector3.one * distanceFromCamera / (4 / leftStickAreaProportion);
        rightStick.transform.localScale = rightstickScale;


        //Move sticks into position;
        if (leftStickPosition == Vector3.zero)
            leftStickPosition = leftStickAreaPlane.position;

        leftStick.transform.position = leftStickPosition;

        if (rightStickPosition == Vector3.zero)
            rightStickPosition = rightStickAreaPlane.position;

        rightStick.transform.position = rightStickPosition;
    }
}
