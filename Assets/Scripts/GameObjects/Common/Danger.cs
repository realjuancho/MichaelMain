using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Danger : MonoBehaviour
{

    public bool IsEnabled = true;
    public int DamageValue = 100;

    Collider dangerCollider;


	private void Awake()
	{
        dangerCollider = GetComponent<Collider>();
	}


}
