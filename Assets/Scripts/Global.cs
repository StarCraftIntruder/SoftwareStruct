using UnityEngine;
using System.Collections;

public enum Scene
{
    MenuScene, GameScene, MakerScene
}

public class Global : MonoBehaviour
{
    //放一些常量、全局变量、后台处理（如back键处理）
    Scene scene;
    public static int card;
    public static bool audioOn;
    GameObject otherStarMaker;
    GameObject menuShow, menuHide;//这两个控制menu动画

    void Awake()
    {
        scene = Scene.MenuScene;
        int audio = PlayerPrefs.GetInt("audio", -1);
        if (audio == -1)
        {
            PlayerPrefs.SetInt("audio", 1);
            audio = 1;
        }
        audioOn = audio == 1;
        GameObject.Find("audioBtn").SendMessage("setOn", audioOn);
    }

    void Start()
    {
        menuShow = GameObject.Find("title");
        menuHide = GameObject.Find("makerBtn");

        otherStarMaker = GameObject.Find("otherStarMaker");

        card = PlayerPrefs.GetInt("card", -1);
        if (card == -1)
        {
            card = 0;
            PlayerPrefs.SetInt("card", card);//读取关卡
        }

        StartCoroutine(delayToShow());
    }
    IEnumerator delayToShow()
    {
        yield return new WaitForSeconds(0.4f);
        menuShow.SendMessage("show");
    }
    void hideMenu(Scene newScene)
    {
        menuHide.SendMessage("hide");
        scene = newScene;
    }
    void showMenu()
    {
        menuShow.SendMessage("show");
        scene = Scene.MenuScene;
    }
    void startGame()
    {
        otherStarMaker.SendMessage("setCard", 0, SendMessageOptions.RequireReceiver);
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            switch (scene) { 
                case Scene.MenuScene:
                    Application.Quit();
                    break;
                case Scene.GameScene:
                    otherStarMaker.SendMessage("removeAll");
                    showMenu();
                    break;
                case Scene.MakerScene:

                    showMenu();
                    break;
            }
        }
    }
}
