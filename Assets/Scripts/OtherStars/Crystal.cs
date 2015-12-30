using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour
{
    void reset() { }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MyStar"))
        {
            other.SendMessage("copyStar");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CopyMyStar"))
        {
            other.tag = "MyStar";
        }
    }
}
