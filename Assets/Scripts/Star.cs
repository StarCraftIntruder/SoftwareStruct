using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Star : MonoBehaviour
{
    public List<Vector2> points;
    float screenLeft, screenRight, screenTop, screenBottom;
    void flip()
    {
        for (int i = 0; i < points.Count; ++i)
            points[i] = new Vector2(-points[i].x, points[i].y);
    }
    void reverse()
    {
        int total = points.Count - 1;
        step = total - step;
        int halfCount=points.Count>>1;
        for (int i = 0; i < halfCount; ++i)
        {
            Vector2 tempP = -points[i];
            points[i] = -points[total - i];
            points[total - i] = tempP;
        }
        if ((halfCount) << 1 != points.Count)
            points[halfCount] = -points[halfCount];
    }
    void setPoints(List<Vector2> points)
    {
        this.points = points;
    }
    void addPoint(Vector2 point)
    {
        points.Add(point);
    }
    public int step;
    void copyPath(Star copy)
    {
        points = new List<Vector2>(copy.points.Count);
        foreach (Vector2 p in copy.points)
            points.Add(p);
        this.step = copy.step;
        rotate(false);
    }
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
            transform.Translate(points[step], Space.Self);
        }
    }
    DateTime timeFlipStart;
    void LateUpdate()
    {
        //碰到两边反弹
        if (transform.localPosition.x >= screenRight || transform.localPosition.x <= screenLeft)
        {
            double timeFlip = (DateTime.Now - timeFlipStart).TotalMilliseconds;
            timeFlipStart = DateTime.Now;
            if (timeFlip > 100)
                flip();
        }
        if (transform.localPosition.y > screenTop || transform.localPosition.y < screenBottom)
        {
            ereaseStar();
        }
    }
    void popStar(Vector3 n)//参数为法线
    {
        //Vector2 v1 = (Vector2)(r - transform.position);
        //v1 = rotate(v1, Mathf.Deg2Rad * 90f);
        //Vector2 v2 = points[step];
        //float cos = Mathf.Cos( Vector2.Angle(v1,v2));
        //float cos_2 = 2 * cos * cos - 1;
        //float angel = Mathf.Acos(cos_2);
        //print(angel*Mathf.Rad2Deg);
        //reverse();
        //for (int i = 0; i < points.Count; ++i)
        //{
        //    points[i] = rotate(points[i], angel);
        //}

        //reverse();
       // Vector2 v1 = (Vector2)(transform.position - r);//关于v1对称  2v1-v
        //Vector2 v2 = points[step];
        ////v1 = v1.normalized * (v2.magnitude * (v1.x * v2.x + v1.y * v2.y) / v1.magnitude / v2.magnitude);
        //print((v1.x * v2.x + v1.y * v2.y) / v1.magnitude/v2.magnitude);
        //Vector2.Reflect
        //v1 = v1.normalized * (v1.x * v2.x + v1.y * v2.y) / v1.magnitude;
        //以上内容推了好久~舍不得删~~

        for (int i = 0; i < points.Count; ++i)
            points[i] = Vector2.Reflect(points[i], n); 
            //points[i] =   Vector2.Reflect(points[i],v1.normalized); //v1 - points[i] + v1;
    }

    void copyStar()
    {
        GameObject starObj = GameObject.Instantiate<GameObject>(gameObject);
        starObj.tag = "CopyMyStar";
        starObj.transform.localPosition = transform.localPosition;
        starObj.SendMessage("copyPath", this);
        transform.parent.SendMessage("copyStar", starObj.transform);

        rotate(true);
    }

    Vector2 rotate(Vector2 v, float angle)
    {
        //详见三角函数诱导公式
        float cos = Mathf.Cos(angle), sin = Mathf.Sin(angle);

        float x, y;
        x = v.x;
        y = v.y;
        return new Vector2(x * cos - y * sin, y * cos + x * sin);
    }

    void rotate(bool top)
    {
        float ra = Mathf.Deg2Rad * (top ? 15 : -15);
        for (int i = 0; i < points.Count; ++i)
        {
            points[i] = rotate(points[i], ra);
        }
    }
}
