using UnityEngine;
using System.Collections;

public class OtherStar : MonoBehaviour
{
    GameObject ball;
    void Start()
    {
        ball = transform.FindChild("Ball").gameObject;
    }
    public void reset()
    {
        if (ball != null)
            ball.SetActive(true);
        tag = "alive";
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MyStar")
        {
            ball.SetActive(false);
            tag = "dead";
            gameObject.SendMessage("explode");
            transform.parent.SendMessage("checkIsWin");
        }
    }
}
