using UnityEngine;
using System.Collections;

public enum PORTAL
{
    None, From, To
}
public class Portal : MonoBehaviour
{
    public float radius;

    public PORTAL state;
    public GameObject tailPrefab;
    void Start()
    {
        state = PORTAL.None;
    }
    void OnTriggerEnter(Collider other)
    {
        if (state == PORTAL.None && CompareTag("alive") && other.tag == "MyStar")
        {
            foreach (Transform t in other.GetComponentsInChildren<Transform>())
            {
                if (t != other.transform)
                    Destroy(t.gameObject);
            }
            transform.parent.SendMessage("setPos", new From(other.transform, this));
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (state == PORTAL.To && CompareTag("alive") && other.CompareTag("MyStar"))
        {
            other.tag = "MyStar";
            GameObject tail = GameObject.Instantiate(tailPrefab) as GameObject;
            Transform tailT = tail.transform;
            tailT.parent = other.transform;
            tailT.localScale = Vector3.one;
            tailT.localPosition = Vector3.zero;
        }
        state = PORTAL.None;
    }
}
