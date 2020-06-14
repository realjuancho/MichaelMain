using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Salud))]
[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Common.PlayerState playerState;
    public Common.PlayerId playerId;
    public bool debugPlayer;

    public Salud health;

	void Awake()
	{
        health = GetComponent<Salud>();
	}

	private void Update()
	{
        if (health.ValorSalud <= 0)
            playerState = Common.PlayerState.Muerto;
        else
            playerState = Common.PlayerState.Jugando;
        
        switch(playerState)
        {
            case Common.PlayerState.Jugando:
                break;
            case Common.PlayerState.Muerto:
                break;
            case Common.PlayerState.EsperandoARevivir:
                break;
        }
	}
}
