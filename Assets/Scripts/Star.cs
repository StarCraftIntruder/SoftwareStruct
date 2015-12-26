using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Star : MonoBehaviour
{
    List<Vector2> points;
    float screenLeft, screenRight, screenTop, screenBottom;
    void flip()
    {
        for (int i = 0; i < points.Count; ++i)
        {
            points[i] = new Vector2(-points[i].x, points[i].y);
        }
    }
    void reverse()
    {
        int total = points.Count - 1;
        step = total - step;
        for (int i = 0; i < points.Count>>1; ++i)
        {
            Vector2 tempP = points[i];
            points[i] = points[total - i];
            points[total - i] = tempP;
        }
        for (int i = 0; i < points.Count; ++i)
        {
            points[i] = -points[i];
        }
    }
    void setPoints(List<Vector2> points)
    {
        this.points = points;
    }
    void addPoint(Vector2 point)
    {
        points.Add(point);
    }
    int step;
    void Start()
    {
        Vector2 b_l = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)),
            t_r = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));
        screenLeft = b_l.x;
        screenBottom = b_l.y - 1f;
        screenRight = t_r.x;
        screenTop = t_r.y + 1f;
        timeFlipStart = DateTime.Now;
    }
    void ereaseStar()
    {
        StartCoroutine(destoryAfter02Sce());
        points.Clear();
        step = 0;
        transform.parent.SendMessage("ereaseStar", transform);
    }
    IEnumerator destoryAfter02Sce()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
    void Update()
    {
        if (points.Count > 0)
        {
            step = (step + 1) % points.Count;
            transform.Translate(points[step]);
        }
    }
    DateTime timeFlipStart;
    void LateUpdate()
    {
        //碰到两边反弹
        if (transform.position.x >= screenRight || transform.position.x <= screenLeft)
        {
            double timeFlip = (DateTime.Now - timeFlipStart).TotalMilliseconds;
            timeFlipStart = DateTime.Now;
            if (timeFlip > 100)
                flip();
        }
        if (transform.position.y > screenTop || transform.position.y < screenBottom)
        {
            ereaseStar();
        }
    }
}
