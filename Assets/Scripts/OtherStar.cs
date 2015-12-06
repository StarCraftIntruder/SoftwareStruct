using UnityEngine;
using System.Collections;

public class OtherStar : MonoBehaviour
{

    void Start()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MyStar")
        {
            GameObject g = transform.parent.parent.gameObject;
            Destroy(gameObject);
            g.SendMessage("checkIsWin");
        }
    }

    void Update()
    {
        //自转啥的
    }
}
