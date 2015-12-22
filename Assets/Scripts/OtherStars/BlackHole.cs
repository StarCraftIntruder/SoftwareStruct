using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour
{
    void reset() { }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MyStar")
        {
            other.SendMessage("ereaseStar");
        }
    }
}
