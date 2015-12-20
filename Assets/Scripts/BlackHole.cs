using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour {
    GameObject starMaker;
	// Use this for initialization
	void Start () {
        starMaker = GameObject.Find("starMaker");
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MyStar")
        {
            starMaker.SendMessage("ereaseStar",other.gameObject);
        }
    }
	// Update is called once per frame
	void Update () {
	    
	}
}
