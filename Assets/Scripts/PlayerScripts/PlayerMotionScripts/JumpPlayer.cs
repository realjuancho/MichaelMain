using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpPlayer : MonoBehaviour
{

    public string jumpButtonName = "Jump";
    [Range(1, 10)]
    public float jumpPower = 6.0f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public int jumpCount = 2;
    [Range(0f, 1f)]
    public float airMoveSpeed = 0.5f;

    public bool debugJump;

    CharacterController controller;
    float slopeLimit;
    float jumpSlopeLimit;

    int maxJumpCount;
    string inputJump;
    bool IsGrounded;

    Player player;
    PlayerInputConfiguration playerInputConfiguration;
    MainAttackPlayer attack;

    TouchControl touchControl;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        attack = GetComponent<MainAttackPlayer>();
        touchControl = GameObject.FindObjectOfType<TouchControl>();
        playerInputConfiguration = GetComponent<PlayerInputConfiguration>();


        InitializeInput();


        slopeLimit = controller.slopeLimit;
        jumpSlopeLimit = Mathf.Clamp(controller.slopeLimit * 2, 0f, 90f);

        maxJumpCount = jumpCount;
    }

    // Update is called once per frame

    Vector3 jumpVelocity;
    Vector3 gravity;
    void Update()
    {

        switch (player.playerState)
        {
            case Common.PlayerState.Jugando:
                HandleJump();
                HandleFall();
                break;
        }

    }

    void InitializeInput()
    {

        if (player.playerId != Common.PlayerId.TouchPlayer)
        {
            switch (player.playerId)
            {

                case Common.PlayerId.Player1:
                    inputJump = Common.Hash.Player1_Input_Prefix;
                    break;
                case Common.PlayerId.Player2:
                    inputJump = Common.Hash.Player2_Input_Prefix;
                    break;
                case Common.PlayerId.Player3:
                    inputJump = Common.Hash.Player3_Input_Prefix;
                    break;
                case Common.PlayerId.Player4:
                    inputJump = Common.Hash.Player4_Input_Prefix;
                    break;
            }

            switch (playerInputConfiguration.JumpButton)
            {
                case Common.PlayerInputButton.Circle_Button:
                    inputJump += Common.Hash.Circle_Button_Suffix;
                    break;
                case Common.PlayerInputButton.Cross_Button:
                    inputJump += Common.Hash.Cross_Button_Suffix;
                    break;
                case Common.PlayerInputButton.Square_Button:
                    inputJump += Common.Hash.Square_Button_Suffix;
                    break;
                case Common.PlayerInputButton.Triangle_Button:
                    inputJump += Common.Hash.Triangle_Button_Suffix;
                    break;
                case Common.PlayerInputButton.L1_Button:
                    inputJump += Common.Hash.L1_Button_Suffix;
                    break;
                case Common.PlayerInputButton.L2_Button:
                    inputJump += Common.Hash.L2_Button_Suffix;
                    break;
                case Common.PlayerInputButton.L3_Button:
                    inputJump += Common.Hash.L3_Button_Suffix;
                    break;
                case Common.PlayerInputButton.R1_Button:
                    inputJump += Common.Hash.R1_Button_Suffix;
                    break;
                case Common.PlayerInputButton.R2_Button:
                    inputJump += Common.Hash.R2_Button_Suffix;
                    break;
                case Common.PlayerInputButton.R3_Button:
                    inputJump += Common.Hash.R3_Button_Suffix;
                    break;
            }

        }
    }

    void HandleJump()
    {
        bool jump = false;

        if (touchControl && player.playerId == Common.PlayerId.TouchPlayer)
            jump = touchControl.GetButtonDown(jumpButtonName);
        else
        {
            jump = Input.GetButtonDown(inputJump) || Input.GetButtonDown("Jump");
        }


        if (jumpCount > 0)
        {
            if (jump)
            {
                float percent = jumpCount / maxJumpCount;
                jumpVelocity += Vector3.up * jumpPower * percent;
                gravity = Vector3.zero;
                jumpCount -= 1;
            }
        }
        controller.Move(jumpVelocity * Time.deltaTime);
        IsGrounded = controller.isGrounded;
    }

    void HandleFall()
    {
        bool jumpDown = false;
        if (touchControl && player.playerId == Common.PlayerId.TouchPlayer)

            jumpDown = touchControl.GetButton(jumpButtonName);
        else
            jumpDown = Input.GetButton(inputJump) || Input.GetButton(jumpButtonName);

        if (!IsGrounded)
        {
            if (!jumpDown) gravity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            else gravity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }


        //Factor in attack fall speed requires MainAttackPlayer component;
        float fallSpeedFactor = 1;
        if (attack)
        {
            if (attack.IsAttacking())
            {
                fallSpeedFactor *= attack.attackFallSpeed;
            }
        }


        controller.Move(gravity * Time.deltaTime * fallSpeedFactor);
        IsGrounded = controller.isGrounded;

        if (gravity.y < 0 && IsGrounded)
        {
            gravity = Vector3.zero;
            jumpVelocity = Vector3.zero;
            jumpCount = maxJumpCount;
        }

        if (IsGrounded)
        {
            controller.slopeLimit = slopeLimit;
        }
        else
        {
            controller.slopeLimit = jumpSlopeLimit;
        }
    }

    public bool IsPlayerGrounded()
    {
        return IsGrounded;
    }
}
