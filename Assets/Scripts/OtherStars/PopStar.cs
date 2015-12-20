using UnityEngine;
using System.Collections;

public class PopStar : MonoBehaviour
{
    public Material normalMat,origMat;
    Renderer render;
    void Start()
    {
        render = GetComponent<Renderer>();
    }
    void reset()
    {
        durable = 2;
        if (render != null)
        {
#if UNITY_EDITOR
            render.material = origMat;
#else  
        render.sharedMaterial = origMat;
#endif
        }
    }
    int durable;
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
                other.gameObject.SendMessage("flip", true);

            }
            else
            {
                gameObject.SetActive(false);
                transform.parent.parent.gameObject.SendMessage("checkIsWin");
            }
        }
    }
}
