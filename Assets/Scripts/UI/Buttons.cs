using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour
{
    public UIEventListener[] buttons;
    GameObject global;

    void Start()
    {
        global = GameObject.Find("Global");
        foreach (UIEventListener btn in buttons)
        {
            btn.onClick = onClick;
        }
    }
    void onClick(GameObject btn)
    {
        switch (btn.name)
        {
            case "preBtn"://上一关
                GameObject.Find("otherStarMaker").SendMessage("setCard", -1);
                break;
            case "nextBtn"://下一关
                GameObject.Find("otherStarMaker").SendMessage("setCard", 1);
                break;
            case "startBtn":
                global.SendMessage("hideMenu", Scene.GameScene);
                StartCoroutine(delayToGame());
                break;
            case "audioBtn":
                Global.audioOn = !Global.audioOn;
                btn.SendMessage("setOn", Global.audioOn);
                PlayerPrefs.SetInt("audio", Global.audioOn ? 1 : 0);
                break;
            case "makerBtn"://制作人员
                global.SendMessage("hideMenu", Scene.MakerScene);
                break;
        }
    }
    IEnumerator delayToGame()
    {
        yield return new WaitForSeconds(0.6f);
        global.SendMessage("startGame");
    }
}
