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
            playerState = Common.PlayerState.Dead;
        else
            playerState = Common.PlayerState.Playing;
        
        switch(playerState)
        {
            case Common.PlayerState.Playing:
                break;
            case Common.PlayerState.Dead:
                break;
            case Common.PlayerState.WaitingForRespawn:
                break;
        }
	}
}
