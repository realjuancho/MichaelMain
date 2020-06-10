using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Seleccion : MonoBehaviour

{
    public string sceneName = "";
    public Jugadores jugadores;
    public enum Jugadores { UN_Jugador, DOS_Jugadores}


    Camera mainCamera;

	private void Awake()
	{
        mainCamera = Camera.main;
	}

	private void Update()
	{
        HandleTouch();
	}

    void HandleTouch()
    {
        if (Input.touchCount > 0)
        {

            Vector3 temp = Input.GetTouch(0).position;
           
            temp.z = Mathf.Abs(mainCamera.transform.position.z) * 100f; 
            Vector3 destination = mainCamera.ScreenToWorldPoint(temp);
            Vector3 direction = destination - mainCamera.transform.position;

            Debug.DrawRay(mainCamera.transform.position, direction, Color.blue);
            Ray ray = new Ray(mainCamera.transform.position, direction);

            RaycastHit rhit = new RaycastHit();
            Physics.Raycast(ray, out rhit);
            if (rhit.collider)
            {

                if(gameObject.name.Equals(rhit.collider.gameObject.name))
                    SceneManager.LoadScene(sceneName);
            } 
        }

    }


	
}
