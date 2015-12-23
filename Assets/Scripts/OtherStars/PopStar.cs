using UnityEngine;
using System.Collections;

public class PopStar : MonoBehaviour
{
    public Material normalMat,origMat;
    Renderer render;
    int durable,maxDur;
    void setData(UserData data) {
        maxDur = 2;
        reset();
    }
    void Start()
    {
        render = GetComponent<Renderer>();
    }
    void reset()
    {
        durable = maxDur;
        if (render != null)
        {
#if UNITY_EDITOR
            render.material = origMat;
#else  
        render.sharedMaterial = origMat;
#endif
        }
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
                gameObject.SetActive(false);
                transform.parent.SendMessage("checkIsWin");
            }
        }
    }
}
