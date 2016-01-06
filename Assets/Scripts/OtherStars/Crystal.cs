using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour
{
    void reset() { }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MyStar"))
        {
            //这一段目前并没有必要
            //Vector2 otherPos = other.transform.position,
            //    myPos = transform.position;
            //Star star = other.GetComponent<Star>();
            //Vector2 vec1 = myPos - otherPos,//圆心向量
            //    vec2 = star.points[star.step];// -star.points[(star.step + star.points.Count - 1) % star.points.Count];
            //vec2 = vec2 / vec2.magnitude;//入射角单位向量
            //float cos = (vec1.x * vec2.x + vec1.y * vec2.y) / vec1.magnitude;//求得余弦

            //other.transform.position = otherPos + vec2 * 0.5f * cos;

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
