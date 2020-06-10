using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FollowPlayerCamera : MonoBehaviour
{
    public bool DebugCamera;
    public bool CameraFollows;
    public float smoothAlignRate = 0.025f;
    public float smoothFollowRate = 1f;


    Vector3 cameraTarget;
    public Vector3 offset;

    Player[] players;
    float maxSmoothAlignRate;


    // Use this for initialization
    void Awake()
    {
        maxSmoothAlignRate = smoothAlignRate;
      
        UpdatePlayers();
    }

    public void UpdatePlayers()
    {
        players = FindObjectsOfType<Player>();
    }

    // Update is called once per frame

    void LateUpdate()
    {
        CheckTarget();

        AdjustDistanceToTarget();
        RotateCameraTowardsTarget();
    }

    void CheckTarget()
    {
        List<Vector3> targets = new List<Vector3>();
        foreach (Player player in players)
        {
            if (player.playerState == Common.PlayerState.Playing)
            {
                targets.Add(player.transform.position);
            }
        }

        if (targets.Count == 0)
            CameraFollows = false;
        else CameraFollows = true;

        if (CameraFollows)
        {
            cameraTarget = GetCentroid(targets.ToArray());
        }
    }


    void AdjustDistanceToTarget()
    {
        Vector3 targetPosition = cameraTarget + offset;
                               
        transform.position = Vector3.MoveTowards( transform.position
                                                 , targetPosition
                                                 , smoothFollowRate);
    }

    void RotateCameraTowardsTarget()
    {
        //
        if (DebugCamera)
        {
            Debug.DrawLine(transform.position, cameraTarget, Color.black);
            Debug.DrawRay(transform.position, transform.forward * Mathf.Infinity, Color.magenta);
        }

        Vector3 rotateDirection = cameraTarget - transform.position;
        float singleStep = smoothAlignRate * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward,
                                                     rotateDirection
                                                     , singleStep
                                                     , 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }



    Vector3 GetCentroid(Vector3[] positions)
    {
        Vector3 centroid = Vector3.zero;
        int count = positions.Length;

        foreach(Vector3 pos in positions)
        {
            centroid.x += pos.x / count;
            centroid.y += pos.y / count;
            centroid.z += pos.z / count;
        }
        return centroid;
    }

    void OnDrawGizmos()
    {
        // get the chosen game object
        if (DebugCamera)
        {
            players = FindObjectsOfType<Player>();
            CheckTarget();
            RotateCameraTowardsTarget();
        }
    }


}
