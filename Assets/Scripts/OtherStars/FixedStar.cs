using UnityEngine;
using System.Collections;
public enum MOON_TYPE//卫星轨迹类型
{
    Circle,
    Ellipse
}

public class FixedStar : MonoBehaviour
{
    Transform ball, moon;//主星球和卫星

    void Awake()
    {
        ball = transform.FindChild("Ball");
        moon = transform.FindChild("Moon");
    }
    void setData(UserData data)
    {
        if (moon != null)
            moon.SendMessage("setPath", data);
        reset();
    }
    void reset()
    {
        if (ball != null)
            ball.gameObject.SetActive(true);
        if (moon != null)
            moon.SendMessage("setActive", true);
        tag = "alive";
    }
    void OnTriggerEnter(Collider other)
    {
        if (CompareTag("alive") && other.tag == "MyStar")
        {
            ball.gameObject.SetActive(false);
            moon.SendMessage("setActive", false);
            tag = "dead";
            gameObject.SendMessage("explode");
            transform.parent.SendMessage("checkIsWin");
        }
    }
}
