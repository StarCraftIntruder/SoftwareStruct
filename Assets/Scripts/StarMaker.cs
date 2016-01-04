using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StarMaker : MonoBehaviour
{
    public GameObject starPrefab;

    Transform otherStarMaker;
    List<Vector2> points;
    void addPoint(Vector2 point)
    {
        if (stars.Count > 0)
            stars[0].SendMessage("addPoint", point);
        else
            points.Add(point);
    }
    List<Transform> stars = new List<Transform>();
    void copyStar(Transform copy)
    {
        copy.parent = transform;
        stars.Add(copy);
    }
    void ereaseStar(Transform ob)
    {
        if (stars.Contains(ob))
            stars.Remove(ob);
        if (stars.Count == 0)
            otherStarMaker.SendMessage("resetStars");
    }
    void ereaseStars(bool reset)
    {
        if (reset && stars.Count > 0)
            otherStarMaker.SendMessage("resetStars");
        foreach (Transform ob in stars)
            Destroy(ob.gameObject);
        stars.Clear();
        points = new List<Vector2>();
    }
    void makeStar(Vector2 pos)
    {
        GameObject starObj = GameObject.Instantiate<GameObject>(starPrefab);
        Transform star = starObj.transform;
        star.parent = transform;
        star.localPosition = pos;
        star.SendMessage("setPoints", points);
        stars.Add(star);
    }
    void Start()
    {
        otherStarMaker = GameObject.Find("otherStarMaker").GetComponent<Transform>();
    }
}
