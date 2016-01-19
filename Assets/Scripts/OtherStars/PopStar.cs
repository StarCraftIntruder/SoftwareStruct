using UnityEngine;
using System.Collections;

public class PopStar : MonoBehaviour
{
    GameObject ball;
    public Material normalMat,origMat;
    Renderer render;
    bool poped;//是否被撞过了
    void Start()
    {
        ball = transform.FindChild("Ball").gameObject;
        render = ball.GetComponent<Renderer>();
    }
    void reset()
    {
        poped = false;
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
        if (CompareTag("alive") && other.tag == "MyStar")
        {
            if (!poped)
            {
#if UNITY_EDITOR
                render.material = normalMat;
#else  
                render.sharedMaterial = normalMat;
#endif
                other.SendMessage("reverse");
                poped = true;
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
