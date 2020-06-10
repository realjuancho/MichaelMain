using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salud : MonoBehaviour
{

    Collider saludCollider = new Collider();

    public float ValorSalud = 100;
    public bool debugHealth;

    float maxHealthValue;
    private void Awake()
    {
        saludCollider = GetComponent<Collider>();
        maxHealthValue = ValorSalud;
    }

    public void TakeDamage(float DamageValue)
    {
        ValorSalud -= DamageValue;
        ValorSalud = Mathf.Clamp(ValorSalud, 0, maxHealthValue);
    }

    public void RestoreHealth()
    {
        ValorSalud = maxHealthValue;
    }

    void RestoreHealth(int RestoreValue)
    {
        ValorSalud += RestoreValue;
        ValorSalud = Mathf.Clamp(ValorSalud, 0, maxHealthValue);
    }

    void OnControllerColliderHit  (ControllerColliderHit hit)
    {
        GameObject other = hit.collider.gameObject;
        Danger danger = other.GetComponent<Danger>();
        if (danger)
        {
            if (danger.IsEnabled)
            {
                if (debugHealth) Debug.Log("Danger:" + danger.DamageValue);
                TakeDamage(danger.DamageValue);
            }
        }

        Player player = other.GetComponent<Player>();
        if(player)
        {
            if(player.playerState == Common.PlayerState.Dead)
                player.GetComponent<Salud>().RestoreHealth();
        }
	}
	
}
