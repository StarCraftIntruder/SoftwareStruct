using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MoveWithPoints : MonoBehaviour
{
    List<Vector2> points = new List<Vector2>();
    public GameObject starPrefab;
    Transform star = null;

    float screenLeft, screenRight, screenTop, screenBottom;

    public GameObject otherStarMaker;
    void addPoint(Vector2 point)
    {
        points.Add(point);
    }
    void ereaseStar()
    {
        if (star != null)
            StartCoroutine(destoryAfter02Sce(star.gameObject));
        star = null;
        points.Clear();
        step = 0;
        //reset
        otherStarMaker.SendMessage("initStars");
    }
    IEnumerator destoryAfter02Sce(GameObject obj)
    {
        yield return new WaitForSeconds(0.2f);
        if (obj != null)
            Destroy(obj);
    }
    void makeStar(Vector2 pos)
    {
        GameObject starObj = GameObject.Instantiate<GameObject>(starPrefab);
        star = starObj.transform;
        star.parent = transform;
        star.position = pos;
    }
    void flip()
    {
        for (int i = 0; i < points.Count - 1; ++i)
        {
            points[i] = new Vector2(-points[i].x, points[i].y);
        }
    }

    int step;
    void Start()
    {
        Vector2 b_l = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)),
            t_r = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));
        screenLeft = b_l.x;
        screenBottom = b_l.y;
        screenRight = t_r.x;
        screenTop = t_r.y;
        timeFlipStart = DateTime.Now;
    }
    void Update()
    {
        if (star != null)
        {
            if (++step >= points.Count)
            {
                step = 0;
            }
            star.Translate(points[step]);
        }
    }
    DateTime timeFlipStart;
    void LateUpdate()
    {
        if (star != null)
        {
            //碰到两边反弹
            if (star.position.x >= screenRight || star.position.x <= screenLeft)
            {
                double timeFlip = (DateTime.Now - timeFlipStart).TotalMilliseconds;
                timeFlipStart = DateTime.Now;
                if (timeFlip > 100)
                    flip();
            }
            if (star.position.y >= screenTop || star.position.y <= screenBottom)
            {
                ereaseStar();
            }
        }
    }
}
