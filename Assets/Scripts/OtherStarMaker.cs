﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserData
{
    public byte[] data;
    int cur;
    public UserData(byte[] data)
    {
        this.data = data;
        this.cur = 0;
    }
    public int readInt()
    {
        if (cur + 3 < data.Length)
        {
            int value = System.BitConverter.ToInt32(data, cur);
            cur += 4;
            return value;
        }
        return -1;
    }
    public float readFloat()
    {
        if (cur + 3 < data.Length)
        {
            float value = System.BitConverter.ToSingle(data, cur);
            cur += 4;
            return value;
        }
        return -1f;
    }
    public void reset()
    {
        cur = 0;
    }
}

public class OtherStarMaker : MonoBehaviour
{
    public GameObject[] starPrefabs;
    int need = 4;//表示小于need的是需要消灭的
    int maxCard = 16;//小于maxCard的关卡都是存在的
    enum STAR_TYPE
    {
        Earth,//普通星球0
        Pop,//弹跳星球1
        Explosion,//自爆星球2
        FixedStar,//带卫星的恒星
        Crystal,//一个变两个的星星
        Hole,//黑洞3
    };
    struct StarInfo
    {
        public Vector2 pos;
        public STAR_TYPE type;
        public float size;//如果是星球则为半径，如果是格挡则为长度
        public UserData userData;//放一些数据，如自爆时间、巡逻路径类型等（每种有自己的格式）
    }

    List<List<StarInfo>> starsInit;
    List<Transform> crossStars, unCrossStars;
    void Awake()
    {
        starsInit = new List<List<StarInfo>>(maxCard);
        for (int i = 0; i < maxCard; ++i)
            starsInit.Add(new List<StarInfo>());

        #region 关卡
        //第一关
        starsInit[0].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Earth });

        //第二关
        starsInit[1].Add(new StarInfo { pos = new Vector2(-2, 0), type = STAR_TYPE.Earth });
        starsInit[1].Add(new StarInfo { pos = new Vector2(2, 0), type = STAR_TYPE.Hole });

        //第三关
        starsInit[2].Add(new StarInfo { pos = new Vector2(-2, 0), type = STAR_TYPE.Earth });
        starsInit[2].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Hole });
        starsInit[2].Add(new StarInfo { pos = new Vector2(2, 0), type = STAR_TYPE.Earth });

        //第四关
        starsInit[3].Add(new StarInfo { pos = new Vector2(-1, -1), type = STAR_TYPE.Earth });
        starsInit[3].Add(new StarInfo { pos = new Vector2(-1, 1), type = STAR_TYPE.Earth });
        starsInit[3].Add(new StarInfo { pos = new Vector2(1, -1), type = STAR_TYPE.Earth });
        starsInit[3].Add(new StarInfo { pos = new Vector2(1, 1), type = STAR_TYPE.Earth });

        //第五关
        starsInit[4].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Earth });
        starsInit[4].Add(new StarInfo { pos = new Vector2(1.5f, 1.5f), type = STAR_TYPE.Earth });
        starsInit[4].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.Earth });
        starsInit[4].Add(new StarInfo { pos = new Vector2(4.5f, 1.5f), type = STAR_TYPE.Earth });
        starsInit[4].Add(new StarInfo { pos = new Vector2(-4.5f, 0), type = STAR_TYPE.Hole });
        starsInit[4].Add(new StarInfo { pos = new Vector2(-1.5f, 0), type = STAR_TYPE.Hole });
        starsInit[4].Add(new StarInfo { pos = new Vector2(1.5f, 0), type = STAR_TYPE.Hole });
        starsInit[4].Add(new StarInfo { pos = new Vector2(4.5f, 0), type = STAR_TYPE.Hole });



        //  starsInit[4].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 192, 63 }) });
        //  starsInit[4].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 1, 0, 0, 0, 35, 0, 0, 0, 154, 153, 153, 63, 0, 0, 0, 64 }) });

        //第六关
        starsInit[5].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Crystal });
        starsInit[5].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Earth });
        starsInit[5].Add(new StarInfo { pos = new Vector2(3, -1.3f), type = STAR_TYPE.Earth });
        starsInit[5].Add(new StarInfo { pos = new Vector2(3, 1.3f), type = STAR_TYPE.Earth });


        // 第七关
        starsInit[6].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Pop });
        starsInit[6].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 192, 63 }) });
        starsInit[6].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 1, 0, 0, 0, 35, 0, 0, 0, 154, 153, 153, 63, 0, 0, 0, 64 }) });

        //第八关
        starsInit[7].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 192, 63 }) });
        starsInit[7].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 1, 0, 0, 0, 35, 0, 0, 0, 154, 153, 153, 63, 0, 0, 0, 64 }) });
        starsInit[7].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Hole });

        //第九关
        starsInit[8].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 192, 63 }) });
        starsInit[8].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 1, 0, 0, 0, 35, 0, 0, 0, 154, 153, 153, 63, 0, 0, 0, 64 }) });
        starsInit[8].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Hole });


        #endregion
    }

    Transform starMaker;
    void Start()
    {
        starMaker = GameObject.Find("starMaker").GetComponent<Transform>();
        crossStars = new List<Transform>();
        unCrossStars = new List<Transform>();
    }
    void removeAll()
    {
        //crossStars、unCrossStars分别代表需要消灭和不需要消灭的东西(典型代表是黑洞）
        foreach (Transform t in crossStars)
            Destroy(t.gameObject);
        crossStars.Clear();
        foreach (Transform t in unCrossStars)
            Destroy(t.gameObject);
        unCrossStars.Clear();
        starMaker.SendMessage("ereaseStars", false);
    }

    void initStars()
    {
        removeAll();
        foreach (var info in starsInit[Global.card])
        {
            GameObject g = Object.Instantiate<GameObject>(starPrefabs[(int)info.type]);
            Transform t = g.transform;
            t.parent = transform;
            ((int)info.type < need ? crossStars : unCrossStars).Add(t);
            t.localPosition = info.pos;
            if (info.userData != null)
                t.SendMessage("setData", info.userData);
        }
    }
    void resetStars()
    {
        foreach (Transform t in crossStars)
            t.SendMessage("reset");
        foreach (Transform t in unCrossStars)
            t.SendMessage("reset");
    }
    void checkIsWin()
    {
        bool win = true;
        foreach (Transform t in crossStars)
        {
            if (t.CompareTag("alive"))
            {
                win = false;
                break;
            }
        }
        if (win)
        {
            StartCoroutine(delayToCard());
        }
    }
    IEnumerator delayToCard()
    {
        yield return new WaitForSeconds(0.6f);
        setCard(1);
    }
    void setCard(int plus)//plus 可为-1,0,1，分别表示上一关、不变和下一关
    {
        int card = Global.card + plus;
        if (card > -1 && card < maxCard)
        {
            Global.card = card;
            //PlayerPrefs.SetInt("card", card);
            initStars();
            starMaker.SendMessage("ereaseStars", true);
        }
        else
        {
            //通关了
        }
    }
}
