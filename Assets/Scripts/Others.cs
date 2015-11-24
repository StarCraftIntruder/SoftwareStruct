using UnityEngine;
using System.Collections;

public class Others : MonoBehaviour {

	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
	}
}
