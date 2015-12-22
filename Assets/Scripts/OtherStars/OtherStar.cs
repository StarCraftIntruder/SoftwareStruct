using UnityEngine;
using System.Collections;

public class OtherStar : MonoBehaviour
{
    void reset() {}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MyStar")
        {
            gameObject.SetActive(false);
            transform.parent.SendMessage("checkIsWin");
        }
    }
}
