using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MainAttackPlayer : MonoBehaviour
{

    public string attackButtonName = "Attack";

    [Range(0f, 1f)]
    public float attackRange;
    [Range(0f, 1f)]
    public float attackMoveSpeed;
    [Range(0f, 1f)]
    public float attackFallSpeed;
    
    public float DamagePower;

    [Range(0f, 10f)]
    public float attackReactionPower;
    public AttackCombo[] attackCombo;

    string InputMainAttack;
    Player player;
    PlayerInputConfiguration playerInputConfiguration;
    AttackPlayer currentAttack;
    TouchControl touchControl;


    void Awake()
    {
        player = GetComponent<Player>();
        playerInputConfiguration = GetComponent<PlayerInputConfiguration>();
        touchControl = GameObject.FindObjectOfType<TouchControl>();

        InitializeInput();

        int i = 0;

        foreach (AttackCombo attack in attackCombo)
        {
            attack.attackId = i;
            attack.attack.AttackComboOrder = i;
            attack.attackRange = attackRange;
            attack.attackReactionPower = attackReactionPower;
            i++;
        }
    }

    void LateUpdate()
    {
        switch (player.playerState)
        {
            case Common.PlayerState.Jugando:
                HandleAttackInput();
                HandleCurrentAttack();
                break;
        }
    }

    void HandleAttackInput()
    {
        bool InputAttack = false;

        if(touchControl && player.playerId == Common.PlayerId.TouchPlayer )
        {
            InputAttack = touchControl.GetButtonDown(attackButtonName);
        }

        else if (Input.GetButtonDown(InputMainAttack) || Input.GetButtonDown(attackButtonName))
        {
            InputAttack = true;
        }

        if (InputAttack)
        {
            int idCurrentAttack = 0;
            if (currentAttack)
            {
                if (!currentAttack.Equals(attackCombo[attackCombo.Length - 1].attack))
                {
                    idCurrentAttack = currentAttack.AttackComboOrder + 1;
                }
            }

            currentAttack = attackCombo[idCurrentAttack].attack;
            currentAttack.attackReaction = attackCombo[idCurrentAttack].attackReaction;
            currentAttack.attackType = attackCombo[idCurrentAttack].attackType;
            currentAttack.attackRange = attackRange;
            currentAttack.attackReactionPower = attackReactionPower;
            currentAttack.Damage = DamagePower;

            currentAttack.Fire();

        }
    }

    void HandleCurrentAttack()
    {
        if (currentAttack)
        {
            if (!currentAttack.IsFired())
            {
                currentAttack = null;
            }
        }
    }


    void InitializeInput()
    {

        if (player.playerId != Common.PlayerId.TouchPlayer)
        {

            switch (player.playerId)
            {

                case Common.PlayerId.Player1:
                    InputMainAttack = Common.Hash.Player1_Input_Prefix;
                    break;
                case Common.PlayerId.Player2:
                    InputMainAttack = Common.Hash.Player2_Input_Prefix;
                    break;
                case Common.PlayerId.Player3:
                    InputMainAttack = Common.Hash.Player3_Input_Prefix;
                    break;
                case Common.PlayerId.Player4:
                    InputMainAttack = Common.Hash.Player4_Input_Prefix;
                    break;
            }
            switch (playerInputConfiguration.AttackButton)
            {
                case Common.PlayerInputButton.Circle_Button:
                    InputMainAttack += Common.Hash.Circle_Button_Suffix;
                    break;
                case Common.PlayerInputButton.Cross_Button:
                    InputMainAttack += Common.Hash.Cross_Button_Suffix;
                    break;
                case Common.PlayerInputButton.Square_Button:
                    InputMainAttack += Common.Hash.Square_Button_Suffix;
                    break;
                case Common.PlayerInputButton.Triangle_Button:
                    InputMainAttack += Common.Hash.Triangle_Button_Suffix;
                    break;
                case Common.PlayerInputButton.L1_Button:
                    InputMainAttack += Common.Hash.L1_Button_Suffix;
                    break;
                case Common.PlayerInputButton.L2_Button:
                    InputMainAttack += Common.Hash.L2_Button_Suffix;
                    break;
                case Common.PlayerInputButton.L3_Button:
                    InputMainAttack += Common.Hash.L3_Button_Suffix;
                    break;
                case Common.PlayerInputButton.R1_Button:
                    InputMainAttack += Common.Hash.R1_Button_Suffix;
                    break;
                case Common.PlayerInputButton.R2_Button:
                    InputMainAttack += Common.Hash.R2_Button_Suffix;
                    break;
                case Common.PlayerInputButton.R3_Button:
                    InputMainAttack += Common.Hash.R3_Button_Suffix;
                    break;
            }
        }
    }

    public bool IsAttacking()
    {
        if (currentAttack)
        {
            return true;
        }
        return false;
    }

    [Serializable]
    public class AttackCombo
    {
        public int attackId;
        public AttackPlayer attack;
        public Common.MainAttackType attackType;
        public Common.AttackReaction attackReaction;
        public float attackRange;
        public float attackReactionPower;
    }

}
