using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlythroughCamera : MonoBehaviour
{
    Vector3[] empties;
    readonly float distancePerDraw = 0.05f;
    List<Vector3> cameraPositions;
    static int currentPosition;
    int maxPositions;
    bool startAnimation;

    // Start is called before the first frame update
    void Start()
    {
        empties = new Vector3[] { GameObject.Find("CameraPath0").transform.position, GameObject.Find("CameraPath1").transform.position
                    , GameObject.Find("CameraPath2").transform.position, GameObject.Find("CameraPath3").transform.position
                    , GameObject.Find("CameraPath4").transform.position, GameObject.Find("CameraPath5").transform.position};

        cameraPositions = new List<Vector3>();
        currentPosition = 0;
        startAnimation = false;
        maxPositions = 0;

        // start flythrough
        for (int i = 0; i < empties.Length - 3; i++)
        {
            float distance = Distance(empties[i + 1], empties[i + 2]);
            int numDraws = Mathf.FloorToInt(distance / distancePerDraw);
            maxPositions += numDraws;
            float resolution = 1.0f / numDraws;

            for (int j = 0; j < numDraws; j++)
            {
                //Find the coordinate between the end points with a Catmull-Rom spline
                Vector3 newPos = GetCatmullRomPosition(resolution * j, empties[i], empties[i + 1], empties[i + 2], empties[i + 3]);
                cameraPositions.Add(newPos);
            }
        }

        // calculate spline locations

    }

    // Update is called once per frame
    void Update()
    {
        //Component[] components = GetComponents(typeof(Camera));

        if (Input.GetKeyDown("s"))
        {
            startAnimation = true;
        }

        if (startAnimation)
        {
            if (currentPosition < maxPositions)
            {
                transform.position = cameraPositions[currentPosition];
                currentPosition++;
                Debug.Log(string.Format("Camera postion: {0}, {1}, {2}", transform.position.x, transform.position.y, transform.position.z));
            }
        }
    }

    float Distance(Vector3 point1, Vector3 point2)
    {
        return (point2 - point1).magnitude;
    }

    Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
        Vector3 a = 2f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

        //The cubic polynomial: a + b * t + c * t^2 + d * t^3
        Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

        return pos;
    }
}
