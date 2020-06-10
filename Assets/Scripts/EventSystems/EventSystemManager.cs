using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemManager : MonoBehaviour
{
    
    public static EnemyEvents enemyEvents;

	private void Awake()
	{
        
        enemyEvents = gameObject.AddComponent<EnemyEvents>();
	}

    public class EnemyEvents : MonoBehaviour
    {
        
		public event Action EnemyKill;
        public void OnEnemyKill()
        {
            if (EnemyKill != null)
            {
                EnemyKill();

                Debug.Log(gameObject.name + " says: Enemy was killed!");
            }
        }

        public event Action EnemySpawn;
        public void OnEnemySpawn()
        {
            if (EnemySpawn != null)
            {
                EnemySpawn();

                Debug.Log(gameObject.name + " says: Enemy was spawned!");
            }
        }
    }


}
