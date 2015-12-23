using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserData
{
    public byte[] data;
}

public class OtherStarMaker : MonoBehaviour
{
    public GameObject[] starPrefabs;
    int need = 3;//表示小于need的是需要消灭的
    int maxCard = 4;//小于maxCard的关卡都是存在的
    enum STAR_TYPE
    {
        Earth,//普通星球0
        Pop,//弹跳星球1
        Explosion,//自爆星球2
        Hole,//黑洞3
    };
    struct StarInfo
    {
        public Vector2 pos;
        public STAR_TYPE type;
        public UserData userData;//放一些数据，如自爆时间、巡逻路径类型等（每种有自己的格式）
    }

    List<List<StarInfo>> starsInit;
    List<Transform> crossStars, unCrossStars;
    void Awake()
    {
        starsInit = new List<List<StarInfo>>(maxCard);
        for (int i = 0; i < maxCard; ++i)
            starsInit.Add(new List<StarInfo>());
        #region 第一关数据
        starsInit[0].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Earth });
        starsInit[0].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.Earth });
        #endregion

        #region 第二关数据
        starsInit[1].Add(new StarInfo { pos = new Vector2(0, -2), type = STAR_TYPE.Earth });
        starsInit[1].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Hole });
        starsInit[1].Add(new StarInfo { pos = new Vector2(0, 2), type = STAR_TYPE.Earth });
        #endregion

        #region 第三关数据
        starsInit[2].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Pop, userData = new UserData() });
        starsInit[2].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.Earth });
        #endregion

        #region 第四关数据
        starsInit[3].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Explosion, userData = new UserData() });
        starsInit[3].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Earth });
        starsInit[3].Add(new StarInfo { pos = new Vector2(0, 3), type = STAR_TYPE.Explosion, userData = new UserData() });
        #endregion
    }

    int card;
    Transform starMaker;
    void Start()
    {
        card = PlayerPrefs.GetInt("card", -1);
        if (card == -1)
        {
            card = 0;
            PlayerPrefs.SetInt("card", card);//读取关卡
        }

        starMaker = GameObject.Find("starMaker").GetComponent<Transform>();
        crossStars = new List<Transform>();
        unCrossStars = new List<Transform>();

        setCard(card);
    }

    void initStars()
    {
        //crossStars、unCrossStars分别代表需要消灭和不需要消灭的东西(典型代表是黑洞）
        foreach (Transform t in crossStars)
            Destroy(t.gameObject);
        crossStars.Clear();
        foreach (Transform t in unCrossStars)
            Destroy(t.gameObject);
        unCrossStars.Clear();

        foreach (var info in starsInit[card])
        {
            GameObject g = Object.Instantiate<GameObject>(starPrefabs[(int)info.type]);
            Transform t = g.transform;
            t.parent = transform;
            ((int)info.type < need ? crossStars : unCrossStars).Add(t);
            t.position = info.pos;
            if (info.userData != null)
                t.SendMessage("setData", info.userData);
        }
    }
    void resetStars()
    {
        foreach (Transform t in crossStars)
        {
            t.gameObject.SetActive(true);
            t.SendMessage("reset");
        }
        foreach (Transform t in unCrossStars)
        {
            t.gameObject.SetActive(true);
            t.SendMessage("reset");
        }
    }
    void checkIsWin()
    {
        bool win = true;
        foreach (Transform t in crossStars)
        {
            if (t.gameObject.activeSelf)
            {
                win = false;
                break;
            }
        }
        if (win)
        {
            int card = this.card + 1;
            if (card < maxCard)
            {
                //PlayerPrefs.SetInt("card", ++card);
                setCard(card);
            }
        }
    }
    void setCard(int card)//选关
    {
        if (card == -1) card = this.card + 1;
        else if (card == -2) card = this.card - 1;

        if (card > -1 && card < maxCard)
        {
            this.card = card;
            //PlayerPrefs.SetInt("card", card);
            initStars();
            starMaker.SendMessage("ereaseStars");
        }
    }
}
