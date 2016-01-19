using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class TouchController : MonoBehaviour
{
    Transform starMaker;
    bool isTouching;

    Vector3 oldPoint;
    void Start()
    {
        isTouching = false;
        starMaker = GameObject.Find("starMaker").GetComponent<Transform>();
        timeTouchStart = DateTime.Now;
    }
    DateTime timeTouchStart;
#if UNITY_EDITOR
    void OnMouseDown()
    {
        if (!isTouching && (DateTime.Now - timeTouchStart).TotalMilliseconds > 350)
        {
            isTouching = true;
            timeTouchStart = DateTime.Now;
            oldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
            firstPoint = oldPoint;
            starMaker.SendMessage("ereaseStars", true);
            StartCoroutine(pauseOneSec(oldPoint));
            maxDis = 0;
        }
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
    void OnMouseUp()
    {
        if (isTouching)
        {
            if (maxDis < 1f)//如果滑动距离太小，则移除
                starMaker.SendMessage("ereaseStars", true);
        }
        isTouching = false;
    }
    void OnMouseExit()
    {
        if (isTouching)
        {
            if (maxDis < 1f)//如果滑动距离太小，则移除
                starMaker.SendMessage("ereaseStars", true);
        }
        isTouching = false;
    }
#else 
    int touchId;
    void Update()
    {
        if (Input.touchCount ==1)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (!isTouching && (DateTime.Now - timeTouchStart).TotalMilliseconds > 350){
                        if(Physics.RaycastAll(Camera.main.ScreenPointToRay(touch.position), 50f).Length > 1)
                            break;
                        isTouching = true;
                        timeTouchStart = DateTime.Now;
                        touchId = touch.fingerId;
                        oldPoint = Camera.main.ScreenToWorldPoint((Vector3)touch.position + Vector3.forward * 10);
                        firstPoint = oldPoint;
                        starMaker.SendMessage("ereaseStars", true);
                        StartCoroutine(pauseOneSec(oldPoint));
                        maxDis = 0;
                 }
                        break;
                case TouchPhase.Moved:
                    if (isTouching && touch.fingerId == touchId)
                    {
                        Vector3 newPoint = Camera.main.ScreenToWorldPoint((Vector3)touch.position + Vector3.forward * 10);
                        starMaker.SendMessage("addPoint", (Vector2)(newPoint - oldPoint));
                        oldPoint = newPoint;

                        float dis = Vector2.Distance(oldPoint, firstPoint);
                        if (dis > maxDis)
                            maxDis = dis;
                        RaycastHit[] hits=Physics.RaycastAll(Camera.main.ScreenPointToRay(touch.position), 50f);
                        foreach(RaycastHit hit in hits){
                            if (hit.transform.CompareTag("alive"))
                            {
                                if (maxDis < 1f)//如果滑动距离太小，则移除
                                    starMaker.SendMessage("ereaseStars", true);
                                isTouching = false;
                            }
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    if (isTouching && touch.fingerId == touchId)
                    {
                        if (maxDis < 1f)//如果滑动距离太小，则移除
                            starMaker.SendMessage("ereaseStars", true);
                    }
                    isTouching = false;
                    break;
            }
        }
    }
#endif
    Vector3 firstPoint;
    float maxDis;
    IEnumerator pauseOneSec(Vector2 pos)
    {
        yield return new WaitForSeconds(0.1f);
        starMaker.SendMessage("makeStar", (Vector2)pos);
    }
}
