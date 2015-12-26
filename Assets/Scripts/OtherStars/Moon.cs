using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Moon : MonoBehaviour
{
    List<Vector3> path = new List<Vector3>();
    int step;
    bool alive;
    void setPath(UserData data)
    {
        step = 0;
        alive = true;
        //data中含有type和（radius）或（a b）等
        MOON_TYPE type = MOON_TYPE.Circle;
        path.Clear();
        switch (type)
        {
            case MOON_TYPE.Circle:
                float radius = 1.5f;
                float r_2 = radius * radius;
                float x = 0, y = 0;
                for (int i = 0; i < 30; ++i)
                {
                    x = Mathf.Lerp(0, 1.5f, i / 30f);
                    y = Mathf.Sqrt(r_2 - x * x);
                    path.Add(new Vector2(x, y));
                }
                for (int i = 30; i >0; --i)
                {
                    x = Mathf.Lerp(0, 1.5f, i / 30f);
                    y = -Mathf.Sqrt(r_2 - x * x);
                    path.Add(new Vector2(x, y));
                }
                for (int i = 0; i < 60; ++i)
                    path.Add(-path[i]);
                break;
            case MOON_TYPE.Ellipse:
                break;
        }
        //计算轨迹
    }
    Transform ball;
    void Awake()
    {
        ball = transform.FindChild("Ball");
    }
    void Update()
    {
        if (alive && path.Count > 0)
        {
            step = (step + 1) % path.Count;
            transform.localPosition = path[step];
        }
    }
    void setActive(bool active)
    {
        ball.gameObject.SetActive(active);
        if (!active)
            gameObject.SendMessage("explode");
        alive = active;
    }
    void OnTriggerEnter(Collider other)
    {
        if (alive && other.tag == "MyStar")
        {
            other.SendMessage("ereaseStar");
        }
    }
}
