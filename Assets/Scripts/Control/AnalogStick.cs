using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogStick : MonoBehaviour
{

    public bool DebugAnalogStick;

    public bool IsEnabled = true;

    Transform analogStick;
    Transform analogStickBase;

    float rayLenght = 2.0f;
    float activeZone = 0.5f;
    float HorizontalInput;
    float VerticalInput;

    // Update is called once per frame
    void Awake()
    {
        InitializeObjects();
        //Saves initial analogstick rotation;
        zeroRotation = analogStick.rotation;
    }

    private void Update()
    {


        if (IsEnabled)
            CalculateInput();
        else
        {
            analogStick.gameObject.GetComponent<MeshRenderer>().enabled = false;
            analogStickBase.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

	public Vector3 moveDirection;
    public void Move()
    {
        //Gets the height of the analogStick bounds
        Bounds bounds = analogStick.GetComponent<MeshRenderer>().bounds;
        float stickHeight = bounds.extents.y * rayLenght;
        //saves the original position of the joystick, and the upwards position
        Vector3 originalPosition = analogStick.position;
        Vector3 upwardsPosition = transform.up.normalized * stickHeight;

        //calculates the direction of the stick calculated from the originalposition vs the move direction;
        Vector3 direction = moveDirection - originalPosition;

        //normalizes the direction to that of the height of the stick
        //direction = direction.normalized * stickHeight;
        Debug.DrawRay(originalPosition, direction, Color.white);
        Debug.DrawRay(originalPosition, upwardsPosition, Color.cyan);


        Vector3 resultante = direction + upwardsPosition;
        Debug.DrawRay(originalPosition, resultante, Color.green);


        //Cambia el vector forward de Z a X
        analogStick.rotation = Quaternion.LookRotation(resultante) * Quaternion.Euler(new Vector3(90, 0, 0));


    }

    Quaternion zeroRotation;
    public void ReturnToZero()
    {
        analogStick.rotation = zeroRotation;
    }

    public void Center(Vector3 moveDirection)
    {

    }


	void InitializeObjects()
    {
        if (!analogStickBase) analogStickBase = transform.Find("analogStick.Base");
        if (!analogStick) analogStick = transform.Find("analogStick.Stick");

       
    }



    void CalculateInput()
    {
        Ray upwardsRay = new Ray(analogStick.position, analogStick.up * rayLenght);
        Ray horizontalRay = new Ray(analogStickBase.position, analogStickBase.right * rayLenght);
        Ray verticalRay = new Ray(analogStickBase.position, analogStickBase.forward * rayLenght);

        HorizontalInput = Vector3.Dot(upwardsRay.direction.normalized, horizontalRay.direction.normalized);
        VerticalInput = Vector3.Dot(upwardsRay.direction.normalized, verticalRay.direction.normalized);

        HorizontalInput = HorizontalInput / activeZone;
        VerticalInput = VerticalInput / activeZone;

        HorizontalInput =  Mathf.Clamp(HorizontalInput, -1, 1);
        VerticalInput = Mathf.Clamp(VerticalInput, -1, 1);
    }

    void OnDrawGizmos()
    {
        if (DebugAnalogStick)
        {
            InitializeObjects();

            Debug.DrawRay(analogStick.position, analogStick.up * rayLenght, Color.green);
            Debug.DrawRay(analogStickBase.position, analogStickBase.right * rayLenght, Color.red);
            Debug.DrawRay(analogStickBase.position, analogStickBase.forward * rayLenght, Color.blue);

            CalculateInput();
        }
    }

    public float GetInput(string Axis)
    {
        switch(Axis)
        {
            case "Horizontal":
                return HorizontalInput;
            case "Vertical":
                return VerticalInput * -1;
            default:
                return 0.0f;
        }

    }
}
