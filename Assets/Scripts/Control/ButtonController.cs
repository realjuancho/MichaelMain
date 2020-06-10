using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public string buttonName;

    bool buttonUp;
    bool buttonDown;
    bool button;
    Animator buttonAnimator;

    void Awake()
    {
        buttonAnimator = gameObject.GetComponent<Animator>();
    }


    public void PushButton()
    {
        buttonAnimator.SetBool("push", true);

        buttonDown = true;
        button = true;
        buttonUp = false;
    }

    public void HoldButton()
    {
        buttonAnimator.SetBool("push", true);

        button = true;
        buttonDown = false;
    }

    public void ReleaseButton()
    {
        buttonAnimator.SetBool("push", true);

        button = false;
        buttonDown = false;
        buttonUp = true;
    }


    public bool GetButton()
    {
        return button;
    }
    public bool GetButtonDown()
    {
        return buttonDown;
    }

    public bool GetButtonUp()
    {
        return buttonUp;
    }


}
