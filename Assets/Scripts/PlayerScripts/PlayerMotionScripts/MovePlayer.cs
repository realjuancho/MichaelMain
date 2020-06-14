using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{

    [Range (1f,20f)]
    public float moveSpeed = 6.0f;
    public float rotateSpeed = 1.0f;

    public bool DebugMovePlayer = false;
    public float DeadZone = 0.2f;

    float moveInput;
    string InputXAxis;
    string InputYAxis;
    float maxMoveSpeed;

    Transform camTransform;
    Vector3 camRelativeForward;
    CharacterController controller;

    TouchControl touchControl;

    JumpPlayer jump;
    MainAttackPlayer attack;

    Player player;
    PlayerInputConfiguration playerInputConfiguration;

    private void Awake()
	{
        controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        jump = GetComponent<JumpPlayer>();
        attack = GetComponent<MainAttackPlayer>();

        touchControl = FindObjectOfType<TouchControl>();
        playerInputConfiguration = GetComponent<PlayerInputConfiguration>();

        maxMoveSpeed = moveSpeed;

        InitializeInput();

        camTransform = Camera.main.transform;
	}

	void Update()
    {
        CheckInput();
        CheckMove();

        GetForwardMovement();
    }


    


    void InitializeInput()
    {
        if (player.playerId != Common.PlayerId.TouchPlayer)
        {
            switch (player.playerId)
            {
                case Common.PlayerId.Player1:
                    InputXAxis = InputYAxis = Common.Hash.Player1_Input_Prefix;
                    break;
                case Common.PlayerId.Player2:
                    InputXAxis = InputYAxis = Common.Hash.Player2_Input_Prefix;
                    break;
                case Common.PlayerId.Player3:
                    InputXAxis = InputYAxis = Common.Hash.Player3_Input_Prefix;
                    break;
                case Common.PlayerId.Player4:
                    InputXAxis = InputYAxis = Common.Hash.Player4_Input_Prefix;
                    break;
            }

            switch (playerInputConfiguration.MoveStickOrDPad)
            {
                case Common.PlayerInputAxis.LStick:
                    InputXAxis += Common.Hash.LStick_Horizontal;
                    InputYAxis += Common.Hash.LStick_Vertical;
                    break;
                case Common.PlayerInputAxis.RStick:
                    InputXAxis += Common.Hash.RStick_Horizontal;
                    InputYAxis += Common.Hash.RStick_Vertical;
                    break;
                case Common.PlayerInputAxis.DPad:
                    InputXAxis += Common.Hash.DPad_Horizontal;
                    InputYAxis += Common.Hash.DPad_Vertical;
                    break;
            }
        }

    }


    void CheckInput()
    {
        if (player.playerState == Common.PlayerState.Jugando)
        {

            float x = 0;
            float z = 0;

            if (touchControl && player.playerId == Common.PlayerId.TouchPlayer)
            {
                x = touchControl.GetAxis(TouchControl.AxisName.LeftStick_Horizontal);
                z = touchControl.GetAxis(TouchControl.AxisName.LeftStick_Vertical);
            }
            //Movimiento
            else
            {
                x = Input.GetAxis(InputXAxis);
                z = Input.GetAxis(InputYAxis);
            }

            if (Mathf.Abs(x) > DeadZone || Mathf.Abs(z) > DeadZone)
            {
                Vector3 posicion = new Vector3(x, 0, z);

                camRelativeForward = Vector3.Scale(camTransform.forward
                                                   , new Vector3(1, 0, 1)).normalized;

                posicion = posicion.x * camTransform.right
                                      - posicion.z * camRelativeForward;

                if (posicion.magnitude > 1.0f)
                    posicion.Normalize();

                moveInput = posicion.magnitude;
                posicion = Vector3.ProjectOnPlane(posicion, Vector3.up);
                PlayerMove(posicion);


                //Rotación         
                Vector3 tpoint = transform.position + posicion;
                if (DebugMovePlayer) Debug.DrawLine(transform.position, tpoint, Color.red);

                Vector3 rotateDirection = tpoint - transform.position;
                float singleStep = rotateSpeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward,
                                                             rotateDirection
                                                             , singleStep, 0.0f);

                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else
            {
                moveInput = 0;
            }

               
        }
    }

    void CheckMove()
    {
        float speedFactor = 1;
        //Jump
        if (jump)
        {
            if (!jump.IsPlayerGrounded())
            {
                speedFactor *= jump.airMoveSpeed;
            }
        }

        //attack
        if(attack)
        {
            if (attack.IsAttacking())
            {
                speedFactor *= attack.attackMoveSpeed;
            }
        }
        moveSpeed = maxMoveSpeed * speedFactor;
    }

    void PlayerMove(Vector3 direccionMovimiento)
    {

        controller.Move(
            direccionMovimiento
            * (moveSpeed * moveInput
            * Time.deltaTime)
            );
    }

 
    public float GetForwardMovement()
    {
        float forwardMovement = moveSpeed * moveInput / maxMoveSpeed;
        return forwardMovement;
    }


}
