using UnityEngine;
using System.Collections;

public class PopStar : MonoBehaviour
{
    GameObject ball;
    public Material normalMat,origMat;
    Renderer render;
    int durable,maxDur;
    void setData(UserData data) {
        maxDur = 2;
        reset();
    }
    void Start()
    {
        ball = transform.FindChild("Ball").gameObject;
        render = ball.GetComponent<Renderer>();
    }
    void reset()
    {
        durable = maxDur;
        if (render != null)
        {
            ball.SetActive(true);
#if UNITY_EDITOR
            render.material = origMat;
#else  
        render.sharedMaterial = origMat;
#endif
        }
        tag = "alive";
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MyStar")
        {
            if (durable-- == 2)
            {
#if UNITY_EDITOR
                render.material = normalMat;
#else  
                render.sharedMaterial = normalMat;
#endif
                other.SendMessage("reverse");

            }
            else
            {
                ball.SetActive(false);
                tag = "dead";
                gameObject.SendMessage("explode");
                transform.parent.SendMessage("checkIsWin");
            }
        }
    }
}
