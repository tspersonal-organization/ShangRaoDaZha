using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGongGao : UIBase<UIGongGao>
{
	// Use this for initialization
	void Start ()
    {
        transform.Find("Base").Find("desc").GetComponent<UILabel>().text = Player.Instance.content;
	}

    public void Close()
    {
        SoundManager.Instance.PlaySound(UIPaths.SOUND_BUTTON);
        UIManager.Instance.HideUIPanel(UIPaths.UIPanel_GongGao);
    }
}
