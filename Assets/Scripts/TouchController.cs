using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchController : MonoBehaviour {

    List<Vector3> touchPoints=new List<Vector3>();
    public Camera mainCamera;
    public Transform starMaker;

    void OnMouseDown()
    {
        touchPoints.Clear();
        touchPoints.Add(mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10));
    }

    void OnMouseDrag()
    {
        touchPoints.Add(mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10));
    }
    IEnumerator pauseOneSec()
    {
        yield return new WaitForSeconds(1);
        starMaker.gameObject.SendMessage("changePoints", touchPoints);
    }

    void OnMouseUp()
    {
        starMaker.gameObject.SendMessage("changePoints", touchPoints);
    }
}
