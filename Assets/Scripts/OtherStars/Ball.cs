using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        transform.parent.SendMessage("OnTriggerEnter",other);
    }
    void OnTriggerExit(Collider other)
    {
        transform.parent.SendMessage("OnTriggerExit", other);
    }
}
