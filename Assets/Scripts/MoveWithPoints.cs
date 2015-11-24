using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveWithPoints : MonoBehaviour
{
    List<Vector2> points = new List<Vector2>();
    public GameObject starPrefab;
    Transform star = null;

    void changePoints(List<Vector3> newPoints)
    {
        if (star != null)
            Destroy(star.gameObject);
        star = null;
        points.Clear();
        step = 0;
        offPos = Vector3.zero;
        //reset

        foreach (Vector3 p in newPoints)
            points.Add(p);

        GameObject starObj = GameObject.Instantiate<GameObject>(starPrefab);
        star = starObj.transform;
        star.parent = transform;
        star.position = points[0];
    }

    int step;
    Vector2 offPos;
    void Update()
    {
        if (star != null)
        {
            if (++step >= points.Count)
            {
                step = 0;
                offPos = points[points.Count - 1] - points[0];
            }
            points[step] += offPos;
            //碰到两边反弹
            if(points[step].x>=Screen.width)

            star.position = points[step];
        }
    }
}
