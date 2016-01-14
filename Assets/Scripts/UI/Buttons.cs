using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {
    UIEventListener [] buttons;
	void Start () {
        buttons = transform.GetComponentsInChildren<UIEventListener>();
        foreach(UIEventListener btn in buttons){
            btn.onClick = onClick;
        }
	}
    void onClick(GameObject btn)
    {
        switch (btn.name)
        {
            case "preBtn"://上一关
                GameObject.Find("otherStarMaker").SendMessage("setCard", -2);
                break;
            case "nextBtn"://下一关
                GameObject.Find("otherStarMaker").SendMessage("setCard", -1);
                break;
        }
    }
	
}
