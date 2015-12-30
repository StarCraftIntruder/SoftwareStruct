using UnityEngine;
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
        starsInit[2].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Pop, userData = new UserData(new byte[] { 0 }) });
        starsInit[2].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.Earth });
        #endregion

        #region 第四关数据
        starsInit[3].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        starsInit[3].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Earth });
        starsInit[3].Add(new StarInfo { pos = new Vector2(0, 3), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        #endregion

        #region 第五关数据
        starsInit[4].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 192, 63 }) });
        starsInit[4].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 1, 0, 0, 0, 35, 0, 0, 0, 154, 153, 153, 63, 0, 0, 0, 64 }) });
        #endregion

        #region 第六关数据
        starsInit[5].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Crystal });
        starsInit[5].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Earth });
        starsInit[5].Add(new StarInfo { pos = new Vector2(3, -1.3f), type = STAR_TYPE.Earth });
        starsInit[5].Add(new StarInfo { pos = new Vector2(3, 1.3f), type = STAR_TYPE.Earth });
        #endregion

        #region 第七关数据
        starsInit[6].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Pop, userData = new UserData(new byte[] { 0 }) });
        starsInit[6].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 192, 63 }) });
        starsInit[6].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 1, 0, 0, 0, 35, 0, 0, 0, 154, 153, 153, 63, 0, 0, 0, 64 }) });
        #endregion

        #region 第八关数据
        starsInit[7].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 192, 63 }) });
        starsInit[7].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 1, 0, 0, 0, 35, 0, 0, 0, 154, 153, 153, 63, 0, 0, 0, 64 }) });
        starsInit[7].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Hole });
        #endregion

        #region 第九关数据
        starsInit[8].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Pop, userData = new UserData(new byte[] { 0 }) });
        starsInit[8].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.Hole });
        #endregion

        #region 第十关数据
        starsInit[9].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Hole });
        starsInit[9].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        starsInit[9].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.Earth });
        starsInit[9].Add(new StarInfo { pos = new Vector2(0, 3), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        #endregion

        #region 第十一关数据
        starsInit[10].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Pop, userData = new UserData(new byte[] { 0 }) });
        starsInit[10].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        starsInit[10].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Earth });
        starsInit[10].Add(new StarInfo { pos = new Vector2(0, 3), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        #endregion

        #region 第十二关数据
        starsInit[11].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 192, 63 }) });
        starsInit[11].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 1, 0, 0, 0, 35, 0, 0, 0, 154, 153, 153, 63, 0, 0, 0, 64 }) });
        starsInit[11].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Hole });
        starsInit[11].Add(new StarInfo { pos = new Vector2(-3, -3), type = STAR_TYPE.Pop, userData = new UserData(new byte[] { 0 }) });
        starsInit[11].Add(new StarInfo { pos = new Vector2(0, 3), type = STAR_TYPE.Earth });
        #endregion

        #region 第十三关数据
        starsInit[12].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        starsInit[12].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Earth });
        starsInit[12].Add(new StarInfo { pos = new Vector2(0, 3), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        starsInit[12].Add(new StarInfo { pos = new Vector2(0, -3), type = STAR_TYPE.Hole });
        starsInit[12].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.Pop, userData = new UserData(new byte[] { 0 }) });
        #endregion

        #region 第十四关数据
        starsInit[13].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 192, 63 }) });
        starsInit[13].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 1, 0, 0, 0, 35, 0, 0, 0, 154, 153, 153, 63, 0, 0, 0, 64 }) });
        starsInit[13].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Hole }); 
        starsInit[13].Add(new StarInfo { pos = new Vector2(3, 3), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        starsInit[13].Add(new StarInfo { pos = new Vector2(0, -3), type = STAR_TYPE.Earth });
        starsInit[13].Add(new StarInfo { pos = new Vector2(0, 3), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        #endregion

        #region 第十五关数据
        starsInit[14].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 192, 63 }) });
        starsInit[14].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 1, 0, 0, 0, 35, 0, 0, 0, 154, 153, 153, 63, 0, 0, 0, 64 }) });
        starsInit[14].Add(new StarInfo { pos = new Vector2(-3, 3), type = STAR_TYPE.Pop, userData = new UserData(new byte[] { 0 }) });
        starsInit[14].Add(new StarInfo { pos = new Vector2(3, -3), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        starsInit[14].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Earth });
        starsInit[14].Add(new StarInfo { pos = new Vector2(0, 3), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        #endregion

        #region 第十六关数据
        starsInit[15].Add(new StarInfo { pos = new Vector2(-3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 192, 63 }) });
        starsInit[15].Add(new StarInfo { pos = new Vector2(3, 0), type = STAR_TYPE.FixedStar, userData = new UserData(new byte[] { 1, 0, 0, 0, 35, 0, 0, 0, 154, 153, 153, 63, 0, 0, 0, 64 }) });
        starsInit[15].Add(new StarInfo { pos = new Vector2(-3, 3), type = STAR_TYPE.Pop, userData = new UserData(new byte[] { 0 }) });
        starsInit[15].Add(new StarInfo { pos = new Vector2(3, -3), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        starsInit[15].Add(new StarInfo { pos = new Vector2(0, 0), type = STAR_TYPE.Earth });
        starsInit[15].Add(new StarInfo { pos = new Vector2(0, 3), type = STAR_TYPE.Explosion, userData = new UserData(new byte[] { 0 }) });
        starsInit[15].Add(new StarInfo { pos = new Vector2(3, 3), type = STAR_TYPE.Hole });
        starsInit[15].Add(new StarInfo { pos = new Vector2(-3, -3), type = STAR_TYPE.Hole }); 
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
        int card = this.card + 1;
        if (card < maxCard)
        {
            //PlayerPrefs.SetInt("card", ++card);
            setCard(card);
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
            starMaker.SendMessage("ereaseStars", true);
        }
    }
}
