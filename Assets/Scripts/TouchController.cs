using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour
{
    Transform starMaker;
    bool isTouching;

    Vector3 oldPoint;
    void Start()
    {
        isTouching = false;
        starMaker = GameObject.Find("starMaker").GetComponent<Transform>();
    }
    void OnMouseDown()
    {
        oldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        firstPoint = oldPoint;
        starMaker.SendMessage("ereaseStars",true);
        StartCoroutine(pauseOneSec(oldPoint));
        isTouching = true;
        maxDis = 0;
    }
    void OnMouseDrag()
    {
        if (isTouching)
        {
            Vector3 newPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
            starMaker.SendMessage("addPoint", (Vector2)(newPoint - oldPoint));
            oldPoint = newPoint;

            float dis = Vector2.Distance(oldPoint, firstPoint);
            if (dis > maxDis)
                maxDis = dis;
        }
    }
    void OnMouseExit()
    {
        if (isTouching)
        {
            if (maxDis < 1f)//如果滑动距离太小，则移除
                starMaker.SendMessage("ereaseStars",true);
        }
        isTouching = false;
    }       
    Vector3 firstPoint;
    float maxDis;   
    IEnumerator pauseOneSec(Vector2 pos)
    {
        yield return new WaitForSeconds(0.3f);
        starMaker.SendMessage("makeStar", (Vector2)pos);
    }
}
