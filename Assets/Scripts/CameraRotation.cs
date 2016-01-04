using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {
    public Vector3 vec;
	void Update () {
        transform.Rotate(vec, Space.Self);
	}
}
