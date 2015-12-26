using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
    float maxSpeed, maxTime;
    float time, scale, speed;
    void setData(UserData data)
    {
        maxSpeed = 0.25f;
        maxTime = 4f;
        reset();
    }
    void reset()
    {
        time = maxTime;
        scale = 1.04f;
        speed = maxSpeed;
        StopAllCoroutines();
        StartCoroutine(scaling());
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MyStar")
        {
            gameObject.SetActive(false);
            transform.parent.SendMessage("checkIsWin");
        }
    }
    void FixedUpdate()
    {
        time -= Time.fixedDeltaTime;
        scale += 0.0008f;
    }
    IEnumerator scaling()
    {
        while (time > 0)
        {//还存活着
            if (transform.localScale.Equals(Vector3.one))
                transform.localScale = new Vector3(scale, scale, scale);
            else
                transform.localScale = Vector3.one;

            speed -= 0.007f;
            yield return new WaitForSeconds(speed);
        }

        print("爆炸");
        //gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        transform.parent.SendMessage("resetStars");
    }
}
