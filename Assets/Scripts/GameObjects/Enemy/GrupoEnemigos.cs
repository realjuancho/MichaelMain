using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrupoEnemigos : MonoBehaviour
{

    Enemigo[] enemigos;
    Player targetPlayer;
    void Awake()
	{
        enemigos = gameObject.GetComponentsInChildren<Enemigo>();
	}


	void Update()
	{
        CheckEnemigos();
	}


    void CheckEnemigos()
    {
        
    }
}
