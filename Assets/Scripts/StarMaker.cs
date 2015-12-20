using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StarMaker : MonoBehaviour
{
    public GameObject starPrefab;

    GameObject otherStarMaker;
    List<Vector2> points;
    void addPoint(Vector2 point)
    {
        if (stars.Count > 0)
            stars[0].SendMessage("addPoint", point);
        else
            points.Add(point);
    }
    List<GameObject> stars = new List<GameObject>();
    void copyStar(GameObject ob)
    {
        GameObject starObj = GameObject.Instantiate<GameObject>(ob);
        Transform star = starObj.transform;
        star.parent = transform;
        //怎么样怎么样
    }
    void ereaseStar(GameObject ob)
    {
        if (stars.Contains(ob))
            stars.Remove(ob);
        if (stars.Count == 0)
            otherStarMaker.SendMessage("resetStars");
    }
    void ereaseStars()
    {
        foreach (GameObject ob in stars)
            Destroy(ob);
        stars.Clear();
        points = new List<Vector2>();
        otherStarMaker.SendMessage("resetStars");
    }
    void makeStar(Vector2 pos)
    {
        GameObject starObj = GameObject.Instantiate<GameObject>(starPrefab);
        Transform star = starObj.transform;
        star.parent = transform;
        star.position = pos;
        starObj.SendMessage("setPoints", points);
        stars.Add(starObj);
    }
    void Start()
    {
        otherStarMaker = GameObject.Find("otherStarMaker");
    }
}
