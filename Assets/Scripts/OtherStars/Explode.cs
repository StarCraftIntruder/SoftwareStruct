using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour {
    public GameObject expPrefab;
    void explode()
    {
        GameObject ob = Object.Instantiate(expPrefab) as GameObject;
        ob.transform.parent = transform;
        ob.transform.localPosition = Vector3.zero;
    }
}
