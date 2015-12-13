using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OtherStarMaker : MonoBehaviour
{
    public GameObject[] starPrefabs;
    enum STAR_TYPE
    {
        Hole,//黑洞
        Blue
    };
    struct StarInfo
    {
        public Vector2 pos;
        public STAR_TYPE type;
    }

    List<List<StarInfo>> starsInit;
    void Awake()
    {
        int cardCount = 10;
        starsInit = new List<List<StarInfo>>(cardCount);
        for (int i = 0; i < cardCount; ++i)
            starsInit.Add(new List<StarInfo>());
        #region 第一关数据
        starsInit[0].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Blue });
        starsInit[0].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.Blue });
        #endregion

        #region 第二关数据
        starsInit[1].Add(new StarInfo { pos = new Vector2(0, -2), type = STAR_TYPE.Blue });
        starsInit[1].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Hole });
        starsInit[1].Add(new StarInfo { pos = new Vector2(0, 2), type = STAR_TYPE.Blue });
        #endregion

        #region 第三关数据
        starsInit[2].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Blue });
        starsInit[2].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.Blue });
        starsInit[2].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Hole });
        #endregion
    }

    int card;
    Transform otherStars, blackHoles;
    GameObject starMaker;
    void Start()
    {
        card = PlayerPrefs.GetInt("card", -1);
        if (card == -1)
        {
            card = 0;
            PlayerPrefs.SetInt("card", card);//读取关卡
        }

        if (card > 2)
            print("通关了");

        otherStars = transform.FindChild("otherStars");
        blackHoles = transform.FindChild("blackHoles");

        starMaker = GameObject.Find("starMaker");

        initStars();
    }

    void initStars()
    {
        foreach (Transform t in otherStars.GetComponentsInChildren<Transform>(true))
        {
            if (t != otherStars)
                Destroy(t.gameObject);
        }
        foreach (Transform t in blackHoles.GetComponentsInChildren<Transform>(true))
        {
            if (t != blackHoles)
                Destroy(t.gameObject);
        }
        foreach (var info in starsInit[card])
        {
            GameObject g = Object.Instantiate<GameObject>(starPrefabs[(int)info.type]);
            Transform t = g.transform;
            t.parent = info.type == STAR_TYPE.Hole ? blackHoles : otherStars;
            t.position = info.pos;
        }
    }
    void resetStars()
    {
        foreach (Transform t in otherStars.GetComponentsInChildren<Transform>(true))
        {
            if (t != otherStars)
                t.gameObject.SetActive(true);
        }
        foreach (Transform t in blackHoles.GetComponentsInChildren<Transform>(true))
        {
            if (t != blackHoles)
                t.gameObject.SetActive(true);
        }
    }
    void checkIsWin()
    {
        int childCount = -1;
        foreach (Transform t in otherStars.GetComponentsInChildren<Transform>(false))
        {
                childCount++;
        }
        if (childCount < 1)
        {
            //PlayerPrefs.SetInt("card", ++card);
            card++;
            if (card < 3)
            {
                initStars();
                starMaker.SendMessage("ereaseStar");
            }
        }
    }
    void Update()
    {

    }
}
