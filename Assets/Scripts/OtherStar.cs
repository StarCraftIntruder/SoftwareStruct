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
            gameObject.SetActive(false);
            transform.parent.parent.gameObject.SendMessage("checkIsWin");
        }
    }

    void Update()
    {
        //自转啥的
    }
}
