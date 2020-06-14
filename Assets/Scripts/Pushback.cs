using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushback : MonoBehaviour
{

    CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }


    private void Update()
    {

        
    }

    public void AddPushback(float power, Vector3 pushBackDirection)
    {

        Vector3 direction = transform.position - pushBackDirection * power ;


        characterController.Move(direction * power * Time.deltaTime);
    }
}
