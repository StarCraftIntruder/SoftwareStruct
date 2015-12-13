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
        otherStars = transform.FindChild("otherStars");
        blackHoles = transform.FindChild("blackHoles");

        starMaker = GameObject.Find("starMaker");

        initStars();
    }

    void initStars()
    {
        foreach (Transform t in otherStars.GetComponentsInChildren<Transform>())
        {
            if (t != otherStars)
                Destroy(t.gameObject);
        }
        foreach (Transform t in blackHoles.GetComponentsInChildren<Transform>())
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
    void checkIsWin()
    {
        if (otherStars.childCount == 1)
        {
            print("win");
            card++;
            if (card < 3)
                starMaker.SendMessage("ereaseStar");
        }
    }
    void Update()
    {

    }
}
