using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AlertArea : MonoBehaviour
{
    public bool DebugAlertArea;
    public Player priorityPlayer;
    [Range(0f, 10f)]
    public float effectAreaSize = 10f;


    Collider alertAreaTrigger;
    Transform target;
    Common.EnemyPriority enemyPriority = Common.EnemyPriority.Random;
    List<Player> players;
    Enemigo parentEnemy;



    // Start is called before the first frame update
    void Awake()
    {
        alertAreaTrigger = GetComponent<Collider>();
        parentEnemy = gameObject.transform.parent.gameObject.GetComponent<Enemigo>();
        players = new List<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEffectArea();
        CheckPlayerPriority();
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckTriggerForPlayer(other, PlayerOperation.addPlayer); 
    }

    private void OnTriggerExit(Collider other)
    {
        CheckTriggerForPlayer(other, PlayerOperation.removePlayer);  
    }

    enum PlayerOperation { addPlayer, removePlayer}
    void CheckTriggerForPlayer(Collider other, PlayerOperation operation)
    {
        Player player = other.GetComponent<Player>();
        bool contains = players.Contains(player);
        if (player)
        {
            switch (operation)
            {
                case PlayerOperation.addPlayer:
                    if (!contains) players.Add(player);
                    break;
                case PlayerOperation.removePlayer:
                    if (contains) players.Remove(player);
                    break;
            }
        }
    }

    void CheckPlayerPriority()
    {

        enemyPriority = parentEnemy.enemyPriority;

        if (players.Count > 1)
        {
            switch (enemyPriority)
            {
                case Common.EnemyPriority.JugadorMasCercano:
                    priorityPlayer = ClosestPlayer(players.ToArray(), transform.position);
                    break;
                case Common.EnemyPriority.JugadorMasLejano:
                    priorityPlayer = FurthestPlayer(players.ToArray(), transform.position);
                    break;
                case Common.EnemyPriority.JugadorMasSano:
                    priorityPlayer = HealthiestPlayer(players.ToArray());
                    break;
                case Common.EnemyPriority.Random:
                    //Solo devuelve un player si no está asignado antes
                    if (!priorityPlayer)
                    {
                        int i = Random.Range(0, players.Count);
                        priorityPlayer = players[i];
                    }
                    break;

            }
        }
        else if (players.Count == 1)
        {
            //Solo devuelve el único jugador en la lista
            priorityPlayer = players[0];
        }
        else
            priorityPlayer = null;

        parentEnemy.targetPlayer = priorityPlayer;

    }


    void CheckEffectArea()
    {
        Vector3 newScale = new Vector3(1, 1, 1);
        newScale *= effectAreaSize * 10f;
        transform.localScale = newScale;
    }

    void OnDrawGizmos()
    {
        if (DebugAlertArea) CheckEffectArea();
    }


    Player ClosestPlayer(Player[] players, Vector3 posicionReferencia)
    {
        Player player = null;
        float distanciaMenor = float.MaxValue;
        foreach (Player p in players)
        {
            if (p.playerState == Common.PlayerState.Playing)
            {
                float distanciaPlayer = Vector3.Distance(p.transform.position, posicionReferencia);
                if (distanciaPlayer < distanciaMenor)
                {
                    distanciaMenor = distanciaPlayer;
                    player = p;
                }
            }
        }

        return player;
    }

    Player FurthestPlayer(Player[] players, Vector3 posicionReferencia)
    {
        Player player = null;
        float distanciaMayor = 0;
        foreach (Player p in players)
        {
            if (p.playerState == Common.PlayerState.Playing)
            {
                float distanciaPlayer = Vector3.Distance(p.transform.position, posicionReferencia);
                if (distanciaPlayer > distanciaMayor)
                {
                    distanciaMayor = distanciaPlayer;
                    player = p;
                }
            }
        }
        return player;
    }

    Player HealthiestPlayer(Player[] players)
    {
        Player player = players[0];
        float saludMayor = 0;
        foreach (Player p in players)
        {
            if (p.playerState == Common.PlayerState.Playing)
            {
                float saludPlayer = player.gameObject.GetComponent<Salud>().ValorSalud;
                if (saludPlayer > saludMayor)
                {
                    saludMayor = saludPlayer;
                    player = p;
                }
            }
        }
        return player;
    }

    Player WeakestPlayer(Player[] players)
    {
        Player player = players[0];
        float saludMenor = int.MaxValue;
        foreach (Player p in players)
        {
            if (p.playerState == Common.PlayerState.Playing)
            {
                float saludPlayer = player.gameObject.GetComponent<Salud>().ValorSalud;
                if (saludPlayer < saludMenor)
                {
                    saludMenor = saludPlayer;
                    player = p;
                }
            }
        }
        return player;
    }

   
}
