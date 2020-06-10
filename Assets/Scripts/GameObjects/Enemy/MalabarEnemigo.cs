using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalabarEnemigo : MonoBehaviour
{
    public float gravityMultiplier = 9.8f;
    public float juggleSpeed = 1.0f;
    public float maxJuggleFactor = 1.0f;
    float JuggleFactor;
    [SerializeField]
    bool IsGrounded;
    CharacterController controller;
    Enemigo enemigo;
    

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        enemigo = GetComponent<Enemigo>();
    }

    private void Update()
    {
        if (enemigo.enemyState != Common.EnemyState.Dead)
        {
            HandleFall();
            HandleJuggle();

        }
    }

    [SerializeField]
    Vector3 gravity = Vector3.zero;

    float currentJuggleFactor;
    void HandleFall()
    {
        gravity += Vector3.up * Physics.gravity.y * (gravityMultiplier-1) * Time.deltaTime;
        float fallSpeedFactor = 1;

        currentJuggleFactor = JuggleFactor;
        if (currentJuggleFactor > 0)
            gravity *= -currentJuggleFactor; 


        controller.Move(gravity * Time.deltaTime * fallSpeedFactor);
        IsGrounded = controller.isGrounded;

        if (gravity.y < 0 && IsGrounded)
        {
            gravity = Vector3.zero;
        }
    }

    void HandleJuggle()
    {
        JuggleFactor = Mathf.MoveTowards(JuggleFactor, 0, juggleSpeed * Time.deltaTime);

        if (enemigo)
        {
            if (JuggleFactor > 0)
            {
                enemigo.enemyState = Common.EnemyState.Juggle;
            }
        
        }
        
    }


    public void AddJuggle(float juggleValue)
    {
        JuggleFactor += juggleValue;

        JuggleFactor = Mathf.Clamp(JuggleFactor, 0, maxJuggleFactor);
    }
}
