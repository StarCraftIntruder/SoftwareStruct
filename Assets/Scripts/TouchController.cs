using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchController : MonoBehaviour
{

    public Camera mainCamera;
    public Transform starMaker;
    bool isTouching;

    Vector3 oldPoint;
    void Start()
    {
        isTouching = false;
    }

    void OnMouseDown()
    {
        oldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        starMaker.gameObject.SendMessage("ereaseStar");
        StartCoroutine(pauseOneSec(oldPoint));
    }

    void OnMouseDrag()
    {
        Vector3 newPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        starMaker.gameObject.SendMessage("addPoint", (Vector2)(newPoint - oldPoint));
        oldPoint = newPoint;
    }
    IEnumerator pauseOneSec(Vector2 pos)
    {
        yield return new WaitForSeconds(0.3f);
        starMaker.gameObject.SendMessage("makeStar", (Vector2)pos);
    }

}
