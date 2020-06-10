using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public Transform origin;
    public Transform[] controlPoints;
    public float factor = 0.5f;
    public int SEGMENT_COUNT = 50;

    public bool debugPoints;
    public Color debugColor = Color.blue;

    int curveCount = 0;


    Vector3[] GetBezierPoints()
    {
        List<Vector3> BezierPoints = new List<Vector3>();
        curveCount = (int)controlPoints.Length / 3;
        for (int j = 0; j < curveCount; j++)
        {
            for (int i = 0; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                int nodeIndex = j * 3;
                Vector3 pixel = CalculateCubicBezierPoint(t, controlPoints[nodeIndex].position 
                                                          , controlPoints[nodeIndex + 1].position 
                                                          , controlPoints[nodeIndex + 2].position 
                                                          , controlPoints[nodeIndex + 3].position 
                                                         );

                BezierPoints.Add(pixel);

            }
        }

        return BezierPoints.ToArray();
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

	private void Update()
	{
        points = GetBezierPoints();
	}

	public Vector3[] points;
    void OnDrawGizmos()
    {

        if (debugPoints)
        {
            
            Gizmos.color = debugColor;

            points = GetBezierPoints();
            if (!origin) origin = transform;
            foreach (Vector3 pixel in points)
            {
                Gizmos.DrawLine(origin.position, pixel);
            }
        }
        foreach (Transform point in controlPoints)
        {
            point.gameObject.GetComponent<MeshRenderer>().enabled = debugPoints;
        }
    }



}
