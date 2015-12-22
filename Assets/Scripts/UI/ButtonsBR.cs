using UnityEngine;
using System.Collections;

public class ButtonsBR : MonoBehaviour {
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
            case "cardBtn":
                UIInput cardTxt = GameObject.Find("cardTxt").GetComponent<UIInput>();
                GameObject.Find("otherStarMaker").SendMessage("setCard", System.Int32.Parse(cardTxt.value)-1);
                break;
        }
    }
	
}
