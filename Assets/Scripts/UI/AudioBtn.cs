using UnityEngine;
using System.Collections;

public class AudioBtn : MonoBehaviour
{
    public GameObject onSprite, offSprite;
    void setOn(bool on)
    {
        onSprite.SetActive(on);
        offSprite.SetActive(!on);
    }
}
