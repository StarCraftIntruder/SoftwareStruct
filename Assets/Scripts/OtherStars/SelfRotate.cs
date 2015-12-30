using UnityEngine;
using System.Collections;

public class SelfRotate : MonoBehaviour
{
    float speed;
    void setRSpeed(float speed)
    {
        this.speed = speed;
    }
    void Awake()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 30);
        speed = 1f;
    }
    void Update()
    {
        transform.Rotate(Vector3.up, speed, Space.Self);
    }
}
