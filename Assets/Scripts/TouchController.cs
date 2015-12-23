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

        UIEventListener ev = GetComponent<UIEventListener>();
        ev.onPress = onPress;
        ev.onDrag = onDrag;
    }

    Vector3 firstPoint;
    float maxDis;
    void onPress(GameObject ob, bool state)
    {//按下state=true、抬起state=false
        if (state)
        {
            oldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
            firstPoint = oldPoint;
            starMaker.SendMessage("ereaseStars");
            StartCoroutine(pauseOneSec(oldPoint));
            isTouching = true;
            maxDis = 0;
        }
        else
        {
            if (isTouching)
            {
                if (maxDis < 1f)//如果滑动距离太小，则移除
                    starMaker.SendMessage("ereaseStars");
            }
            isTouching = false;
        }
    }

    void onDrag(GameObject ob, Vector2 point)
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
    IEnumerator pauseOneSec(Vector2 pos)
    {
        yield return new WaitForSeconds(0.3f);
        starMaker.SendMessage("makeStar", (Vector2)pos);
    }
}
