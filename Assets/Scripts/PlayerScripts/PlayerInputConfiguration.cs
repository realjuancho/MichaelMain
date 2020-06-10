using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputConfiguration : MonoBehaviour
{

    public bool AllowPlayerInput = true;

    public Common.PlayerInputButton JumpButton;
    public Common.PlayerInputButton ActionButton;
    public Common.PlayerInputButton AttackButton;

    public Common.PlayerInputAxis MoveStickOrDPad;

}
