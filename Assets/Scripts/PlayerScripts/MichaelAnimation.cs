using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MichaelAnimation : MonoBehaviour
{
    Animator animatorController;
    MovePlayer movePlayer;

    const string forwardMovement_Float = "ForwardMovement";

    // Start is called before the first frame update
    void Awake()
    {
        movePlayer = GetComponentInParent<MovePlayer>();
        animatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float f = movePlayer.GetForwardMovement();

        animatorController.SetFloat(forwardMovement_Float, f);
    }
}
