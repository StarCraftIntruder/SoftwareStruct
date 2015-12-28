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
        //reset
        step = 0;
        alive = true;
        /*data格式：
         * type int
         * time int
         * type=0: radius float
         * type=1: a float,b float
         */
        MOON_TYPE type = (MOON_TYPE)data.readInt();
        int time = data.readInt();//转1/4圈需要的帧数
        path.Clear();
        float x = 0, y = 0;
        switch (type)
        {
            case MOON_TYPE.Circle:
                float radius = data.readFloat();
                float r_2 = radius * radius;
                for (int i = 0; i < time; ++i)
                {
                    x = radius * Mathf.Sin(i * 90.0f / time * Mathf.Deg2Rad);
                    y = Mathf.Sqrt(r_2 - x * x);
                    path.Add(new Vector2(x, y));
                }
                for (int i = time; i > 0; --i)
                {
                    x = radius * Mathf.Sin(i * 90.0f / time * Mathf.Deg2Rad);
                    y = -Mathf.Sqrt(r_2 - x * x);
                    path.Add(new Vector2(x, y));
                }
                for (int i = 0; i < 60; ++i)
                    path.Add(-path[i]);
                break;
            case MOON_TYPE.Ellipse:
                float a = data.readFloat(), b = data.readFloat();
                float a_2 = a * a, b_2 = b * b;
                for (int i = 0; i <= time; ++i)
                {
                    x = a * Mathf.Sin(i * 90.0f / time * Mathf.Deg2Rad);
                    float temp = (1 - x * x / a_2) * b_2;
                    y = temp < 0 ? 0 : Mathf.Sqrt(temp);
                    path.Add(new Vector2(x, y));
                }
                for (int i = path.Count - 1; i > 0; --i)
                {
                    path.Add(new Vector2(path[i].x, -path[i].y));
                }
                for (int i = 0; i < time * 2; ++i)
                    path.Add(-path[i]);
                break;
        }
        //计算轨迹
        data.reset();
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
