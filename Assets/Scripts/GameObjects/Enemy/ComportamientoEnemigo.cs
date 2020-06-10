using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoEnemigo : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float moveSpeed = 0.5f;
    [Range(0.1f, 1f)]
    public float rotateSpeed = 0.5f;
    public float distanceToBodySizeRatio = 1.5f;

    float minDistanceFromPlayer;
    float distanceFromPlayer;

    public Common.EnemyType[] enemyTypes;
    Enemigo parentEnemy;

    void Awake()
    {
        parentEnemy = gameObject.GetComponent<Enemigo>();
        Bounds bounds = transform.GetComponent<MeshRenderer>().bounds;
        minDistanceFromPlayer = bounds.size.z * distanceToBodySizeRatio;
    }

    // Update is called once per frame
    void Update()
    {
        CheckComportamiento();   
    }


    void CheckComportamiento()
    {
        if(parentEnemy.targetPlayer)
        {
            foreach (Common.EnemyType e in enemyTypes)
            {
                if (e == Common.EnemyType.Chaser)
                    ChasePlayer();
            }
        }
    }

    void ChasePlayer()
    {

        if (parentEnemy.enemyState == Common.EnemyState.Active)
        {
            //Move towards Player
            Vector3 targetPosition = parentEnemy.targetPlayer.transform.position;
            distanceFromPlayer = Vector3.Distance(transform.position, targetPosition);

            if (distanceFromPlayer > minDistanceFromPlayer)
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);


            //Rota hacia donde está el jugador
            Vector3 rotateDirection = parentEnemy.targetPlayer.transform.position - transform.position;
            float singleStep = rotateSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward,
                                                         rotateDirection
                                                         , singleStep, 0.0f);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
