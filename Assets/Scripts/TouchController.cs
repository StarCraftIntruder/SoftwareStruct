using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchController : MonoBehaviour
{
    GameObject starMaker;
    bool isTouching;

    Vector3 oldPoint;
    void Start()
    {
        isTouching = false;
        starMaker = GameObject.Find("starMaker");
    }

    void OnMouseDown()
    {
        oldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        starMaker.SendMessage("ereaseStar");
        StartCoroutine(pauseOneSec(oldPoint));
        isTouching = true;
    }

    void OnMouseDrag()
    {
        if (isTouching)
        {
            Vector3 newPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
            starMaker.SendMessage("addPoint", (Vector2)(newPoint - oldPoint));
            oldPoint = newPoint;
        }
    }
    void OnMouseExit()
    {
        isTouching = false;
    }
    IEnumerator pauseOneSec(Vector2 pos)
    {
        yield return new WaitForSeconds(0.3f);
        starMaker.SendMessage("makeStar", (Vector2)pos);
    }
}
