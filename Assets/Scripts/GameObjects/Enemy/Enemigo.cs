using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Salud))]
public class Enemigo : MonoBehaviour
{

    public bool DebugEnemy;
    public Common.EnemyPriority enemyPriority = Common.EnemyPriority.Random;
    public Player targetPlayer;
   
    public Common.EnemyState enemyState;


    float distanciaTargetPlayer;
    Salud health;
    CharacterController characterController;
    Danger danger;

    void Awake()
    {
        health = GetComponent<Salud>();
        danger = GetComponent<Danger>();
        characterController = GetComponent<CharacterController>();

    }

    private void Start()
    {
        if (EventSystemManager.enemyEvents != null)
        {
            EventSystemManager.enemyEvents.EnemyKill += EnemyKilled;
        }
    }


	private void Update()
	{
        HandleEnemyHealth();
        HandlePlayer();
	}

    void HandleEnemyHealth()
    {

        if (health.ValorSalud <= 0)
        {
            enemyState = Common.EnemyState.Muerto;
        }

        switch(enemyState)
        {

            case Common.EnemyState.Muerto:
                if(EventSystemManager.enemyEvents) EventSystemManager.enemyEvents.OnEnemyKill();
                EnemyKilled();
                break;

            case Common.EnemyState.Juggle:
                if (characterController)
                {
                    if (characterController.isGrounded && health.ValorSalud > 0)
                    {
                        enemyState = Common.EnemyState.Activo;
                    }
                }
                break;

        }
    }

    void EnemyKilled()
    {
        if(danger)
        {
            danger.IsEnabled = false;
        }

        //TODO: Add dead animation
        Destroy(gameObject);
    }


    void HandlePlayer()
    {
        if (enemyState == Common.EnemyState.Activo)
        {
            if (targetPlayer)
            {
                if (DebugEnemy) Debug.DrawLine(transform.position, targetPlayer.transform.position, Color.red);
                distanciaTargetPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);
            }
        }

    }
}
