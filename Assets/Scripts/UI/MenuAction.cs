using UnityEngine;
using System.Collections;

public class MenuAction : MonoBehaviour
{
    public TweenPosition tweenP, tweenB;
    TweenPosition tween;
    void Start()
    {
        tween = GetComponent<TweenPosition>();
    }
    void show()
    {
        tween.onFinished.Clear();
        if (tweenP != null)
            tween.SetOnFinished(
                delegate()
                {
                    tweenP.SendMessage("show");
                }
                );
        tween.PlayForward();
    }
    void hide()
    {
        tween.onFinished.Clear();
        if (tweenB != null)
            tween.SetOnFinished(
                delegate()
                {
                    tweenB.SendMessage("hide");
                }
                );
        tween.PlayReverse();
    }
}
